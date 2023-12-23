using BPA.AppConfig.BusinessEntity.Application;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation
{
    public class DLWorkObject : IDisposable
    {
        private BETenant _oTenant = null;

        public DLWorkObject(BETenant oTenant)
        { _oTenant = oTenant; }

        public void Dispose()
        { _oTenant = null; }


        private const string SQL_CHECK_USER_IN_SUPER_OR_FUNC_ROLE = @"begin
                                                                        if exists(select URM.UserID from config.tblUserRoleMapping (nolock) URM inner join config.tblRoleMaster (nolock) RM on URM.RoleID=RM.RoleID
                                                                                 where (RM.RoleName like '%Super Admin%' OR RM.RoleName like '%Functional Admin%') and ISNULL(URM.Disabled,0)=0
                                                                                 and URM.UserID=@UserID)
	                                                                        select 1
                                                                        else
	                                                                        select 0
                                                                        end";
        private const string PARAM_USERID = "@UserId";

        public int CheckUserIsSuperOrFunctionalAdmin(int UserId)
        {
            DatabaseProviderFactory factoy = new DatabaseProviderFactory();
            //Database db = factoy.Create(DL_Shared.Connection);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_CHECK_USER_IN_SUPER_OR_FUNC_ROLE);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            return Convert.ToInt32(db.ExecuteScalar(dbCommand));
        }
    }
}
