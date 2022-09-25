namespace Master5.Features;

public class HiddenFilesFolders : BaseFeature, IFeature
{
    public string Id => GetType().GUID.ToString()[..2];
    public string Name => GetType().Name;
    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {

        }, cancellationToken);
    }
}