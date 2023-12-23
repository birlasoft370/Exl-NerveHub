using MicUI.EmailManagement.Helper.Sessions;
using MicUI.EmailManagement.Module.Common;
using MicUI.EmailManagement.Module.MailConfiguration;

namespace MicUI.EmailManagement.Module.ServiceRegistrationExtension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ServiceCollectionExtension(this IServiceCollection services)
        {
            services.AddScoped<IGetSetSessionValues, GetSetSessionValues>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IProcessService, ProcessService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddTransient<IMailConfigurationService, MailConfigurationService>();
            services.AddTransient<IMailTemplateService, MailTemplateService>();
            services.AddTransient<ITimeZoneService, TimeZoneService>();
            services.AddTransient<ILanguagesService, LanguagesService>();
            services.AddTransient<IWorkObjectService, WorkObjectService>();
            return services;
        }
    }
}
