using Microsoft.Extensions.DependencyInjection;
using MonitoraEstoqueServico.Services;
using MonitoraEstoqueServico;
using ServiceMonitor.Services;
using Microsoft.Extensions.Hosting;

namespace ServiceMonitor
{
    internal static class Program
    {
        private static ServiceProvider _serviceProvider;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            ApplicationConfiguration.Initialize();

            var mainForm = _serviceProvider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService() 
            .ConfigureServices((hostContext, services) =>
            {
                ConfigureServices(services);
            });


        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ConfiguracaoService>();
            services.AddSingleton<MonitoramentoService>();
            services.AddHttpClient<ApiService>();
            services.AddSingleton<MainForm>();
        }
    }
}
