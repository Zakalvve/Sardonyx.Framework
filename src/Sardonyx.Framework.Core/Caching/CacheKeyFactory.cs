using System.Reflection;

namespace Sardonyx.Framework.Core.Caching
{
    public static class CacheKeyFactory
    {
        public static string CreateKey(params object?[] values)
        {
            var keyParts = new List<string>();

            foreach (var value in values)
            {
                if (value is null)
                {
                    keyParts.Add("null");
                }
                else if (value.GetType().IsPrimitive || value is string || value is Guid)
                {
                    keyParts.Add(value.ToString());
                }
                else
                {
                    var props = value.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.CanRead && !p.IsDefined(typeof(CacheKeyIgnoreAttribute), inherit: true));

                    var prefixProps = props
                        .Where(p => p.IsDefined(typeof(CacheKeyPrefixAttribute), inherit: true));

                    var prefix = string.Join(":", prefixProps.Select(p => p.GetValue(value)?.ToString() ?? "null"));

                    if (!string.IsNullOrEmpty(prefix))
                    {
                        keyParts.Add(prefix);
                    }

                    props = props.Except(prefixProps)
                        .OrderBy(p => p.Name);

                    foreach (var prop in props)
                    {
                        var propValue = prop.GetValue(value);
                        keyParts.Add($"{prop.Name}={propValue?.ToString() ?? "null"}");
                    }
                }
            }

            return string.Join(":", keyParts);
        }
    }
}
