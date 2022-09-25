using Master5;
using Master5.Configuration;
using Master5.Utils;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog(SerilogConfiguration.SetLoggerConfiguration)
    .ConfigureServices((context, services) =>
    {
        Helper.GetTypes<IFeature>().ForEach(x => services.AddSingleton(x));

        services.AddSingleton<IFeatureFactory, FeatureFactory>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
