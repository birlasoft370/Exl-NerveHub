namespace MicUI.WorkManagement.Helper
{
    //Reference :-https://stackoverflow.com/questions/43526630/how-can-i-get-the-baseurl-of-my-site-in-asp-net-core
    public class MyHttpContext
    {
        private static IHttpContextAccessor m_httpContextAccessor;

        public static HttpContext Current => m_httpContextAccessor.HttpContext;
        public static string relativeUrl = "/";
        public static string AppBaseUrl => $"{Current.Request.Scheme}://{Current.Request.Host.Value}{Current.Request.PathBase}{relativeUrl}";

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            m_httpContextAccessor = contextAccessor;
        }
    }

    public static class HttpContextExtensions
    {
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseHttpContext(this IApplicationBuilder app)
        {
            MyHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            return app;
        }
    }
}
