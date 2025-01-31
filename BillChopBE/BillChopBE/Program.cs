using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using BillChopBE.Extensions;
using Microsoft.Extensions.Logging;

namespace BillChopBE
{
    public class Program
    {
        protected Program() {}

        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateBillChopDb()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) => 
                {
                    logging.ClearProviders();

                    if (context.HostingEnvironment.IsDevelopment())
                        logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
