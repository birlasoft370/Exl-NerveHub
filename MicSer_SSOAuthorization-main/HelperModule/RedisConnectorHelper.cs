using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicSer.SSOAuthorization.HelperModule
{
    class RedisConnectorHelper
    {
        static RedisConnectorHelper()
        {
            RedisConnectorHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                string HostAndPort = GlobalObject.RedisAPI;

                var configurationOptions = new ConfigurationOptions {  };
                configurationOptions.EndPoints.Add(HostAndPort);
                configurationOptions.AbortOnConnectFail = false;

                return ConnectionMultiplexer.Connect(configurationOptions);
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

    }
}
