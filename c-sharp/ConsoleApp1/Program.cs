using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IJokeService, JokeService>();
            services.AddTransient<ConsoleAppHandler>();
            services.AddHttpClient<JokeService>();

            var serviceProvider = services.BuildServiceProvider();
            var consoleApp = serviceProvider.GetRequiredService<ConsoleAppHandler>();


            await consoleApp.RunAsync();
        }
    }
}
