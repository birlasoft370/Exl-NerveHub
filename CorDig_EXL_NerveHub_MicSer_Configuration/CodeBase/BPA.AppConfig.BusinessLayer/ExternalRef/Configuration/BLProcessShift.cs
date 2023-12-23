using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.Datalayer.ExternalRef.Configuration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessLayer.ExternalRef.Configuration
{
    public class BLProcessShift : IProcessShiftService, IDisposable
    {
        public void Dispose()
        { }
        public BLProcessShift()
        { }

        public void InsertData(BEProcessShiftInfo oShift, int iFormID, BETenant oTenant)
        {
            if (oShift.iProcessID.ToString() == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Process Shift Mapping " + GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLProcessShift objShift = new DLProcessShift(oTenant))
            {
                objShift.InsertData(oShift);
            }
        }
        public void UpdateData(BEProcessShiftInfo oShift, int iFormID, BETenant oTenant)
        {
            using (DLProcessShift objShift = new DLProcessShift(oTenant))
            {
                objShift.UpdateData(oShift);
            }
        }
        public IList<BEProcessShiftInfo> GetProcessShift(int ProcessID, BETenant oTenant)
        {
            using (DLProcessShift oShift = new DLProcessShift(oTenant))
            {
                return oShift.GetProcessShift(ProcessID);
            }
        }
    }
}
