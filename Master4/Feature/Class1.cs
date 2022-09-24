using Microsoft.Extensions.Logging;

namespace Master4.Feature;

public class ChangeColor : IFeature
{
    private readonly ILogger<ChangeColor> _logger;

    public ChangeColor(ILogger<ChangeColor> logger)
    {
        _logger = logger;
    }


    public string Id => "00";
    public string Name => "Change Color";

    public async Task Execute()
    {
        await Task.Run(() => _logger.LogInformation(typeof(ChangeColor).FullName));
    }
}