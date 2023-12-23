using MicUI.Configuration.Helper;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Services.Configuration;

namespace MicUI.Configuration.Module.Administration.MasterValue
{
    public interface IMasterValueService
    {
        MasterValueModel GetMasterDetails(int iFieldID);
        List<MasterValueModel> GetMasterList(string masterValueSearchName, bool iFieldID);
        string InsertData(MasterValueModel oMasterType, int iFormID);
        string UpdateData(MasterValueModel oMasterType, int iFormID);
    }
    public class MasterValueService : IMasterValueService
    {
        private readonly IConfigApiService _configApiService;

        public MasterValueService(IConfigApiService configApiServiceApiService)
        {
            _configApiService = configApiServiceApiService;
        }
        public MasterValueModel GetMasterDetails(int iFieldID)
        {
            var result = _configApiService.GetMasterDetailAsync(iFieldID).GetAwaiter().GetResult();

            return result.data;
        }

        public List<MasterValueModel> GetMasterList(string masterValueSearchName, bool iFieldID)
        {
            var result = _configApiService.GetMasterListAsync(masterValueSearchName).GetAwaiter().GetResult();

            return result.data;
        }

        public string? InsertData(MasterValueModel oMasterType, int iFormID)
        {
            var result = _configApiService.AddMasterTypeValueAsync(oMasterType).GetAwaiter().GetResult();

            if (result != null && result.status)
            {
                return result.data;
            }
            else
            {
                //if (ex.Number == 547)
                //{
                //    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                //}
                if (result.message.Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.Contains(GlobalConstant.ExlSpecificClientAlreadyDefined))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_ClientAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }

        public string? UpdateData(MasterValueModel oMasterType, int iFormID)
        {
            var result = _configApiService.UpdateMasterTypeValueAsync(oMasterType).GetAwaiter().GetResult();
            if (result != null && result.status)
            {
                return result.data;
            }
            else
            {
                //if (ex.Number == 547)
                //{
                //    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                //}
                if (result.message.Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.Contains(GlobalConstant.ExlSpecificClientAlreadyDefined))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_ClientAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }

    }
}
