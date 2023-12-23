using MicUI.Configuration.Helper;
using MicUI.Configuration.Models;
using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.ProcessInfoSetup
{
    public interface ILocationService
    {
        List<BELocation> GetLocationList();
        List<LocationModel> GetLocationList(string searchLocation);
        string InsertData(LocationModel LocationModel);
        string UpdateData(LocationModel LocationModel);
    }

    public class LocationService : ILocationService
    {
        private readonly IConfigApiService _configService;
       

        public LocationService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public List<BELocation> GetLocationList()
        {
            var result = _configService.GetLocationListAsync().GetAwaiter().GetResult();
            return result.data;
        }
        public List<LocationModel> GetLocationList(string searchLocation)
        {
            var result = _configService.GetLocationListAsync(searchLocation).GetAwaiter().GetResult();
            return result.data;
        }
        public string InsertData(LocationModel LocationModel)
        {
            var result = _configService.AddLocationAsync(LocationModel).GetAwaiter().GetResult();
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
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_LocationAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
        public string UpdateData(LocationModel LocationModel)
        {
            var result = _configService.UpdateLocationAsync(LocationModel).GetAwaiter().GetResult();
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
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_LocationAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
    }
}
