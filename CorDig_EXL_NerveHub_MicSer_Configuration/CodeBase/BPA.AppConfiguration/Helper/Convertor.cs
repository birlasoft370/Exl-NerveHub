using System.Text.Json;

namespace BPA.AppConfiguration.Helper
{
    public static class Convertor
    {
        public static string IsNullThenEmpty(this string str)
        {
            return str ?? string.Empty;
        }

        internal static string Serialize<T>(T result)
        {
            return JsonSerializer.Serialize<T>(result);
        }

        internal static T Deserialize<T>(string result)
        {
            return JsonSerializer.Deserialize<T>(result);
        }
        internal static T DeserializeSpecial<T>(string result)
        {
            return JsonSerializer.Deserialize<T>(result.Replace("\"{","{").Replace("}\"", "}").Replace("\\", ""));
        }
    }
}
