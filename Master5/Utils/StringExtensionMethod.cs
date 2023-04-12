using System.Text;
using System.Text.RegularExpressions;

namespace Master5.Utils;

public static class StringExtensionMethod
{
    public static string AddSpacesToSentence(this string value)
    {
        return Regex.Replace(value, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
    }

    public static string CSharpName(this Type type)
    {
        var name = type.Name;
        if (!type.IsGenericType) return name;

        var sb = new StringBuilder();
        sb.Append(name[..name.IndexOf('`')]);
        sb.Append('<');
        sb.Append(string.Join(", ", type.GetGenericArguments()
            .Select(t => t.CSharpName())));
        sb.Append('>');
        return sb.ToString();
    }
}