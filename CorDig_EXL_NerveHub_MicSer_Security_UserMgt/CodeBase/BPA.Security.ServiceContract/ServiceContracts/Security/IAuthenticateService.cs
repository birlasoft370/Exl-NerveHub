using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.ServiceContract.FaultContracts;
using System.ServiceModel;


namespace BPA.Security.ServiceContracts.Security
{
    [ServiceContract(Name = "AuthenticateServiceContract")]
    public interface IAuthenticateService : IDisposable
    {
        [OperationContract(Name = "AuthenticateUser")]
        [FaultContract(typeof(ServiceFault))]
        BEUserInfo AuthenticateUser(string LoginID, string Password, string SessionID, string UserHostName, out Boolean Disabled, BETenant oTenant);

        [OperationContract(Name = "IsLADPUser")]
        [FaultContract(typeof(ServiceFault))]
        List<BEUserInfo> IsLADPUser(string LoginName, BESession oBESession, out int iSessionID, out bool bProcessMap, BETenant oTenant, bool isWindowsAuthentication = true, int cachDuration = 24);

        [OperationContract(Name = "InsertSessionLogOut")]
        [FaultContract(typeof(ServiceFault))]
        void InsertSessionLogOut(string ServerName, string UserAgent, string IP, string Host, string UserName, string URLTReferrer, string URL, string SessionID, Boolean IsNewSession, int SessionTimeOut, BETenant oTenant);

        [OperationContract]
        // [AuthenticationBehavior]
        bool IsAuthenticated(string UserName, string Passsword, BETenant oTenant);

        [OperationContract]
        // [AuthenticationBehavior]
        bool IsDomainActivation(BETenant oTenant);

        [OperationContract(Name = "GetUserList")]
        [FaultContract(typeof(ServiceFault))]
        IList<BELdapUserInfo> GetUserList(string LoginName, string Domain, BETenant oTenant);

        [OperationContract(Name = "ChangePassword")]
        [FaultContract(typeof(ServiceFault))]
        string ChangePd(string sLoginName, string sCurrentPd, string sNewPass, BETenant oTenant, bool isWindowsAuthenticate = true, int iUserId = 0);

        //[OperationContract(Name = "ChangePassword")]
        //[FaultContract(typeof(ServiceFault))]
        //string ChangePassword(string sLoginName, string sCurrentPassword, string sNewPass, BETenant oTenant, bool isWindowsAuthenticate = true, int iUserId = 0);

        void InsertUserPageLogin(string SystemSessionID, string HostName, string IPAddress, string PageName, int iUserId, BETenant oTenant);
        string CheckPasswordExpire(string strLoginName, out string output, out int DaysLeft, string Domain, BETenant oTenant);

        string Change_Password(string sLoginName, string sCurrentPassword, string sNewPass, string Domain, BETenant oTenant);


    }
}
