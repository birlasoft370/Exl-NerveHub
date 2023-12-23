using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Mvc.Razor;
using MicUI.NerveHub.Module.Security;


var builder = WebApplication.CreateBuilder(args);

// Add framework services.
builder.Services
	.AddControllersWithViews();
	builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Add Kendo UI services to the services container
builder.Services.AddKendo();

// Add framework services.
builder.Services
    .AddControllersWithViews();
builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Add Kendo UI services to the services container
builder.Services.AddKendo();

// Add services to the container.

builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSession();
builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages();
builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddHttpClient("AuthenticatioUser", config => config.BaseAddress = new System.Uri("https://localhost:7220/"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
var supportedCulture = new[] { "en", "es", "pl","da" };
var localizationOption = new RequestLocalizationOptions().SetDefaultCulture(supportedCulture[0])
    .AddSupportedCultures(supportedCulture)
    .AddSupportedUICultures(supportedCulture);
app.UseRequestLocalization(localizationOption);
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
               name: "default", // Route name
               pattern: "{controller=Home}/{action=Index}/{id?}"
             
              );
app.MapControllerRoute(
               name: "option2", // Route name
               pattern: "{tanentName?}/{controller=Home}/{action=Index}/{id?}"
              
              );



app.Run();
