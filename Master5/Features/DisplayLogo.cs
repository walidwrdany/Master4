namespace Master5.Features;

public class DisplayLogo : BaseFeature, IFeature
{
    private readonly ILogger<DisplayLogo> _logger;
    private readonly IFeatureFactory _feature;

    public DisplayLogo(ILogger<DisplayLogo> logger, IFeatureFactory feature)
    {
        _logger = logger;
        _feature = feature;
    }

    public string Id => "";
    public string Name => GetType().Name;
    public async Task ExecuteAsync()
    {
        await Task.Run(() =>
        {
            _feature.GetAllFeatures()
                .Where(x => !string.IsNullOrEmpty(x.Id))
                .ToList()
                .ForEach(feature =>
                {
                    Console.WriteLine($"\t{feature.Id} | {feature.Name}");
                });
        });
    }
}