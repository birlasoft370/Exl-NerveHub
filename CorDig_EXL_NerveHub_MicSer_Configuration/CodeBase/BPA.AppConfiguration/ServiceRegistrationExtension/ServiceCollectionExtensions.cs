using BPA.AppConfig.BusinessLayer.Config;
using BPA.AppConfig.BusinessLayer.ExternalRef;
using BPA.AppConfig.BusinessLayer.ExternalRef.BIReports;
using BPA.AppConfig.BusinessLayer.ExternalRef.Configuration;
using BPA.AppConfig.BusinessLayer.ExternalRef.Security;
using BPA.AppConfig.BusinessLayer.ExternalRef.SharePointConfiguration;
using BPA.AppConfig.BusinessLayer.ExternalRef.WorkAllocation;
using BPA.AppConfig.BusinessLayer.Security;
using BPA.AppConfig.Datalayer.ExternalRef;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.BIReports;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Security;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.SharePointConfiguration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Security;
using Microsoft.OpenApi.Models;

namespace BPA.AppConfiguration.ServiceRegistrationExtension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ServiceCollectionExtension(this IServiceCollection services)
        {
            #region SwaggerGen

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "You api title", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,// "header",
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey// "apiKey"
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
                });

            });

            #endregion

            services.AddTransient<IVerticalService, BLVertical>();
            services.AddTransient<ISBUInfoService, BLSBUInfo>();
            services.AddTransient<IClientService, BLClient>();
            services.AddTransient<ISubProcessService, BLSubProcess>();
            services.AddTransient<IProcessService, BLProcess>();
            services.AddTransient<ICalenderService, BLCalendar>();
            services.AddTransient<IMasterTableService, BLMasterTable>();
            services.AddTransient<ILocationService, BLLocation>();
            services.AddTransient<ITimeZoneService, BLTimeZone>();
            services.AddTransient<ICampaignService, BLCampaign>();
            services.AddTransient<ISkillService, BLSkill>();
            services.AddTransient<IFacilityService, BLFacility>();
            services.AddTransient<ILOBService, BLLOB>();
            services.AddTransient<IShiftService, BLShift>();
            services.AddTransient<IProcessShiftService, BLProcessShift>();
            services.AddTransient<ILanguagesService, BLLanguages>();
            services.AddTransient<IProcessShiftWindowService, BLProcessShiftWindow>();
            services.AddTransient<IWatReport, BLWATReport>();
            services.AddTransient<ISharePointConfiguration, BLSharePointConfiguration>();
            services.AddTransient<IBreakService, BLBreak>();
            services.AddTransient<ITerminationCodeService, BLTerminationCode>();
            services.AddTransient<ITeamService, BLTeam>();
            services.AddTransient<IPermissionService, BLPermission>();
            services.AddTransient<IProcessOffService, BLProcessOff>();
            services.AddTransient<ICampTermCodeMappingService, BLCampTermCodeMapping>();
            services.AddTransient<IStoreService, BLStore>();
            services.AddTransient<IUserPreferenceService, BLUserPreference>();
            services.AddTransient<IAuthorizationUserService, BLAuthorization>();
            services.AddTransient<IWorkObjectService, BLWorkObject>();
            services.AddTransient<IProcessBreakMappingService, BLProcessBreakMapping>();
            return services;
        }
    }
}
