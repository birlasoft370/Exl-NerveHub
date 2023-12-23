using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.Security;
using BPA.AppConfig.Datalayer.ExternalRef.Security;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Security;

namespace BPA.AppConfig.BusinessLayer.ExternalRef.Security
{
    //[ExceptionShielding("WCF Exception Shielding")]
    public class BLTeam : ITeamService, IDisposable//, IDataOperation<BETeamInfo>
    {
        public void Dispose()
        { }
        public List<BETeamInfo> GetTeamList(int iLoggedinUserID, bool bActiveTeam, BETenant oTenant)
        {
            using (DLTeam oTeam = new DLTeam(oTenant))
            {
                return oTeam.GetTeamList(iLoggedinUserID, bActiveTeam);
            }
        }
        public List<BETeamInfo> GetTeamList(string sTeamName, int iLoggedinUserID, bool bActiveTeam, BETenant oTenant)
        {
            using (DLTeam oTeam = new DLTeam(oTenant))
            {
                return oTeam.GetTeamList(sTeamName, iLoggedinUserID, bActiveTeam);
            }
        }
        public List<BETeamInfo> GetTeamList(int iTeamID, BETenant oTenant)
        {
            using (DLTeam oTeam = new DLTeam(oTenant))
            {
                return oTeam.GetTeamList(iTeamID);
            }
        }

        public void InsertData(BETeamInfo oTeam, int FormID, BETenant oTenant)
        {
            /*
            if (oTeam.sTeamName == string.Empty || oTeam.sTeamName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Team Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!BLCheckPermission.hasPermission(FormID, oTeam.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }*/
            using (DLTeam objTeam = new DLTeam(oTenant))
            {
                objTeam.InsertData(oTeam);
            }

        }

        public void UpdateData(BETeamInfo oTeam, int FormID, BETenant oTenant)
        {
            /*
            if (oTeam.sTeamName == string.Empty || oTeam.sTeamName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Team Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!BLCheckPermission.hasPermission(FormID, oTeam.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }*/
            using (DLTeam objTeam = new DLTeam(oTenant))
            {
                objTeam.UpdateData(oTeam);
            }

        }

        public List<BETeamInfo> GetProcessWiseTeamList(int iProcess, BETenant oTenant)
        {
            using (DLTeam oTeam = new DLTeam(oTenant))
            {
                return oTeam.GetProcessWiseTeamList(iProcess);
            }
        }
    }
}
