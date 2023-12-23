using MicUI.WorkManagement.Helper;
using MicUI.WorkManagement.Services.Configuration;
using MicUI.WorkManagement.Services.ServiceModel;
using MicUI.WorkManagement.Services.WorkManagement;
using Newtonsoft.Json;
using System.Data;

namespace MicUI.WorkManagement.Module.WorkManagement.WorkMaster
{
    public interface IWorkObjectService
    {
        List<BEWorkObject> GetObjectList(int StoreId, bool bGetAll);
        List<BEWorkObjectChoice> GetObjectChoiceList(int iObjectID);
        List<BEValidations> GetValidations();
        List<BETechBuisness> GetBusinessList(int iprocessid);
        int CheckUserIsSuperOrFunctionalAdmin();
        List<GetPendingWorkObjectApprovalModel> GetPendingWorkObjectApproval(DateTime startDate, DateTime endDate);
        GetWorkObjRequestDetails FetchWorkObjRequestDetails(int iApprovalId);
        void InsertObjectRequestChange(int iApprovalId, int iUserLevel, string sChangeRequest);
        void UpdateObjectRequestChange(BEWorkObjectApprover oWorkObject);
        List<BEObjectformula> GetWorkObjectFormulaSearch(Int32 filterId);
        List<GetWorkObjectForControlNameModel> GetWorkObjectForControlName(int CampaignID);
        List<BEFormulaData> GetFilterList_Formula(int iCampaignId, string sFilterName, bool bActiveFilter);
        List<GetWorkObjectForFormulaModel> GetWorkObjectForFormula(int CampaignID);
        bool GetExistObjFormula(BEObjectformula bEObjectformula);
        void InsertObjectformula(ObjectformulaModel objectformula);
        int DeleteObjectFormula(int FormulaId);
        DataSet GetGridConfigurationData(int iStoreID);
        DataSet GetAllLinkCampaignData(int SCampaignId);
        int GridInserUpdateData(LinkCampaignGridConfigurationModel objBEGridConfiguration, int userid, string Action);
    }

    public class WorkObjectService : IWorkObjectService
    {
        private readonly IWorkManagementApiService _workManagementApiService;
        private readonly IConfigApiService _configService;

        public WorkObjectService(IWorkManagementApiService workManagementApiService, IConfigApiService configService)
        {
            _workManagementApiService = workManagementApiService;
            _configService = configService;
        }

        public List<BEWorkObject> GetObjectList(int StoreId, bool bGetAll)
        {
            var result = _workManagementApiService.GetObjectListAsync(StoreId, bGetAll).GetAwaiter().GetResult();
            return result.data ?? new List<BEWorkObject>();
        }
        public List<BEWorkObjectChoice> GetObjectChoiceList(int iObjectID)
        {
            var result = _workManagementApiService.GetObjectChoiceListAsync(iObjectID).GetAwaiter().GetResult();
            return result.data ?? new List<BEWorkObjectChoice>();
        }

        public List<BEValidations> GetValidations()
        {
            var result = _workManagementApiService.GetValidationsAsync().GetAwaiter().GetResult();
            return result.data ?? new List<BEValidations>();
        }
        public List<BETechBuisness> GetBusinessList(int iprocessid)
        {
            var result = _workManagementApiService.GetBusinessApproverListAsync(iprocessid).GetAwaiter().GetResult();
            return result.data ?? new List<BETechBuisness>();
        }
        public int CheckUserIsSuperOrFunctionalAdmin()
        {
            var result = _configService.GetCheckUserIsSuperOrFunctionalAdminAsync().GetAwaiter().GetResult();
            return result.data;
        }
        public List<GetPendingWorkObjectApprovalModel> GetPendingWorkObjectApproval(DateTime startDate, DateTime endDate)
        {
            var result = _workManagementApiService.GetPendingWorkObjectApprovalAsync(startDate, endDate).GetAwaiter().GetResult();
            return result.data ?? new List<GetPendingWorkObjectApprovalModel>();
        }
        public GetWorkObjRequestDetails FetchWorkObjRequestDetails(int iApprovalId)
        {
            var result = _workManagementApiService.FetchWorkObjRequestDetailsAsync(iApprovalId).GetAwaiter().GetResult();
            return result.data ?? new();
        }
        public void InsertObjectRequestChange(int iApprovalId, int iUserLevel, string sChangeRequest)
        {
            _workManagementApiService.InsertObjectRequestChangeAsync(iApprovalId, iUserLevel, sChangeRequest).GetAwaiter().GetResult();
        }
        public void UpdateObjectRequestChange(BEWorkObjectApprover oWorkObject)
        {
            _workManagementApiService.UpdateObjectRequestChangeAsync(oWorkObject).GetAwaiter().GetResult();
        }
        public List<BEObjectformula> GetWorkObjectFormulaSearch(Int32 filterId)
        {
            var result = _workManagementApiService.GetWorkObjectFormulaSearchAsync(filterId).GetAwaiter().GetResult();
            return result.data.ToList() ?? new List<BEObjectformula>();
        }
        public List<GetWorkObjectForControlNameModel> GetWorkObjectForControlName(int CampaignID)
        {
            var result = _workManagementApiService.GetWorkObjectForControlNameAsync(CampaignID).GetAwaiter().GetResult();
            return result.data ?? new List<GetWorkObjectForControlNameModel>();
        }
        public List<BEFormulaData> GetFilterList_Formula(int iCampaignId, string sFilterName, bool bActiveFilter)
        {
            var result = _workManagementApiService.GetFilterList_FormulaAsync(iCampaignId, sFilterName, bActiveFilter).GetAwaiter().GetResult();
            return result.data ?? new List<BEFormulaData>();
        }
        public List<GetWorkObjectForFormulaModel> GetWorkObjectForFormula(int CampaignID)
        {
            var result = _workManagementApiService.GetWorkObjectForFormulaAsync(CampaignID).GetAwaiter().GetResult();
            return result.data ?? new List<GetWorkObjectForFormulaModel>();
        }
        public bool GetExistObjFormula(BEObjectformula bEObjectformula)
        {
            var result = _workManagementApiService.GetExistObjFormulaAsync(bEObjectformula.iCampaignID, bEObjectformula.iDSObjID, bEObjectformula.sDObjEvent).GetAwaiter().GetResult();
            return result.data;
        }
        public void InsertObjectformula(ObjectformulaModel objectformula)
        {
            var result = _workManagementApiService.InsertObjectformulaAsync(objectformula).GetAwaiter().GetResult();

            if (result != null && result.message != null)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
        public int DeleteObjectFormula(int FormulaId)
        {
            var result = _workManagementApiService.DeleteObjectFormulaAsync(FormulaId).GetAwaiter().GetResult();
            return result.data;
        }
        public DataSet GetGridConfigurationData(int iStoreID)
        {
            var result = _workManagementApiService.GetGridConfigurationDataAsync(iStoreID).GetAwaiter().GetResult();
            if (result != null && result.data != null)
            {
                return JsonConvert.DeserializeObject<DataSet>(result.data) ?? new DataSet();
            }
            return new DataSet();

        }
        public DataSet GetAllLinkCampaignData(int SCampaignId)
        {
            var result = _workManagementApiService.GetAllLinkCampaignDataAsync(SCampaignId).GetAwaiter().GetResult();
            if (result != null && result.data != null)
            {
                return JsonConvert.DeserializeObject<DataSet>(result.data) ?? new DataSet();
            }
            return new DataSet();
        }
        public int GridInserUpdateData(LinkCampaignGridConfigurationModel objBEGridConfiguration, int userid, string Action)
        {
            var result = _workManagementApiService.GridInserUpdateDataAsync(objBEGridConfiguration, userid, Action).GetAwaiter().GetResult();
            return result.data;
        }
    }
}
