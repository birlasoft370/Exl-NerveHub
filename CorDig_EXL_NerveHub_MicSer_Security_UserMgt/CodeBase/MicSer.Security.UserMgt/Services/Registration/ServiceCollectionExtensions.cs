using BPA.Security.BusinessLayer;
using BPA.Security.BusinessLayer.ExternalRef;
using BPA.Security.BusinessLayer.Security;
using BPA.Security.ServiceContract;
using BPA.Security.ServiceContract.ExternalRef;
using BPA.Security.ServiceContracts.Security;
using Microsoft.OpenApi.Models;

namespace MicSer.Security.UserMgt.Services.Registration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGeneratorService(this IServiceCollection services)
    {

        #region SwaggerGen

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MicSer.Security.UserMgt", Version = "v1" });
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

        services.AddScoped<IMenuService, BLMenu>();
        services.AddScoped<IRolesService, BLRoles>();
        services.AddScoped<IFacilityService, BLFacility>();
        services.AddScoped<ILOBService, BLLOB>();
        services.AddScoped<ISBUInfoService, BLSBUInfo>();
        services.AddScoped<IPermissionService, BLPermission>();
        services.AddScoped<IClientService, BLClient>();
        services.AddScoped<ICampaignService, BLCampaign>();
        services.AddScoped<IProcessService, BLProcess>();
        services.AddScoped<IAuthenticateService, BLAuthenticate>();
        services.AddScoped<IUserAccessRequestService, BLUserAccessRequest>();
        services.AddScoped<IERPJobRoleMapService, BLERPJobRoleMap>();
        services.AddScoped<IMasterTableService, BLMasterTable>();


        return services;
    }

}