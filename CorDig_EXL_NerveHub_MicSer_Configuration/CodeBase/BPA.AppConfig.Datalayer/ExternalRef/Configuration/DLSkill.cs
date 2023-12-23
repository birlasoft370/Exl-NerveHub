using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;

namespace BPA.AppConfig.Datalayer.ExternalRef.Configuration
{
    public class DLSkill : IDisposable
    {
        private BETenant _oTenant = null;

        public DLSkill(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SELECT_SKILLALL = @"SELECT SkillID,SkillName + (case DISABLED when 1 then '(Disabled)' else '' end) as SkillName,isnull(Description,'') as DESCRIPTION,DISABLED,createdOn FROM Config.tblSkillMaster(NOLOCK) WHERE SkillName Like '%'+ @SkillName ORDER BY SkillName";
        private const string SQL_SELECT_SKILLID = @"SELECT SkillID,SkillName,isnull(Description,'') as DESCRIPTION,DISABLED,createdOn FROM Config.tblSkillMaster(NOLOCK) WHERE SkillID = @SkillID";
        private const string SQL_INSERT_SKILL = @"if exists(select SkillName from Config.tblSkillMaster where SkillName=@SkillName)
                                                                 Begin
                                                                 select 'Skill Name already exists'
                                                                 End
                                                                 else
                                                                 Begin
                                                                INSERT INTO Config.tblSkillMaster (SkillName,Description,Disabled,CreatedBy) VALUES(@SkillName,@SkillDesc,@Disabled,@CreatedBy)
                                                                  select ''  
                                                                  End";
        // private const string SQL_UPDATE_SKILL = @"UPDATE Config.tblSkillMaster SET SkillName=@SkillName,Description=@SkillDesc,Disabled=@Disabled,MODIFIEDBY=@ModifiedBy , MODIFIEDON=GetDate()  WHERE SkillID=@SkillID";
        private const string SQL_UPDATE_SKILL = @"if exists(select SkillName from Config.tblSkillMaster where SkillName=@SkillName and SkillID !=@SkillID)
                                                                 Begin
                                                                 select 'Skill Name already exists'
                                                                 End
                                                                 else
                                                                 Begin
                                                                   UPDATE Config.tblSkillMaster SET SkillName=@SkillName,Description=@SkillDesc,Disabled=@Disabled,MODIFIEDBY=@ModifiedBy , MODIFIEDON=GetDate()  WHERE SkillID=@SkillID
                                                                  select ''  
                                                                  End";
        private const string SQL_SELECT_ACTIVESKILL = @"SELECT SkillID,SkillName FROM Config.tblSkillMaster(NOLOCK) WHERE DISABLED=0";
        private const string SQL_INSERT_CAMPAIGNSKILL = @"INSERT INTO Config.tblCampaignSkillMap(CampaignID, SkillID, CreatedBy) values(@CampaignID, @SkillID, @CreatedBy)";
        private const string SQL_SELECT_CHECKSKILL = @"SELECT * FROM Config.tblCampaignSkillMap (NOLOCK) Where CampaignID=@CampaignID and SkillID =@SkillID";
        private const string SQL_UPDATE_INACTIVATESKILL = @"UPDATE Config.tblCampaignSkillMap SET Disabled=1 WHERE CampaignID=@CampaignID";
        private const string SQL_UPDATE_ACTIVATESKILL = @"UPDATE Config.tblCampaignSkillMap SET Disabled=0 Where CampaignID=@CampaignID and SkillID =@SkillID";
        private const string SQL_SELECT_SKILLBYCAMPID = @"select Skill.SkillID, Skill.SkillName 
                                                        from Config.tblCampaignMaster CampMaster(NOLOCK)
                                                        inner join Config.tblCampaignSkillMap CampSkillMap (NOLOCK)
	                                                        on CampMaster.CampaignID=CampSkillMap.CampaignID 
                                                        INNER JOIN Config.tblSkillMaster Skill (NOLOCK) 
	                                                        on Skill.SkillID=CampSkillMap.SkillID 
                                                        where CampMaster.CampaignID=@CampaignID and CampSkillMap.Disabled=0 and Skill.Disabled=0";
        private const string SQL_SELECT_MAPSKILL = @"Select USM.UserId, CSM.SkillID, SM.SkillName from Config.tblUserSkillMap (NoLock) USM join Config.tblCampaignSkillMap (NoLock) CSM
                                                        on USM.CampaignSkillID = CSM.CampaignSkillID 
                                                        join Config.tblSkillMaster (NoLock) SM on SM.SkillID = CSM.SkillID
                                                       where CSM.CampaignID = @CampaignID and CSM.Disabled=0 and USM.Disabled=0 and SM.Disabled=0";
        private const string SP_INSERTUPDATE_USERSKILL = @"[Config].[USP_InsertUpdateUserSkillMap]";

        private const string PARAM_SKILLID = "@SkillID";
        private const string PARAM_SKILLNAME = "@SkillName";
        private const string PARAM_SKILLDESC = "@SkillDesc";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_CAMPAIGNID = "@CampaignID";
        private const string PARAM_USERID = "@UserID";
        private const string PARAM_XML = "@strDataXml";
        private const string PARAM_CAMPAIGNId = "@CampaignId";

        public List<BESkillInfo> GetSkillList(int iSkillID)
        {
            //throw new System.NotImplementedException();
            List<BESkillInfo> ISkill = new List<BESkillInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_SKILLID);
            db.AddInParameter(dbCommand, PARAM_SKILLID, DbType.Int32, iSkillID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BESkillInfo objSkill = new BESkillInfo(Convert.ToInt32(rdr["SkillID"]), rdr["SkillName"].ToString(), rdr["DESCRIPTION"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0, Convert.ToDateTime(rdr["CreatedOn"]));

                    ISkill.Add(objSkill);
                    objSkill = null;

                }
            }
            return ISkill;
        }

        public string InsertData(BESkillInfo oShift)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_SKILL);
                db.AddInParameter(dbCommand, PARAM_SKILLID, DbType.Int32, oShift.iSkillID);
                db.AddInParameter(dbCommand, PARAM_SKILLNAME, DbType.String, oShift.sSkillName);
                db.AddInParameter(dbCommand, PARAM_SKILLDESC, DbType.String, oShift.sSkillDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oShift.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oShift.iCreatedBy);
                return db.ExecuteScalar(dbCommand).ToString();
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

        public List<BESkillInfo> GetSkillListByName(string sSkillName, bool bGetAll)
        {
            List<BESkillInfo> lstSkill = new List<BESkillInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_SKILLALL);
            db.AddInParameter(dbCommand, PARAM_SKILLNAME, DbType.String, "%" + sSkillName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BESkillInfo objSkill = new BESkillInfo(Convert.ToInt32(rdr["SkillID"]), rdr["SkillName"].ToString(), rdr["DESCRIPTION"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0, Convert.ToDateTime(rdr["CreatedOn"]));
                    lstSkill.Add(objSkill);
                    objSkill = null;

                }
            }
            return lstSkill;
        }

        public string UpdateData(BESkillInfo oShift)
        {
            try
            {


                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_SKILL);
                db.AddInParameter(dbCommand, PARAM_SKILLID, DbType.Int32, oShift.iSkillID);
                db.AddInParameter(dbCommand, PARAM_SKILLNAME, DbType.String, oShift.sSkillName);
                db.AddInParameter(dbCommand, PARAM_SKILLDESC, DbType.String, oShift.sSkillDescription);

                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oShift.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oShift.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oShift.iModifiedBy);

                return db.ExecuteScalar(dbCommand).ToString();
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

        public List<BESkillInfo> GetActiveSkillList()
        {
            List<BESkillInfo> lstSkill = new List<BESkillInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_ACTIVESKILL);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BESkillInfo objSkill = new BESkillInfo()
                    {
                        iSkillID = Convert.ToInt32(rdr["SkillID"]),
                        sSkillName = rdr["SkillName"].ToString()
                    };
                    lstSkill.Add(objSkill);
                    objSkill = null;

                }
            }
            return lstSkill;
        }

        public List<BESkillInfo> GetCampaignSkillList(int CampaignID)
        {
            BECampaignInfo objCamp = new BECampaignInfo();
            List<BECampaignInfo> lCampaign = new List<BECampaignInfo>();
            List<BESkillInfo> lSkillInfo = new List<BESkillInfo>();


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbSkillCommand = db.GetSqlStringCommand(SQL_SELECT_SKILLBYCAMPID);
            db.AddInParameter(dbSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignID);

            using (IDataReader dr = db.ExecuteReader(dbSkillCommand))
            {
                // Scroll through the results
                while (dr.Read())
                {
                    BESkillInfo SkillItem = new BESkillInfo();
                    SkillItem.iSkillID = Convert.ToInt32(dr["SkillID"]);
                    SkillItem.sSkillName = dr["SkillName"].ToString();
                    lSkillInfo.Add(SkillItem);
                    SkillItem = null;
                }
            }
            objCamp.iSkillID = lSkillInfo;
            //lCampaign.Add(objCamp);
            objCamp = null;
            return lSkillInfo;
        }

        public void UpdateCampSkillMap(int CampaignId, string strSkillID, int CreatedBy)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbInsertCSkillCommand = db.GetSqlStringCommand(SQL_INSERT_CAMPAIGNSKILL);
            db.AddInParameter(dbInsertCSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignId);
            db.AddInParameter(dbInsertCSkillCommand, PARAM_SKILLID, DbType.Int32);
            db.AddInParameter(dbInsertCSkillCommand, PARAM_CREATEDBY, DbType.Int32, CreatedBy);

            ////*********************************************************************************
            DbCommand dbCheckSkillCommand = db.GetSqlStringCommand(SQL_SELECT_CHECKSKILL);
            db.AddInParameter(dbCheckSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignId);
            db.AddInParameter(dbCheckSkillCommand, PARAM_SKILLID, DbType.Int32);

            DbCommand dbInactivateSkillCommand = db.GetSqlStringCommand(SQL_UPDATE_INACTIVATESKILL);
            db.AddInParameter(dbInactivateSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignId);

            DbCommand dbActivateSkillCommand = db.GetSqlStringCommand(SQL_UPDATE_ACTIVATESKILL);
            db.AddInParameter(dbActivateSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignId);
            db.AddInParameter(dbActivateSkillCommand, PARAM_SKILLID, DbType.Int32);

            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open
                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {

                        db.ExecuteNonQuery(dbInactivateSkillCommand, trans);
                        string[] strSkillArray = strSkillID.Split(',');

                        for (int i = 0; i < strSkillArray.Length; i++)
                        {
                            db.SetParameterValue(dbCheckSkillCommand, PARAM_SKILLID, strSkillArray[i]);
                            object oRowCounter = db.ExecuteScalar(dbCheckSkillCommand, trans);
                            if (oRowCounter == null)
                            {
                                db.SetParameterValue(dbInsertCSkillCommand, PARAM_SKILLID, strSkillArray[i]);
                                db.ExecuteNonQuery(dbInsertCSkillCommand, trans);
                            }
                            else
                            {
                                db.SetParameterValue(dbActivateSkillCommand, PARAM_SKILLID, strSkillArray[i]);
                                db.ExecuteNonQuery(dbActivateSkillCommand, trans);
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

        public List<BEUserSkillMap> GetMapSkill(int iCampaignId)
        {
            List<BEUserSkillMap> lSkillInfo = new List<BEUserSkillMap>();


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_MAPSKILL);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampaignId);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BEUserSkillMap item = new BEUserSkillMap();
                    item.iUserID = int.Parse(rdr["UserId"].ToString());
                    item.iSkillID = int.Parse(rdr["SkillID"].ToString());
                    item.sSkillName = rdr["SkillName"].ToString();
                    lSkillInfo.Add(item);
                    item = null;
                }
            }
            return lSkillInfo;
        }

        public void InsertUserSkillMap(string strXml)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbInsertCSkillCommand = db.GetStoredProcCommand(SP_INSERTUPDATE_USERSKILL);
                db.AddInParameter(dbInsertCSkillCommand, PARAM_XML, DbType.String, strXml);
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbInsertCSkillCommand, trans);
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
                db.ExecuteNonQuery(dbInsertCSkillCommand);
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


    }
}
