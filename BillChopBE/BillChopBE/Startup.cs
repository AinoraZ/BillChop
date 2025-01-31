using BillChopBE.DataAccessLayer.Filters.Factories;
using BillChopBE.Extensions;
using BillChopBE.Middleware;
using BillChopBE.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using BillChopBE.Services.Configurations;
using Microsoft.Extensions.Hosting;

namespace BillChopBE
{
    public class Startup
    {
        readonly string localHostOrigins = "_localHostOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(localHostOrigins, builder => 
                {
                    builder
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            // Example of creating injectable config
            services.ConfigureValidatableSetting<JwtConfig>(Configuration.GetSection("Jwt"));

            services.AddBillChopContext(Configuration.GetConnectionString("BillChopDb"));
            services.AddBillChopRepositories();
            services.AddAutoMapper();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<ILoanDbFilterFactory, LoanDbFilterFactory>();
            services.AddScoped<IBillDbFilterFactory, BillDbFilterFactory>();
            services.AddScoped<IPaymentDbFilterFactory, PaymentDbFilterFactory>();

            services.AddSwaggerGen(c => 
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                    Array.Empty<string>()
                    }
                });
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Billchop API v0.0.1-dev.1");
            });

            app.UseHttpsRedirection();

            app.UseMiddleware(typeof(TransactionMiddleware));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment()) 
            {
                app.UseCors(localHostOrigins);
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
