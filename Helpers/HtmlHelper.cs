using System.Net;
using System.Text;

namespace Helpers;

public static class HtmlHelper
{
    public static string ToHtmlTable(Dictionary<string, string?> configValues)
    {
        var builder = new StringBuilder();
        builder.Append("<html>");
        builder.Append("<body>");
        builder.Append("<table border='1'>");
        builder.Append("<tr><th>Key</th><th>Value</th></tr>");

        foreach (var kvp in configValues)
        {
            builder.Append("<tr>");
            builder.Append($"<td style=\"padding:8px\"> {WebUtility.HtmlEncode(kvp.Key)} </td>");
            builder.Append($"<td style=\"padding:8px\"> {WebUtility.HtmlEncode(kvp.Value)} </td>");
            builder.Append("</tr>");
        }

        builder.Append("</table>");
        builder.Append("</body>");
        builder.Append("</html>");

        return builder.ToString();
    }

    public static string ToHtmlTable<T>(T configValues) where T : class
    {
        var dictionary = new Dictionary<string, string>();

        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(configValues);
            if (value != null)
            {
                dictionary.Add(property.Name, $"{value}");
            }
        }

        return ToHtmlTable(dictionary);
    }
}