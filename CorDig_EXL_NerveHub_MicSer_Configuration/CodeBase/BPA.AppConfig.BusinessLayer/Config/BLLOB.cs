using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.Datalayer.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessLayer.Config
{
    public class BLLOB : ILOBService, IDisposable
    {
        public void Dispose()
        { }


        public List<BELOBInfo> GetLOBList(bool IsActiveLOB, BETenant oTenant)
        {
            return GetLOBList("", IsActiveLOB, oTenant);
        }

        public List<BELOBInfo> GetLOBList(string LOBName, bool IsActiveLOB, BETenant oTenant)
        {
            using (DLLOB objLOB = new DLLOB(oTenant))
            {
                return objLOB.GetLOBList(LOBName, IsActiveLOB);
            }
        }


        public List<BELOBInfo> GetLOBList(int LOBID, BETenant oTenant)
        {
            using (DLLOB objLOB = new DLLOB(oTenant))
            {
                return objLOB.GetLOBList(LOBID);
            }
        }

        public void InsertData(BELOBInfo oLOB, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oLOB.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oLOB.sLOBName == string.Empty || oLOB.sLOBName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("LOB Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLLOB objLOB = new DLLOB(oTenant))
            {
                objLOB.ManageLOB(oLOB, "Add");
            }
        }
        public void UpdateData(BELOBInfo oLOB, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oLOB.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (oLOB.sLOBName == string.Empty || oLOB.sLOBName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("LOB Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLLOB objLOB = new DLLOB(oTenant))
            {
                objLOB.ManageLOB(oLOB, "Update");
            }
        }
    }
}
