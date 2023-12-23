using MicUI.Configuration.Helper.Sessions;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Services.Logger
{
    public interface ILoggerApiService
    {
        Task<string> SaveLogDetailsAsync(LoggerInfo loggerInfo);
    }

    public class LoggerApiService : ILoggerApiService
    {
        private readonly HttpClient _client;

        public LoggerApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> SaveLogDetailsAsync(LoggerInfo loggerInfo)
        {
            var response = await _client.PostAsJsonAsync("Log/SaveLogDetails", loggerInfo);
            return await response.Content.ReadAsStringAsync() ?? "";
        }
    }

    public class LoggerAuthenticationHandler : DelegatingHandler
    {
        private readonly IGetSetSessionValues _getSetSessionValues;

        public LoggerAuthenticationHandler(IGetSetSessionValues getSetSessionValues)
        {
            _getSetSessionValues = getSetSessionValues;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Authorization", $"Bearer {_getSetSessionValues.GetUserToken()}");
            var result = await base.SendAsync(request, cancellationToken);
            return result;
        }
    }
}
