namespace InvertApi.Extensions;

public static class StringExtensions
{
    public static string FromBase64(this string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}