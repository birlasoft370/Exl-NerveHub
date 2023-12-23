using MicUI.Configuration.Helper.Sessions;

namespace MicUI.Configuration.Services
{
    public interface ICreateHeaderInfo
    {
        void CreateHeaderInfo(HttpClient _client, IHttpContextAccessor _httpContextAccessor);
    }

    public abstract class BaseApiService : ICreateHeaderInfo
    {
        protected readonly HttpClient _client;
        private IHttpContextAccessor _httpContextAccessor;
        public BaseApiService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            CreateHeaderInfo(_client, _httpContextAccessor);
        }
        public void CreateHeaderInfo(HttpClient _client, IHttpContextAccessor _httpContextAccessor)
        {
            if (_httpContextAccessor is not null)
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_httpContextAccessor.HttpContext.Session.GetString(SessionVariables.SessionKeyUserToken)}");
        }

        internal string SessionLoginName
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session.GetString(SessionVariables.SessionKeyLoginName);
            }
        }      
    }
}
