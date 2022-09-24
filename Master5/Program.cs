using Master5;
using Master5.Configuration;
using Master5.Utils;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog(SerilogConfiguration.SetLoggerConfiguration)
    .ConfigureServices((context, services) =>
    {
        foreach (var type in Helper.GetTypes<IFeature>()) services.AddSingleton(type);

        services.AddSingleton<IFeatureFactory, FeatureFactory>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
