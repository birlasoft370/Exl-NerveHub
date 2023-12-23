using MicUI.WorkManagement.Helper.Sessions;
using MicUI.WorkManagement.Module.Common;
using MicUI.WorkManagement.Module.WorkManagement.WorkMaster;
using MicUI.WorkManagement.Module.Administration;

namespace MicUI.WorkManagement.Module.ServiceRegistrationExtension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ServiceCollectionExtension(this IServiceCollection services)
        {
            services.AddScoped<IGetSetSessionValues, GetSetSessionValues>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IProcessService, ProcessService>();
            services.AddTransient<ICampaignService, CampaignService>();
            services.AddTransient<ILanguagesService, LanguagesService>();
            services.AddTransient<IWorkObjectService, WorkObjectService>();
            services.AddTransient<IControlTypeService, ControlTypeService>(); 
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IDocDetailService, DocDetailService>();
            services.AddTransient<IAgentDashBoardService, AgentDashBoardService>();
            services.AddTransient<IWorkLayoutService, WorkLayoutService>();
            
            services.AddScoped<IHolidayCalenderMaintenanceService, HolidayCalenderMaintenanceService>();
            services.AddScoped<ICalenderService, CalenderService>();
            services.AddScoped<IShiftService, ShiftService>();
            services.AddScoped<ITerminationCodeService, TerminationCodeService>();
            services.AddScoped<IBreakService, BreakService>();
            return services;
        }
    }
}
