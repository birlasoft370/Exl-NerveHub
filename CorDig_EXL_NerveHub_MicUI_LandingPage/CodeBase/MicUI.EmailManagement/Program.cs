using Microsoft.AspNetCore.Authentication.Negotiate;
using MicUI.EmailManagement.Helper;
using MicUI.EmailManagement.Helper.Extensions;
using MicUI.EmailManagement.Module.Authentication;
using MicUI.EmailManagement.Module.Logger;
using MicUI.EmailManagement.Module.Menus;
using MicUI.EmailManagement.Module.ServiceRegistrationExtension;
using MicUI.EmailManagement.Services.Authentication;
using MicUI.EmailManagement.Services.Configuration;
using MicUI.EmailManagement.Services.Logger;
using MicUI.EmailManagement.Services.MailConfiguration;
using MicUI.EmailManagement.Services.ModuleMenus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);//.AddRazorRuntimeCompilation();
builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Add Kendo UI services to the services container
builder.Services.AddKendo();
//builder.Services.AddUnobtrusiveAjax(useCdn: true, injectScriptIfNeeded: false);

builder.Services.AddSession();

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
builder.Services.AddHttpClient<IMailConfigurationApiService, MailConfigurationApiService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("MailConfigurationService"));
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
//if (!app.Environment.IsDevelopment())
//{
app.UseDeveloperExceptionPage();
app.UseExceptionHandler("/Home/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
//}
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

app.UseHttpsRedirection();
app.UseHttpContext();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=GenerateToken}/{id?}");

app.Run();
