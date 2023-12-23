using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MicSer.SSOAuthorization.BaseController;
using MicSer.SSOAuthorization.Constants;
using MicSer.SSOAuthorization.HelperModule;
using MicSer.SSOAuthorization.Models;
using MicSer.SSOAuthorization.Services;
using MicSer.Tenant.Filters;
using System;
using System.Threading.Tasks;

namespace MicSer.SSOAuthorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SSOValidationController : BaseController<SSOValidationController>
    {
        IAuthenticateService _authenticateService;
        IJwtTokenService _jwtTokenService;

        public SSOValidationController(IAuthenticateService authenticateService, IJwtTokenService jwtTokenService, ILogger<SSOValidationController> logger, IWebHostEnvironment env, IConfiguration configuration) : base(logger, env, configuration)
        {
            _authenticateService = authenticateService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost(Name = "SSOTokenValidation")]
        [JwtAuthenticationFilter]
        public async Task<ActionResult<string>> Post()
        {
            try
            {
                HttpContext.Items.TryGetValue("jWTInfo", out var jWTInfo);

                return $"{Convertor.Serialize<JWTInfo>(jWTInfo as JWTInfo)}";
            }
            catch (Exception ex)
            {
                return $"{ ex?.Message} { ex?.InnerException?.Message} { ex?.StackTrace}";
            }
        }

        [HttpPut(Name = "SSOTokenValidation2")]
        public async Task<ActionResult<string>> Post2()
        {
            try
            {
                var objJwtAuthentication = new JwtAuthentication();

                return $"{Convertor.Serialize<JWTInfo>(objJwtAuthentication.GetJwtAuthentication(HttpContext))}";
            }
            catch (Exception ex)
            {
                return $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}";
            }
        }
    }
}
