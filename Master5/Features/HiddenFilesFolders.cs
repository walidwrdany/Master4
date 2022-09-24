namespace Master5.Features;

public class HiddenFilesFolders : BaseFeature, IFeature
{
    private readonly ILogger<HiddenFilesFolders> _logger;

    public HiddenFilesFolders(ILogger<HiddenFilesFolders> logger)
    {
        _logger = logger;
    }

    public string Id => "1";
    public string Name => GetType().Name;
    public async Task ExecuteAsync()
    {
        await Task.Run(() => _logger.LogInformation($"{Id} | {Name}"));
    }
}