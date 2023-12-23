using MicUI.Configuration.Helper.Sessions;
using MicUI.Configuration.Module.Administration.Facility;
using MicUI.Configuration.Module.Administration.LOB;
using MicUI.Configuration.Module.Administration.MasterValue;
using MicUI.Configuration.Module.Administration.SkillMaster;
using MicUI.Configuration.Module.Configuration.CampaignInfoSetup;
using MicUI.Configuration.Module.Configuration.ClientInfoSetup;
using MicUI.Configuration.Module.Configuration.Languages;
using MicUI.Configuration.Module.Configuration.ProcessInfoSetup;
using MicUI.Configuration.Module.Configuration.SubProcess;
using MicUI.Configuration.Module.Configuration.TimeZone;
using MicUI.Configuration.Module.Reports;
using MicUI.Configuration.Module.Security;
using MicUI.Configuration.Module.Security.UserMaster;
using MicUI.Configuration.Module.UserPreference;

namespace MicUI.Configuration.Module.ServiceRegistrationExtension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ServiceCollectionExtension(this IServiceCollection services)
        {
            services.AddScoped<IGetSetSessionValues, GetSetSessionValues>();
            services.AddTransient<IUserPreferenceService, UserPreferenceService>();
            services.AddTransient<ITimeZoneService, TimeZoneService>();
            services.AddTransient<ILanguagesService, LanguagesService>();
            services.AddTransient<ISBUInfoService, SBUInfoService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<ICalenderService, CalenderService>();
            services.AddTransient<IMasterTableService, MasterTableService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IProcessService, ProcessService>();
            services.AddTransient<ILOBService, LOBService>();
            services.AddTransient<IMasterValueService, MasterValueService>();
            services.AddTransient<ISkillService, SkillService>();
            services.AddTransient<IFacilityService, FacilityService>();
            services.AddTransient<IProcessOwnerService, ProcessOwnerService>();
            services.AddTransient<ICampaignService, CampaignService>();
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IWorkObjectService, WorkObjectService>();
            services.AddTransient<ISubProcessService, SubProcessService>();
            services.AddTransient<IRolesService, RolesService>();
            services.AddTransient<IERPJobRoleMapService, ERPJobRoleMapService>();
            services.AddTransient<IERPJobUserRoleMapService, ERPJobUserRoleMapService>();
            services.AddTransient<IReportsService, ReportsService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IUserAccessRequestService, UserAccessRequestService>();
            
            return services;
        }
    }
}
