using MicUI.WorkManagement.Helper;
using MicUI.WorkManagement.Module.Authentication;
using MicUI.WorkManagement.Module.Menus;
using MicUI.WorkManagement.Module.ServiceRegistrationExtension;
using MicUI.WorkManagement.Services.Authentication;
using MicUI.WorkManagement.Services.Configuration;
using MicUI.WorkManagement.Services.ModuleMenus;
using MicUI.WorkManagement.Services.WorkManagement;

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

builder.Services.AddHttpClient<IWorkManagementApiService, WorkManagementApiService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("WorkManagementService"));
    options.Timeout = TimeSpan.FromMinutes(10);
}); 


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.ServiceCollectionExtension();

//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
builder.Services.AddMvc().AddSessionStateTempDataProvider();
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Error/PageNotFound";
        await next();
    }
    else if (context.Response.StatusCode == 401)
    {
        context.Request.Path = "/Error/AccessDenied";
        await next();
    }
    else if (context.Response.StatusCode == 500)
    {
        context.Request.Path = "/Error/InternalServerError";
        await next();
    }
});

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
