using MicUI.Configuration.Helper;
using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.Security;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.CampaignInfoSetup
{
    public interface ICampaignService
    {
        bool CheckPermission();
        List<BECampaignInfo> GetProcessWiseCampaignList(int iProcessID);
        List<BEUserInfo> GetUserApproverListByProcess(int ProcessId);
        void InsertData(CampaignModel ocampaign);
        void UpdateData(CampaignModel ocampaign);
        List<Services.ServiceModel.CampaignApproval> GetPandingCampaignApproval(DateTime dFrom, DateTime dTo);
        string ApprovalAction(int approvalId, string action);
        CampaignRequestDetails FetchCampaignRequestDetails(int level, int iApprovalId);
        void InsertCampaignRequestChange(int iApprovalId, int iUserLevel, string sChangeRequest);
        void UpdateCampaignRequestChange(CampaignRequestChange oCampaign);
        List<BECampaignInfo> GetCampaignList(int iProcessID, string sCampaignName, bool bActiveCampaign);
        BECampaignInfo GetCampaignByCampaignId(int iCampaignID);
        List<BECampaignInfo> GetCampaignAccessList(int iAgentID, bool bActiveCampaign);
    }


    public class CampaignService : ICampaignService
    {
        private readonly IConfigApiService _configService;
        private readonly ISecurityApiService _securityService;
        public CampaignService(IConfigApiService configService, ISecurityApiService securityService)
        {
            _configService = configService;
            _securityService = securityService;
        }

        public bool CheckPermission()
        {
            var result = _configService.GetCheckPermission().GetAwaiter().GetResult();
            return result.data;
        }
        public List<BECampaignInfo> GetProcessWiseCampaignList(int iProcessID)
        {
            var result = _configService.GetProcessWiseCampaignListAsync(iProcessID).GetAwaiter().GetResult();
            return result.data;
        }
        public List<BEUserInfo> GetUserApproverListByProcess(int ProcessId)
        {
            var result = _configService.GetBusinessApproverListAsync(ProcessId).GetAwaiter().GetResult();
            return result.data;
        }
        public void InsertData(CampaignModel ocampaign)
        {
            var result = _configService.AddCampaignAsync(ocampaign).GetAwaiter().GetResult();
            if (!result.status)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.msg_CampaignAlready.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_CampaignAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
        public void UpdateData(CampaignModel ocampaign)
        {
            var result = _configService.UpdateCampaignAsync(ocampaign).GetAwaiter().GetResult();
            if (!result.status)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.msg_CampaignAlready.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_CampaignAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
        public List<Services.ServiceModel.CampaignApproval> GetPandingCampaignApproval(DateTime dFrom, DateTime dTo)
        {
            var result = _configService.GetPendingCampaignApprovalListAsync(dFrom, dTo).GetAwaiter().GetResult();
            return result.data;
        }
        public string ApprovalAction(int approvalId, string action)
        {
            var result = _configService.ApprovalActionAsync(approvalId, action).GetAwaiter().GetResult();
            return result.data;
        }
        public CampaignRequestDetails FetchCampaignRequestDetails(int level, int iApprovalId)
        {
            var result = _configService.FetchCampaignRequestDetailsAsync(level, iApprovalId).GetAwaiter().GetResult();
            return result.data;
        }
        public void InsertCampaignRequestChange(int iApprovalId, int iUserLevel, string sChangeRequest)
        {
            var result = _configService.AddCampaignRequestChangeAsync(iUserLevel, iApprovalId, sChangeRequest).GetAwaiter().GetResult();
            if (!result.status)
            {
                throw new Exception(result.message);
            }
        }
        public void UpdateCampaignRequestChange(CampaignRequestChange oCampaign)
        {
            var result = _configService.UpdateCampaignRequestChangeAsync(oCampaign).GetAwaiter().GetResult();
            if (!result.status)
            {
                throw new Exception(result.message);
            }
        }
        public List<BECampaignInfo> GetCampaignList(int iProcessID, string sCampaignName, bool bActiveCampaign)
        {
            var result = _configService.GetCampaignListAsync(iProcessID, sCampaignName, bActiveCampaign).GetAwaiter().GetResult();
            return result.data;
        }
        public BECampaignInfo GetCampaignByCampaignId(int iCampaignID)
        {
            var result = _configService.GetCampaignByIdAsync(iCampaignID).GetAwaiter().GetResult();
            return result.data;
        }
        public List<BECampaignInfo> GetCampaignAccessList(int iAgentID, bool bActiveCampaign)
        {
            var result = _securityService.GetCampaignAccessListAsync(iAgentID, bActiveCampaign).GetAwaiter().GetResult();
            return result.data??new List<BECampaignInfo>();
        }
    }
}
