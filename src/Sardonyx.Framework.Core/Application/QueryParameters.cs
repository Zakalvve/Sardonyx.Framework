namespace Sardonyx.Framework.Core.Application
{
    public interface IQueryParameters
    {
        string Query();
    }

    /// <summary>
    /// Base class for parameter models. Exposes access to <see cref="Query" /> which creates a query string based on the properties of the class that implements this class.
    /// </summary>
    public abstract class QueryParameters : IQueryParameters
    {
        public string Query()
        {
            var properties = GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            var arrayProperties = GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(List<string>));

            var query = properties.Aggregate("?",
                (acc, cur) =>
                {
                    var val = cur.GetValue(this) as string;
                    bool shouldAppend = !string.IsNullOrWhiteSpace(val);
                    return shouldAppend ? acc + $"{cur.Name}={cur.GetValue(this)}&" : acc;
                })
                .TrimEnd('&');

            query = query + arrayProperties.Aggregate("&",
                (acc, cur) =>
                {
                    var array = cur.GetValue(this) as List<string> ?? new List<string>();
                    var result = string.Empty;
                    foreach (var item in array)
                    {
                        bool shouldAppend = !string.IsNullOrEmpty(item);
                        if (shouldAppend)
                        {
                            result += $"{cur.Name}[]={item}&";
                        }
                    }

                    return acc + result;
                })
                .TrimEnd('&');

            return query == "?" ? string.Empty : query;
        }
    }
}
