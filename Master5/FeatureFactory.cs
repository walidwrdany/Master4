using Master5.Utils;

namespace Master5;

public class FeatureFactory : IFeatureFactory
{
    private readonly IServiceProvider _serviceProvider;

    public FeatureFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IFeature GetFeature<T>() where T : IFeature
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    public IEnumerable<IFeature> GetAllFeatures()
    {
        var types = Helper.GetTypes<IFeature>()
            .Select(_serviceProvider.GetRequiredService)
            .Cast<IFeature>();

        return types;
    }

    public async Task Goto(string? command)
    {
        await Task.Run(() =>
        {
            var feature = GetAllFeatures()
                .FirstOrDefault(x => x.Id == command);
            
            if (feature == null)
            {
                return;
            }

            feature.ExecuteAsync(null, default);
        });
    }
}