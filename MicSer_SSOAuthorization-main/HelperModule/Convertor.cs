using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicSer.SSOAuthorization.HelperModule
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
    }
}
