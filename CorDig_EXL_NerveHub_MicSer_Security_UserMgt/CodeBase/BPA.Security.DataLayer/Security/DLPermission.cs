/* Copyright © 2007 company 
 * project Name                 :Datalayer
 * Class Name                   :DLPermission
 * Namespace                    :BPA.Security.Datalayer.Security
 * Purpose                      :Manage User Permission. assign campaign, role and Skill set to user
 * Description                  :
 * Dependency                   :
 * Related Table                :CDS_UserCampaignMap,Config.tblUserRoleMapping,Config.tblUserMaster
 * Related Class                :
 * Related StoreProcdure        :
 * Author                       :Tarun Bounthiyal
 * Created on                   :2-mar-2007
 * Reviewed on                  :          
 * Reviewed by                  :
 * Tested on                    :
 * Tested by                    :
 * Modification history         :
 * modify1 on                   :
 * modify1 By                   :
 * Overall effect               :
 */


using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace BPA.Security.Datalayer.Security
{
    /// <summary>
    /// User Permission 
    /// </summary>
    public class DLPermission : IDisposable
    {
        private BETenant _oTenant = null;
        #region Fields
        private string SP_GET_USERLISTBYCLIENT = @"Config.UspGetUserListByClient";
        private const string SQL_SELECT_USER_PROCESSFAMILYOWNER = @"select UR.USERID, ltrim(isnull(UR.FirstName,'') + ' '+isnull(UR.MiddleName,'')+' ' + isnull(UR.LastName,'') +' ( '+isnull(UR.LoginName,'')+' )')  as Name from Config.tblUserMaster(nolock) UR where disabled=0 and UR.FirstName Like @Name or  UR.LastName Like @Name order by UR.Firstname,UR.Middlename,UR.LastName";
        private const string SQL_SELECT_USER_MONTEEBASED = @"select UR.FirstName +' ' + UR.LastName from Config.tblUserMaster(nolock) UR where UR.userid in (@user1, @user2)";
        private const string SP_SELECT_MONTEE = @"select UM.UserID,UM.EmpID,UM.FirstName,UM.MiddleName,UM.LastName,UM.LoginName,
                                                UM.Password,isnull(UM.UserLevelID,0) as UserLevelID ,UM.disabled,UM.IsLanIDUser,'FacilityId'=isnull(UM.FacilityId,0),isnull(UM.createdby,0) as createdby from Config.tblUserProcessMap(nolock) UPM,Config.tblUserMaster(nolock) UM where 
                                                UM.UserID = UPM.UserID
                                                and  UPM.ProcessID=@ProcessID";
        //Dead Code: Unused Field
        // private const string SP_INSERT_USERCAMP = @"Insert into Config.tblUserRoleMapping(UserID,CampaignID,CreatedBy) VALUES(@UserID,@CampaignID,@CreatedBy)";
        private const string SQL_CHECKADMIN = @"Select [IsAdmin]=dbo.fn_CheckAdmin(@UserID)";
        // private const string SQL_SELECT_USERTEAMID = @"Select * from dbo.fn_MyTeamData (@UserID,1)";

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
        //Dead Code: Unused Field
        //        private const string SQL_SELECT_USERROLEID = @"SELECT SR.RoleID, SR.RoleName ,Camapign.CampaignID
        //	                                                    FROM Config.tblUserRoleMapping AS URM (Nolock) 
        //	                                                    INNER JOIN Config.tblRoleMaster AS SR (Nolock) 
        //	                                                    ON SR.RoleID = URM.RoleID 
        //	                                                    INNER JOIN Config.tblCampaignMaster As Camapign (NOlock)
        //	                                                    ON Camapign.CampaignID=URM.CampaignID
        //	                                                    WHERE (URM.UserID =@UserID) AND (SR.Disabled = 0) AND (URM.Disabled = 0)";

        //private const string SQL_SELECT_USERCAMPID = @"SELECT Camp.CampaignID, Camp.CampaignName FROM Config.tblCampaignMaster AS Camp WITH (NOLOCK) INNER JOIN" +
        //                                          " CDS_UserCampaignMap AS UC WITH (NOLOCK) ON UC.CampaignID = Camp.CampaignID WHERE (Camp.Disabled = 0) AND (UC.Disabled = 0) AND (UC.UserID = @UserID)";

        private const string SQL_SELECT_CAMPAIGNUSERS = @"select UserMaster.UserID, FirstName + ' ' + MiddleName + ' ' + LastName as UserName from Config.tblUserRoleMapping CampMap (nolock) inner join Config.tblUserMaster UserMaster (nolock) on CampMap.UserID=UserMaster.UserID where CampMap.Disabled=0 and CampaignID=@CampaignID AND RoleID=9";
        //Dead Code: Unused Field
        // private const string SQL_SELECT_CHECKCAMAPIGNMEMBER = @"SELECT * FROM CDS_UserCampaignMap (Nolock) Where CampaignID=@CampaignID and UserID =@UserID";
        //private const string SQL_SELECT_CHECKROLESMEMBER = @"SELECT * FROM Config.tblUserRoleMapping (Nolock) Where RoleID=@RoleID and UserID =@UserID";

        private const string SQL_SELECT_USERROLECAMPAIGN = @"SELECT Distinct UserID,RoleID,CreatedBy FROM Config.tblUserRoleMapping WHERE UserID=@UserID AND Disabled=0";

        private const string SQL_SELCET_USERID = @"SELECT UserID,ModifiedOn FROM Config.tblUserMaster (nolock) WHERE LoginName=@LoginName";

        private const string SQL_SELCET_LASTPDCHANGE = @"SELECT top 1 UserMaster.UserID,Pass.CreatedOn FROM Config.tblUserMaster UserMaster (nolock) inner join  CDS_PasswordLog Pass (nolock) on UserMaster.UserId=Pass.UserId WHERE LoginName=@LoginName order by Pass.CreatedOn desc";

        private const string SQL_SELECT_PD = @"SELECT Password FROM Config.tblUserMaster WHERE UserID=@UserID";

        private const string SQL_INSERT_UNSUCCESSFULLLOGIN = @"INSERT INTO CDS_Login_Unsucessfull(UserID,SessionID,HostName) VALUES(@UserID,@SessionID,@HostName)";
        private const string SQL_INSERT_USER = @"INSERT INTO Config.tblUserMaster (EmpID,FirstName,MiddleName,LastName,LoginName,Password,UserLevelID,IsLanIDUser,CreatedBy,FacilityId) values " +
                                                " (@EmpID,@FirstName,@MiddleName,@LastName,@LoginName,@Password,@UserLevelID,@IsLanIDUser,@CreatedBy,@FacilityId)";
        private const string SQL_INSERT_CLIENTUSER = @"Config.Usp_CDS_Insert_ClientUser";
        private const string SQL_INSERT_USERROLE = @"[Config].[Usp_Insert_UserRole]";
        //Dead Code: Unused Field
        //private const string SQL_INSERT_USERROLES = @"INSERT INTO  Config.tblUserRoleMapping (UserID,RoleID,CreatedBy) VALUES (@UserID,@RoleID,@CreatedBy)";
        private const string SQL_INSERT_USERCAMPAIGN = @"INSERT INTO   CDS_UserCampaignMap(UserID,CampaignID,CreatedBy) VALUES (@UserID,@CampaignID,@CreatedBy)";
        private const string SQL_UPDATE_CLIENTUSER = @"Usp_CDS_Update_ClientUser";

        private const string SQL_UPDATE_USERLEVEL = @"UPDATE Config.tblUserMaster set UserLevelID=@UserLevelID WHERE UserID=@UserID";
        //Dead Code: Unused Field
        //private const string SQL_UPDATE_INACTIVATECAMPMEMBER = @"UPDATE  CDS_UserCampaignMap SET Disabled=1 WHERE UserID=@UserID";
        //private const string SQL_UPDATE_ACTIVATECAMPMEMBER = @"UPDATE  CDS_UserCampaignMap SET Disabled=0 WHERE CampaignID=@CampaignID AND UserID=@UserID";
        //private const string SQL_UPDATE_INACTIVATEROLEMEMBER = @"UPDATE Config.tblUserRoleMapping SET Disabled=1 WHERE UserID=@UserID";
        //Dead Code: Unused Field
        //private const string SQL_UPDATE_ACTIVATEROLEMEMBER = @"UPDATE Config.tblUserRoleMapping SET Disabled=0 WHERE RoleID=@RoleID AND UserID=@UserID";
        private const string SQL_UPDATE_PD = @"UPDATE Config.tblUserMaster SET Password=@Password,ModifiedOn=GetDate()  WHERE UserID=@UserID";
        private const string SQL_UPDATE_USERSTATUS = @"UPDATE Config.tblUserMaster SET Disabled=1 WHERE UserID=@UserID";
        //Dead Code: Unused Field
        //private const string SQL_UPDATE_USERROLES = @"INSERT INTO  Config.tblUserRoleMapping (UserID,RoleID,CreatedBy) VALUES (@UserID,@RoleID,@CreatedBy)";
        // private const string SQL_UPDATE_USERCAMPAIGN = @"INSERT INTO  Config.tblUserRoleMapping (UserID,CampaignID,CreatedBy) VALUES (@UserID,@CampaignID,@CreatedBy)";

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
        //Dead Code: Unused Field
        //private const string SQL_DELETE_USERCAMPAIGN = @"DELETE FROM CDS_UserCampaignMap WHERE UserID=@UserID";
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
        //        private const string SQL_SELECT_USER_SETTING = @"SELECT Distinct UPM.UserID,URM.RoleID,ClientID,[ProcessID]=isnull(ProcessID,0),[CampaignID]=isnull(UPM.CampaignID,0),MappedOn 
        //                                                            FROM Config.tblUserProcessMap(NOLOCK) UPM
        //                                                            INNER JOIN Config.tblUserRoleMapping (NOLOCK)URM
        //                                                            ON UPM.UserID = URM.UserID
        //                                                            WHERE UPM.UserID=@UserID and RoleId=@RoleId and MappedOn=@MappedOn 
        //                                                            and UPM.Disabled=0 and URM.Disabled=0";
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
        private const string SP_SELECT_AGENTSNEW = @"[WM].[Usp_GetAgentListNew]";
        private const string SP_SELECT_PROCESSAGENTS = @"Config.Usp_GetProcessUser";
        private const string SP_SELECT_PROCESSAGENTSAM = @"QMS.USP_GetProcessAgentAM";
        private const string SP_SELECT_PROCESSAGENTSAMCLIENTQCA = @"QMS.USP_GetProcessAgentAMClientQCA";
        private const string SP_SELECT_PROCESSAGENTSONLY = @"QMS.USP_GetProcessAgents";
        private const string SP_SELECT_PROCESSVPABOVE = @"Prompt.Usp_CDS_GetProcessVPAndAbove";
        private const string SP_UPDATE_USER = @"Config.USP_UpdateUser";
        private const string SP_LOGINUSER = @"dbo.USP_CDS_USERLOGIN";
        private const string SP_INSERT_USERCAMPAIGNROLE = @"Config.USP_INSERT_USERCAMPAIGNROLE";
        private const string SP_USERLIST = @"dbo.USP_CDS_GETUSERLIST";
        //new for test 
        //private const string SP_USERLIST_NEW = @"[dbo].[Usp_CDS_GetUserList_Test]";
        private const string SP_USERLIST_NEW = @"[dbo].[Usp_CDS_GetUserList]";

        private const string SP_USERLIST1 = @"[dbo].[Usp_CDS_GetClientUserList]";
        private const string SP_MAINTAIN_PD_LOG = @"dbo.USP_CDS_CHANGEPASSWORD";
        private const string SP_SELECT_USERAUTH = @"dbo.USP_CDS_AUTHENTICATEUSER";

        //BIND Roles                                                                             
        private const string SP_SELECT_USERSETTING = @"[Config].[Usp_GET_ERPUserSetting]";

        //Get role list of Client user
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
        //Dead Code: Unused Field
        //        private const string SQL_SELECT_TESTGROUPING = @"SELECT        Config.tblClientMaster.ClientID, Config.tblClientMaster.ClientName, Config.tblProcessMaster.ProcessID, Config.tblProcessMaster.ProcessName
        //                                                                  FROM            Config.tblClientMaster INNER JOIN
        //                                                                    Config.tblProcessMaster ON Config.tblClientMaster.ClientID = Config.tblProcessMaster.ClientID";


        private const string SQL_USP_GETAPPROVERLISTUSRROLE = @"[Config].[Usp_GetApproverListUserRole]";
        private const string SQL_USP_GETAPPROVERLIST = @"[Config].[Usp_GetApproverList]";
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


        //added by Omkar
        private const string PARAM_LOBID = "@LOBId";
        private const string PARAM_SBUID = "@SBUId";
        private const string PARAM_TMID = "@TMid";
        private const string PARAM_OLDPD = "@OldPd";
        private const string PARAM_NEWPD = "@NewPd";

        private const string PARAM_GETDOMAIN = @"[Config].[USP_DomainActivation]";


        //        private const string SQL_SELECT_MENTORWITHTMID = @"Select distinct UM.UserID, FirstName + ' ' + coalesce(MiddleName, '') + ' ' + coalesce(LastName, '')  + ' (' + Convert(varchar, EmpId) + ')' As Agent 
        //                                                           From Config.tblUserMaster (nolock) UM inner join Prompt.CDS_P_SampleTransaction (nolock) ST on UM.UserId=ST.CreatedBy
        //                                                           inner join Prompt.CDS_P_Sampling (NOLOCK) Sampling on ST.SampleId=Sampling.SampleId
        //                                                           where ST.IsRejected=1  and Convert(varchar(10),ST.CreatedOn,101) between @StartDate And @EndDate AND Sampling.TMDefinitionID=@TMid";
        private const string SQL_SELECT_MENTORWITHTMID = @"Usp_CDS_GetMentorWithTMidAndDate";

        private const string PARAM_AUTHENTICATIONTYPE = @"@IsWindowsAuthentication";
        private const string PARAM_USERPAGELOGIN = @"[dbo].[USP_UserPageLogin]";

        private const string PARAM_PageName = "@PageName";
        #endregion

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLPermission"/> class.
        /// </summary>
        public DLPermission(BETenant oTenant)
        { _oTenant = oTenant; }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        #region GetUser List
        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <returns></returns>
        public List<BEUserInfo> GetUserList(bool bActiveUser, int iLoggedinUserID, int iClientUser, string SearchCondition)
        {
            return GetUserList("", bActiveUser, iLoggedinUserID, iClientUser, SearchCondition);
        }
        #endregion

        #region IsLDAPUser
        /// <summary>
        /// Determines whether [is LDAP user] [the specified login name].
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <param name="oBESession">The BE session.</param>
        /// <param name="iSessionID">The session ID.</param>
        /// <param name="bProcessMap">if set to <c>true</c> [process map].</param>
        /// <returns>
        /// 	<c>true</c> if [is LDAP user] [the specified login name]; otherwise, <c>false</c>.
        /// </returns>
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
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(@error);
                }
            }
            return lUser;
        }


        public string InsertUpdatePassword(string LoginName, string sOldPd, string sNewPd, int iUserId)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbUpdateCommand = db.GetSqlStringCommand(SQL_CHANGE_PD);
            db.AddInParameter(dbUpdateCommand, PARAM_LOGINNAME, DbType.String, LoginName);
            db.AddInParameter(dbUpdateCommand, PARAM_OLDPD, DbType.String, sOldPd);
            db.AddInParameter(dbUpdateCommand, PARAM_NEWPD, DbType.String, sNewPd);
            db.AddInParameter(dbUpdateCommand, PARAM_USERID, DbType.Int32, iUserId);
            return db.ExecuteScalar(dbUpdateCommand).ToString();
        }


        #endregion

        #region Gets the user list.


        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="sLoginName">Name of the s login.</param>
        /// <param name="bActiveUser">if set to <c>true</c> [active user].</param>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <returns></returns>
        /// 



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





        public List<BEUserInfo> GetUserList(string sLoginName, bool bActiveUser, int iLoggedinUserID, int iClientUser, string SearchCondition)
        {
            List<BEUserInfo> lUser = new List<BEUserInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            //string sQuery = iClientUser == 0 ? SP_USERLIST : SP_USERLIST1;
            string sQuery = iClientUser == 0 ? SP_USERLIST_NEW : SP_USERLIST1;
            DbCommand dbCommand = db.GetStoredProcCommand(sQuery);
            db.AddInParameter(dbCommand, PARAM_LOGINNAME, DbType.String, sLoginName);
            db.AddInParameter(dbCommand, PARAM_CURRENTUSER, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVEUSERLIST, DbType.Boolean, bActiveUser);
            db.AddInParameter(dbCommand, PARAM_SEARCHCONDITION, DbType.String, SearchCondition);
            dbCommand.CommandTimeout = 10000;
            IDataReader rdr = db.ExecuteReader(dbCommand);
            while (rdr.Read())
            {
                BEUserInfo oUser = new BEUserInfo
                {
                    iUserID = Convert.ToInt32(rdr["UserID"]),
                    iEmployeeID = Convert.ToInt32(rdr["EmpID"]),
                    sFirstName = rdr["FirstName"].ToString(),
                    sMiddleName = rdr["MiddleName"].ToString(),
                    sLastName = rdr["LastName"].ToString() + " " + (Convert.ToBoolean(rdr["disabled"]) ? "(Disabled)" : ""),
                    sLoginName = rdr["LoginName"].ToString(),
                    sPassword = rdr["Password"].ToString(),
                    iUserLevel = Convert.ToInt32(rdr["UserLevelID"]),
                    bLanID = Convert.ToBoolean(rdr["IsLanIDUser"]),
                    bClientUser = Convert.ToBoolean(rdr["IsClient"].ToString() == "" ? false : rdr["IsClient"]),
                    bDisabled = Convert.ToBoolean(rdr["disabled"]),
                    iCreatedBy = Convert.ToInt32(rdr["CreatedBy"]),
                    iUserFacility = Convert.ToInt32(rdr["FacilityId"])
                };

                lUser.Add(oUser);
                oUser = null;
            }

            //}
            return lUser;
        }



        public IList<BEUserInfo> GetUserListbyClient(string sLoginName, bool bActiveUser, int iClientID, int iUserID)
        {
            IList<BEUserInfo> lUser = new List<BEUserInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_USERLISTBYCLIENT);
            db.AddInParameter(dbCommand, PARAM_LOGINNAME, DbType.String, sLoginName);
            db.AddInParameter(dbCommand, PARAM_ACTIVEUSERLIST, DbType.Boolean, bActiveUser);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, iClientID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {

                while (rdr.Read())
                {

                    BEUserInfo oUser = new BEUserInfo
                    {
                        iUserID = Convert.ToInt32(rdr["UserID"]),
                        iEmployeeID = Convert.ToInt32(rdr["EmpID"]),
                        sFirstName = rdr["FirstName"].ToString(),
                        sMiddleName = rdr["MiddleName"].ToString(),
                        sLastName = rdr["LastName"].ToString() + " " + (Convert.ToBoolean(rdr["disabled"]) ? "(Disabled)" : ""),
                        sLoginName = rdr["LoginName"].ToString(),
                        sPassword = rdr["Password"].ToString(),
                        iUserLevel = Convert.ToInt32(rdr["UserLevelID"]),
                        bLanID = Convert.ToBoolean(rdr["IsLanIDUser"]),
                        bDisabled = Convert.ToBoolean(rdr["disabled"]),
                        iCreatedBy = Convert.ToInt32(rdr["CreatedBy"]),
                        iUserFacility = Convert.ToInt32(rdr["FacilityId"])
                    };

                    lUser.Add(oUser);
                    oUser = null;
                }

            }
            return lUser;
        }

        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="sLoginName">Name of the s login.</param>
        /// <param name="bActiveUser">if set to <c>true</c> [active user].</param>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <returns></returns>
        /// <summary>
        /// Gets the user list with FM role.
        /// </summary>
        /// <param name="sLoginName">Name of the s login.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserListWithFMRole(string sLoginName)
        {
            List<BEUserInfo> lUser = new List<BEUserInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERLISTWITHFMROLE);
            db.AddInParameter(dbCommand, PARAM_LOGINNAME, DbType.String, "%" + sLoginName + "%");

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEUserInfo oUser = new BEUserInfo
                    {
                        iUserID = Convert.ToInt32(rdr["UserID"]),
                        sFirstName = rdr["FirstName"].ToString(),
                        sMiddleName = rdr["MiddleName"].ToString(),
                        sLastName = rdr["LastName"].ToString(),
                        bDisabled = Convert.ToBoolean(rdr["Disabled"].ToString()),
                        iEmployeeID = Convert.ToInt32(rdr["EmpId"].ToString())
                    };

                    lUser.Add(oUser);
                    oUser = null;
                }

            }
            return lUser;
        }
        #endregion

        #region Get The user Data
        /// <summary>
        /// Get The user Data
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="bAllActiveUser">if set to <c>true</c> [all active user].</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserList(int iUserID, bool bAllActiveUser)
        {
            List<BEUserInfo> lUser = new List<BEUserInfo>();
            //List<BERoleInfo> lRole = new List<BERoleInfo>();
            //List<BECampaignInfo> lCampaign = new List<BECampaignInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            DbCommand dbRoleCampaignCommand = db.GetSqlStringCommand(SQL_SELECT_USERROLECAMPAIGN);
            db.AddInParameter(dbRoleCampaignCommand, PARAM_USERID, DbType.Int32, iUserID);

            DataSet dstRoleCampaign = new DataSet();
            db.LoadDataSet(dbRoleCampaignCommand, dstRoleCampaign, "DRoleCampaign");

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEUserInfo oUser = new BEUserInfo
                    {
                        iUserID = Convert.ToInt32(rdr["UserID"]),
                        iEmployeeID = Convert.ToInt32(rdr["EmpID"]),
                        sFirstName = rdr["FirstName"].ToString(),
                        sMiddleName = rdr["MiddleName"].ToString(),
                        sLastName = rdr["LastName"].ToString(),
                        sLoginName = rdr["LoginName"].ToString(),
                        sPassword = rdr["Password"].ToString(),
                        iUserLevel = Convert.ToInt32(rdr["UserLevelID"]),
                        bLanID = Convert.ToBoolean(rdr["IsLanIDUser"]),
                        bDisabled = Convert.ToBoolean(rdr["disabled"]),
                        iCreatedBy = Convert.ToInt32(rdr["CreatedBy"]),
                        iUserFacility = Convert.ToInt32(rdr["FacilityId"])
                    };
                    oUser.iUserLevel = Convert.ToInt32(rdr["UserLevelID"]);
                    oUser.dstRoleCampaign = dstRoleCampaign;
                    lUser.Add(oUser);
                    oUser = null;
                    dstRoleCampaign = null;
                }
            }
            return lUser;
        }
        /// <summary>
        /// Gets the client user details.
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        /// <returns></returns>
        public BEUserInfo GetClientUserDetails(int iUserID)
        {
            BEUserInfo oUser = new BEUserInfo();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENTUSERID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    if (rdr["UserID"] != DBNull.Value)
                    {
                        oUser.iUserID = Convert.ToInt32(rdr["UserID"]);
                    }
                    if (rdr["EmpID"] != DBNull.Value)
                    {
                        oUser.iEmployeeID = Convert.ToInt32(rdr["EmpID"]);
                    }
                    oUser.sFirstName = rdr["FirstName"].ToString();
                    oUser.sMiddleName = rdr["MiddleName"].ToString();
                    oUser.sLastName = rdr["LastName"].ToString();
                    oUser.sEmail = rdr["Email"].ToString();
                    oUser.sLoginName = rdr["LoginName"].ToString();
                    oUser.bIsBot = Convert.ToBoolean(rdr["IsBOT"]);
                    oUser.bClientUser = Convert.ToBoolean(rdr["IsClient"].ToString() == "" ? false : rdr["IsClient"]);
                    if (rdr["IsLanIDUser"] != DBNull.Value)
                    {
                        oUser.bLanID = Convert.ToBoolean(rdr["IsLanIDUser"]);
                    }
                    oUser.bDisabled = Convert.ToBoolean(rdr["disabled"]);
                    BERoleInfo oRoles = new BERoleInfo();
                    if (rdr["RoleID"] != DBNull.Value)
                    {
                        oRoles.iRoleID = Convert.ToInt32(rdr["RoleID"]);
                    }
                    if (rdr["DOJ"] != DBNull.Value)
                    {
                        oUser.dDOJ = DateTime.Parse(rdr["DOJ"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                        //oUser.dDOJ = Convert.ToDateTime(rdr["DOJ"]);
                    }
                    if (rdr["FacilityId"] != DBNull.Value)
                    {
                        oUser.iFacilityId = Convert.ToInt32(rdr["FacilityId"]);
                    }
                    if (rdr["SupervisorId"] != DBNull.Value)
                    {
                        oUser.iSupervisorID = Convert.ToInt32(rdr["SupervisorId"]);
                    }
                    oUser.sSupervisorName = rdr["SupervisorName"].ToString();
                    if (rdr["LOBID"] != DBNull.Value)
                    {
                        oUser.iLOBID = Convert.ToInt32(rdr["LOBID"]);//added by Omkar
                    }
                    if (rdr["SBUID"] != DBNull.Value)
                    {
                        oUser.iSBUID = Convert.ToInt32(rdr["SBUID"]);//added by Omkar

                        oUser.oRoles.Add(oRoles);
                    }
                    if (rdr["JOBID"] != DBNull.Value)
                    {
                        oUser.iJobID = Convert.ToInt32(rdr["JOBID"]);//added by Deepak

                        oUser.oRoles.Add(oRoles);
                    }


                }
                rdr.NextResult();
                string sProcess = string.Empty;
                while (rdr.Read())
                {
                    if (sProcess == string.Empty)
                    {
                        oUser.iClientID = int.Parse(rdr["ClientId"].ToString());
                        sProcess = rdr["ProcessId"].ToString();
                    }
                    else
                        sProcess += "," + rdr["ProcessId"].ToString();
                }
                oUser.sProcess = sProcess;

            }
            return oUser;
        }

        /// <summary>
        /// Gets the user details with role.
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        /// <returns></returns>
        public BEUserInfo GetUserDetailsWithRole(int iUserID)
        {
            BEUserInfo oUser = new BEUserInfo();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERDETAILSWITHROLE);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {

                    oUser.iUserID = Convert.ToInt32(rdr["UserID"]);
                    oUser.iEmployeeID = Convert.ToInt32(rdr["EmpID"]);
                    oUser.sFirstName = rdr["FirstName"].ToString();
                    oUser.sMiddleName = rdr["MiddleName"].ToString();
                    oUser.sLastName = rdr["LastName"].ToString();
                    oUser.sLoginName = rdr["LoginName"].ToString();
                    oUser.sEmail = rdr["Email"].ToString();
                    oUser.bLanID = Convert.ToBoolean(rdr["IsLanIDUser"]);
                    oUser.bDisabled = Convert.ToBoolean(rdr["disabled"]);
                    BERoleInfo oRoles = new BERoleInfo();
                    oRoles.iRoleID = Convert.ToInt32(rdr["RoleID"]);
                    oUser.iJobID = Convert.ToInt32(rdr["ApprovalPending"]);
                    oUser.oRoles.Add(oRoles);
                }
                rdr.NextResult();
                string sProcess = string.Empty;
                while (rdr.Read())
                {
                    oUser.iClientID = int.Parse(rdr["ClientId"].ToString());
                }

            }
            return oUser;
        }
        #endregion

        #region Gets the supervisor list.
        /// <summary>
        /// Gets the supervisor list.
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <returns>List of all the supervisor in the team</returns>
        public List<BEUserInfo> GetSupervisorList(int iUserID)
        {
            List<BEUserInfo> lUser = new List<BEUserInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_SUPERVISOR);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEUserInfo oUser = new BEUserInfo
                    {
                        iUserID = Convert.ToInt32(rdr["UserID"]),
                        iEmployeeID = Convert.ToInt32(rdr["EmpID"]),
                        sFirstName = rdr["FirstName"].ToString(),
                        sMiddleName = rdr["MiddleName"].ToString(),
                        sLastName = rdr["LastName"].ToString(),
                        sLoginName = rdr["LoginName"].ToString(),
                        sPassword = rdr["Password"].ToString(),
                        iUserLevel = Convert.ToInt32(rdr["UserLevelID"]),
                        bLanID = Convert.ToBoolean(rdr["IsLanIDUser"]),
                        bDisabled = Convert.ToBoolean(rdr["disabled"]),
                        iCreatedBy = Convert.ToInt32(rdr["CreatedBy"]),
                        iUserFacility = Convert.ToInt32(rdr["FacilityId"])
                    };

                    lUser.Add(oUser);
                    oUser = null;
                }

            }
            return lUser;
        }
        #endregion

        #region Insert Data
        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="oUserData">The user data.</param>
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
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }

        /// <summary>
        /// Inserts the client user data.
        /// </summary>
        /// <param name="oUserData">The o user data.</param>
        public void InsertClientUserData(BEUserInfo oUserData)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbInsertUserCommand = db.GetStoredProcCommand(SQL_INSERT_CLIENTUSER);
                db.AddInParameter(dbInsertUserCommand, PARAM_EMPID, DbType.Int32, oUserData.iEmployeeID);
                db.AddInParameter(dbInsertUserCommand, PARAM_LANUSER, DbType.Boolean, oUserData.bLanID);
                db.AddInParameter(dbInsertUserCommand, PARAM_ISCLIENTUSER, DbType.Boolean, oUserData.bClientUser);
                db.AddInParameter(dbInsertUserCommand, PARAM_FIRSTNAME, DbType.String, oUserData.sFirstName);
                db.AddInParameter(dbInsertUserCommand, PARAM_MIDDLENAME, DbType.String, oUserData.sMiddleName);
                db.AddInParameter(dbInsertUserCommand, PARAM_LASTNAME, DbType.String, oUserData.sLastName);
                db.AddInParameter(dbInsertUserCommand, PARAM_EMAIL, DbType.String, oUserData.sEmail);
                db.AddInParameter(dbInsertUserCommand, PARAM_LOGINNAME, DbType.String, oUserData.sLoginName);
                db.AddInParameter(dbInsertUserCommand, PARAM_DISABLED, DbType.Boolean, oUserData.bDisabled);
                db.AddInParameter(dbInsertUserCommand, PARAM_ISBOT, DbType.Boolean, oUserData.bIsBot);
                db.AddInParameter(dbInsertUserCommand, PARAM_CREATEDBY, DbType.Int32, oUserData.iCreatedBy);
                db.AddInParameter(dbInsertUserCommand, PARAM_CLIENTID, DbType.Int32, oUserData.iClientID);
                db.AddInParameter(dbInsertUserCommand, PARAM_PROCESS, DbType.String, oUserData.sProcess);
                db.AddInParameter(dbInsertUserCommand, PARAM_ROLEID, DbType.String, oUserData.oRoles.Count > 0 ? oUserData.oRoles[0].iRoleID.ToString() + ":" + oUserData.iRoleApprover.ToString() : "0:0");
                db.AddInParameter(dbInsertUserCommand, PARAM_DOJ, DbType.DateTime, oUserData.dDOJ);
                db.AddInParameter(dbInsertUserCommand, PARAM_FACILITYID, DbType.Int32, oUserData.iFacilityId);
                db.AddInParameter(dbInsertUserCommand, PARAM_SUPERVISORID, DbType.Int32, oUserData.iSupervisorID);

                db.AddInParameter(dbInsertUserCommand, PARAM_LOBID, DbType.Int32, oUserData.iLOBID); //added by Omkar
                db.AddInParameter(dbInsertUserCommand, PARAM_SBUID, DbType.Int32, oUserData.iSBUID); //added by Omkar

                db.AddInParameter(dbInsertUserCommand, PARAM_JOBID, DbType.Int32, oUserData.iJobID); //add by deepak

                db.ExecuteNonQuery(dbInsertUserCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                //if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_UserNameAlready))
                //{
                //   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_UserNameAlready);
                //}
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }

        /// <summary>
        /// Inserts the client user data.SaveUpdatePowerUser
        /// </summary>
        /// <param name="oUserData">The o user data.</param>
        public void InsertUserRoleData(BEUserInfo oUserData, int iMode)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbInsertUserCommand = db.GetStoredProcCommand(SQL_INSERT_USERROLE);
                db.AddInParameter(dbInsertUserCommand, PARAM_EMPID, DbType.Int32, oUserData.iEmployeeID);
                db.AddInParameter(dbInsertUserCommand, PARAM_LANUSER, DbType.Boolean, oUserData.bLanID);
                db.AddInParameter(dbInsertUserCommand, PARAM_FIRSTNAME, DbType.String, oUserData.sFirstName);
                db.AddInParameter(dbInsertUserCommand, PARAM_MIDDLENAME, DbType.String, oUserData.sMiddleName);
                db.AddInParameter(dbInsertUserCommand, PARAM_LASTNAME, DbType.String, oUserData.sLastName);
                db.AddInParameter(dbInsertUserCommand, PARAM_EMAIL, DbType.String, oUserData.sEmail);
                db.AddInParameter(dbInsertUserCommand, PARAM_LOGINNAME, DbType.String, oUserData.sLoginName);
                db.AddInParameter(dbInsertUserCommand, PARAM_DISABLED, DbType.Boolean, oUserData.bDisabled);
                db.AddInParameter(dbInsertUserCommand, PARAM_CREATEDBY, DbType.Int32, oUserData.iCreatedBy);
                db.AddInParameter(dbInsertUserCommand, PARAM_APPROVERID, DbType.String, oUserData.oRoles[0].iCreatedBy);
                db.AddInParameter(dbInsertUserCommand, PARAM_ROLEID, DbType.Int32, oUserData.oRoles[0].iRoleID);
                db.AddInParameter(dbInsertUserCommand, PARAM_MODE, DbType.Int32, iMode);
                db.ExecuteNonQuery(dbInsertUserCommand);
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }

        /// <summary>
        /// Inserts the agent migration data.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <param name="strCampaignRole">The STR campaign role.</param>
        /// <param name="CreatedBy">The created by.</param>
        public void InsertAgentMigrationData(int UserId, string strCampaignRole, int CreatedBy)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbInsertCampaignRoleCommand = db.GetStoredProcCommand(SP_INSERT_USERCAMPAIGNROLE);
                db.AddInParameter(dbInsertCampaignRoleCommand, PARAM_ROLECAMPAIGN, DbType.String, strCampaignRole);

                db.AddInParameter(dbInsertCampaignRoleCommand, PARAM_USERID, DbType.Int32, UserId);
                db.AddInParameter(dbInsertCampaignRoleCommand, PARAM_CREATEDBY, DbType.Int32, CreatedBy);

                DbCommand dbUpdateUserLevel = db.GetSqlStringCommand(SQL_UPDATE_USERLEVEL);
                db.AddInParameter(dbUpdateUserLevel, PARAM_USERLEVEL, DbType.Int32, 7);
                db.AddInParameter(dbUpdateUserLevel, PARAM_USERID, DbType.Int32, UserId);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbUpdateUserLevel, trans);
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
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }

        /// <summary>
        /// Inserts the unsuccessfull login.
        /// </summary>
        /// <param name="UserID">The user ID.</param>
        /// <param name="SessionID">The session ID.</param>
        /// <param name="HostName">Name of the host.</param>
        public void InsertUnsuccessfullLogin(int UserID, string SessionID, string HostName)
        {
            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbInsertUnsuccessfullLoginCommand = db.GetSqlStringCommand(SQL_INSERT_UNSUCCESSFULLLOGIN);

            db.AddInParameter(dbInsertUnsuccessfullLoginCommand, PARAM_USERID, DbType.Int32, UserID);
            db.AddInParameter(dbInsertUnsuccessfullLoginCommand, PARAM_SESSIONID, DbType.String, SessionID);
            db.AddInParameter(dbInsertUnsuccessfullLoginCommand, PARAM_HOSTNAME, DbType.String, HostName);

            db.ExecuteNonQuery(dbInsertUnsuccessfullLoginCommand);
        }

        #endregion

        #region Update Data
        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="oUserData">The user data.</param>
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


                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        trans.Rollback();
                        if (ex.Number == 547)
                        {
                            // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                        }
                        if (ex.Number == 2627)
                        {
                            //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                        }
                        throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }
                    conn.Close();
                }
            }
        }
        /// <summary>
        /// Updatets the client user data.
        /// </summary>
        /// <param name="oUserData">The o user data.</param>
        public void UpdatetClientUserData(BEUserInfo oUserData)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbInsertUserCommand = db.GetStoredProcCommand(SQL_UPDATE_CLIENTUSER);
                db.AddInParameter(dbInsertUserCommand, PARAM_USERID, DbType.Int32, oUserData.iUserID);
                db.AddInParameter(dbInsertUserCommand, PARAM_EMPID, DbType.Int32, oUserData.iEmployeeID);
                db.AddInParameter(dbInsertUserCommand, PARAM_LANUSER, DbType.Boolean, oUserData.bLanID);
                db.AddInParameter(dbInsertUserCommand, PARAM_ISCLIENTUSER, DbType.Boolean, oUserData.bClientUser);
                db.AddInParameter(dbInsertUserCommand, PARAM_FIRSTNAME, DbType.String, oUserData.sFirstName);
                db.AddInParameter(dbInsertUserCommand, PARAM_MIDDLENAME, DbType.String, oUserData.sMiddleName);
                db.AddInParameter(dbInsertUserCommand, PARAM_LASTNAME, DbType.String, oUserData.sLastName);
                db.AddInParameter(dbInsertUserCommand, PARAM_EMAIL, DbType.String, oUserData.sEmail);
                db.AddInParameter(dbInsertUserCommand, PARAM_LOGINNAME, DbType.String, oUserData.sLoginName);
                db.AddInParameter(dbInsertUserCommand, PARAM_ISBOT, DbType.Boolean, oUserData.bIsBot);
                db.AddInParameter(dbInsertUserCommand, PARAM_DISABLED, DbType.Boolean, oUserData.bDisabled);
                db.AddInParameter(dbInsertUserCommand, PARAM_CREATEDBY, DbType.Int32, oUserData.iCreatedBy);
                db.AddInParameter(dbInsertUserCommand, PARAM_CLIENTID, DbType.Int32, oUserData.iClientID);
                db.AddInParameter(dbInsertUserCommand, PARAM_PROCESS, DbType.String, oUserData.sProcess);
                db.AddInParameter(dbInsertUserCommand, PARAM_ROLEID, DbType.String, oUserData.oRoles.Count > 0 ? (oUserData.oRoles[0].iRoleID.ToString() + ":" + oUserData.iRoleApprover.ToString()) : "0:0");
                db.AddInParameter(dbInsertUserCommand, PARAM_DELETEDPROCESS, DbType.String, oUserData.sDeletedProcess);
                db.AddInParameter(dbInsertUserCommand, PARAM_DOJ, DbType.DateTime, oUserData.dDOJ);
                db.AddInParameter(dbInsertUserCommand, PARAM_FACILITYID, DbType.Int32, oUserData.iFacilityId);
                db.AddInParameter(dbInsertUserCommand, PARAM_SUPERVISORID, DbType.Int32, oUserData.iSupervisorID);

                db.AddInParameter(dbInsertUserCommand, PARAM_LOBID, DbType.Int32, oUserData.iLOBID);//added by Omkar
                db.AddInParameter(dbInsertUserCommand, PARAM_SBUID, DbType.Int32, oUserData.iSBUID);//added by Omkar
                db.AddInParameter(dbInsertUserCommand, PARAM_JOBID, DbType.Int32, oUserData.iJobID);//added by Deepak
                db.ExecuteNonQuery(dbInsertUserCommand);
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                  //  throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                //if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_UpdateUserApprovalPending))
                //{
                //    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_UpdateUserApprovalPending);
                //}
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }



        #endregion

        #region Delete Data
        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="oUserData">The user data.</param>
        public void DeleteData(BEUserInfo oUserData)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);

                ////*************************************Delete User Campaign Map
                //DbCommand dbDeleteCampaignCommand = db.GetSqlStringCommand(SQL_DELETE_USERCAMPAIGN);
                //db.AddInParameter(dbDeleteCampaignCommand, PARAM_USERID, DbType.Int32, oUserData.iUserID);

                //*************************************Delete Role Campaign Map
                DbCommand dbDeleteRolesCommand = db.GetSqlStringCommand(SQL_DELETE_USERROLES);
                db.AddInParameter(dbDeleteRolesCommand, PARAM_USERID, DbType.Int32, oUserData.iUserID);

                //*************************************Delete User
                DbCommand dbDeleteUserCommand = db.GetSqlStringCommand(SQL_DELETE_USER);
                db.AddInParameter(dbDeleteUserCommand, PARAM_USERID, DbType.Int32, oUserData.iUserID);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            //db.ExecuteNonQuery(dbDeleteCampaignCommand, trans);
                            db.ExecuteNonQuery(dbDeleteRolesCommand, trans);
                            db.ExecuteNonQuery(dbDeleteUserCommand, trans);
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
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the client user data.
        /// </summary>
        /// <param name="oUserData">The o user data.</param>
        public void DeleteClientUserData(BEUserInfo oUserData)
        {
            try
            {
                //var dbFactory = new DatabaseProviderFactory();
                //Database db = dbFactory.Create(DL_Shared.Connection);
                Database db = DL_Shared.dbFactory(_oTenant);
                //*************************************Delete User
                DbCommand dbDeleteUserCommand = db.GetSqlStringCommand(SQL_DELETE_USER);
                db.AddInParameter(dbDeleteUserCommand, PARAM_USERID, DbType.Int32, oUserData.iUserID);
                //*************************************Delete Role Campaign Map
                DbCommand dbDeleteRolesCommand = db.GetSqlStringCommand(SQL_DELETE_USERROLES);
                db.AddInParameter(dbDeleteRolesCommand, PARAM_USERID, DbType.Int32, oUserData.iUserID);
                //*************************************Delete User Process Mapping
                DbCommand dbDeleteUserProcessMapCommand = db.GetSqlStringCommand(SQL_DELETE_USERPROCESSMAP);
                db.AddInParameter(dbDeleteUserProcessMapCommand, PARAM_USERID, DbType.Int32, oUserData.iUserID);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbDeleteRolesCommand, trans);
                            db.ExecuteNonQuery(dbDeleteUserProcessMapCommand, trans);
                            db.ExecuteNonQuery(dbDeleteUserCommand, trans);
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
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the user role data.
        /// </summary>
        /// <param name="oUserData">The o user data.</param>
        public void DeleteUserRoleData(BEUserInfo oUserData)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);

                //*************************************Delete User
                DbCommand dbDeleteUserCommand = db.GetSqlStringCommand(SQL_DELETE_USERROLE);
                db.AddInParameter(dbDeleteUserCommand, PARAM_USERID, DbType.Int32, oUserData.iUserID);
                db.ExecuteNonQuery(dbDeleteUserCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }
        #endregion

        #region Change Password
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="oUserInfo">The user info.</param>
        public void ChangePd(BEUserInfo oUserInfo)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbChangePdCommand = db.GetSqlStringCommand(SQL_UPDATE_PD);
            db.AddInParameter(dbChangePdCommand, PARAM_PD, DbType.String, oUserInfo.sPassword);
            db.AddInParameter(dbChangePdCommand, PARAM_USERID, DbType.Int32, oUserInfo.iUserID);
            db.ExecuteNonQuery(dbChangePdCommand);
        }
        #endregion

        #region Change User Status
        /// <summary>
        /// Change User Status
        /// </summary>
        /// <param name="UserID"></param>
        public void ChangeUserStatus(int UserID)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbChangeUserStatusCommand = db.GetSqlStringCommand(SQL_UPDATE_USERSTATUS);
            db.AddInParameter(dbChangeUserStatusCommand, PARAM_USERID, DbType.Int32, UserID);
            db.ExecuteNonQuery(dbChangeUserStatusCommand);
        }
        #endregion

        #region Maintains Changed Password Log
        /// <summary>
        /// Maintains Changed Password Log
        /// </summary>
        /// <param name="oUserInfo"></param>
        public int ChangePdLog(BEUserInfo oUserInfo)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbChangePdCommand = db.GetStoredProcCommand(SP_MAINTAIN_PD_LOG);
            db.AddInParameter(dbChangePdCommand, PARAM_PD, DbType.String, oUserInfo.sPassword);
            db.AddInParameter(dbChangePdCommand, PARAM_USERID, DbType.Int32, oUserInfo.iUserID);
            return db.ExecuteNonQuery(dbChangePdCommand);
        }
        #endregion

        #region GetAuthenticateUser
        /// <summary>
        /// Gets the authenticate user.
        /// </summary>
        /// <param name="LoginID">The login ID.</param>
        /// <param name="Password">The password.</param>
        /// <param name="SessionID">The session ID.</param>
        /// <param name="UserHostName">Name of the user host.</param>
        /// <param name="Disabled">if set to <c>true</c> [disabled].</param>
        /// <returns></returns>
        public BEUserInfo GetAuthenticateUser(string LoginID, string Password, string SessionID, string UserHostName, out Boolean Disabled)
        {
            Disabled = false;
            BEUserInfo oUser = new BEUserInfo();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_USERAUTH);
            db.AddInParameter(dbCommand, PARAM_LOGINNAME, DbType.String, LoginID);
            db.AddInParameter(dbCommand, PARAM_PD, DbType.String, Password);
            db.AddInParameter(dbCommand, PARAM_SESSIONID, DbType.String, SessionID);
            db.AddInParameter(dbCommand, PARAM_HOSTNAME, DbType.String, UserHostName);
            db.AddParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, ParameterDirection.Output, "", DataRowVersion.Default, Disabled);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {

                while (rdr.Read())
                {
                    oUser = new BEUserInfo
                    {
                        iUserID = Convert.ToInt32(rdr["UserID"]),
                        iEmployeeID = Convert.ToInt32(rdr["EmpID"]),
                        sFirstName = rdr["FirstName"].ToString(),
                        sMiddleName = rdr["MiddleName"].ToString(),
                        sLastName = rdr["LastName"].ToString(),
                        sLoginName = rdr["LoginName"].ToString(),
                        sPassword = rdr["Password"].ToString(),
                        iUserLevel = Convert.ToInt32(rdr["UserLevelID"]),
                        bLanID = Convert.ToBoolean(rdr["IsLanIDUser"]),
                        bDisabled = Convert.ToBoolean(rdr["disabled"]),
                        iCreatedBy = Convert.ToInt32(rdr["CreatedBy"]),
                        iUserFacility = Convert.ToInt32(rdr["FacilityId"])
                    };
                }
            }
            Disabled = (Boolean)db.GetParameterValue(dbCommand, PARAM_DISABLED);
            return oUser;
        }

        /// <summary>
        /// Checks the old pass word.
        /// </summary>
        /// <param name="UserID">The user ID.</param>
        /// <returns></returns>
        public DataSet CheckOldPd(int UserID)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PD);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserID);
            db.LoadDataSet(dbCommand, ds, "Password");
            return ds;
        }

        public DataSet GetUserBasedMomteeID(string user1, string user2)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USER_MONTEEBASED);
            db.AddInParameter(dbCommand, PARAM_USER1, DbType.String, user1);
            db.AddInParameter(dbCommand, PARAM_USER2, DbType.String, user2);
            db.LoadDataSet(dbCommand, ds, "Record");
            return ds;
        }


        /// <summary>
        /// Gets the user ID.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <returns></returns>
        public DataSet GetUserID(string LoginName)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELCET_USERID);
            db.AddInParameter(dbCommand, PARAM_LOGINNAME, DbType.String, LoginName);
            db.LoadDataSet(dbCommand, ds, "UserRoles");
            return ds;
        }


        public DateTime GetLastPasswordChange(string LoginName)
        {
            DateTime CurrentDate = DateTime.Now.ToUniversalTime();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELCET_LASTPDCHANGE);
            db.AddInParameter(dbCommand, PARAM_LOGINNAME, DbType.String, LoginName);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    CurrentDate = Convert.ToDateTime(rdr["CreatedOn"]);
                }
            }
            return CurrentDate;
        }



        #endregion

        #region Authorization
        /// <summary>
        /// Determines whether the specified form ID is authorization.
        /// </summary>
        /// <param name="FormID">The form ID.</param>
        /// <param name="UserID">The user ID.</param>
        /// <param name="Action">The action.</param>
        /// <returns>
        /// 	<c>true</c> if the specified form ID is authorization; otherwise, <c>false</c>.
        /// </returns>
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
        #endregion

        #region Gets the campaign user list.
        /// <summary>
        /// Gets the campaign user list.
        /// </summary>
        /// <param name="iCampID">The i camp ID.</param>
        /// <returns></returns>
        public DataSet GetCampaignUserList(int iCampID)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CAMPAIGNUSERS);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampID);
            db.LoadDataSet(dbCommand, ds, "UserDetail");
            return ds;
        }
        #endregion

        #region Gets the user campaigns.
        /// <summary>
        /// Gets the user campaigns.
        /// </summary>
        /// <param name="Clients">The clients.</param>
        /// <param name="UserId">The user id.</param>
        /// <returns></returns>
        public DataSet GetUserCampaigns(string Clients, int UserId)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_USERCAMPAIGNS);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            db.AddInParameter(dbCommand, PARAM_CLIENTS, DbType.String, Clients);
            db.LoadDataSet(dbCommand, ds, "UserCampaigns");
            return ds;
        }
        #endregion

        #region Gets the user roles.
        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <param name="CampaignId">The campaign id.</param>
        /// <param name="UserId">The user id.</param>
        /// <returns></returns>
        public DataSet GetUserRoles(int CampaignId, int UserId)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERROLES);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignId);
            db.LoadDataSet(dbCommand, ds, "UserRoles");
            return ds;
        }

        public List<BERoleInfo> GetUserRoles(int UserId)
        {
            List<BERoleInfo> ds = new List<BERoleInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERROLESLIST);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BERoleInfo oRole = new BERoleInfo();
                    oRole.iRoleID = int.Parse(rdr["RoleID"].ToString());
                    oRole.sRoleName = rdr["RoleName"].ToString();
                    ds.Add(oRole);
                    oRole = null;
                }
            }
            return ds;
        }

        #endregion

        #region Checks the user
        /// <summary>
        /// Checks the user.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <returns></returns>
        public DataSet CheckUser(string LoginName)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERLOGINNAME);
            db.AddInParameter(dbCommand, PARAM_LOGINNAME, DbType.String, LoginName);
            db.LoadDataSet(dbCommand, ds, "UserRoles");
            return ds;
        }
        #endregion

        #region Inserts the session log out.
        /// <summary>
        /// Inserts the session log out.
        /// </summary>
        /// <param name="ServerName">Name of the server.</param>
        /// <param name="UserAgent">The user agent.</param>
        /// <param name="IP">The IP.</param>
        /// <param name="Host">The host.</param>
        /// <param name="UserName">Name of the user.</param>
        /// <param name="URLTReferrer">The URLT referrer.</param>
        /// <param name="URL">The URL.</param>
        /// <param name="SessionID">The session ID.</param>
        /// <param name="IsNewSession">if set to <c>true</c> [is new session].</param>
        /// <param name="SessionTimeOut">The session time out.</param>
        public void InsertSessionLogOut(string ServerName, string UserAgent, string IP, string Host, string UserName, string URLTReferrer, string URL, string SessionID, Boolean IsNewSession, int SessionTimeOut)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbInsertSessionLogOut = db.GetSqlStringCommand(SQL_SELECT_INSERTSESSIONLOGOUT);
            db.AddInParameter(dbInsertSessionLogOut, PARAM_SERVERNAME, DbType.String, ServerName);
            db.AddInParameter(dbInsertSessionLogOut, PARAM_USERAGENT, DbType.String, UserAgent);
            db.AddInParameter(dbInsertSessionLogOut, PARAM_USERIP, DbType.String, IP);
            db.AddInParameter(dbInsertSessionLogOut, PARAM_HOSTNAME, DbType.String, Host);
            db.AddInParameter(dbInsertSessionLogOut, PARAM_USERNAME, DbType.String, UserName);
            db.AddInParameter(dbInsertSessionLogOut, PARAM_URLREFERRER, DbType.String, URLTReferrer);
            db.AddInParameter(dbInsertSessionLogOut, PARAM_RAWURL, DbType.String, URL);
            db.AddInParameter(dbInsertSessionLogOut, PARAM_SESSIONID, DbType.String, SessionID);
            db.AddInParameter(dbInsertSessionLogOut, PARAM_ISNEWSESSION, DbType.Boolean, IsNewSession);
            db.AddInParameter(dbInsertSessionLogOut, PARAM_SESSIONTIMEOUT, DbType.Int32, SessionTimeOut);
            db.ExecuteNonQuery(dbInsertSessionLogOut);
        }
        #endregion

        #region ChangeUser_LANIDUser
        /// <summary>
        /// Changes the user_ LANID user.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        public void ChangeUser_LANIDUser(string LoginName)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbChangeUserType = db.GetSqlStringCommand(SQL_UPDATE_USERTYPE_LANID);
            db.AddInParameter(dbChangeUserType, PARAM_LOGINNAME, DbType.String, LoginName);

            db.ExecuteNonQuery(dbChangeUserType);
        }
        #endregion

        #region ChangeUser_NONLANIDUser
        /// <summary>
        /// Changes the user_ NONLANID user.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        public void ChangeUser_NONLANIDUser(string LoginName)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbChangeUserType = db.GetSqlStringCommand(SQL_UPDATE_USERTYPE_NONLANID);
            db.AddInParameter(dbChangeUserType, PARAM_LOGINNAME, DbType.String, LoginName);

            db.ExecuteNonQuery(dbChangeUserType);
        }
        #endregion

        #region Changepassword
        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        public void ChangeUserPd(string LoginName)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbChangeUserPd = db.GetSqlStringCommand(SQL_UPDATE_USERPD);
            db.AddInParameter(dbChangeUserPd, PARAM_LOGINNAME, DbType.String, LoginName);

            db.ExecuteNonQuery(dbChangeUserPd);
        }
        #endregion

        #region isLDAPuser
        /// <summary>
        /// Determines whether [is LDAP user] [the specified login name].
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <returns>
        /// 	<c>true</c> if [is LDAP user] [the specified login name]; otherwise, <c>false</c>.
        /// </returns>
        public Boolean IsLANUser(string LoginName)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCheckUser = db.GetSqlStringCommand(SQL_SELECT_ISLANUSER);
            db.AddInParameter(dbCheckUser, PARAM_LOGINNAME, DbType.String, LoginName);

            Boolean bl = Convert.ToBoolean(db.ExecuteScalar(dbCheckUser));
            return bl;
        }
        #endregion

        #region Get ERP Team

        /// <summary>
        /// Gets User ERP team.
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        public List<BEUserInfo> GetERPTeam(int iUserID)
        {
            List<BEUserInfo> ds = new List<BEUserInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERERP_TEAM);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEUserInfo oUser = new BEUserInfo
                    {
                        iUserID = int.Parse(rdr["UserID"].ToString()),
                        sLoginName = rdr["UserName"].ToString(),
                        sJobDesc = rdr["JobDesc"].ToString(),
                        iJobID = int.Parse(rdr["JobID"].ToString())
                    };
                    ds.Add(oUser);
                }
            }
            return ds;
        }
        /// <summary>
        /// Gets User ERP team.
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        public List<BEUserInfo> GetUserListForExtraRole(int iUserID)
        {
            List<BEUserInfo> ds = new List<BEUserInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SP_SELECT_USERFOREXTRAROLE);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEUserInfo oUser = new BEUserInfo
                    {
                        iUserID = int.Parse(rdr["UserID"].ToString()),
                        sLoginName = rdr["UserName"].ToString(),
                        sJobDesc = rdr["JobDesc"].ToString(),
                        iJobID = int.Parse(rdr["JobID"].ToString())
                    };
                    ds.Add(oUser);
                }
            }
            return ds;
        }

        #endregion

        #region Gets the user list
        /// <summary>
        /// Gets User List for Process Family Owner team.
        /// </summary>
        /// <param name="sUserName">Name of the s user.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetProcessFamilyOwnerList(string sUserName)
        {
            List<BEUserInfo> lUserInfo = new List<BEUserInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USER_PROCESSFAMILYOWNER);
            db.AddInParameter(dbCommand, PARAM_NAME, DbType.String, sUserName + "%");

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEUserInfo oUser = new BEUserInfo
                    {
                        iUserID = int.Parse(rdr["UserID"].ToString()),
                        sFirstName = rdr["Name"].ToString(),
                    };
                    lUserInfo.Add(oUser);
                }
            }
            return lUserInfo;
        }
        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="Process">The process.</param>
        /// <param name="Campaign">The campaign.</param>
        /// <param name="Team">The team.</param>
        /// <param name="StartDate">The start date.</param>
        /// <param name="EndDate">The end date.</param>
        /// <returns></returns>
        //public DataSet GetUserList(int Process, int Campaign, int Team, DateTime? StartDate, DateTime? EndDate)
        //{
        //    DataSet ds = new DataSet();

        //    Database db = DL_Shared.dbFactory(_oTenant);
        //    DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_AGENTS);
        //    db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, Campaign);
        //    db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, Process);
        //    db.AddInParameter(dbCommand, PARAM_TEAMID, DbType.Int32, Team);
        //    db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, StartDate);
        //    db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, EndDate);
        //    db.LoadDataSet(dbCommand, ds, "UserRoles");
        //    return ds;
        //}
        public DataSet GetUserList(int ClientID, int Process, int Campaign, int Team, DateTime? StartDate, DateTime? EndDate)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            // DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_AGENTS);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_AGENTSNEW);
            db.AddInParameter(dbCommand, "@ClientID", DbType.Int32, ClientID);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, Campaign);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, Process);
            db.AddInParameter(dbCommand, PARAM_TEAMID, DbType.Int32, Team);
            db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, StartDate);
            db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, EndDate);
            db.LoadDataSet(dbCommand, ds, "UserRoles");
            return ds;
        }
        #endregion



        //  #region Gets the tEST GROUPING.
        ///// <summary>
        ///// Gets the user list process montee.
        ///// </summary>
        ///// <param name="Process">The process.</param>
        ///// <returns></returns>
        //public List<BEUserInfo> GetCustomers()
        //{
        //    List<BEUserInfo> lUser = new List<BEUserInfo>();
        //    Database db = DL_Shared.dbFactory(_oTenant);
        //    DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_TESTGROUPING);

        //    using (IDataReader rdr = db.ExecuteReader(dbCommand))
        //    {
        //        while (rdr.Read())
        //        {
        //            BEUserInfo oUser = new BEUserInfo
        //            {

        //                iClientID = Convert.ToInt32(rdr["ClientID"]),
        //                sFirstName = rdr["ClientName"].ToString(),
        //                iEmployeeID =Convert.ToInt32( rdr["ProcessID"]),
        //                sProcess = rdr["ProcessName"].ToString(),

        //            };

        //            lUser.Add(oUser);
        //            oUser = null;
        //        }

        //    }
        //    return lUser;
        //}
        //#endregion


        #region Gets the user list process.
        /// <summary>
        /// Gets the user list process.
        /// </summary>
        /// <param name="Process">The process.</param>
        /// <returns></returns>
        public DataSet GetUserListProcess(int Process)
        {
            DataSet ds = new DataSet();


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_PROCESSAGENTS);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, Process);
            db.LoadDataSet(dbCommand, ds, "ProcessUser");
            return ds;
        }
        /// <summary>
        /// Gets the user list process.
        /// </summary>
        /// <param name="Process">The process.</param>
        /// <param name="IsAllUsers">if set to <c>true</c> [is all users].</param>
        /// <returns></returns>
        public DataSet GetUserListProcess(int Process, bool IsAllUsers)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_PROCESSAGENTS);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, Process);
            db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, IsAllUsers);
            db.LoadDataSet(dbCommand, ds, "ProcessUser");
            return ds;
        }
        /// <summary>
        /// Gets the user list process agent AM.
        /// </summary>
        /// <param name="Process">The process.</param>
        /// <returns></returns>
        public DataSet GetUserListProcessAgentAM(int Process)
        {
            DataSet ds = new DataSet();


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_PROCESSAGENTSAM);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, Process);
            db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, false);
            db.LoadDataSet(dbCommand, ds, "ProcessUser");
            return ds;
        }
        /// <summary>
        /// Gets the user list process agent AM client QCA.
        /// </summary>
        /// <param name="Process">The process.</param>
        /// <returns></returns>
        public DataSet GetUserListProcessAgentAMClientQCA(int Process)
        {
            DataSet ds = new DataSet();

            //Database db = DL_Shared.dbFactory(_oTenant);

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_PROCESSAGENTSAMCLIENTQCA);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, Process);
            db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, false);
            db.LoadDataSet(dbCommand, ds, "ProcessUser");
            return ds;
        }

        //added by Omkar
        /// <summary>
        /// Return Mentor with TM Name
        /// </summary>
        /// <param name="iTMId"></param>
        /// <param name="sStartDate"></param>
        /// <param name="sEndDate"></param>
        /// <returns></returns>
        public DataSet GetMentorWithTMid(int iTMId, string sStartDate, string sEndDate)
        {
            DataSet ds = new DataSet();


            Database db = DL_Shared.dbFactory(_oTenant);
            //DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_MENTORWITHTMID);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SELECT_MENTORWITHTMID);
            db.AddInParameter(dbCommand, PARAM_TMID, DbType.Int32, iTMId);
            db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, sStartDate);
            db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, sEndDate);
            db.LoadDataSet(dbCommand, ds, "MentorName");
            return ds;
        }
        /// <summary>
        /// Gets the user list process agent AM.
        /// </summary>
        /// <param name="Process">The process.</param>
        /// <returns></returns>
        public DataSet GetAllUserListProcessAgentAM(int Process)
        {
            DataSet ds = new DataSet();


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_PROCESSAGENTSAM);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, Process);
            db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, true);
            db.LoadDataSet(dbCommand, ds, "ProcessUser");
            return ds;
        }

        /// <summary>
        /// Gets the user list process agent.
        /// </summary>
        /// <param name="Process">The process.</param>
        /// <returns></returns>
        public DataSet GetUserListProcessAgent(int Process)
        {
            DataSet ds = new DataSet();

            //Database db = DL_Shared.dbFactory(_oTenant);

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_PROCESSAGENTSONLY);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, Process);
            db.LoadDataSet(dbCommand, ds, "ProcessUser");
            return ds;
        }

        /// <summary>
        /// Gets the user list process VP and above.
        /// </summary>
        /// <returns></returns>
        public DataSet GetUserListVPAndAbove(string sUser)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_PROCESSVPABOVE);
            db.AddInParameter(dbCommand, PARAM_NAME, DbType.String, sUser + "%");
            db.LoadDataSet(dbCommand, ds, "ProcessUser");
            return ds;
        }
        #endregion

        #region Gets the user list process montee.
        /// <summary>
        /// Gets the user list process montee.
        /// </summary>
        /// <param name="Process">The process.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserListProcessMontee(string Process)
        {
            List<BEUserInfo> lUser = new List<BEUserInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SP_SELECT_MONTEE);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.String, Process);


            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEUserInfo oUser = new BEUserInfo
                    {
                        iUserID = Convert.ToInt32(rdr["UserID"]),
                        iEmployeeID = Convert.ToInt32(rdr["EmpID"]),
                        sFirstName = rdr["FirstName"].ToString(),
                        sMiddleName = rdr["MiddleName"].ToString(),
                        sLastName = rdr["LastName"].ToString(),
                        sLoginName = rdr["LoginName"].ToString(),
                        sPassword = rdr["Password"].ToString(),
                        iUserLevel = Convert.ToInt32(rdr["UserLevelID"]),
                        bLanID = Convert.ToBoolean(rdr["IsLanIDUser"]),
                        bDisabled = Convert.ToBoolean(rdr["disabled"]),
                        iCreatedBy = Convert.ToInt32(rdr["CreatedBy"]),
                        iUserFacility = Convert.ToInt32(rdr["FacilityId"])
                    };
                    oUser.iUserLevel = Convert.ToInt32(rdr["UserLevelID"]);
                    lUser.Add(oUser);
                    oUser = null;
                }

            }
            return lUser;
        }
        #endregion

        #region GetUserSetting

        /// <summary>
        /// Gets the user process map.
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        /// <returns></returns>
        public BEUserMapping GetUserProcessMap(int iUserID)
        {

            BEUserMapping lUserMapping = new BEUserMapping();
            lUserMapping.oClient = new List<BEClientInfo>();
            lUserMapping.oProcess = new List<BEProcessInfo>();
            lUserMapping.oCampaign = new List<BECampaignInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERPROCESSMAP);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    lUserMapping.oClient.Add(new BEClientInfo { iClientID = int.Parse(rdr["ClientID"].ToString()) });
                    if (rdr["ProcessID"].ToString() != "")
                    {
                        lUserMapping.oProcess.Add(new BEProcessInfo { iProcessID = int.Parse(rdr["ProcessID"].ToString()) });
                    }
                    if (rdr["CampaignID"].ToString() != "")
                    {
                        lUserMapping.oCampaign.Add(new BECampaignInfo { iCampaignID = int.Parse(rdr["CampaignID"].ToString()) });
                    }
                    lUserMapping.MappedOn = int.Parse(rdr["MappedON"].ToString());
                    lUserMapping.iUserProcessMapID = int.Parse(rdr["UserPRocessMapID"].ToString());
                }
            }
            return lUserMapping;
        }
        /// <summary>
        /// Gets the user setting.
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <returns></returns>
        public List<BEUserSetting> GetUserSetting(int iUserID)
        {
            List<BEUserSetting> lUserSetting = new List<BEUserSetting>();
            //var dbFactory = new DatabaseProviderFactory();
            //Database db = dbFactory.Create(DL_Shared.Connection);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_USERSETTING);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEUserSetting oUser = new BEUserSetting
                    {
                        ERPProcessID = int.Parse(rdr["ERPProcessID"].ToString()),
                        ERPProcessName = rdr["ERPProcessName"].ToString(),
                        MappedOn = int.Parse(rdr["MappedON"].ToString()),
                        JobID = int.Parse(rdr["JobID"].ToString()),
                        JobDesc = rdr["JobDesc"].ToString(),
                        LoginName = rdr["LoginName"].ToString(),
                        ProcessID = int.Parse(rdr["ProcessID"].ToString()),
                        ProcessName = rdr["ProcessName"].ToString(),
                        RoleID = int.Parse(rdr["RoleID"].ToString()),
                        RoleName = rdr["RoleName"].ToString(),
                        IsClientRole = Convert.ToBoolean(rdr["IsClientRole"])
                    };
                    lUserSetting.Add(oUser);
                    oUser = null;
                }
            }
            return lUserSetting;
        }

        //public List<BEUserSetting> GetRolesUserClient(int iUserID,bool isclient)
        //{
        //    List<BEUserSetting> lUserSetting = new List<BEUserSetting>();
        //    //var dbFactory = new DatabaseProviderFactory();
        //    //Database db = dbFactory.Create(DL_Shared.Connection);
        //    Database db = DL_Shared.dbFactory(_oTenant);
        //    DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_USERSETTING);
        //    db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
        //    db.AddInParameter(dbCommand, PARAM_ISCLIENTUSER, DbType.Int32, isclient);

        //    using (IDataReader rdr = db.ExecuteReader(dbCommand))
        //    {
        //        while (rdr.Read())
        //        {
        //            BEUserSetting oUser = new BEUserSetting
        //            {
        //                ERPProcessID = int.Parse(rdr["ERPProcessID"].ToString()),
        //                ERPProcessName = rdr["ERPProcessName"].ToString(),
        //                MappedOn = int.Parse(rdr["MappedON"].ToString()),
        //                JobID = int.Parse(rdr["JobID"].ToString()),
        //                JobDesc = rdr["JobDesc"].ToString(),
        //                LoginName = rdr["LoginName"].ToString(),
        //                ProcessID = int.Parse(rdr["ProcessID"].ToString()),
        //                ProcessName = rdr["ProcessName"].ToString(),
        //                RoleID = int.Parse(rdr["RoleID"].ToString()),
        //                RoleName = rdr["RoleName"].ToString()
        //            };
        //            lUserSetting.Add(oUser);
        //            oUser = null;
        //        }
        //    }
        //    return lUserSetting;
        //}

        #endregion

        #region user mapping
        /// <summary>
        /// Gets the user mapping.
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        /// <returns></returns>
        public BEUserMapping GetUserMapping(int iUserID, int iRoleID, int iMappedOn)
        {
            List<BEUserSetting> lUserSetting = new List<BEUserSetting>();
            //var dbFactory = new DatabaseProviderFactory();
            //Database db = dbFactory.Create(DL_Shared.Connection);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USER_SETTING);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
            db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, iRoleID);
            db.AddInParameter(dbCommand, PARAM_MAPPEDON, DbType.Int32, iMappedOn);
            List<BEClientInfo> lClient = new List<BEClientInfo>();
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();
            List<BECampaignInfo> lCampaignInfo = new List<BECampaignInfo>();
            BEUserMapping oUser = new BEUserMapping();

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    oUser.oRole = new BERoleInfo { iRoleID = int.Parse(rdr["RoleID"].ToString()) };
                    oUser.MappedOn = iMappedOn;
                    switch (iMappedOn)
                    {
                        case 1:
                            BEClientInfo oclient = new BEClientInfo { iClientID = int.Parse(rdr["ClientID"].ToString()), iApprovedAccess = int.Parse(rdr["Approved"].ToString()) };
                            lClient.Add(oclient);
                            oclient = null;
                            break;
                        case 2:
                            BEProcessInfo oProcess = new BEProcessInfo { iProcessID = int.Parse(rdr["ProcessID"].ToString()), iApprovedAccess = int.Parse(rdr["Approved"].ToString()) };
                            lProcess.Add(oProcess);
                            oProcess = null;
                            break;
                        case 3:
                            BECampaignInfo oCampaign = new BECampaignInfo { iCampaignID = int.Parse(rdr["CampaignID"].ToString()), iApprovedAccess = int.Parse(rdr["Approved"].ToString()) };
                            lCampaignInfo.Add(oCampaign);
                            oclient = null;
                            break;
                    }
                }

            }
            oUser.oCampaign = lCampaignInfo;
            oUser.oClient = lClient;
            oUser.oProcess = lProcess;
            return oUser;
        }
        #endregion

        #region Insert User Mapping
        /// <summary>
        /// Inserts the user mapping.
        /// </summary>
        /// <param name="oUserMapping">The user mapping.</param>
        /// <param name="sDeletedNodes">The deleted nodes.</param>
        public void InsertUserMapping(BEUserMapping oUserMapping, string sDeletedNodes)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbInsertUserMapping = db.GetStoredProcCommand(SP_INSERT_USERSETTING);
            db.AddInParameter(dbInsertUserMapping, PARAM_ROLEID, DbType.Int32, oUserMapping.oRole.iRoleID);
            db.AddInParameter(dbInsertUserMapping, PARAM_USERID, DbType.Int32, oUserMapping.oUser.iUserID);
            db.AddInParameter(dbInsertUserMapping, PARAM_CREATEDBY, DbType.Int32, oUserMapping.iCreatedBy);
            db.AddInParameter(dbInsertUserMapping, PARAM_DISABLED, DbType.Boolean, oUserMapping.bDisabled);
            db.AddInParameter(dbInsertUserMapping, PARAM_MAPPEDON, DbType.Int32, oUserMapping.MappedOn);
            db.AddInParameter(dbInsertUserMapping, PARAM_DELETEDNODES, DbType.String, sDeletedNodes);
            string TableString = "";
            switch (oUserMapping.MappedOn)
            {
                case 1:
                    for (int i = 0; i < oUserMapping.oClient.Count; i++)
                    {
                        TableString += oUserMapping.oClient[i].iClientID.ToString() + ",";

                    }
                    break;
                case 2:
                    for (int i = 0; i < oUserMapping.oProcess.Count; i++)
                    {
                        TableString += oUserMapping.oProcess[i].iProcessID.ToString() + ",";
                    }
                    break;
                case 3:
                    for (int i = 0; i < oUserMapping.oCampaign.Count; i++)
                    {
                        TableString += oUserMapping.oCampaign[i].iCampaignID.ToString() + ",";
                    }
                    break;

            }
            db.AddInParameter(dbInsertUserMapping, "@TableItems", DbType.String, TableString);

            try
            {
                db.ExecuteNonQuery(dbInsertUserMapping);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        public void CancelRequestInBetween(int iRequestID, int iUserID)
        {
            //var dbFactory = new DatabaseProviderFactory();
            //Database db = dbFactory.Create(DL_Shared.Connection);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_CANCEL_ACCESSREQUEST_INBETWEEN);
            db.AddInParameter(dbCommand, PARAM_REQUESTID, DbType.Int32, iRequestID);
            db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, iUserID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw;// new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        /// <summary>
        /// Approves the access request.
        /// </summary>
        /// <param name="iRequestID">The i request ID.</param>
        /// <param name="iRequestTypeID">The i request type ID.</param>
        /// <param name="iRequestType">Type of the i request.</param>
        /// <param name="iApprovalLevel">The i approval level.</param>
        /// <param name="iUserID">The i user ID.</param>
        /// <param name="iFormID">The i form ID.</param>
        public void ApproveAccessRequest(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iUserID)
        {
            //var dbFactory = new DatabaseProviderFactory();
            //Database db = dbFactory.Create(DL_Shared.Connection);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_APPROVE_ACCESSREQUEST);
            db.AddInParameter(dbCommand, PARAM_REQUESTID, DbType.Int32, iRequestID);
            db.AddInParameter(dbCommand, PARAM_REQUESTTYPEID, DbType.Int32, iRequestTypeID);
            db.AddInParameter(dbCommand, PARAM_REQUESTTYPE, DbType.Int32, iRequestType);
            db.AddInParameter(dbCommand, PARAM_APPROVALLEVEL, DbType.Int32, iApprovalLevel);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                //if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_Requestor_request_approver))
                //{
                //    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_Requestor_request_approver);
                //}
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        /// <summary>
        /// Rejects the access request.
        /// </summary>
        /// <param name="iRequestID">The i request ID.</param>
        /// <param name="iRequestTypeID">The i request type ID.</param>
        /// <param name="iRequestType">Type of the i request.</param>
        /// <param name="iApprovalLevel">The i approval level.</param>
        /// <param name="iUserID">The i user ID.</param>
        /// <param name="iFormID">The i form ID.</param>
        public void RejectAccessRequest(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iUserID)
        {
            //var dbFactory = new DatabaseProviderFactory();
            //Database db = dbFactory.Create(DL_Shared.Connection);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_REJECT_ACCESSREQUEST);
            db.AddInParameter(dbCommand, PARAM_REQUESTID, DbType.Int32, iRequestID);
            db.AddInParameter(dbCommand, PARAM_REQUESTTYPEID, DbType.Int32, iRequestTypeID);
            db.AddInParameter(dbCommand, PARAM_REQUESTTYPE, DbType.Int32, iRequestType);
            db.AddInParameter(dbCommand, PARAM_APPROVALLEVEL, DbType.Int32, iApprovalLevel);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw;// new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw;// new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        public void CancelAccessRequest(int iRequestID, int iUserID)
        {
            //var dbFactory = new DatabaseProviderFactory();
            //Database db = dbFactory.Create(DL_Shared.Connection);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_CANCEL_ACCESSREQUEST);
            db.AddInParameter(dbCommand, PARAM_REQUESTID, DbType.Int32, iRequestID);
            db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, iUserID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }/*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw;// new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw;// new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }

        /// <summary>
        /// Inserts the user mapping approvers.
        /// </summary>
        /// <param name="dtApproverList">The dt approver list.</param>
        /// <param name="iUserID">The i user ID.</param>
        public void InsertUserMappingApprovers(DataTable dtApproverList, int iUserID)
        {
            //var dbFactory = new DatabaseProviderFactory();
            //Database db = dbFactory.Create(DL_Shared.Connection);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbInsertUserMapping = null;
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open

                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        for (int i = 0; i < dtApproverList.Rows.Count; i++)
                        {
                            dbInsertUserMapping = db.GetStoredProcCommand(SP_INSERT_USERMAPPINGAPPROVER);

                            db.AddInParameter(dbInsertUserMapping, PARAM_CREATEDBY, DbType.Int32, iUserID);
                            db.AddInParameter(dbInsertUserMapping, PARAM_REQUESTID, DbType.Int32, int.Parse(dtApproverList.Rows[i]["RequestId"].ToString()));
                            db.AddInParameter(dbInsertUserMapping, PARAM_REQUESTTYPE, DbType.Int32, int.Parse(dtApproverList.Rows[i]["RequestType"].ToString()));
                            db.AddInParameter(dbInsertUserMapping, PARAM_REQUESTTYPEID, DbType.Int32, int.Parse(dtApproverList.Rows[i]["RequestTypeId"].ToString()));
                            db.AddInParameter(dbInsertUserMapping, PARAM_APPROVER1ID, DbType.Int32, int.Parse(dtApproverList.Rows[i]["Approver1Id"].ToString()));
                            if (dtApproverList.Rows[i]["Approver2Id"].ToString() == "")
                                db.AddInParameter(dbInsertUserMapping, PARAM_APPROVER2ID, DbType.Int32, 0);
                            else
                                db.AddInParameter(dbInsertUserMapping, PARAM_APPROVER2ID, DbType.Int32, int.Parse(dtApproverList.Rows[i]["Approver2Id"].ToString()));
                            db.ExecuteNonQuery(dbInsertUserMapping, trans);
                        }
                        trans.Commit(); //Commit Transaction 
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();//Transaction RollBack
                        throw;
                    }
                }
                conn.Close();
            }

        }
        /// <summary>
        /// Inserts the user mapping for approval.
        /// </summary>
        /// <param name="oUserMapping">The o user mapping.</param>
        /// <param name="sDeletedNodes">The s deleted nodes.</param>
        /// <returns></returns>
        public int InsertUserMappingForApproval(BEUserMapping oUserMapping, string sDeletedNodes)
        {
            int retunvalue = 0;
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbInsertUserMapping = db.GetStoredProcCommand(SP_INSERT_USERSETTING_FORAPPROVAL);
            db.AddInParameter(dbInsertUserMapping, PARAM_ROLEID, DbType.Int32, oUserMapping.oRole.iRoleID);
            db.AddInParameter(dbInsertUserMapping, PARAM_USERID, DbType.Int32, oUserMapping.oUser.iUserID);
            db.AddInParameter(dbInsertUserMapping, PARAM_CREATEDBY, DbType.Int32, oUserMapping.iCreatedBy);
            db.AddInParameter(dbInsertUserMapping, PARAM_DISABLED, DbType.Boolean, oUserMapping.bDisabled);
            db.AddInParameter(dbInsertUserMapping, PARAM_MAPPEDON, DbType.Int32, oUserMapping.MappedOn);
            db.AddInParameter(dbInsertUserMapping, PARAM_EFFECTIVEDATE, DbType.DateTime, oUserMapping.dtEffectiveDate);
            db.AddInParameter(dbInsertUserMapping, PARAM_DELETEDNODES, DbType.String, sDeletedNodes);
            string TableString = "";
            switch (oUserMapping.MappedOn)
            {
                case 1:
                    for (int i = 0; i < oUserMapping.oClient.Count; i++)
                    {
                        TableString += oUserMapping.oClient[i].iClientID.ToString() + ",";

                    }
                    break;
                case 2:
                    for (int i = 0; i < oUserMapping.oProcess.Count; i++)
                    {
                        TableString += oUserMapping.oProcess[i].iProcessID.ToString() + ",";
                    }
                    break;
                case 3:
                    for (int i = 0; i < oUserMapping.oCampaign.Count; i++)
                    {
                        TableString += oUserMapping.oCampaign[i].iCampaignID.ToString() + ",";
                    }
                    break;

            }
            db.AddInParameter(dbInsertUserMapping, "@TableItems", DbType.String, TableString);

            try
            {
                retunvalue = Convert.ToInt32(db.ExecuteScalar(dbInsertUserMapping));
            }/*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                 if (ex.Number == 2627)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                if (ex.Message.Contains("Request for Approval is Pending for this User"))
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(ex.Message);
                }

                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
               
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            return retunvalue;
        }
        #endregion

        #region CheckAdmin User
        /// <summary>
        /// Determines whether [is admin user] [the specified userID].
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        /// <returns>
        /// 	<c>true</c> if [is admin user] [the specified userID]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAdminUser(int iUserID)
        {
            List<BEUserSetting> lUserSetting = new List<BEUserSetting>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_CHECKADMIN);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            bool isAdmin = bool.Parse(db.ExecuteScalar(dbCommand).ToString());
            return isAdmin;
        }

        /// <summary>
        /// Gets the default page.
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        /// <returns></returns>
        public string GetDefaultPage(int iUserID)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_DEFUALTPAGE);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);

            string url = db.ExecuteScalar(dbCommand).ToString();
            return url;
        }
        #endregion

        #region Get Access Request Approval Workflow Data

        /// <summary>
        /// Gets the type of the user request.
        /// </summary>
        /// <param name="oUserMapping">The o user mapping.</param>
        /// <param name="RequestId">The request id.</param>
        /// <returns></returns>
        public DataSet GetUserRequestType(BEUserMapping oUserMapping, int RequestId)
        {
            DataSet ds = new DataSet();
            //var dbFactory = new DatabaseProviderFactory();
            //Database db = dbFactory.Create(DL_Shared.Connection);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_CHECK_USERACCESS_REQUESTTYPE);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, oUserMapping.oUser.iUserID);
            db.AddInParameter(dbCommand, PARAM_REQUESTID, DbType.Int32, RequestId);
            db.AddInParameter(dbCommand, PARAM_MAPPEDON, DbType.Int32, oUserMapping.MappedOn);
            db.LoadDataSet(dbCommand, ds, "User");

            return ds;
        }
        /// <summary>
        /// Gets the user request status.
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        /// <param name="dtFromDate">The dt from date.</param>
        /// <param name="dtToDate">The dt to date.</param>
        /// <returns></returns>
        public DataSet GetUserRequestStatus(int iUserID, DateTime dtFromDate, DateTime dtToDate)
        {
            DataSet ds = new DataSet();
            //var dbFactory = new DatabaseProviderFactory();
            //Database db = dbFactory.Create(DL_Shared.Connection);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_USERACCESS_REQUESTSTATUS);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
            db.AddInParameter(dbCommand, PARAM_FROMDATE, DbType.DateTime, dtFromDate);
            db.AddInParameter(dbCommand, PARAM_TODATE, DbType.DateTime, dtToDate);
            db.LoadDataSet(dbCommand, ds, "User");
            return ds;
        }
        /// <summary>
        /// Gets the user approval list.
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        /// <returns></returns>
        public DataSet GetUserApprovalList(int iUserID)
        {
            DataSet ds = new DataSet();
            //var dbFactory = new DatabaseProviderFactory();
            //Database db = dbFactory.Create(DL_Shared.Connection);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_USERACCESS_APPROVALLIST);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
            db.LoadDataSet(dbCommand, ds, "User");
            return ds;
        }
        /// <summary>
        /// Gets the user approver list.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <param name="ClientId">The client id.</param>
        /// <param name="ProcessId">The process id.</param>
        /// <param name="Flag">The flag.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserApproverListByProcess(int UserId, int iFormId, int ProcessId)
        {
            List<BEUserInfo> lApprovelInfo = new List<BEUserInfo>();
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_USP_GETAPPROVERLIST);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            db.AddInParameter(dbCommand, PARAM_FORMID, DbType.Int32, iFormId);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, ProcessId);
            db.LoadDataSet(dbCommand, ds, "Approvers");
            foreach (DataRow dr in ds.Tables["Approvers"].Rows)
            {
                BEUserInfo obj = new BEUserInfo();
                obj.iUserID = Convert.ToInt32(dr["UserID"].ToString());
                obj.sUserName = dr["UserName"].ToString();
                lApprovelInfo.Add(obj);

            }
            return lApprovelInfo;
        }
        /// <summary>
        /// Gets the user approver list.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <param name="ClientId">The client id.</param>
        /// <param name="ProcessId">The process id.</param>
        /// <param name="Flag">The flag.</param>
        /// <returns></returns>
        public DataSet GetUserApproverList(int UserId, int ClientId, int ProcessId, int Flag, int iFormID)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_APPROVERLIST);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, ClientId);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, ProcessId);
            db.AddInParameter(dbCommand, PARAM_FORMID, DbType.Int32, iFormID);
            db.AddInParameter(dbCommand, PARAM_FLAG, DbType.Int32, Flag);
            db.LoadDataSet(dbCommand, ds, "Approvers");
            return ds;
        }

        public List<BEUserInfo> GetUserRoleApproverListUserRole(int iUserid, int iFormId)
        {
            List<BEUserInfo> lRoleInfo = new List<BEUserInfo>();
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_USP_GETAPPROVERLISTUSRROLE);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserid);
            db.AddInParameter(dbCommand, PARAM_FORMID, DbType.Int32, iFormId);
            db.LoadDataSet(dbCommand, ds, "Approvers");
            foreach (DataRow dr in ds.Tables["Approvers"].Rows)
            {
                BEUserInfo obj = new BEUserInfo();
                obj.iUserID = Convert.ToInt32(dr["UserID"].ToString());
                obj.sUserName = dr["UserName"].ToString();
                lRoleInfo.Add(obj);

            }
            return lRoleInfo;
        }
        /// <summary>
        /// Gets the user role approver list.
        /// </summary>
        /// <param name="iRoleID">The i role ID.</param>
        /// <returns></returns>
        public DataSet GetUserRoleApproverList(int iRoleID, int iFormId)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_ROLEAPPROVERLIST);
            db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, iRoleID);
            db.AddInParameter(dbCommand, PARAM_FORMID, DbType.Int32, iFormId);
            db.LoadDataSet(dbCommand, ds, "Approvers");
            return ds;
        }
        /// <summary>
        /// Gets the client request approver list.
        /// </summary>
        /// <param name="ProcessId">The process id.</param>
        /// <returns></returns>
        public DataSet GetClientRequestApproverList(int ProcessId)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENTREQUESTAPPROVERLIST);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, ProcessId);
            db.LoadDataSet(dbCommand, ds, "Approvers");
            return ds;
        }
        #endregion

        private const string PARAM_ACTIVEBOTUSER = "@ActiveBotUser";
        private const string PARAM_ACTIVEREASON = "@ActiveReason";
        private const string SQL_SP_BOT_GETBOTUSERS = @"USP_BOT_GetBOTUsers";
        private const string SQL_SP_BOT_GETDOWNTIMEREASON = @"USP_BOT_GetDowntimeReason";
        public List<BEUserInfo> GetBotUserList(int iUserID, int iProcessId, bool bActive)
        {
            DataSet ds = new DataSet();
            List<BEUserInfo> lBotreason = new List<BEUserInfo>();
            string[] aryTableNames = { "BotUserDetails" };
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_BOT_GETBOTUSERS);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessId);
            db.AddInParameter(dbCommand, PARAM_ACTIVEBOTUSER, DbType.Boolean, bActive);
            db.LoadDataSet(dbCommand, ds, aryTableNames);
            //return ds;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BEUserInfo obj = new BEUserInfo();
                obj.iUserID = Convert.ToInt32(dr["UserID"].ToString());
                obj.sName = dr["UserName"].ToString();
                lBotreason.Add(obj);
            }

            return lBotreason;
        }

        public List<BEBOTDowntimeInfo> GetDowntimeReason(bool bActive)
        {
            List<BEBOTDowntimeInfo> lBotreason = new List<BEBOTDowntimeInfo>();
            DataSet ds = new DataSet();
            string[] aryTableNames = { "BotDowntimeReason" };
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_BOT_GETDOWNTIMEREASON);
            db.AddInParameter(dbCommand, PARAM_ACTIVEREASON, DbType.Boolean, bActive);
            db.LoadDataSet(dbCommand, ds, aryTableNames);
            //return ds;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BEBOTDowntimeInfo obj = new BEBOTDowntimeInfo();
                obj.iDowntimeReasonId = Convert.ToInt32(dr["DowntimeReasonId"].ToString());
                obj.sDowntimeReason = dr["DowntimeReason"].ToString();
                lBotreason.Add(obj);
            }

            return lBotreason;
            ///
            //
        }

        private const string PARAM_CAPTUREID = "@CaptureId";

        private const string SQL_SP_GETSEARCHDATA = @"USP_GetBotDowntimeCapture";

        public List<BEBOTDowntimeInfo> GetSearchData(int iUserId, string CampaignName)
        {
            List<BEBOTDowntimeInfo> lBotreason = new List<BEBOTDowntimeInfo>();
            DataSet ds = new DataSet();
            string[] aryTableNames = { "BotDowntimeReason" };
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_GETSEARCHDATA);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNNAME, DbType.String, CampaignName);
            db.AddInParameter(dbCommand, PARAM_CAPTUREID, DbType.Int32, -1);
            db.LoadDataSet(dbCommand, ds, aryTableNames);
            //return ds;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BEBOTDowntimeInfo obj = new BEBOTDowntimeInfo();
                obj.iCaptureId = Convert.ToInt32(dr["CaptureId"].ToString());
                obj.sCampaignName = dr["CampaignName"].ToString();
                lBotreason.Add(obj);
            }
            return lBotreason;
            ///
            //
        }


        public List<BEBOTDowntimeInfo> GetSearchDataList(int iUserId, int CaptureId)
        {
            List<BEBOTDowntimeInfo> lBotreason = new List<BEBOTDowntimeInfo>();
            DataSet ds = new DataSet();
            string[] aryTableNames = { "BotDowntimeReason" };
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_GETSEARCHDATA);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNNAME, DbType.String, "");
            db.AddInParameter(dbCommand, PARAM_CAPTUREID, DbType.Int32, CaptureId);
            db.LoadDataSet(dbCommand, ds, aryTableNames);
            //return ds;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BEBOTDowntimeInfo obj = new BEBOTDowntimeInfo();
                obj.iCaptureId = Convert.ToInt32(dr["CaptureId"].ToString());
                obj.iCampaignID = Convert.ToInt32(dr["CampaignID"].ToString());
                obj.sCampaignName = dr["CampaignName"].ToString();
                obj.iClientID = Convert.ToInt32(dr["ClientID"].ToString());
                obj.iProcessID = Convert.ToInt32(dr["ProcessID"].ToString());
                obj.iDowntimeReasonId = Convert.ToInt32(dr["DowntimeReasonId"].ToString());
                obj.StartDate = Convert.ToDateTime(dr["StartDateTime"].ToString());
                obj.EndDate = Convert.ToDateTime(dr["EndDateTime"].ToString());
                obj.sDowntimeReason = dr["DowntimeReason"].ToString();
                obj.sModeIds = dr["BotUsersId"].ToString();
                obj.sComment = dr["Comment"].ToString();
                lBotreason.Add(obj);
            }
            return lBotreason;
            ///
            //
        }
        public void InsertUserPageLogin(string SystemSessionID, string HostName, string IPAddress, string PageName, int iUserId)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbInsertCommand = db.GetStoredProcCommand(PARAM_USERPAGELOGIN);
            db.AddInParameter(dbInsertCommand, PARAM_USERID, DbType.String, iUserId.ToString());
            db.AddInParameter(dbInsertCommand, PARAM_SYSTEMSESSIONID, DbType.String, SystemSessionID);
            db.AddInParameter(dbInsertCommand, PARAM_HOSTNAME, DbType.String, HostName);
            db.AddInParameter(dbInsertCommand, PARAM_IPADDRESS, DbType.String, IPAddress);
            db.AddInParameter(dbInsertCommand, PARAM_PageName, DbType.String, PageName);
            db.ExecuteNonQuery(dbInsertCommand);
        }

        public bool IsDomainActivation()
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbInsertCommand = db.GetStoredProcCommand(PARAM_GETDOMAIN);
            var result = db.ExecuteScalar(dbInsertCommand);
            return result != null ? (bool)result : false;
        }
    }
}
