using BPA.AppConfig.BusinessEntity.Application;
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
    public class BLTerminationCode : ITerminationCodeService, IDisposable//, IDataOperation<BETerminationCodeInfo>
    {
        public void Dispose()
        { }
        public BLTerminationCode()
        { }

        public List<BETerminationCodeInfo> GetTermCodeList(string sTermcodeName, bool bGetActive, BETenant oTenant)
        {
            using (DLTerminationCode oTermCode = new DLTerminationCode(oTenant))
            {
                return oTermCode.GetTermCodeList(sTermcodeName, bGetActive);
            }
        }

        public List<BETerminationCodeInfo> GetTermCodeList(int iTermCodeID, BETenant oTenant)
        {
            using (DLTerminationCode oTermCode = new DLTerminationCode(oTenant))
            {
                return oTermCode.GetTermCodeList(iTermCodeID);
            }
        }

        public string InsertData(BETerminationCodeInfo oTermCode, int iFormID, BETenant oTenant)
        {
            /*
            if (oTermCode.sTermCodeName == string.Empty || oTermCode.sTermCodeName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Termination Code " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!BLCheckPermission.hasPermission(iFormID, oTermCode.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }*/
            using (DLTerminationCode objTerm = new DLTerminationCode(oTenant))
            {
                return objTerm.InsertData(oTermCode);
            }

        }

        public void UpdateData(BETerminationCodeInfo oTermCode, int iFormID, BETenant oTenant)
        {
            /*
            if (oTermCode.sTermCodeName == string.Empty || oTermCode.sTermCodeName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Termination Code " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!BLCheckPermission.hasPermission(iFormID, oTermCode.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }*/
            using (DLTerminationCode objTerm = new DLTerminationCode(oTenant))
            {
                objTerm.UpdateData(oTermCode);
            }
        }

        public List<BETerminationCodeInfo> GetTermCodeListByCamp(int iCampID, BETenant oTenant)
        {
            using (DLTerminationCode oTermCode = new DLTerminationCode(oTenant))
            {
                return oTermCode.GetTermCodeListByCamp(iCampID);
            }
        }
        public List<BETerminationCodeInfo> GetTermCodeListByCamp(string sTermName, int iCampID, BETenant oTenant)
        {
            using (DLTerminationCode oTermCode = new DLTerminationCode(oTenant))
            {
                return oTermCode.GetTermCodeListByCamp(sTermName, iCampID);
            }
        }
    }
}
