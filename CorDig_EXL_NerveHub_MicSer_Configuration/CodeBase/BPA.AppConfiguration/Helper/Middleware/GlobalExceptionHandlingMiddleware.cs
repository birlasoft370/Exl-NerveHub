using BPA.AppConfiguration.Constants;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Response;

namespace BPA.AppConfiguration.Helper.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            /* catch (BearerNotFoundException)
             {
                 throw;
             }*/
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, _logger, _httpClientFactory);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger _logger, IHttpClientFactory _httpClientFactory)
        {
            var result = new MessageResponse<string>();
            string actionName = context.Request.RouteValues["action"].ToString();
            var correlationId = Common.GetValue<dynamic>(context, GlobalConst.CorrelationIdHeader);// Read from Base Controller

            var exceptionType = ex.GetType();

            //if (exceptionType == typeof(BearerNotFoundException))
            //{
            //    throw new Exception("No bearer found");
            //}
            if (ex.Message.Contains("No bearer found"))//else
            {
                throw new Exception("No bearer found");
            }
            else if (ex.Message.Contains("Invalid token"))
            {
                throw new Exception("Invalid token");
            }
            else if (ex.GetType() == typeof(System.Data.SqlClient.SqlException))
            {
                System.Data.SqlClient.SqlException exception = (System.Data.SqlClient.SqlException)ex;
                if (exception.Number == 547)
                {
                    result.Message = "ReferenceConstraint";
                }
                else if (exception.Number == 2627)
                {
                    result.Message = "UniqueConstraints";
                }
                else
                {
                    result.Message = _logger.Error(new LoggerInfo() { actionName = actionName, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = correlationId });
                }
            }
            else
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = actionName, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = correlationId });
            }

            var task = Task.Run(() => CallLoggerApi(result, _httpClientFactory));

            //Final Response
            await context.Response.WriteJson(result, "application/json");

        }

        private async Task CallLoggerApi(MessageResponse<string> result, IHttpClientFactory _httpClientFactory)
        {
            var client = _httpClientFactory.CreateClient("loggerApiClient");
            var response = await client.PostAsJsonAsync($"WeatherForecast/InsertLogger", result);
            /*
            using (HttpClient httpClient = new())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "Url");
                request.Content = new StringContent(JsonConvert.SerializeObject(result));
                var response = await httpClient.SendAsync(request);
            }*/
        }
    }

}
