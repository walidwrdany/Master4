namespace Master5;

public interface IFeatureFactory
{
    IFeature GetFeature<T>() where T : IFeature;
    IEnumerable<IFeature> GetAllFeatures();
    Task Goto(ConsoleKeyInfo key);
}