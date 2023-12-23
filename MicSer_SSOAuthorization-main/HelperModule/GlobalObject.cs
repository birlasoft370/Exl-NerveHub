using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicSer.SSOAuthorization.HelperModule
{
    internal static class GlobalObject
    {
        internal static IConfiguration IConfigurationObject { get; set; }

        internal static string JWT_SECURITY_KEY
        {
            get
            {
                return GlobalObject.IConfigurationObject.GetSection("jwtTokenSecret:JWT_SECURITY_KEY").Value ?? "";
            }
        }

        internal static int JWT_TOKEN_VALIDITY_MINS
        {
            get
            {
                return Convert.ToInt32(GlobalObject.IConfigurationObject.GetSection("jwtTokenSecret:JWT_TOKEN_VALIDITY_MINS").Value ?? "10");
            }
        }

        internal static string RedisAPI
        {
            get
            {
                return GlobalObject.IConfigurationObject.GetSection("APIDetails:RedisAPI").Value ?? "";
            }
        }

        internal static string TenantAPI
        {
            get
            {
                return GlobalObject.IConfigurationObject.GetSection("APIDetails:TenantAPI").Value ?? "";
            }
        }
        

    }
}
