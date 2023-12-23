using Microsoft.AspNetCore.Mvc;
using MicSer.SSOAuthorization.Constants;
using MicSer.SSOAuthorization.HelperModule;

namespace MicSer.SSOAuthorization.BaseController
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
            GlobalObject.IConfigurationObject = _configuration = configuration;
            _applicationName =  _configuration.GetSection(GlobalConst.ApplicationName)?.Value;
        }
    }
}
