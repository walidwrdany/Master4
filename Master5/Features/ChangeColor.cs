namespace Master5.Features;

public class ChangeColor : BaseFeature, IFeature
{
    private readonly ILogger<ChangeColor> _logger;

    public ChangeColor(ILogger<ChangeColor> logger)
    {
        _logger = logger;
    }

    public string Id => "0";
    public string Name => GetType().Name;
    public async Task ExecuteAsync()
    {
        await Task.Run(() => _logger.LogInformation($"{Id} | {Name}"));
    }
}