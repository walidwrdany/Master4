namespace Master5;

public interface IFeature : IFeatureBase
{
    Task ExecuteAsync(string[]? args, CancellationToken cancellationToken);
}

public interface IFeatureBase
{
    string Id { get; }
    string Name { get; }
}