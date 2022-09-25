namespace Master5;

public interface IFeature
{
    string Id { get; }
    string Name { get; }
    Task ExecuteAsync(string[] args, CancellationToken cancellationToken);
}