using AspNetCore.Unobtrusive.Ajax;
using Microsoft.AspNetCore.Authentication.Negotiate;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Helper.Extensions;
using MicUI.Configuration.Module.Authentication;
using MicUI.Configuration.Module.Configuration.ClientInfoSetup;
using MicUI.Configuration.Module.Logger;
using MicUI.Configuration.Module.Menus;
using MicUI.Configuration.Module.ServiceRegistrationExtension;
using MicUI.Configuration.Services.Authentication;
using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.Logger;
using MicUI.Configuration.Services.Reports;
using MicUI.Configuration.Services.Security;

var builder = WebApplication.CreateBuilder(args);

// Add framework services.
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);//.AddRazorRuntimeCompilation();
builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Add Kendo UI services to the services container
builder.Services.AddKendo();
builder.Services.AddUnobtrusiveAjax(useCdn: true, injectScriptIfNeeded: false);

builder.Services.AddSession();
//(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(25);
//});

builder.Services.AddHttpClient<IAuthenticationApiService, AuthenticationApiService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("SSOAuthS"));
    options.Timeout = TimeSpan.FromMinutes(10);
});
builder.Services.AddTransient<IAuthenticationTokenService, AuthenticationTokenService>();


builder.Services.AddHttpClient<ISecurityApiService, SecurityApiService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("SecurityService"));
    options.Timeout = TimeSpan.FromMinutes(10);
});
builder.Services.AddTransient<IMenus, Menus>();


builder.Services.AddHttpClient<IConfigApiService, ConfigApiService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ConfigurationService"));
    options.Timeout = TimeSpan.FromMinutes(10);
});
builder.Services.AddTransient<IVerticalService, VerticalService>();

builder.Services.AddHttpClient<IReportsApiService, ReportsApiService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ReportsService"));
    options.Timeout = TimeSpan.FromMinutes(10);
});

builder.Services.AddTransient<ILoggerService, LoggerService>();
builder.Services.AddTransient<LoggerAuthenticationHandler>();
builder.Services.AddHttpClient<ILoggerApiService, LoggerApiService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("LoggerService"));
    options.Timeout = TimeSpan.FromMinutes(10);
}).AddHttpMessageHandler<LoggerAuthenticationHandler>();


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.ServiceCollectionExtension();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
builder.Services.AddMvc().AddSessionStateTempDataProvider();
// Add services to the container.
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Error/PageNotFound";
        await next();
    }
});

LoggerExtension.InitLogger(app.Services.GetService<ILoggerService>());

//app.UseStatusCodePagesWithRedirects("/Error/PageNotFound");
app.UseHttpContext();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


