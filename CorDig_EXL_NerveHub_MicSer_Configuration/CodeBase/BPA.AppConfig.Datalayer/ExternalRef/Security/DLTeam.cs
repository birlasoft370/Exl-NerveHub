using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.AppConfig.BusinessEntity.Security;

namespace BPA.AppConfig.Datalayer.ExternalRef.Security
{
    public class DLTeam : IDisposable
    {
        private BETenant _oTenant = null;
        public DLTeam(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SELECT_TEAM = @"WM.USP_GetTeamList";//"dbo.Usp_CDS_GetTeamList";
        private const string SQL_SELECT_TEAMBYID = @" SELECT TEAMINFO.TeamID,TEAMINFO.TeamName,TEAMINFO.Description,TEAMINFO.ProcessID,TEAMINFO.disabled
                                                      ,TEAMINFO.createdby,TEAMINFO.IsClientLevelTeam,ProcessMstr.ClientID FROM TM.tblTeamInfo TEAMINFO 
                                                       (NOLOCK) INNER JOIN Config.tblProcessMaster ProcessMstr on  TEAMINFO.ProcessID= ProcessMstr.ProcessID
                                                         WHERE TEAMINFO.TeamID =@TeamID ";

        private const string SQL_SELECT_PROCESSTEAM = @"SELECT TeamID,TeamName,Description,ProcessID, disabled,createdby FROM TM.tblTeamInfo (NOLOCK)
	                                                    WHERE Disabled=0 AND ProcessID = @ProcessID 	                                                    
	                                                    order by TeamName";
        private const string SQL_SELECT_CAMPTEAM = @"Usp_CDS_TeamList";
        private const string SQL_SELECT_CAMPTEAMDASBOARD = @"[WM].[Usp_GetTeamCampaignWise]";
        private const string SQL_SELECT_CAMPTEAMALL = @"SELECT TeamID,TeamName,Team.Description,Team.ProcessID, Team.disabled,Team.createdby FROM TM.tblTeamInfo Team (NOLOCK) INNER JOIN Config.tblCampaignMaster Camp on Team.ProcessID=Camp.ProcessID WHERE Camp.CampaignID =@CampId";
        private const string SQL_SELECT_TEAMMEBERBYID = @"SELECT distinct TM.tblTeamMemberInfo.TeamID, TM.tblTeamMemberInfo.UserID, Config.tblUserMaster.EmpID,ISNULL(Config.tblUserMaster.LoginName, '') As LoginName, ISNULL(Config.tblUserMaster.FirstName, '') " +
                                                        " + ' ' + ISNULL(Config.tblUserMaster.MiddleName, '') + ' ' + ISNULL(Config.tblUserMaster.LastName, '') AS Name" +
                                                        " FROM TM.tblTeamMemberInfo WITH (NOLOCK) INNER JOIN" +
                                                        " Config.tblUserMaster WITH (NOLOCK) ON TM.tblTeamMemberInfo.UserID = Config.tblUserMaster.UserID" +
                                                        " WHERE     (TM.tblTeamMemberInfo.Disabled = 0) AND (TM.tblTeamMemberInfo.TeamID = @TeamID)" +
                                                        " Order by TeamID,Name";
        private const string SQL_SELECT_MAXTEAMID = @"SELECT ISNULL(MAX(TeamID),1) FROM TM.tblTeamInfo (NOLOCK)";
        private const string SQL_SELECT_CHECKMEMBER = @"SELECT * FROM TM.tblTeamMemberInfo Where TeamID=@TeamID and UserID =@UserID";

        private const string SQL_UPDATE_INACTIVATEMEMBER = @"UPDATE TM.tblTeamMemberInfo SET Disabled=1, ModifiedBy =@ModifiedBy, ModifiedOn=GetDate()  WHERE TeamID=@teamID AND Disabled=0";
        private const string SQL_UPDATE_ACTIVATEMEMBER = @"UPDATE TM.tblTeamMemberInfo SET Disabled=0, ModifiedBy =@ModifiedBy, ModifiedOn=GetDate() ,EffectiveDate=GetDate()  WHERE TeamID=@teamID AND UserID=@UserID AND Disabled=1";

        private const string SQL_INSERT_TEAM = @"INSERT INTO TM.tblTeamInfo(TeamName,Description,ProcessID,disabled,createdby,IsClientLevelTeam) VALUES (@TeamName,@Description,@ProcessID,@disabled,@CreatedBy,@IsClientLevelTeam)";
        private const string SQL_INSERT_TEAMMEMBER = @"INSERT INTO TM.tblTeamMemberInfo (TeamID,UserID,Createdby,EffectiveDate) VALUES (@TeamID,@UserID,@CreatedBy,GetDate() )";

        private const string SQL_UPDATE_TEAM = @"UPDATE TM.tblTeamInfo SET TeamName=@TeamName,ProcessID=@ProcessID,Description=@Description,Disabled=@Disabled,ModifiedBy=@ModifiedBy,ModifiedOn=GetDate() ,IsClientLevelTeam=@IsClientLevelTeam WHERE TeamID=@TeamID";

        private const string SQL_DELETE_TEAM = @"DELETE FROM TM.tblTeamInfo WHERE TeamID=@TeamID";
        private const string SQL_DELETE_AGENT = @"UPDATE TM.tblTeamMemberInfo SET Disabled=1, ModifiedBy =@ModifiedBy, ModifiedOn=GetDate()  WHERE UserID=@UserID AND Disabled=0";
        private const string SQL_DELETE_TEAMMBER = @"DELETE FROM TM.tblTeamMemberInfo WHERE TeamID=@TeamID";
        private const string SQL_SELECT_AGENTUSERS = @" select UserMaster.UserID, isnull(FirstName,'') + ' ' + isnull(MiddleName,'') + ' ' + isnull(LastName,'') as UserName from TM.tblTeamMemberInfo (nolock)  Team inner join Config.tblUserMaster (nolock)  UserMaster  on Team.UserID=UserMaster.UserID and Team.Disabled=0 and UserMaster.Disabled=0 where Team.TeamId in (select TeamId from TM.tblTeamMemberInfo (nolock) where UserId=@UserID and Disabled=0)";
        private const string SQL_SELECT_CLIENTWISETEAM = @"Usp_CDS_ClientWiseTeamList";

        private const string SQL_SELECT_TEAMDUPLICACY = @"select count(*) from TM.tblTeamInfo (nolock) where TeamName=@TeamName";
        private const string SQL_SELECT_CAMPSKILL = @"Select CSM.CampaignSkillID,SM.SkillName from Config.tblCampaignSkillMap (NoLock) CSM join Config.tblSkillMaster (NoLock) SM 
                                                       on CSM.SkillID = SM.SkillID where CampaignID=@CampaignId and SM.Disabled=0 and CSM.Disabled = 0";


        private const string PARAM_USERID = "@UserID";
        private const string PARAM_CAMPID = "@CampId";
        private const string PARAM_TEAMID = "@TeamID";
        private const string PARAM_TEAMNAME = "@TeamName";
        private const string PARAM_PROCESSID = "@ProcessID";
        private const string PARAM_DESCRIPTION = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBY";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_ACTIVETEAMLIST = "@ActiveTeamList";
        private const string PARAM_CURRENTUSER = "@CurrentUserID";
        private const string PARAM_CLIENTID = "@ClientID";
        private const string PARAM_CLIENTLEVEL = "@IsClientLevelTeam";

        private const string PARAM_CAMPAIGNID = "@CampaignId";

        public List<BETeamInfo> GetTeamList(int iLoggedinUserID, bool bActiveTeam)
        {
            return GetTeamList("", iLoggedinUserID, bActiveTeam);
        }

        public List<BETeamInfo> GetTeamList(string sTeamName, int iLoggedinUserID, bool bActiveTeam)
        {
            List<BETeamInfo> lTeamInfo = new List<BETeamInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SELECT_TEAM);
            db.AddInParameter(dbCommand, PARAM_TEAMNAME, DbType.String, sTeamName + "%");
            db.AddInParameter(dbCommand, PARAM_CURRENTUSER, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVETEAMLIST, DbType.Boolean, bActiveTeam);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BETeamInfo item = new BETeamInfo(Convert.ToInt32(rdr["TeamID"]), rdr["TeamName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToInt32(rdr["ProcessID"]), Convert.ToBoolean(rdr["Disabled"]), Convert.ToInt32(rdr["CreatedBy"]), rdr["Clientname"].ToString(), rdr["ProcessName"].ToString());
                    lTeamInfo.Add(item);
                }
            }
            return lTeamInfo;
        }


        public List<BETeamInfo> GetTeamList(int iTeamID)
        {
            List<BETeamInfo> lTeamInfo = new List<BETeamInfo>();
            List<BEUserInfo> lUserInfo = new List<BEUserInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_TEAMBYID);
            db.AddInParameter(dbCommand, PARAM_TEAMID, DbType.Int32, iTeamID);

            DbCommand dbUserCommand = db.GetSqlStringCommand(SQL_SELECT_TEAMMEBERBYID);
            db.AddInParameter(dbUserCommand, PARAM_TEAMID, DbType.Int32, iTeamID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BETeamInfo item = new BETeamInfo(Convert.ToInt32(rdr["TeamID"]), rdr["TeamName"].ToString(), rdr["Description"].ToString(), Convert.ToInt32(rdr["ProcessID"]), Convert.ToBoolean(rdr["Disabled"]), Convert.ToInt32(rdr["CreatedBy"]), rdr["IsClientLevelTeam"] == DBNull.Value ? false : Convert.ToBoolean(rdr["IsClientLevelTeam"]), Convert.ToInt32(rdr["ClientID"]));

                    using (IDataReader dr = db.ExecuteReader(dbUserCommand))
                    {
                        // Scroll through the results
                        while (dr.Read())
                        {
                            BEUserInfo useritem = new BEUserInfo();
                            useritem.iUserID = Convert.ToInt32(dr["UserID"]);
                            useritem.iEmployeeID = Convert.ToInt32(dr["EmpID"]);
                            useritem.sFirstName = dr["Name"].ToString();
                            useritem.sLoginName = dr["LoginName"].ToString();
                            useritem.bChecked = false;
                            lUserInfo.Add(useritem);
                            useritem = null;
                        }
                    }
                    item.iUserID = lUserInfo;
                    lTeamInfo.Add(item);
                    item = null;
                }
            }
            return lTeamInfo;
        }
        public void InsertData(BETeamInfo oTeam)
        {
            try
            {
                //*************************************
                //  Database db = DL_Shared.dbFactory(_oTenant);

                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbInsertTeamCommand = db.GetSqlStringCommand(SQL_INSERT_TEAM);
                db.AddInParameter(dbInsertTeamCommand, PARAM_TEAMNAME, DbType.String, oTeam.sTeamName);
                db.AddInParameter(dbInsertTeamCommand, PARAM_DESCRIPTION, DbType.String, oTeam.sTeamDesc);
                db.AddInParameter(dbInsertTeamCommand, PARAM_PROCESSID, DbType.Int32, oTeam.iProcessID);
                db.AddInParameter(dbInsertTeamCommand, PARAM_DISABLED, DbType.Boolean, oTeam.bDisabled);
                db.AddInParameter(dbInsertTeamCommand, PARAM_CREATEDBY, DbType.Int32, oTeam.iCreatedBy);
                db.AddInParameter(dbInsertTeamCommand, PARAM_CLIENTLEVEL, DbType.Boolean, oTeam.bClientLevel);

                //*************************************
                DbCommand dbInsertTMemeberCommand = db.GetSqlStringCommand(SQL_INSERT_TEAMMEMBER);
                db.AddInParameter(dbInsertTMemeberCommand, PARAM_TEAMID, DbType.Int32);
                db.AddInParameter(dbInsertTMemeberCommand, PARAM_USERID, DbType.Int32);
                db.AddInParameter(dbInsertTMemeberCommand, PARAM_CREATEDBY, DbType.Int32, oTeam.iCreatedBy);

                //*************************************
                DbCommand dbSelectMaxCommand = db.GetSqlStringCommand(SQL_SELECT_MAXTEAMID);

                //*************************************
                DbCommand dbDeleteAgent = db.GetSqlStringCommand(SQL_DELETE_AGENT);
                db.AddInParameter(dbDeleteAgent, PARAM_USERID, DbType.Int32);
                db.AddInParameter(dbDeleteAgent, PARAM_MODIFIEDBY, DbType.Int32, oTeam.iCreatedBy);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbInsertTeamCommand, trans);
                            int MaxID = (int)db.ExecuteScalar(dbSelectMaxCommand, trans);
                            db.SetParameterValue(dbInsertTMemeberCommand, PARAM_TEAMID, MaxID);

                            int MemnerCount = oTeam.iUserID.Count;
                            for (int i = 0; i < MemnerCount; i++)
                            {
                                db.SetParameterValue(dbDeleteAgent, PARAM_USERID, oTeam.iUserID[i].iUserID);
                                db.ExecuteNonQuery(dbDeleteAgent, trans);   // delete the instance of user form all the team; 
                                db.SetParameterValue(dbInsertTMemeberCommand, PARAM_USERID, oTeam.iUserID[i].iUserID);
                                db.ExecuteNonQuery(dbInsertTMemeberCommand, trans);// Insert the user into the team.
                            }
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
        public void UpdateData(BETeamInfo oTeam)
        {
            try
            {
                //*************************************

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbUpdateTeamCommand = db.GetSqlStringCommand(SQL_UPDATE_TEAM);
                db.AddInParameter(dbUpdateTeamCommand, PARAM_TEAMID, DbType.Int32, oTeam.iTeamID);
                db.AddInParameter(dbUpdateTeamCommand, PARAM_TEAMNAME, DbType.String, oTeam.sTeamName);
                db.AddInParameter(dbUpdateTeamCommand, PARAM_DESCRIPTION, DbType.String, oTeam.sTeamDesc);
                db.AddInParameter(dbUpdateTeamCommand, PARAM_PROCESSID, DbType.Int32, oTeam.iProcessID);
                db.AddInParameter(dbUpdateTeamCommand, PARAM_DISABLED, DbType.Boolean, oTeam.bDisabled);
                db.AddInParameter(dbUpdateTeamCommand, PARAM_MODIFIEDBY, DbType.Int32, oTeam.iCreatedBy);
                db.AddInParameter(dbUpdateTeamCommand, PARAM_CLIENTLEVEL, DbType.Boolean, oTeam.bClientLevel);

                //*************************************
                DbCommand dbInactivateMemberCommand = db.GetSqlStringCommand(SQL_UPDATE_INACTIVATEMEMBER);

                db.AddInParameter(dbInactivateMemberCommand, PARAM_TEAMID, DbType.Int32, oTeam.iTeamID);
                db.AddInParameter(dbInactivateMemberCommand, PARAM_MODIFIEDBY, DbType.Int32, oTeam.iCreatedBy);
                //*************************************
                DbCommand dbInsertTMemeberCommand = db.GetSqlStringCommand(SQL_INSERT_TEAMMEMBER);
                db.AddInParameter(dbInsertTMemeberCommand, PARAM_TEAMID, DbType.Int32, oTeam.iTeamID);
                db.AddInParameter(dbInsertTMemeberCommand, PARAM_USERID, DbType.Int32);
                db.AddInParameter(dbInsertTMemeberCommand, PARAM_CREATEDBY, DbType.Int32, oTeam.iCreatedBy);

                //*************************************
                DbCommand dbCheckMemberCommand = db.GetSqlStringCommand(SQL_SELECT_CHECKMEMBER);
                db.AddInParameter(dbCheckMemberCommand, PARAM_TEAMID, DbType.Int32, oTeam.iTeamID);
                db.AddInParameter(dbCheckMemberCommand, PARAM_USERID, DbType.Int32);

                //*************************************
                DbCommand dbActivateMemberCommand = db.GetSqlStringCommand(SQL_UPDATE_ACTIVATEMEMBER);
                db.AddInParameter(dbActivateMemberCommand, PARAM_TEAMID, DbType.Int32, oTeam.iTeamID);
                db.AddInParameter(dbActivateMemberCommand, PARAM_USERID, DbType.Int32);
                db.AddInParameter(dbActivateMemberCommand, PARAM_MODIFIEDBY, DbType.Int32, oTeam.iCreatedBy);

                //*************************************
                DbCommand dbDeleteAgent = db.GetSqlStringCommand(SQL_DELETE_AGENT);
                db.AddInParameter(dbDeleteAgent, PARAM_USERID, DbType.Int32);
                db.AddInParameter(dbDeleteAgent, PARAM_MODIFIEDBY, DbType.Int32, oTeam.iCreatedBy);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbUpdateTeamCommand, trans);
                            db.ExecuteNonQuery(dbInactivateMemberCommand, trans);

                            int MemberCount = oTeam.iUserID.Count;
                            for (int i = 0; i < MemberCount; i++)
                            {
                                db.SetParameterValue(dbDeleteAgent, PARAM_USERID, oTeam.iUserID[i].iUserID);
                                db.ExecuteNonQuery(dbDeleteAgent, trans);   // delete the instance of user form all the team; 

                                db.SetParameterValue(dbCheckMemberCommand, PARAM_USERID, oTeam.iUserID[i].iUserID);
                                object oRowCounter = db.ExecuteScalar(dbCheckMemberCommand, trans);
                                if (oRowCounter == null)
                                {
                                    db.SetParameterValue(dbInsertTMemeberCommand, PARAM_USERID, oTeam.iUserID[i].iUserID);
                                    db.ExecuteNonQuery(dbInsertTMemeberCommand, trans);
                                }
                                else
                                {
                                    db.SetParameterValue(dbActivateMemberCommand, PARAM_USERID, oTeam.iUserID[i].iUserID);
                                    //db.ExecuteNonQuery(dbActivateMemberCommand, trans);
                                    db.SetParameterValue(dbInsertTMemeberCommand, PARAM_USERID, oTeam.iUserID[i].iUserID);
                                    db.ExecuteNonQuery(dbInsertTMemeberCommand, trans);
                                }
                            }

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
        public List<BETeamInfo> GetProcessWiseTeamList(int iProcess)
        {
            List<BETeamInfo> lTeamInfo = new List<BETeamInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSTEAM);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcess);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BETeamInfo item = new BETeamInfo(Convert.ToInt32(rdr[0]), rdr.GetString(1).ToString(), rdr.GetString(2).ToString(), rdr.GetInt32(3), rdr.GetBoolean(4), rdr.GetInt32(5), "", "");
                    lTeamInfo.Add(item);
                }
            }
            return lTeamInfo;
        }
    }
}
