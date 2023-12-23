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
    public class BLProcessBreakMapping : IProcessBreakMappingService, IDisposable//, IDataOperation<BEProcessBreakMapping>
    {
        public void Dispose()
        { }

        public List<BEProcessInfo> GetProcessBreakMappedList(int ClientID, string ProcessName, int UserId, BETenant oTenant)
        {
            using (DLProcessBreakMapping objProcess = new DLProcessBreakMapping(oTenant))
            {
                return objProcess.GetProcessBreakMappedList(ClientID, ProcessName, UserId);
            }
        }

        public BEProcessBreakMapping GetProcessBreakMappedDetails(int iProcessId, BETenant oTenant)
        {
            using (DLProcessBreakMapping objProcess = new DLProcessBreakMapping(oTenant))
            {
                return objProcess.GetProcessBreakMappedDetails(iProcessId);
            }
        }
        public void InsertData(BEProcessBreakMapping oProcess, int FormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(FormID, oProcess.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            */
            using (DLProcessBreakMapping objProcess = new DLProcessBreakMapping(oTenant))
            {
                objProcess.InsertData(oProcess);
            }
        }
        public void UpdateData(BEProcessBreakMapping oProcess, int FormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(FormID, oProcess.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            */
            using (DLProcessBreakMapping objProcess = new DLProcessBreakMapping(oTenant))
            {
                objProcess.UpdateData(oProcess);
            }
        }
    }
}
