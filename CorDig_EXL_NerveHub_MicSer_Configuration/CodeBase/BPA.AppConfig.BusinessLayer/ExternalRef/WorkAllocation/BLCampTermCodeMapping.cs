using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessLayer.ExternalRef.WorkAllocation
{
    public class BLCampTermCodeMapping : ICampTermCodeMappingService, IDisposable//, IDataOperation<BECampTermCodeMapping>
    {
        public void Dispose()
        { }

        public List<BECampaignInfo> GetCampTermMappedList(int ProcessId, string CampaignName, BETenant oTenant)
        {
            using (DLCampTermCodeMappjg objTermination = new DLCampTermCodeMappjg(oTenant))
            {
                return objTermination.GetCampTermMappedList(ProcessId, CampaignName);
            }
        }
        public BECampTermCodeMapping GetCampTermMappedDetails(int CampID, BETenant oTenant)
        {
            using (DLCampTermCodeMappjg objTermination = new DLCampTermCodeMappjg(oTenant))
            {
                return objTermination.GetCampTermMappedDetails(CampID);
            }
        }
        public void InsertData(BECampTermCodeMapping oTermination, int FormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(FormID, oTermination.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            */
            using (DLCampTermCodeMappjg objTermination = new DLCampTermCodeMappjg(oTenant))
            {
                objTermination.InsertData(oTermination);
            }
        }
        public void UpdateData(BECampTermCodeMapping oTermination, int FormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(FormID, oTermination.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            */
            using (DLCampTermCodeMappjg objTermination = new DLCampTermCodeMappjg(oTenant))
            {
                objTermination.UpdateData(oTermination);
            }
        }
    }
}
