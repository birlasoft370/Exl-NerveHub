using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.Datalayer.Config;

namespace BPA.AppConfig.BusinessLayer.Config
{
    public class BLSubProcess : ISubProcessService, IDisposable, IDataOperation<BESubProcess>
    {
        public void Dispose()
        { }

        public List<BESubProcess> GetSubProcessListProcessWise(int iProcessId, BETenant oTenant)
        {
            using (DLSubProcess oSubProcess = new DLSubProcess(oTenant))
            {
                return oSubProcess.GetSubProcessListProcessWise(iProcessId);
            }
        }
        public BESubProcess GetSubProcessList(int iSubProcessID, BETenant oTenant)
        {
            using (DLSubProcess oSubProcess = new DLSubProcess(oTenant))
            {
                return oSubProcess.GetSubProcessList(iSubProcessID);
            }
        }

        public List<BESubProcess> GetSubProcessList(int ProcessID, string SubProcessName, bool bActiveSubProcess, BETenant oTenant)
        {
            using (DLSubProcess oSubProcess = new DLSubProcess(oTenant))
            {
                return oSubProcess.GetSubProcessList(ProcessID, SubProcessName, bActiveSubProcess);
            }
        }

        public void InsertData(BESubProcess oSubprocess, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oSubprocess.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oSubprocess.sSubProcessName == string.Empty || oSubprocess.sSubProcessName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Sub Process Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            */
            using (DLSubProcess oSubProcess = new DLSubProcess(oTenant))
            {
                oSubProcess.InsertData(oSubprocess);
            }
        }

        public void UpdateData(BESubProcess oSubprocess, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oSubprocess.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (oSubprocess.sSubProcessName == string.Empty || oSubprocess.sSubProcessName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Sub Process Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            */
            using (DLSubProcess oSubProcess = new DLSubProcess(oTenant))
            {
                oSubProcess.UpdateData(oSubprocess);
            }
        }
        public void DeleteData(BESubProcess ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }
    }
}
