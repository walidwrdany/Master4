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

        await Task.Run(() =>
        {
            
            _factory.GetAllFeatures()
                .Where(x => !string.IsNullOrEmpty(x.Id))
                .ToList()
                .ForEach(f =>
                {
                    Console.WriteLine($" {new string(' ', GetNum(f.Id.Length))} {f.Id} | {f.Name}");
                });

        }, stoppingToken);

        Console.WriteLine("{0}>>", AppDomain.CurrentDomain.BaseDirectory);
        await _factory.Goto(Console.ReadKey());

        goto Start;
    }

    private static int GetNum(int f)
    {
        return 3 - f;
    }
}