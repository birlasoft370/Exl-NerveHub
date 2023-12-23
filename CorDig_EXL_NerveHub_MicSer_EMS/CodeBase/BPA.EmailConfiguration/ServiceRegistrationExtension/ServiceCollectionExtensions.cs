using BPA.EmailConfiguration.Controllers;
using BPA.EmailManagement.BusinessLayer;
using BPA.EmailManagement.BusinessLayer.ExternalRef.Config;
using BPA.EmailManagement.ServiceContract.ExternalRef.Config;
using BPA.EmailManagement.ServiceContract.ServiceContracts;
using Microsoft.OpenApi.Models;

namespace BPA.EmailConfiguration.ServiceRegistrations
{
    /*
    public class CustomHeaderSwaggerAttribute : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Correlation-Id",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }

    }
     c.OperationFilter<CustomHeaderSwaggerAttribute>();
     */

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

            services.AddTransient<IMailConfigurationService, BLMailConfiguration>();
            services.AddTransient<IMailTemplateService, BLMailTemplate>();
            services.AddTransient<IMailServiceGraph, BLEmailFactoryGraph>();
            services.AddTransient<IMailService, BLEmailFactory>();
            services.AddTransient<ILanguagesService, BLLanguages>();
            services.AddTransient<IMailServicTest, BLEmailFactoryTest>();
            return services;
        }
    }
}
