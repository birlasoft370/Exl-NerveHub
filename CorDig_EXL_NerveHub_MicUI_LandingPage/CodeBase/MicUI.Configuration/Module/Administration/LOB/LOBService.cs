using MicUI.Configuration.Helper;
using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Administration.LOB
{
    public class LOBService : ILOBService
    {
        private readonly IConfigApiService _configService;
        public LOBService( IConfigApiService configService)
        {
            _configService = configService;
        }
        public List<BELOBInfo> GetLOBList(string lobName,bool IsActiveLOB)
        {
            var result = _configService.GetLOBListAsync(lobName, IsActiveLOB).GetAwaiter().GetResult();
            return result.data??new List<BELOBInfo>();
        }
        public string UpdateData(LOBModel lOBModel)
        {
            var result = _configService.UpdateLOBAsync(lOBModel).GetAwaiter().GetResult();
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
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_LOBAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }

        public string InsertData(LOBModel lOBModel)
        {
            var result = _configService.AddLOBAsync(lOBModel).GetAwaiter().GetResult();
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
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_LOBAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
        public LOBModel GetLOBById(int LOBId)
        {
            var result = _configService.GetLOBByIdAsync(LOBId).GetAwaiter().GetResult();
            return result.data;

        }
    }
}
