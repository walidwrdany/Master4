using System.Reflection;

namespace Master5.Utils;

public static class Helper
{
    public static List<Type> GetTypes<T>()
    {
        return Assembly.GetExecutingAssembly()
            .GetExportedTypes()
            .Where(x => x.GetInterfaces().Any(i => i == typeof(T)) && x is { IsClass: true, IsAbstract: false })
            .ToList();
    }
}