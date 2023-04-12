using System.Text;

namespace Master5;

public class Worker : BackgroundService
{
    private readonly IFeatureFactory _factory;
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger, IFeatureFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.OutputEncoding = Encoding.UTF8;

        while (!stoppingToken.IsCancellationRequested)
        {
            await NewMethod(stoppingToken); 
        }
    }

    private async Task NewMethod(CancellationToken stoppingToken)
    {
        Console.WriteLine(@"          +-----------------------------------------------+          ");
        Console.WriteLine(@"        _ |                                               | _        ");
        Console.WriteLine(@"       /O)|        Hi 👋, I'm Walid Wrdany                |(O\       ");
        Console.WriteLine(@"      / / |      Senior Full Stack .NET Developer         | \ \      ");
        Console.WriteLine(@"     ( (_ |  _                                         _  | _) )     ");
        Console.WriteLine(@"    ((\ \)+-/O)---------------------------------------(O\-+(/ /))    ");
        Console.WriteLine(@"    (\\\ \_/ /                                         \ \_/ ///)    ");
        Console.WriteLine(@"     \      /                                           \      /     ");
        Console.WriteLine(@"      \____/                                             \____/      ");
        Console.WriteLine(@"     ============================================================    ");
        Console.WriteLine(@" __________|___________                        __________|___________");
        Console.WriteLine();
        await Task.Run(() =>
        {
            _factory.GetAllFeatures()
                .ToList()
                .ForEach(f => Console.WriteLine(f.ToString()));
        }, stoppingToken);

        Console.Write("{0}>>", AppDomain.CurrentDomain.BaseDirectory);
        await _factory.Goto(Console.ReadLine());

        Console.ReadKey(false);
        Console.Clear();
    }
}