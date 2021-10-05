using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using BillChopBE.DataAccessLayer;
using BillChopBE.DataAccessLayer.Repositories;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BillChopBE.Extensions
{
    public static class ServiceCollectionBillChopContextExtensions
    {
        public static IServiceCollection AddBillChopContext(this IServiceCollection services, string connectionString, IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            return services
                .ConfigurePerRequestDbConnection(connectionString)
                .ConfigurePerRequestTransaction(level)
                .ConfigurePerRequestDbOptions()
                .ConfigurePerRequestBillChopContext();
        }

        public static IServiceCollection AddBillChopRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserEFRepository>();
            services.AddScoped<IGroupRepository, GroupEFRepository>();
            services.AddScoped<IBillRepository, BillEFRepository>();
            services.AddScoped<ILoanRepository, LoanEFRepository>();
            services.AddScoped<IPaymentRepository, PaymentEFRepository>();

            return services;
        }

        private static IServiceCollection ConfigurePerRequestDbConnection(this IServiceCollection services, string connectionString)
        {
            return services.AddScoped<DbConnection>((serviceProvider) =>
            {
                var dbConnection = new SqlConnection(connectionString);
                return dbConnection;
            });
        }

        private static IServiceCollection ConfigurePerRequestTransaction(this IServiceCollection services, IsolationLevel level)
        {
            return services.AddScoped((serviceProvider) =>
            {
                var dbConnection = serviceProvider.GetRequiredService<DbConnection>();
                dbConnection.Open();

                return dbConnection.BeginTransaction(level);
            });
        }

        private static IServiceCollection ConfigurePerRequestDbOptions(this IServiceCollection services)
        {
            return services.AddScoped((serviceProvider) =>
            {
                var dbConnection = serviceProvider.GetRequiredService<DbConnection>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                return new DbContextOptionsBuilder<BillChopContext>()
                    .UseLoggerFactory(loggerFactory)
                    .UseLazyLoadingProxies()
                    .UseSqlServer(dbConnection)
                    .Options;
            });
        }

        private static IServiceCollection ConfigurePerRequestBillChopContext(this IServiceCollection services)
        {
            return services.AddScoped((serviceProvider) =>
            {
                var transaction = serviceProvider.GetRequiredService<DbTransaction>();

                var config = serviceProvider.GetRequiredService<IOptions<BillChopConfig>>();
                var options = serviceProvider.GetRequiredService<DbContextOptions<BillChopContext>>();

                var context = new BillChopContext(config, options);
                context.Database.UseTransaction(transaction);
                return context;
            });
        }
    }
}
