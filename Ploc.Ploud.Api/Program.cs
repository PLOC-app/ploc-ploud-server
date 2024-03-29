using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Ploc.Ploud.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Limits.MaxRequestBodySize = 2000 * 1024 * 1024; // 2gb
                    });
                    webBuilder.UseIISIntegration();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
