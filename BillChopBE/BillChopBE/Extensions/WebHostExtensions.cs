using BillChopBE.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BillChopBE.Extensions
{
    public static class WebHostExtensions
    {
        public static IHost MigrateBillChopDb(this IHost webHost)
        {
            var dbConfig = webHost.Services.GetRequiredService<IOptions<BillChopConfig>>();
            var options = new DbContextOptionsBuilder<BillChopContext>()
                .UseSqlServer(dbConfig.Value.BillChopDb)
                .UseLazyLoadingProxies()
                .Options;

            var dbContext = new BillChopContext(dbConfig, options);
            dbContext.Database.Migrate();

            return webHost;
        }
    }
}
