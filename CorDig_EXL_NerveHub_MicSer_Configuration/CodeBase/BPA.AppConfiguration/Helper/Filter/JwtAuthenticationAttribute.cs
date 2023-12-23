using Microsoft.AspNetCore.Mvc.Filters;
using BPA.AppConfiguration.Models;
using System.Net.Sockets;
using System.Net;

namespace BPA.AppConfiguration.Helper.Filter;

public class JwtAuthenticationAttribute : Attribute, IAsyncActionFilter
{
    private static readonly string _bearer = "bearer ";
    private static readonly string _token = "X-Token-Id";
    private static readonly string _tokenInfo = "X-Token-In";
    private static readonly string _ipAddress = "ipAddress";

    public bool AllowMultiple => false;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;

        Microsoft.Extensions.Primitives.StringValues headerContent;
        request.Headers.TryGetValue("Authorization", out headerContent);

        if (headerContent.Count == 1)
        {
            var bearer = headerContent[0].Substring(0, _bearer.Length);

            if (bearer.ToLower().Contains(_bearer))
            {
                var token = headerContent[0].Substring(_bearer.Length);
                request.HttpContext.Items.Add(_token, token);

                var TokenDetail = await CallAPI.HttpClientAsync<TokenDetail>(HttpMethod.Post, GlobalObject.URLValidateSSO, string.Empty, token);

                if (TokenDetail?.LoginName == null)
                {
                    //_logger.Trace("JWTAuthenticate: Invalid Jwt token");
                    throw new Exception("Invalid token");
                }

                request.HttpContext.Items.Add(_tokenInfo, TokenDetail);
                request.HttpContext.Items.Add(_ipAddress, GetIpAddress(context));

                await next();
                return;
            }

        }
        throw new Exception("No bearer found");

    }

    private static string GetIpAddress(ActionExecutingContext context)
    {
        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
        string clientIp = "";
        if (remoteIp != null)
        {
            clientIp = remoteIp.ToString();
        }
        return clientIp;
    }
}

