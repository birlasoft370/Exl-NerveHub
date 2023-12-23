using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MicSer.SSOAuthorization.HelperModule;
using MicSer.SSOAuthorization.Models;
using MicSer.SSOAuthorization.Services;
using MicSer.Tenant.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicSer.Tenant.Filters
{
    public class JwtAuthentication
    {
        private static readonly string _bearer = "bearer ";

        public JWTInfo GetJwtAuthentication(Microsoft.AspNetCore.Http.HttpContext context)
        {
            //if (!context.Request.IsHttps)
            //throw new Exception("ForbidResult");

            var request = context.Request;

            Microsoft.Extensions.Primitives.StringValues headerContent;
            request.Headers.TryGetValue("Authorization", out headerContent);

            if (headerContent.Count == 1)
            {
                var bearer = headerContent[0].Substring(0, _bearer.Length);

                if (bearer.ToLower().Contains(_bearer))
                {
                    var token = headerContent[0].Substring(_bearer.Length);

                    var key = token;

                    token = GetStore(token);

                    var jWTInfo = AuthenticateJwtToken(token).Result;

                    if (jWTInfo == null || jWTInfo.EmpId == null)
                    {
                        //_logger.Trace("JWTAuthenticate: Invalid Jwt token");
                        throw new Exception("Invalid token");
                    }

                    //context.Items.Add("jWTInfo", jWTInfo);

                    ReGenerateToken(jWTInfo, key);
                    return jWTInfo;
                }

            }

            throw new Exception("No bearer found");
        }

        private void ReGenerateToken(JWTInfo jWTInfo, string key)
        {
            IJwtTokenService iJwtTokenService = new JwtTokenService();
            iJwtTokenService.GenerateJwtToken(jWTInfo, key);
        }

        private string GetStore(string token)
        {
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            return cache.StringGet(token);
        }

        protected Task<JWTInfo> AuthenticateJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var validations = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(GlobalObject.JWT_SECURITY_KEY)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            var stringJson = tokenSecure.ToString();

            stringJson = stringJson.Replace("{{", string.Empty).Replace("}}", string.Empty);

            stringJson = stringJson.Replace("{\"alg\":\"HS256\",\"typ\":\"JWT\"}.", string.Empty);
            var jWTInfo = Convertor.Deserialize<JWTInfo>(stringJson);

            return Task.FromResult(jWTInfo);
        }



        //public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(0);
        //}
    }
}
