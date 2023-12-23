using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace BPA.AppConfig.Datalayer.Security
{
    public class DLPermission : IDisposable
    {
        private BETenant _oTenant = null;
        public DLPermission(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private string SP_GET_USERLISTBYCLIENT = @"Config.UspGetUserListByClient";
        private const string SQL_SELECT_USER_PROCESSFAMILYOWNER = @"select UR.USERID, ltrim(isnull(UR.FirstName,'') + ' '+isnull(UR.MiddleName,'')+' ' + isnull(UR.LastName,'') +' ( '+isnull(UR.LoginName,'')+' )')  as Name from Config.tblUserMaster(nolock) UR where disabled=0 and UR.FirstName Like @Name or  UR.LastName Like @Name order by UR.Firstname,UR.Middlename,UR.LastName";
        private const string SQL_SELECT_USER_MONTEEBASED = @"select UR.FirstName +' ' + UR.LastName from Config.tblUserMaster(nolock) UR where UR.userid in (@user1, @user2)";
        private const string SP_SELECT_MONTEE = @"select UM.UserID,UM.EmpID,UM.FirstName,UM.MiddleName,UM.LastName,UM.LoginName,
                                                UM.Password,isnull(UM.UserLevelID,0) as UserLevelID ,UM.disabled,UM.IsLanIDUser,'FacilityId'=isnull(UM.FacilityId,0),isnull(UM.createdby,0) as createdby from Config.tblUserProcessMap(nolock) UPM,Config.tblUserMaster(nolock) UM where 
                                                UM.UserID = UPM.UserID
                                                and  UPM.ProcessID=@ProcessID";
        private const string SQL_CHECKADMIN = @"Select [IsAdmin]=dbo.fn_CheckAdmin(@UserID)";
        private const string SQL_SELECT_USERID = @"SELECT UserID,EmpID,FirstName,MiddleName,LastName,LoginName,Password,isnull(UserLevelID,0) as UserLevelID ,disabled,IsLanIDUser,isnull(createdby,0) as createdby ,  'FacilityId'=isnull(FacilityId,0)  FROM Config.tblUserMaster (Nolock) WHERE UserID=@UserID ";

        private const string SQL_SELECT_CLIENTUSERID = @"Begin SELECT isnull(URM.RoleId,0) RoleId,UM.UserID,UM.EmpID,UM.FirstName,UM.MiddleName,UM.LastName,UM.IsClient, UM.Email, UM.LoginName,UM.disabled,UM.IsLanIDUser,isnull(UM.createdby,0) as createdby, Convert(varchar(10),UM.DOJ,101) as DOJ, UM.FacilityId, UM.SupervisorId, isnull(UM.LOBID, 0) as LOBID, ISNULL(UM.SBUID, 0) AS SBUID,isnull(UM.IsBOT, 0) as IsBOT,
                                                        Sup.FirstName + ' ' + isnull(Sup.MiddleName,'') + ' ' + isnull(Sup.LastName,'') + ' (' + Convert(varchar,Sup.EmpId) + ')' AS SupervisorName ,UM.JOBID
                                                        FROM Config.tblUserMaster (Nolock) UM  
                                                        inner join Config.tblUserMaster (nolock) Sup on UM.SupervisorId=Sup.UserId
                                                        left outer join Config.tblUserRoleMapping (Nolock) URM on UM.UserId=URM.UserId and URM.disabled=0
                                                        WHERE UM.UserID=@UserID
                                                        

                                                         select ClientId,ProcessId,UserProcessMapId from Config.tblUserProcessMap where UserId=@UserId and Mappedon=2  and disabled=0 
                                                          --  Union
                                                         --select UPM.ClientId,UPM.ProcessId,UPM.UserProcessMapId from Config.tblUserProcessMap_Approval (NOLOCK) UPM
                                                          --  inner join dbo.CDS_S_UserRequestApproval (NOLOCK) URA  ON UPM.RequestId=URA.RequestId and IsApproved=0 and IsRejected=0 and Iscancelled=0    
                                                        --where UPM.UserId=@UserId and UPM.Mappedon=2 and IsApproved=0 and IsRejected=0
                                                        End";
        private const string SQL_SELECT_USERDETAILSWITHROLE = @"Begin
                                                                SELECT 0 as 'ApprovalPending', isnull(URM.RoleId,0) RoleId,UM.UserID,UM.EmpID,UM.FirstName,UM.MiddleName,UM.LastName,UM.Email,UM.LoginName,UM.disabled,UM.IsLanIDUser,isnull(UM.createdby,0) as createdby 
                                                                FROM Config.tblUserMaster (Nolock) UM  left outer join Config.tblUserRoleMapping (Nolock) URM on UM.UserId=URM.UserId and URM.disabled=0 and URM.RoleId in (262,276,1)
                                                                WHERE UM.UserID=@UserID

                                                                union

                                                                SELECT  1 as 'ApprovalPending', isnull(URM.RoleId,0) RoleId,UM.UserID,UM.EmpID,UM.FirstName,UM.MiddleName,UM.LastName,UM.Email,UM.LoginName,UM.disabled,UM.IsLanIDUser,isnull(UM.createdby,0) as createdby 
                                                                FROM Config.tblUserMaster (Nolock) UM  inner join Config.tblUserRoleMapping_Approval (Nolock) URM on UM.UserId=URM.UserId and URM.disabled=0 and URM.RoleId in (262,276,1)
                                                                inner join Config.tblUserRequestApproval (NOLOCK) URA  ON URM.RequestId=URA.RequestId and IsApproved=0 and IsRejected=0 and Iscancelled=0 
                                                                WHERE UM.UserID=@UserID

                                                                select ClientId,UserProcessMapId from Config.tblUserProcessMap where UserId=@UserId and Mappedon=1  and disabled=0 
                                                                Union
                                                                select UPM.ClientId,UPM.UserProcessMapId from Config.tblUserProcessMap_Approval (NOLOCK) UPM
                                                                inner join Config.tblUserRequestApproval (NOLOCK) URA  ON UPM.RequestId=URA.RequestId and IsApproved=0 and IsRejected=0 and Iscancelled=0    
                                                                where UPM.UserId=@UserId and UPM.Mappedon=1
                                                                End";
        private const string SQL_SELECT_USERLOGINNAME = @"Select cnt=count(*) from Config.tblUserMaster (nolock) where LoginName=@LoginName";

        private const string SQL_SELECT_INSERTSESSIONLOGOUT = @"INSERT INTO Config.tblSessionLogOut VALUES(GetDate() , @ServerName, @UserAgent, @UserIp, @HostName, @UserName, @SessionID, @SessionTimeOut, @IsNewSession, @URLReferrer, @RawURL)";

        private const string SQL_SELECT_SUPERVISOR = @"Select Distinct UserMaster.* from dbo.CDS_TeamMemberInfo  TeamMember (NOLOCK) 
				                                                INNER JOIN 
				                                                (
					                                                Select Distinct TeamID from dbo.CDS_TeamMemberInfo (NOLOCK) 
				                                                ) TeamID
				                                                On TeamMember.TeamID = TeamID.TeamID AND TeamMember.Disabled=0
				                                                INNER JOIN Config.tblUserMaster UserMaster (NOLOCK) 
				                                                ON UserMaster.UserID = TeamMember.USerID
				                                                INNER JOIN
				                                                (Select UserLevel.ParentID ,UserMaster.* from Config.tblUserLevel UserLevel (NOLOCK) 
					                                                INNER JOIN Config.tblUserMaster UserMaster (NOLOCK) 
					                                                ON UserLevel.UserLevelID=UserMaster.UserLevelID
					                                                Where UserMaster.UserID=@UserID
				                                                )ParentID
				                                                ON ParentID.ParentID=UserMaster.UserLevelID order by UserMaster.FirstName
                                                     ";

        private const string SQL_SELECT_MAXUSERID = @"SELECT ISNULL(MAX(UserID),1) FROM Config.tblUserMaster (NOLOCK)";
        private const string SQL_SELECT_CAMPAIGNUSERS = @"select UserMaster.UserID, FirstName + ' ' + MiddleName + ' ' + LastName as UserName from Config.tblUserRoleMapping CampMap (nolock) inner join Config.tblUserMaster UserMaster (nolock) on CampMap.UserID=UserMaster.UserID where CampMap.Disabled=0 and CampaignID=@CampaignID AND RoleID=9";
        private const string SQL_SELECT_USERROLECAMPAIGN = @"SELECT Distinct UserID,RoleID,CreatedBy FROM Config.tblUserRoleMapping WHERE UserID=@UserID AND Disabled=0";

        private const string SQL_SELCET_USERID = @"SELECT UserID,ModifiedOn FROM Config.tblUserMaster (nolock) WHERE LoginName=@LoginName";

        private const string SQL_SELCET_LASTPDCHANGE = @"SELECT top 1 UserMaster.UserID,Pass.CreatedOn FROM Config.tblUserMaster UserMaster (nolock) inner join  CDS_PasswordLog Pass (nolock) on UserMaster.UserId=Pass.UserId WHERE LoginName=@LoginName order by Pass.CreatedOn desc";

        private const string SQL_SELECT_PD = @"SELECT Password FROM Config.tblUserMaster WHERE UserID=@UserID";

        private const string SQL_INSERT_UNSUCCESSFULLLOGIN = @"INSERT INTO CDS_Login_Unsucessfull(UserID,SessionID,HostName) VALUES(@UserID,@SessionID,@HostName)";
        private const string SQL_INSERT_USER = @"INSERT INTO Config.tblUserMaster (EmpID,FirstName,MiddleName,LastName,LoginName,Password,UserLevelID,IsLanIDUser,CreatedBy,FacilityId) values " +
                                                " (@EmpID,@FirstName,@MiddleName,@LastName,@LoginName,@Password,@UserLevelID,@IsLanIDUser,@CreatedBy,@FacilityId)";
        private const string SQL_INSERT_CLIENTUSER = @"Config.Usp_CDS_Insert_ClientUser";
        private const string SQL_INSERT_USERROLE = @"[Config].[Usp_Insert_UserRole]";
        private const string SQL_INSERT_USERCAMPAIGN = @"INSERT INTO   CDS_UserCampaignMap(UserID,CampaignID,CreatedBy) VALUES (@UserID,@CampaignID,@CreatedBy)";
        private const string SQL_UPDATE_CLIENTUSER = @"Usp_CDS_Update_ClientUser";

        private const string SQL_UPDATE_USERLEVEL = @"UPDATE Config.tblUserMaster set UserLevelID=@UserLevelID WHERE UserID=@UserID";
        private const string SQL_UPDATE_PD = @"UPDATE Config.tblUserMaster SET Password=@Password,ModifiedOn=GetDate()  WHERE UserID=@UserID";
        private const string SQL_UPDATE_USERSTATUS = @"UPDATE Config.tblUserMaster SET Disabled=1 WHERE UserID=@UserID";
        private const string SQL_DELETE_USER = @"DELETE FROM Config.tblUserMaster WHERE UserID=@UserID";
        private const string SQL_DELETE_USERPROCESSMAP = @"Declare @RequestId as int                                                            
                                                            select @RequestId=RequestId from Config.tblUserRequestApproval where userid=@userid and IsApproved=0 and IsRejected=0 and IsCancelled=0
                                                            Delete Config.tblUserApproval where userid=@userid 
                                                            DELETE Config.tblUserProcessMap_Approval WHERE RequestId=@RequestId 
                                                            DELETE Config.tblUserProcessMap WHERE UserID=@UserID
                                                            Delete Config.tblUserRequestApproval where RequestId=@RequestId
                                                            Delete Config.tblUserRequest_Approvers where RequestId=@RequestId";
        private const string SQL_DELETE_USERROLES = @"Declare @RequestId as int                                                            
                                                        select @RequestId=RequestId from Config.tblUserRequestApproval where userid=@userid and IsApproved=0 and IsRejected=0 and IsCancelled=0
                                                        Delete Config.tblUserRoleMapping_Approval where RequestId=@RequestId 
                                                        Delete Config.tblUserRoleMapping where userid=@userid DELETE FROM Config.tblUserRoleMapping WHERE UserID=@UserID";
        private const string SQL_DELETE_USERROLE = @"Begin Update Config.tblUserRoleMapping set disabled=1 WHERE UserID=@UserID and RoleId=@RoleId
                                                        Update Config.tblUserProcessMap set disabled=1 WHERE UserID=@UserID and ClientId=@ClientId and Mapped=1
                                                        End";
        private const string SQL_SELECT_USERROLESLIST = @"select distinct Roles.RoleId,RoleName from Config.tblUserRoleMapping UserRole (nolock) 
	                                                inner join  Config.tblRoleMaster Roles (nolock) on UserRole.RoleId=Roles.RoleId and  ISNULL(Roles.IsClientRole,0)<>1
                                                     where UserId=@UserID and UserRole.Disabled=0 and Roles.Disabled=0";
        private const string SQL_SELECT_USERROLES_LOGIN = @"SELECT distinct Roles.RoleId,RoleName FROM Config.tblUserRoleMapping UserRole (nolock) 
                                                       INNER JOIN Config.tblRoleMaster Roles (NOLOCK) ON UserRole.RoleId=Roles.RoleId 
                                                       WHERE UserId=@UserID and UserRole.Disabled=0 and Roles.Disabled=0";
        private const string SQL_SELECT_USERROLES = @"select distinct Roles.RoleId,RoleName from Config.tblUserRoleMapping UserRole (nolock) 
	                                                inner join  Config.tblRoleMaster Roles (nolock) on UserRole.RoleId=Roles.RoleId and ISNULL(Roles.IsClientRole,0)<>1
                                                     where UserId=@UserID and CampaignId=@CampaignID";

        private const string SQL_UPDATE_USERTYPE_LANID = "update Config.tblUserMaster set IsLANIdUser=1, Password=null where LoginName=@LoginName";
        private const string SQL_UPDATE_USERTYPE_NONLANID = "update Config.tblUserMaster set IsLANIdUser=0, Password='jfhFhWK/oKjO9GxZRRsy2Q==' where LoginName=@LoginName";

        private const string SQL_UPDATE_USERPD = @"update Config.tblUserMaster set Password='jfhFhWK/oKjO9GxZRRsy2Q==' where LoginName=@LoginName";
        private const string SQL_SELECT_ISLANUSER = @"select IsLANIdUser from Config.tblUserMaster (nolock) where LoginName=@LoginName";
        private const string SQL_SELECT_USERERP_TEAM = @"Select * from dbo.fn_MyTeamData(@UserID,0) where UserID<>@UserID";
        private const string SQL_SELECT_USER_SETTING = @" SELECT Distinct UPM.UserID,URM.RoleID,ClientID,[ProcessID]=isnull(ProcessID,0),[CampaignID]=isnull(UPM.CampaignID,0),MappedOn, 1 as 'Approved' 
                                                         FROM Config.tblUserProcessMap(NOLOCK) UPM
                                                         INNER JOIN Config.tblUserRoleMapping (NOLOCK)URM
                                                         ON UPM.UserID = URM.UserID
                                                         WHERE UPM.UserID=@UserId --and RoleId=@RoleId 
                                                         and MappedOn=@MappedOn 
                                                         and UPM.Disabled=0 and URM.Disabled=0
                                                         Union 
                                                         --Un-Approved Access List for Other than Default Role
                                                         SELECT Distinct UPM.UserID,URM.RoleID,ClientID,[ProcessID]=isnull(ProcessID,0),[CampaignID]=isnull(UPM.CampaignID,0),MappedOn , 0 as 'Approved'
                                                         FROM Config.tblUserProcessMap_Approval(NOLOCK) UPM
                                                         INNER JOIN Config.tblUserRoleMapping_Approval (NOLOCK)URM
                                                         ON UPM.UserID = URM.UserID
                                                         inner join Config.tblUserRequestApproval (NOLOCK)URA 
                                                         ON UPM.RequestId=URA.RequestId and IsApproved=0 and IsRejected=0
                                                         WHERE UPM.UserID=@UserId --and RoleId=@RoleId 
                                                         and MappedOn=@MappedOn 
                                                         and UPM.Disabled=0 and URM.Disabled=0 and IsCancelled=0
                                                         Union 
                                                         --Un-Approved Access List for Default Role
                                                         SELECT Distinct UPM.UserID,URM.RoleID,ClientID,[ProcessID]=isnull(ProcessID,0),[CampaignID]=isnull(UPM.CampaignID,0),MappedOn , 0 as 'Approved'
                                                         FROM Config.tblUserProcessMap_Approval(NOLOCK) UPM
                                                         INNER JOIN Config.tblUserRoleMapping (NOLOCK)URM
                                                         ON UPM.UserID = URM.UserID
                                                         inner join Config.tblUserRequestApproval (NOLOCK)URA 
                                                         ON UPM.RequestId=URA.RequestId and IsApproved=0 and IsRejected=0 and Iscancelled=0
                                                         WHERE UPM.UserID=@UserId --and RoleId=@RoleId 
                                                         and MappedOn=@MappedOn 
                                                         and UPM.Disabled=0 and URM.Disabled=0";

        private const string SQL_SELECT_USERPROCESSMAP = @"Select UserPRocessMapID,ClientID,ProcessID,CampaignID,MappedON,UserID from Config.tblUserProcessMap where USerID=@UserID and Disabled=0";

        private const string SQL_CANCEL_ACCESSREQUEST_INBETWEEN = @"Begin 
                                                                    delete Config.tblUserProcessMap_Approval where requestid=@requestid
                                                                    delete Config.tblUserRoleMapping_Approval where requestid=@requestid 
                                                                    delete Config.tblUserRequestApproval where requestid=@requestid 
                                                                    End";

        private const string SQL_SELECT_CLIENTREQUESTAPPROVERLIST = @"select UM.UserId,LTRIM(RTRIM(ISNULL(UM.FIRSTNAME,'')+' '+ISNULL(UM.MIDDLENAME,'')+' '+ISNULL(UM.LASTNAME,''))) as UserName from Config.tblUserRoleMapping (nolock) URM
                                                                    inner join Config.tblUserMaster  (nolock) UM on URM.UserId=UM.UserId
                                                                    inner join Config.tblRoleMaster (nolock) RM on URM.RoleId=RM.RoleId and RM.Roleid in (275,276,277,278)
                                                                    inner join Config.tblUserProcessMap (nolock) UPM on URM.UserId=UPM.UserId and UPM.disabled=0
                                                                    where  URM.disabled=0 and UPM.Processid=@ProcessId";

        private const string SQL_SELECT_USERLISTWITHFMROLE = @"select Distinct UM.UserId,FirstName, LastName, Middlename,UM.Disabled,EmpId from Config.tblUserMaster (nolock) UM 
                                                            inner join Config.tblUserRoleMapping (nolock) URM on UM.UserId=URM.UserId 
                                                            where 
                                                            URM.RoleId in (262,276,1) and 
                                                            LoginName like @LoginName";

        private const string SQL_SELECT_LANGUAGE = @"select[Language],LanguageID,Culture from config.tblLanguage where Disabled=0";

        private const string SP_CHECK_AUTHORIZATION = @"Usp_CDS_CheckUserPermission";
        private const string SP_SELECT_USERCAMPAIGNS = @"Usp_CDS_GetUserCampaigns";
        private const string SP_SELECT_AGENTS = @"[WM].[Usp_GetAgentList]";
        private const string SP_SELECT_PROCESSAGENTS = @"Config.Usp_GetProcessUser";
        private const string SP_SELECT_PROCESSAGENTSAM = @"QMS.USP_GetProcessAgentAM";
        private const string SP_SELECT_PROCESSAGENTSAMCLIENTQCA = @"QMS.USP_GetProcessAgentAMClientQCA";
        private const string SP_SELECT_PROCESSAGENTSONLY = @"QMS.USP_GetProcessAgents";
        private const string SP_SELECT_PROCESSVPABOVE = @"Prompt.Usp_CDS_GetProcessVPAndAbove";
        private const string SP_UPDATE_USER = @"Config.USP_UpdateUser";
        private const string SP_LOGINUSER = @"dbo.USP_CDS_USERLOGIN";
        private const string SP_INSERT_USERCAMPAIGNROLE = @"Config.USP_INSERT_USERCAMPAIGNROLE";
        private const string SP_USERLIST = @"dbo.USP_CDS_GETUSERLIST";
        private const string SP_USERLIST_NEW = @"[dbo].[Usp_CDS_GetUserList]";

        private const string SP_USERLIST1 = @"[dbo].[Usp_CDS_GetClientUserList]";
        private const string SP_MAINTAIN_PD_LOG = @"dbo.USP_CDS_CHANGEPASSWORD";
        private const string SP_SELECT_USERAUTH = @"dbo.USP_CDS_AUTHENTICATEUSER";
        private const string SP_SELECT_USERSETTING = @"[Config].[Usp_GET_ERPUserSetting]";
        private const string SQL_SELECT_CLIENTROLESALL = @"SELECT ROLEID,ROLENAME,Description,isnull(LevelID,0) as LevelID, disabled,isnull(IsClientRole,0) as IsClientRole,createdby FROM CDS_S_RoleMaster (Nolock) WHERE RoleName like @RoleName And IsClientRole=1 Order By ROLENAME";


        private const string SP_INSERT_USERSETTING = @"dbo.USP_CDS_INSERT_USERMAPPING";
        private const string SP_CANCEL_ACCESSREQUEST = @"[Config].[Usp_Cancel_AccessRequest]";

        private const string SP_INSERT_USERMAPPINGAPPROVER = @"[Config].[Usp_Insert_SendMail_UserMappingApprover]";

        private const string SP_INSERT_USERSETTING_FORAPPROVAL = @"[Config].[Usp_Insert_UserMapping_Approval]";
        private const string SP_CHECK_USERACCESS_REQUESTTYPE = @"[Config].[Usp_Check_UserMapping_RequestType]";
        private const string SP_GET_USERACCESS_REQUESTSTATUS = @"[Config].[Usp_GetAccessRequestStatus]";
        private const string SP_GET_USERACCESS_APPROVALLIST = @"Config.Usp_GetAccessRequestApprovalList";

        private const string SP_SELECT_APPROVERLIST = @"[Config].[Usp_GetApproverListForAccessRequest]";
        private const string SP_SELECT_ROLEAPPROVERLIST = @"[Config].[USP_GetRoleApproverList]";
        private const string SP_APPROVE_ACCESSREQUEST = @"[Config].[USP_ApproveUserAccessRequest]";
        private const string SP_REJECT_ACCESSREQUEST = @"[Config].[USP_RejectUserAccessRequest]";
        private const string SP_SELECT_DEFUALTPAGE = @"Prompt.Usp_CDS_GetDefaultPage";
        private const string SP_SELECT_USERFOREXTRAROLE = @"Select * from dbo.fn_MyTeamData(@UserID,0) where userid<>userId and JobId not in (select value from Config.tblMasterTable where fieldid=61)";

        private const string SQL_CHANGE_PD = @"
                                                        Declare @DBSPd nvarchar(50)
                                                        select @DBSPd=password from Config.tblUserMaster(nolock) where LoginName= @LoginName
                                                        if @DBSPd is not null
                                                        begin
                                                         if @DBSPd=@OldPd
	                                                         Begin
		                                                        update Config.tblUserMaster set Password=@NewPd,ModifiedBy=@UserID,ModifiedOn=GetDate()  where LoginName=@LoginName
		                                                        select 'Password Changed Sucessfully.'
	                                                         End
	                                                        Else
	                                                        Begin
		                                                        select 'Wrong Password Entered.'
	                                                        End	 
	 
                                                        end
                                                        Else
	                                                        Begin
	                                                         update Config.tblUserMaster set Password=@NewPd,ModifiedBy=@UserID,ModifiedOn=GetDate()  where LoginName=@LoginName
	                                                         select 'Password Changed SucessFully.'
	                                                        End";

        private const string SP_SELECT_AGENTSTEST = @"[WM].[Usp_GetAgentListTest]";
        private const string SQL_USP_GETAPPROVERLISTUSRROLE = @"[Config].[Usp_GetApproverListUserRole]";
        private const string SQL_USP_GETAPPROVERLIST = @"[Config].[Usp_GetApproverList]";
        private const string PARAM_GETDOMAIN = @"[Config].[USP_DomainActivation]";
        private const string SQL_SELECT_MENTORWITHTMID = @"Usp_CDS_GetMentorWithTMidAndDate";

        private const string PARAM_AUTHENTICATIONTYPE = @"@IsWindowsAuthentication";
        private const string PARAM_USERPAGELOGIN = @"[dbo].[USP_UserPageLogin]";

        private const string PARAM_URLREFERRER = "@URLReferrer";
        private const string PARAM_RAWURL = "@RawURL";
        private const string PARAM_ISNEWSESSION = "@IsNewSession";
        private const string PARAM_SESSIONTIMEOUT = "@SessionTimeOut";
        private const string PARAM_SYSTEMSESSIONID = "@SystemSessionID";
        private const string PARAM_IPADDRESS = "@IPAddress";
        private const string PARAM_SERVERNAME = "@ServerName";
        private const string PARAM_USERAGENT = "@UserAgent";
        private const string PARAM_USERIP = "@UserIp";
        private const string PARAM_USERNAME = "@UserName";
        private const string PARAM_CLIENTS = "@Clients";
        private const string PARAM_USERID = "@UserID";
        private const string PARAM_EMPID = "@EmpID";
        private const string PARAM_LANUSER = "@IsLanIDUser";
        private const string PARAM_ISCLIENTUSER = "@IsClient";
        private const string PARAM_FIRSTNAME = "@FirstName";
        private const string PARAM_MIDDLENAME = "@MiddleName";
        private const string PARAM_LASTNAME = "@LastName";
        private const string PARAM_EMAIL = "@Email";
        private const string PARAM_LOGINNAME = "@LoginName";
        private const string PARAM_NAME = "@Name";
        private const string PARAM_PD = "@Password";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_ISBOT = "@IsBOT";
        private const string PARAM_USERLEVEL = "@UserLevelID";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_MAPPEDON = "@MappedON";
        private const string PARAM_ROLEID = "@RoleID";
        private const string PARAM_CAMPAIGNID = "@CampaignID";
        private const string PARAM_CAMPAIGNNAME = "@CampaignName";
        private const string PARAM_PROCESSID = "@ProcessId";
        private const string PARAM_PROCESS = "@Process";
        private const string PARAM_DELETEDPROCESS = "@DeletedProcess";

        private const string PARAM_CLIENTID = "@ClientId";

        private const string PARAM_ACTIONNAME = "@ActionName";
        private const string PARAM_FORMID = "@FormID";
        private const string PARAM_ACTIVEUSERLIST = "@ActiveUserList";
        private const string PARAM_CURRENTUSER = "@CurrentUserID";
        private const string PARAM_ROLECAMPAIGN = @"RoleCampaignString";
        private const string PARAM_USERFACILITY = "@FacilityId";
        private const string PARAM_SESSIONID = "@SessionID";
        private const string PARAM_HOSTNAME = "@HostName";

        private const string PARAM_TEAMID = "@TeamId";
        private const string PARAM_USER1 = "@user1";
        private const string PARAM_USER2 = "@user2";
        private const string PARAM_DELETEDNODES = "@DeletedNodes";
        private const string PARAM_REQUESTID = "@RequestId";
        private const string PARAM_REQUESTTYPE = "@RequestType";
        private const string PARAM_REQUESTTYPEID = "@RequestTypeId";
        private const string PARAM_APPROVER1ID = "@Approver1Id";
        private const string PARAM_APPROVER2ID = "@Approver2Id";
        private const string PARAM_APPROVERID = "@ApproverId";

        private const string PARAM_FLAG = "@Flag";
        private const string PARAM_FROMDATE = "@FromDate";
        private const string PARAM_TODATE = "@ToDate";
        private const string PARAM_APPROVALLEVEL = "@ApprovalLevel";

        private const string PARAM_MODE = "@Mode";
        private const string PARAM_EFFECTIVEDATE = "@EffectiveDate";
        private const string PARAM_STARTDATE = "@StartDate";
        private const string PARAM_ENDDATE = "@EndDate";

        private const string PARAM_DOJ = "@DOJ";
        private const string PARAM_FACILITYID = "@FacilityId";
        private const string PARAM_SUPERVISORID = "@SupervisorId";

        private const string PARAM_JOBID = "@JobId";

        private const string PARAM_SEARCHCONDITION = "@SearchCondition";
        private const string PARAM_LOBID = "@LOBId";
        private const string PARAM_SBUID = "@SBUId";
        private const string PARAM_TMID = "@TMid";
        private const string PARAM_OLDPD = "@OldPd";
        private const string PARAM_NEWPD = "@NewPd";
        private const string PARAM_PageName = "@PageName";
        public List<BEUserInfo> IsLDAPUser(string LoginName, BESession oBESession, out int iSessionID, out bool bProcessMap, bool isMossApplicationMenu, bool isWindowsAuthentication = true)
        {
            iSessionID = 0;
            bProcessMap = false;
            int processMaped = 0;
            string @error = "";
            List<BEUserInfo> lUser = new List<BEUserInfo>();
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbSelectCommand = db.GetStoredProcCommand(SP_LOGINUSER);

            db.AddInParameter(dbSelectCommand, PARAM_LOGINNAME, DbType.String, LoginName);
            db.AddInParameter(dbSelectCommand, PARAM_SYSTEMSESSIONID, DbType.String, oBESession.sSystemSessionID);
            db.AddInParameter(dbSelectCommand, PARAM_IPADDRESS, DbType.String, oBESession.sIPAddress);
            db.AddInParameter(dbSelectCommand, PARAM_HOSTNAME, DbType.String, oBESession.sHostName);
            db.AddParameter(dbSelectCommand, "@ErrorStatus", DbType.String, ParameterDirection.Output, "@ErrorStatus", DataRowVersion.Default, @error);
            db.AddInParameter(dbSelectCommand, PARAM_AUTHENTICATIONTYPE, DbType.Boolean, isWindowsAuthentication);


            DbCommand dbCommandRole = db.GetSqlStringCommand(SQL_SELECT_USERROLES_LOGIN);
            db.AddInParameter(dbCommandRole, PARAM_USERID, DbType.Int32);

            List<BERoleInfo> dsRole = null;
            //Vipul
            DbCommand dbCommandLanguage = db.GetSqlStringCommand(SQL_SELECT_LANGUAGE);
            List<BELanguages> dsLanguage = null;

            using (IDataReader rdr = db.ExecuteReader(dbSelectCommand))
            {

                while (rdr.Read())
                {
                    iSessionID = int.Parse(rdr["maxSessionID"].ToString());
                    processMaped = int.Parse(rdr["ProcessMap"].ToString());
                    if (processMaped > 0) bProcessMap = true;

                    BEUserInfo oUser = new BEUserInfo
                    {
                        iUserID = Convert.ToInt32(rdr["UserID"]),
                        iEmployeeID = Convert.ToInt32(rdr["EmpID"]),
                        sFirstName = rdr["FirstName"].ToString(),
                        sMiddleName = rdr["MiddleName"].ToString(),
                        sLastName = rdr["LastName"].ToString(),
                        sEmail = rdr["EMailID"].ToString(),
                        sLoginName = rdr["LoginName"].ToString(),
                        sPassword = rdr["Password"] == DBNull.Value ? null : rdr["Password"].ToString(),
                        iUserLevel = Convert.ToInt32(rdr["UserLevelID"]),
                        bLanID = Convert.ToBoolean(rdr["IsLanIDUser"]),
                        bDisabled = Convert.ToBoolean(rdr["disabled"]),
                        iCreatedBy = Convert.ToInt32(rdr["CreatedBy"]),
                        iUserFacility = Convert.ToInt32(rdr["FacilityId"]),
                        iTimeZoneID = rdr["TimeZoneID"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["TimeZoneID"]),
                        sUserTimeZone = rdr["TimeZoneName"] == DBNull.Value ? null : Convert.ToString(rdr["TimeZoneName"]),
                        sLanguage = rdr["Language"] == DBNull.Value ? null : Convert.ToString(rdr["Language"]),
                        sServerTimeZone = rdr["ServerTimeZone"] == DBNull.Value ? null : Convert.ToString(rdr["ServerTimeZone"])
                    };


                    dsRole = new List<BERoleInfo>();
                    db.SetParameterValue(dbCommandRole, PARAM_USERID, oUser.iUserID);
                    using (IDataReader rdrole = db.ExecuteReader(dbCommandRole))
                    {
                        while (rdrole.Read())
                        {
                            BERoleInfo oRole = new BERoleInfo();
                            oRole.iRoleID = int.Parse(rdrole["RoleID"].ToString());
                            oRole.sRoleName = rdrole["RoleName"].ToString();
                            dsRole.Add(oRole);
                            oRole = null;
                        }
                    }

                    dsLanguage = new List<BELanguages>();

                    using (IDataReader rdLanguage = db.ExecuteReader(dbCommandLanguage))
                    {
                        while (rdLanguage.Read())
                        {
                            BELanguages oLanguage = new BELanguages();
                            oLanguage.iLanguageID = int.Parse(rdLanguage["LanguageID"].ToString());
                            oLanguage.sLanguage = rdLanguage["Language"].ToString();
                            oLanguage.sCulture = rdLanguage["Culture"].ToString();
                            dsLanguage.Add(oLanguage);
                            oLanguage = null;
                        }
                    }
                    oUser.oLanguage = dsLanguage;

                    oUser.oRoles = dsRole;
                    lUser.Add(oUser);
                    oUser = null;
                    dsRole = null;
                }
                @error = (db.GetParameterValue(dbSelectCommand, "@ErrorStatus") == null) ? "" : db.GetParameterValue(dbSelectCommand, "@ErrorStatus").ToString();
                if (@error != "" || @error != string.Empty)
                {
                    //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(@error);
                }
            }
            return lUser;
        }
        public DataSet GetUserListD(string sLoginName, bool bActiveUser, int iLoggedinUserID, int iClientUser, string SearchCondition)
        {
            DataSet lUser = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            string sQuery = iClientUser == 0 ? SP_USERLIST : SP_USERLIST1;
            DbCommand dbCommand = db.GetStoredProcCommand(sQuery);
            db.AddInParameter(dbCommand, PARAM_LOGINNAME, DbType.String, sLoginName);
            db.AddInParameter(dbCommand, PARAM_CURRENTUSER, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVEUSERLIST, DbType.Boolean, bActiveUser);
            db.AddInParameter(dbCommand, PARAM_SEARCHCONDITION, DbType.String, SearchCondition);
            lUser = db.ExecuteDataSet(dbCommand);
            return lUser;
        }

        public void InsertData(BEUserInfo oUserData)
        {
            try
            {
                //*************************************Insert User Data

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbInsertUserCommand = db.GetSqlStringCommand(SQL_INSERT_USER);
                db.AddInParameter(dbInsertUserCommand, PARAM_EMPID, DbType.Int32, oUserData.iEmployeeID);
                db.AddInParameter(dbInsertUserCommand, PARAM_LANUSER, DbType.Boolean, oUserData.bLanID);
                db.AddInParameter(dbInsertUserCommand, PARAM_FIRSTNAME, DbType.String, oUserData.sFirstName);
                db.AddInParameter(dbInsertUserCommand, PARAM_MIDDLENAME, DbType.String, oUserData.sMiddleName);
                db.AddInParameter(dbInsertUserCommand, PARAM_LASTNAME, DbType.String, oUserData.sLastName);
                db.AddInParameter(dbInsertUserCommand, PARAM_LOGINNAME, DbType.String, oUserData.sLoginName);
                db.AddInParameter(dbInsertUserCommand, PARAM_PD, DbType.String, oUserData.sPassword);
                db.AddInParameter(dbInsertUserCommand, PARAM_DISABLED, DbType.Boolean, oUserData.bDisabled);
                db.AddInParameter(dbInsertUserCommand, PARAM_USERLEVEL, DbType.Int32, oUserData.iUserLevel);
                db.AddInParameter(dbInsertUserCommand, PARAM_CREATEDBY, DbType.Int32, oUserData.iCreatedBy);
                db.AddInParameter(dbInsertUserCommand, PARAM_USERFACILITY, DbType.Int32, oUserData.iUserFacility);

                //************************************INSERT USER CAMPAIGN ROLE
                DbCommand dbInsertCampaignRoleCommand = db.GetStoredProcCommand(SP_INSERT_USERCAMPAIGNROLE);
                //db.AddInParameter(dbInsertCampaignCommand, PARAM_CAMPAIGNID, DbType.Int32);
                db.AddInParameter(dbInsertCampaignRoleCommand, PARAM_ROLECAMPAIGN, DbType.String, oUserData.sRoleCampaign);
                db.AddInParameter(dbInsertCampaignRoleCommand, PARAM_USERID, DbType.Int32);
                db.AddInParameter(dbInsertCampaignRoleCommand, PARAM_CREATEDBY, DbType.Int32, oUserData.iCreatedBy);

                //*************************************Insert User Campaign
                DbCommand dbInsertCampaignCommand = db.GetSqlStringCommand(SQL_INSERT_USERCAMPAIGN);
                db.AddInParameter(dbInsertCampaignCommand, PARAM_CAMPAIGNID, DbType.Int32);
                db.AddInParameter(dbInsertCampaignCommand, PARAM_USERID, DbType.Int32);
                db.AddInParameter(dbInsertCampaignCommand, PARAM_CREATEDBY, DbType.Int32, oUserData.iCreatedBy);


                DbCommand dbChangePasswordCommand = db.GetStoredProcCommand(SP_MAINTAIN_PD_LOG);
                db.AddInParameter(dbChangePasswordCommand, PARAM_PD, DbType.String, oUserData.sPassword);
                db.AddInParameter(dbChangePasswordCommand, PARAM_USERID, DbType.Int32);


                //*************************************Insert Role Campaign
                //DbCommand dbInsertRolesCommand = db.GetSqlStringCommand(SQL_INSERT_USERROLES);
                //db.AddInParameter(dbInsertRolesCommand, PARAM_ROLEID, DbType.Int32);
                //db.AddInParameter(dbInsertRolesCommand, PARAM_USERID, DbType.Int32);
                //db.AddInParameter(dbInsertRolesCommand, PARAM_CREATEDBY, DbType.Int32, oUserData.iCreatedBy);

                //*************************************Get Max Id After Insertion
                DbCommand dbSelectMaxCommand = db.GetSqlStringCommand(SQL_SELECT_MAXUSERID);


                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbInsertUserCommand, trans);

                            //Get Max Id from database for the newly added user
                            int MaxID = (int)db.ExecuteScalar(dbSelectMaxCommand, trans);
                            if (oUserData.bLanID == false)
                            {
                                db.SetParameterValue(dbChangePasswordCommand, PARAM_USERID, MaxID);
                                db.ExecuteNonQuery(dbChangePasswordCommand, trans);
                            }
                            db.SetParameterValue(dbInsertCampaignRoleCommand, PARAM_USERID, MaxID);
                            db.ExecuteNonQuery(dbInsertCampaignRoleCommand, trans);


                            trans.Commit(); //Commit Transaction
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();//Transaction RollBack
                            throw ex;
                        }
                    }
                    conn.Close();
                }
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }
        public void UpdateData(BEUserInfo oUserData)
        {

            //*************************************

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbUpdateUserCommand = db.GetStoredProcCommand(SP_UPDATE_USER);
            db.AddInParameter(dbUpdateUserCommand, PARAM_USERID, DbType.Int32, oUserData.iUserID);
            db.AddInParameter(dbUpdateUserCommand, PARAM_EMPID, DbType.Int32, oUserData.iEmployeeID);
            db.AddInParameter(dbUpdateUserCommand, PARAM_LANUSER, DbType.Boolean, oUserData.bLanID);
            db.AddInParameter(dbUpdateUserCommand, PARAM_FIRSTNAME, DbType.String, oUserData.sFirstName);
            db.AddInParameter(dbUpdateUserCommand, PARAM_MIDDLENAME, DbType.String, oUserData.sMiddleName);
            db.AddInParameter(dbUpdateUserCommand, PARAM_LASTNAME, DbType.String, oUserData.sLastName);
            db.AddInParameter(dbUpdateUserCommand, PARAM_LOGINNAME, DbType.String, oUserData.sLoginName);
            db.AddInParameter(dbUpdateUserCommand, PARAM_PD, DbType.String, oUserData.sPassword);
            db.AddInParameter(dbUpdateUserCommand, PARAM_DISABLED, DbType.Boolean, oUserData.bDisabled);
            db.AddInParameter(dbUpdateUserCommand, PARAM_USERLEVEL, DbType.Int32, oUserData.iUserLevel);
            db.AddInParameter(dbUpdateUserCommand, PARAM_CREATEDBY, DbType.Int32, oUserData.iCreatedBy);


            db.AddInParameter(dbUpdateUserCommand, PARAM_USERFACILITY, DbType.Int32, oUserData.iUserFacility);



            //************************************INSERT USER CAMPAIGN ROLE
            DbCommand dbInsertCampaignRoleCommand = db.GetStoredProcCommand(SP_INSERT_USERCAMPAIGNROLE);
            db.AddInParameter(dbInsertCampaignRoleCommand, PARAM_ROLECAMPAIGN, DbType.String, oUserData.sRoleCampaign);
            db.AddInParameter(dbInsertCampaignRoleCommand, PARAM_USERID, DbType.Int32, oUserData.iUserID);
            db.AddInParameter(dbInsertCampaignRoleCommand, PARAM_CREATEDBY, DbType.Int32, oUserData.iCreatedBy);
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open
                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        db.ExecuteNonQuery(dbUpdateUserCommand, trans);
                        db.ExecuteNonQuery(dbInsertCampaignRoleCommand, trans);

                        trans.Commit(); //Commit Transaction
                    }

                    /*
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        trans.Rollback();
                        if (ex.Number == 547)
                        {
                            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                        }
                        if (ex.Number == 2627)
                        {
                            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                        }
                        throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }*/
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }
                    conn.Close();
                }
            }
        }

        public bool IsAuthorization(int FormID, int UserID, string Action)
        {
            bool CheckAuthorization = false;

            Database db = DL_Shared.dbFactory(_oTenant); //DatabaseFactory.CreateDatabase(DL_Shared.Connection);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_CHECK_AUTHORIZATION);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserID);
            db.AddInParameter(dbCommand, PARAM_FORMID, DbType.Int32, FormID);
            db.AddInParameter(dbCommand, PARAM_ACTIONNAME, DbType.String, Action.ToUpper());
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    if (rdr.GetBoolean(0) == true)
                    { CheckAuthorization = true; }
                }
            }
            CheckAuthorization = true;
            return CheckAuthorization;
        }
    }
}
