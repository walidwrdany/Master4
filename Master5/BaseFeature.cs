using Master5.Utils;

namespace Master5;

public abstract class BaseFeature : IFeature
{
    public virtual string Id => GetType().GUID.ToString()[..2];
    public virtual string Name => GetType().Name.AddSpacesToSentence();
    public virtual async Task ExecuteAsync(string[]? args, CancellationToken cancellationToken)
    {
        await Task.Run(() => Console.WriteLine("this is {0} Base Execution", GetType().Name), cancellationToken);
    }


    public override string ToString()
    {
        return $" {new string(' ', 3 - Id.Length)} {Id} | {Name}";
    }
}