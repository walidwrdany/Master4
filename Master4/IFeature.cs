namespace Master4;

public interface IFeature
{
    public string Id { get; }
    public string Name { get; }

    public Task Execute();
}