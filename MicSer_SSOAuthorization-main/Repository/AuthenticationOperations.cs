using Dapper;
using MicSer.SSOAuthorization.Models;
using System.Data;
using System.Threading.Tasks;

namespace MicSer.SSOAuthorization.Repository
{
    public interface IAuthenticationOperations
    {
        Task<BEUserInfo> IsLDAPUser(AuthenticationRequest authenticationRequest, string connectionString);
        Task<BERoleInfo> UserRoleInfoById(int userId, string connectionString);
    }

    public class AuthenticationOperations : IAuthenticationOperations
    {
        private DapperUtility dapperUtilityObj;
        public AuthenticationOperations()
        {
            
        }
        public async Task<BEUserInfo> IsLDAPUser(AuthenticationRequest authenticationRequest, string connectionString)
        {
            dapperUtilityObj = DapperUtility.CreateInstance(connectionString);
            var procedureName = "USP_CDS_UserLogin";

            DynamicParameters parameterList = new DynamicParameters();
            parameterList.Add("@LOGINNAME", authenticationRequest.LoginName, DbType.String);
            parameterList.Add("@SystemSessionID", string.Empty, DbType.String);
            parameterList.Add("@HostName", string.Empty, DbType.String);
            parameterList.Add("@IPAddress", string.Empty, DbType.String);
            parameterList.Add("@ErrorStatus", string.Empty, DbType.String, ParameterDirection.Output);

            var result = await dapperUtilityObj.GetAsync<BEUserInfo>(procedureName, parameterList);
            result.ConnectionString = connectionString;
            // string ErrorStatus = parameterList.Get<string>("@ErrorStatus");
            return result;
        }

        public async Task<BERoleInfo> UserRoleInfoById(int userId, string connectionString)
        {
            dapperUtilityObj = DapperUtility.CreateInstance(connectionString);
            string SQL_SELECT_USERROLES_LOGIN = @"SELECT distinct Roles.RoleId,RoleName FROM Config.tblUserRoleMapping UserRole (nolock) 
                                                       INNER JOIN Config.tblRoleMaster Roles (NOLOCK) ON UserRole.RoleId=Roles.RoleId 
                                                       WHERE UserId=@UserID and UserRole.Disabled=0 and Roles.Disabled=0";

            DynamicParameters parameterList = new DynamicParameters();
            parameterList.Add("@UserID", userId, DbType.String);
            var result = await dapperUtilityObj.Get<BERoleInfo>(SQL_SELECT_USERROLES_LOGIN, parameterList);
            return result;
        }
    }
}
