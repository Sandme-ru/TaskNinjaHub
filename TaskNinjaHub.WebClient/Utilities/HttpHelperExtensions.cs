using System.Reflection;
using System.Runtime.Serialization;

namespace TaskNinjaHub.WebClient.Utilities;

public static class HttpHelperExtensions
{
    public static Dictionary<string, string?> ToPropertyDictionary(this object obj)
    {
        return obj
            .GetType()
            .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
            .Where(p => !p.GetType().IsAbstract && p.GetValue(obj) != null && p.GetValue(obj) != default && !Attribute.IsDefined(p, typeof(IgnoreDataMemberAttribute)))
            .ToDictionary
            (
                p => p.Name,
                p => $"{p.GetValue(obj)}"
            )!;
    }
}