using BPA.Security.ServiceContract.FaultContracts;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace BPA.Security.ServiceContracts.Security
{
    [ServiceContract(Name = "PermissionServiceContract")]
    public interface IPermissionService : IDisposable
    {

        //[OperationContract(Name = "GetUserListActive")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEUserInfo> GetUserList(bool bActiveUser, int iLoggedinUserID, string SearchCondition, BETenant oTenant);

        //[OperationContract(Name = "GetUserListWithLoginName")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEUserInfo> GetUserList(string sLoginName, bool bActiveUser, int iLoggedinUserID, string SearchCondition, BETenant oTenant);

        //[OperationContract(Name = "GetClientUserList")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEUserInfo> GetClientUserList(string sLoginName, bool bActiveUser, int iLoggedinUserID, string SearchCondition, BETenant oTenant);

        //[OperationContract(Name = "GetUserListWithFMRole")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEUserInfo> GetUserListWithFMRole(string sLoginName, BETenant oTenant);

        //[OperationContract(Name = "GetClientUserDetails")]
        //[FaultContract(typeof(ServiceFault))]
        BEUserInfo GetClientUserDetails(int iUserID, BETenant oTenant);

        //[OperationContract(Name = "GetUserDetailsWithRole")]
        //[FaultContract(typeof(ServiceFault))]
        BEUserInfo GetUserDetailsWithRole(int iUserID, BETenant oTenant);

        //[OperationContract(Name = "GetSupervisorList")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEUserInfo> GetSupervisorList(int iUserID, BETenant oTenant);

        //[OperationContract(Name = "InsData")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertData(BEUserInfo oUserData, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "InsertClientUserData")]
        //[FaultContract(typeof(ServiceFault))]
        //void InsertClientUserData(BEUserInfo oUserData);

        //[OperationContract(Name = "InsertClientUserRecord")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertClientUserRecord(BEUserInfo oUser, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "InsertUserRoleData")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertUserRoleData(BEUserInfo oUserData, int iMode, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "InsertAgentMigrationData")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertAgentMigrationData(int UserId, string strCampaignRole, int CreatedBy, BETenant oTenant);

        //[OperationContract(Name = "InsertUnsuccessfullLogin")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertUnsuccessfullLogin(int UserID, string SessionID, string HostName, BETenant oTenant);

        //[OperationContract(Name = "GetUserList")]
        //[FaultContract(typeof(ServiceFault))]
        //  DataSet GetUserList(int Process, int Campaign, int Team, DateTime? StartDate, DateTime? EndDate, BETenant oTenant);
        DataSet GetUserList(int ClientID, int Process, int Campaign, int Team, DateTime? StartDate, DateTime? EndDate, BETenant oTenant);
        //[OperationContract(Name = "UpdData")]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BEUserInfo oUserData, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "UpdatetClientUserData")]
        //[FaultContract(typeof(ServiceFault))]
        //void UpdatetClientUserData(BEUserInfo oUserData);

        //[OperationContract(Name = "UpdateClientUserRecord")]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateClientUserRecord(BEUserInfo oUser, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "DelData")]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BEUserInfo oUserData, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "DeleteClientUserData")]
        //[FaultContract(typeof(ServiceFault))]
        //void DeleteClientUserData(BEUserInfo oUserData);

        //[OperationContract(Name = "DeleteClientUserRecord")]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteClientUserRecord(BEUserInfo oUser, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "DeleteUserRoleData")]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteUserRoleData(BEUserInfo oUserData, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "ChangePassword")]
        //[FaultContract(typeof(ServiceFault))]
        void ChangePd(BEUserInfo oUserInfo, BETenant oTenant);

        //[OperationContract(Name = "ChangeUserStatus")]
        //[FaultContract(typeof(ServiceFault))]
        void ChangeUserStatus(int iUserID, BETenant oTenant);

        //[OperationContract(Name = "ChangePasswordLog")]
        //[FaultContract(typeof(ServiceFault))]
        int ChangePdLog(BEUserInfo oUserInfo, BETenant oTenant);

        //[OperationContract(Name = "GetAuthenticateUser")]
        //[FaultContract(typeof(ServiceFault))]
        //BEUserInfo GetAuthenticateUser(string LoginID, string Password, string SessionID, string UserHostName, out Boolean Disabled);

        //[OperationContract(Name = "CheckOldPassWord")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet CheckOldPd(int UserID, BETenant oTenant);

        //[OperationContract(Name = "GetUserBasedMomteeID")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserBasedMomteeID(string user1, string user2, BETenant oTenant);

        //[OperationContract(Name = "GetUserID")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserID(string LoginName, BETenant oTenant);

        //[OperationContract(Name = "GetLastPasswordChange")]
        //[FaultContract(typeof(ServiceFault))]
        DateTime GetLastPasswordChange(string LoginName, BETenant oTenant);

        //[OperationContract(Name = "IsAuthorization")]
        //[FaultContract(typeof(ServiceFault))]
        //bool IsAuthorization(int FormID, int UserID, string Action);

        //[OperationContract(Name = "GetCampaignUserList")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetCampaignUserList(int iCampID, BETenant oTenant);

        //[OperationContract(Name = "GetUserCampaigns")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserCampaigns(string Clients, int UserId, BETenant oTenant);

        //[OperationContract(Name = "GetUserRolesWithCampaignId")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserRoles(int CampaignId, int UserId, BETenant oTenant);

        //[OperationContract(Name = "GetUserRolesWithUserId")]
        //[FaultContract(typeof(ServiceFault))]
        List<BERoleInfo> GetUserRoles(int UserId, BETenant oTenant);

        //[OperationContract(Name = "CheckUser")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet CheckUser(string LoginName, BETenant oTenant);

        //[OperationContract(Name = "InsertSessionLogOut")]
        //[FaultContract(typeof(ServiceFault))]
        //void InsertSessionLogOut(string ServerName, string UserAgent, string IP, string Host, string UserName, string URLTReferrer, string URL, string SessionID, Boolean IsNewSession, int SessionTimeOut);

        //[OperationContract(Name = "ChangeUser_LANIDUser")]
        //[FaultContract(typeof(ServiceFault))]
        //void ChangeUser_LANIDUser(string LoginName);

        //[OperationContract(Name = "ChangeUser_NONLANIDUser")]
        //[FaultContract(typeof(ServiceFault))]
        //void ChangeUser_NONLANIDUser(string LoginName);

        //[OperationContract(Name = "ChangeUserPassword")]
        //[FaultContract(typeof(ServiceFault))]
        void ChangeUserPd(string LoginName, BETenant oTenant);

        //[OperationContract(Name = "IsLANUser")]
        //[FaultContract(typeof(ServiceFault))]
        Boolean IsLANUser(string LoginName, BETenant oTenant);

        //[OperationContract(Name = "GetERPTeam")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEUserInfo> GetERPTeam(int iUserID, BETenant oTenant);

        //[OperationContract(Name = "GetUserListForExtraRole")]
        //[FaultContract(typeof(ServiceFault))]
        //List<BEUserInfo> GetUserListForExtraRole(int iUserID, BETenant oTenant);

        //[OperationContract(Name = "GetProcessFamilyOwnerList")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEUserInfo> GetProcessFamilyOwnerList(string sUserName, BETenant oTenant);

        //[OperationContract(Name = "GetUserListProcessWithProcess")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserListProcess(int Process, BETenant oTenant);

        //[OperationContract(Name = "GetUserListProcessAllUsers")]
        //[FaultContract(typeof(ServiceFault))]
        //List<AgentList> GetUserListProcess(int Process, bool IsAllUsers, BETenant oTenant);

        //[OperationContract(Name = "GetUserListProcessAgentAM")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserListProcessAgentAM(int Process, BETenant oTenant);

        //[OperationContract(Name = "GetUserListProcessAgentAMClientQCA")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserListProcessAgentAMClientQCA(int Process, BETenant oTenant);

        //[OperationContract(Name = "GetMentorWithTMid")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetMentorWithTMid(int iTMId, string sStartDate, string sEndDate, BETenant oTenant);

        //[OperationContract(Name = "GetAllUserListProcessAgentAM")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetAllUserListProcessAgentAM(int Process, BETenant oTenant);

        //[OperationContract(Name = "GetUserListProcessAgent")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserListProcessAgent(int Process, BETenant oTenant);

        //[OperationContract(Name = "GetUserListVPAndAbove")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserListVPAndAbove(string sUser, BETenant oTenant);

        //[OperationContract(Name = "GetUserListProcessMontee")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEUserInfo> GetUserListProcessMontee(string Process, BETenant oTenant);

        //[OperationContract(Name = "GetUserProcessMap")]
        //[FaultContract(typeof(ServiceFault))]
        BEUserMapping GetUserProcessMap(int iUserID, BETenant oTenant);

        //[OperationContract(Name = "GetUserSetting")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEUserSetting> GetUserSetting(int iUserID, BETenant oTenant);

        //[OperationContract(Name = "GetUserMapping")]
        //[FaultContract(typeof(ServiceFault))]
        BEUserMapping GetUserMapping(int iUserID, int iRoleID, int iMappedOn, BETenant oTenant);

        //[OperationContract(Name = "InsertUserMapping")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertUserMapping(BEUserMapping oUserMapping, string sDeletedNodes, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "CancelRequestInBetween")]
        //[FaultContract(typeof(ServiceFault))]
        void CancelRequestInBetween(int iRequestID, int iUserID, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "ApproveAccessRequest")]
        //[FaultContract(typeof(ServiceFault))]
        void ApproveAccessRequest(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iUserID, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "RejectAccessRequest")]
        //[FaultContract(typeof(ServiceFault))]
        void RejectAccessRequest(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iUserID, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "CancelAccessRequest")]
        //[FaultContract(typeof(ServiceFault))]
        void CancelAccessRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "InsertUserMappingApprovers")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertUserMappingApprovers(DataTable dtApproverList, int iUserID, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "InsertUserMappingForApproval")]
        //[FaultContract(typeof(ServiceFault))]
        int InsertUserMappingForApproval(BEUserMapping oUserMapping, string sDeletedNodes, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "IsAdminUser")]
        //[FaultContract(typeof(ServiceFault))]
        bool IsAdminUser(int iUserID, BETenant oTenant);

        //[OperationContract(Name = "GetDefaultPage")]
        //[FaultContract(typeof(ServiceFault))]
        string GetDefaultPage(int iUserID, BETenant oTenant);

        //[OperationContract(Name = "GetUserRequestType")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserRequestType(BEUserMapping oUserMapping, int RequestId, BETenant oTenant);

        //[OperationContract(Name = "GetUserRequestStatus")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserRequestStatus(int iUserID, DateTime dtFromDate, DateTime dtToDate, BETenant oTenant);

        //[OperationContract(Name = "GetUserApprovalListWithUserID")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserApprovalList(int iUserID, BETenant oTenant);

        //[OperationContract(Name = "GetUserApproverListWithClientID")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserApproverList(int UserId, int ClientId, int ProcessId, int Flag, int iFormID, BETenant oTenant);

        List<BEUserInfo> GetUserApproverListByProcessId(int UserId, int iFormId, int ProcessId, BETenant oTenant);

        //[OperationContract(Name = "GetUserRoleApproverList")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserRoleApproverList(int iRoleID, BETenant oTenant);

        //[OperationContract(Name = "GetClientRequestApproverList")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetClientRequestApproverList(int ProcessId, BETenant oTenant);

        //[OperationContract(Name = "GetUserListD")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetUserListD(string sLoginName, bool bActiveUser, int iLoggedinUserID, int iClientUser, string SearchCondition, BETenant oTenant);

        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        BEUserInfo GetUserInformation(string sHostName, string sIPAddress, string UserName, BETenant oTenant, bool isWindowsAuthentication = true);

        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<BEUserInfo> GetUserListbyClient(bool bActiveUser, int iClientID, int iUserID, BETenant oTenant);


        List<BEUserInfo> GetUserRoleListUserRole(int iUserid, int iFormId, BETenant oTenant);

        //[OperationContract(Name = "GetBotUserDetails")]
        // [FaultContract(typeof(ServiceFault))]
        //DataSet GetBotUserList(int iUserID, int iProcessId, bool bActive, BETenant oTenant);
        //DataSet GetDowntimeReason(bool bActive, BETenant oTenant);
        List<BEUserInfo> GetBotUserList(int iUserID, int iProcessId, bool bActive, BETenant oTenant);
        List<BEBOTDowntimeInfo> GetDowntimeReason(bool bActive, BETenant oTenant);

        List<BEBOTDowntimeInfo> GetSearchData(int iUserId, string CampaignName, BETenant oTenant);
        List<BEBOTDowntimeInfo> GetSearchDataList(int iUserId, int CaptureId, BETenant oTenant);
    }
    // 
}

