using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Administration.Facility
{
    public class FacilityService : IFacilityService
    {
        private readonly IConfigApiService _configService;       
        public FacilityService( IConfigApiService configService)
        {            
            _configService = configService;
        }
        public List<BEFacility> GetFacilityList(string searchFacility, bool bActiveFacility)
        {
            var result = _configService.GetFacilityListAsync(searchFacility, bActiveFacility).GetAwaiter().GetResult();
            return result.data??new List<BEFacility>();
        }
        public FacilityModel GetFacilityById(int searchFacility)
        {
            var result = _configService.GetFacilityByIdAsync(searchFacility).GetAwaiter().GetResult();
            return result.data;
        }
        public SelectList GetLocations()
        {
            var result = _configService.GetLocationsAsync().GetAwaiter().GetResult();
            return result.data;
        }
        public string InsertData(FacilityModel facilityModel)
        {
            var result = _configService.AddFacilityAsync(facilityModel).GetAwaiter().GetResult();
            if (result != null && result.status)
            {
                return result.data;
            }
            else
            {
                
                if (result.message.Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.Contains(GlobalConstant.ExlSpecificClientAlreadyDefined))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_FacilityAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
        public string UpdateData(FacilityModel facilityModel)
        {
            var result = _configService.UpdateFacilityAsync(facilityModel).GetAwaiter().GetResult();
            if (result != null && result.status)
            {
                return result.data;
            }
            else
            {
              
                if (result.message.Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.Contains(GlobalConstant.ExlSpecificClientAlreadyDefined))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_FacilityAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
    }
}

    

