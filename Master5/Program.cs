using Master5;
using Master5.Utils;
using Serilog;
using Serilog.Events;

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog((_, configuration) =>
    {
        configuration
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .WriteTo.Console();
    })
    .ConfigureServices((_, services) =>
    {
        Helper.GetTypes<IFeature>().ForEach(x => services.AddSingleton(x));
        services.AddSingleton<IFeatureFactory, FeatureFactory>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();