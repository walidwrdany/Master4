using Master4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

CreateHostBuilder(args).Build().AppInitialize().Run();


static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .UseSerilog(SerilogConfiguration.SetLoggerConfiguration)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddOptions();
            IConfiguration configuration = hostContext.Configuration;

            services.AddScoped(typeof(IFeature));
        });
}


public static class ApplicationInitialization
{
    /// <summary>
    /// Initializes objects on initialization of the Application.
    /// For example - Initialize all cache stores
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static IHost AppInitialize(this IHost host)
    {
        IServiceScopeFactory scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        using IServiceScope scope = scopeFactory.CreateScope();
        return host;
    }
}