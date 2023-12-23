using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MicSer.Tenant.Constants;

namespace MicSer.Tenant.BaseController
{
    public class BaseController<T> : ControllerBase
    {
        internal readonly ILogger<T> _logger;
        internal readonly IWebHostEnvironment _env;
        internal readonly IConfiguration _configuration;
        internal readonly string _applicationName;
        public BaseController(ILogger<T> logger, IWebHostEnvironment env, IConfiguration configuration)
        {
            _logger = logger;
            _env = env;
            _configuration = configuration;
            _applicationName =  _configuration.GetSection(GlobalConst.ApplicationName)?.Value;
        }
    }
}
