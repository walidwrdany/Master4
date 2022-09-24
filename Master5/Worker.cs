using Master5.Features;

namespace Master5;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IFeatureFactory _factory;

    public Worker(ILogger<Worker> logger, IFeatureFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Start:

        await _factory.GetFeature<DisplayLogo>().ExecuteAsync();
        await _factory.Goto(Console.ReadKey());

        goto Start;
    }
}