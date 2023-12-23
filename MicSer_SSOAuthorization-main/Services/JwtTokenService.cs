
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MicSer.SSOAuthorization.Models;
using System.Threading.Tasks;
using MicSer.SSOAuthorization.HelperModule;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;

namespace MicSer.SSOAuthorization.Services
{
    public interface IJwtTokenService
    {
        Task<AuthenticationResponse> GenerateJwtToken(JWTInfo jWTInfo, string key = "");
    }

    public class JwtTokenService : IJwtTokenService
    {
        public async Task<AuthenticationResponse> GenerateJwtToken(JWTInfo userInfoRequest, string key = "")
        {
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(GlobalObject.JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(GlobalObject.JWT_SECURITY_KEY);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userInfoRequest.UserInfo.IsNullThenEmpty()),
                new Claim(ClaimTypes.Role,userInfoRequest.Role.IsNullThenEmpty()),
                new Claim("RoleID",Convert.ToString(userInfoRequest.RoleID).IsNullThenEmpty()),
                new Claim("ConString", userInfoRequest.ConString.IsNullThenEmpty()),
                new Claim("UserId", Convert.ToString( userInfoRequest.UserId).IsNullThenEmpty()),
                new Claim("EmpId", Convert.ToString(userInfoRequest.EmpId).IsNullThenEmpty()),
                new Claim("TimeZoneId", Convert.ToString(userInfoRequest.TimeZoneId).IsNullThenEmpty()),
                new Claim("TimeZoneName", userInfoRequest.TimeZoneName.IsNullThenEmpty()),
                new Claim("Language", userInfoRequest.Language.IsNullThenEmpty()),
                new Claim("ServerTimeZone", userInfoRequest.ServerTimeZone.IsNullThenEmpty()),
                new Claim(ClaimTypes.Email,userInfoRequest.Email.IsNullThenEmpty())
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                IssuedAt = DateTime.UtcNow,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            if (string.IsNullOrEmpty(key))
                token = StoreToken(token);
            else
                token = StoreTokenSameKey(token, key);

            return new AuthenticationResponse
            {
                UserName = userInfoRequest.UserInfo,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token
            };
        }

        private string StoreToken(string token)
        {

            var key = Convert.ToString(Guid.NewGuid());
            key += $".{Convert.ToString(Guid.NewGuid())}";
            key = key.Replace("-", string.Empty);
            return StoreTokenSameKey(token, key);
        }

        private string StoreTokenSameKey(string token, string key)
        {
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            cache.StringSet(key, token, TimeSpan.FromSeconds(GlobalObject.JWT_TOKEN_VALIDITY_MINS * 60));
            return key;
        }
    }
}
