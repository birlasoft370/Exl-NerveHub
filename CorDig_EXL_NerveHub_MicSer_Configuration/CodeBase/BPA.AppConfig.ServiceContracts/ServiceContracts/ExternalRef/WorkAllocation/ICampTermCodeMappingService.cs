using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation
{
    public interface ICampTermCodeMappingService
    {       
        List<BECampaignInfo> GetCampTermMappedList(int ProcessId, string CampaignName, BETenant oTenant);
        BECampTermCodeMapping GetCampTermMappedDetails(int CampID, BETenant oTenant);
        void InsertData(BECampTermCodeMapping oTermination, int FormID, BETenant oTenant);
        void UpdateData(BECampTermCodeMapping oTermination, int FormID, BETenant oTenant);
    }
}
