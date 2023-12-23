using System.Text.Json;

namespace MicSer.Tenant.HelperModule
{
    internal static class Convertor
    {
        internal static string Serialize<T>(T result)
        {
            return JsonSerializer.Serialize<T>(result);
        }
    }
}
