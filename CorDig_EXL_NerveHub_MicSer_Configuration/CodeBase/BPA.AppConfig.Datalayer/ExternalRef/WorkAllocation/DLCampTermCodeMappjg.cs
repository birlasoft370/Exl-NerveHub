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
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;

namespace BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation
{
    public class DLCampTermCodeMappjg : IDisposable
    {
        private BETenant _oTenant = null;
        public DLCampTermCodeMappjg(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }
        private const string SQL_INSERT_CAMPTERMINATIONMAP = @"INSERT INTO WM.tblCampaignTermCodeMap(CampaignID,TerminationID,Disabled,IsProductive,CreatedBy,IsEnd)" +
                                                            " VALUES(@CampaignID,@TerminationID,@Disabled,@IsProductive,@CreatedBy,@IsEnd)";

        private const string SQL_CHECK_CAMPTERMINATIONMAP = @"SELECT COUNT(*) FROM WM.tblCampaignTermCodeMap (NoLock) where CampaignID = @CampaignID AND TerminationID=@TerminationID";

        private const string SQL_DELETE_CAMPTERMINATIONMAP = "Delete from WM.tblCampaignTermCodeMap where CampaignID =@CampaignID ";


        private const string SQL_SELECT_CAMPTERMMAPPEDDETLS = @"SELECT CampaignID,TerminationID,Disabled,IsProductive,IsEnd " +
                                                               " FROM WM.tblCampaignTermCodeMap (nolock) Where CampaignID=@CampaignID";

        private const string SQL_SELECT_CAMPTERMMAPPEDLIST = @"Select Distinct Campaign.campaignID, CampaignName from Config.tblCampaignMaster Campaign (NoLock)" +
                                                               " INNER JOIN WM.tblCampaignTermCodeMap CampaignTermCodeMap (NoLock)" +
                                                               " ON Campaign.CampaignID = CampaignTermCodeMap.CampaignID" +
                                                               " INNER JOIN Config.tblProcessMaster ProcessMaster (Nolock)" +
                                                               " ON Campaign.ProcessID = ProcessMaster.ProcessID and ProcessMaster.Disabled=0" +
                                                               " INNER JOIN Config.tblClientMaster ClientMaster (Nolock)" +
                                                               " ON ProcessMaster.ClientID= ClientMaster.ClientID and ClientMaster.Disabled=0" +
                                                               " where Campaign.ProcessID=isnull(@ProcessId,Campaign.ProcessID) and Campaign.Disabled=0 and CampaignName like @CampaignName";


        private const string PARAM_CAMPAIGNID = "@CampaignID";
        private const string PARAM_CAMPAIGNNAME = "@CampaignName";
        private const string PARAM_TERMINATIONCODEID = "@TerminationID";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_ISPRODUCTIVE = "@IsProductive";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_PROCESSID = "@ProcessId";
        private const string PARAM_ISEND = "@IsEnd";
        private const string PARAM_TERMINATIONNAME = "@TerminationName";

        public List<BECampaignInfo> GetCampTermMappedList(int ProcessID, string CampaignName)
        {
            List<BECampaignInfo> lCampaign = new List<BECampaignInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CAMPTERMMAPPEDLIST);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNNAME, DbType.String, "%" + CampaignName + "%");
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, ProcessID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BECampaignInfo objCampaign = new BECampaignInfo
                    {
                        iCampaignID = Convert.ToInt32(rdr["campaignID"]),
                        sCampaignName = rdr["CampaignName"].ToString()

                    };
                    lCampaign.Add(objCampaign);
                    objCampaign = null;
                }

            }
            return lCampaign;
        }
        public BECampTermCodeMapping GetCampTermMappedDetails(int CampId)
        {
            DataTable dt = new DataTable("DtCampTermMapped");
            dt.Columns.Add("TerminationId", System.Type.GetType("System.Int32"));
            dt.Columns.Add("IsProductive", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Disabled", System.Type.GetType("System.Int32"));
            dt.Columns.Add("IsEnd", System.Type.GetType("System.Int32"));

            BECampTermCodeMapping objParam = new BECampTermCodeMapping();


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CAMPTERMMAPPEDDETLS);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, CampId);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["TerminationId"] = rdr["TerminationID"];
                    dr["IsProductive"] = rdr["IsProductive"];
                    dr["Disabled"] = rdr["Disabled"];
                    dr["IsEnd"] = (rdr["IsEnd"] == null || rdr["IsEnd"].ToString() == "") ? 0 : rdr["IsEnd"];
                    dt.Rows.Add(dr);
                }
                objParam.iCampaignID = CampId;
                objParam.dtTerminationTable = dt;

            }
            return objParam;

        }
        public void InsertData(BECampTermCodeMapping oTermination)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_CAMPTERMINATIONMAP);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, oTermination.iCampaignID);
                db.AddInParameter(dbCommand, PARAM_TERMINATIONCODEID, DbType.Int32);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean);
                db.AddInParameter(dbCommand, PARAM_ISPRODUCTIVE, DbType.Boolean);
                db.AddInParameter(dbCommand, PARAM_ISEND, DbType.Boolean);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oTermination.iCreatedBy);


                //Query to check the Deuplicacy in DB
                DbCommand dbCheckDuplicacy = db.GetSqlStringCommand(SQL_CHECK_CAMPTERMINATIONMAP);
                db.AddInParameter(dbCheckDuplicacy, PARAM_TERMINATIONCODEID, DbType.Int32);
                db.AddInParameter(dbCheckDuplicacy, PARAM_CAMPAIGNID, DbType.Int32, oTermination.iCampaignID);



                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            foreach (DataRow dr in oTermination.dtTerminationTable.Rows)
                            {
                                //For Each BreakId, Check if it alreday exits or not
                                db.SetParameterValue(dbCheckDuplicacy, PARAM_TERMINATIONCODEID, dr[0]);
                                int iCheckCounter = (int)db.ExecuteScalar(dbCheckDuplicacy, trans);

                                //If There is no entry in DB
                                if (iCheckCounter == 0)
                                {
                                    db.SetParameterValue(dbCommand, PARAM_TERMINATIONCODEID, dr[0]);
                                    db.SetParameterValue(dbCommand, PARAM_ISPRODUCTIVE, bool.Parse(dr[1].ToString()));
                                    db.SetParameterValue(dbCommand, PARAM_ISEND, bool.Parse(dr[2].ToString()));
                                    db.SetParameterValue(dbCommand, PARAM_DISABLED, bool.Parse(dr[3].ToString()));
                                    db.ExecuteNonQuery(dbCommand, trans);
                                }
                            }

                            trans.Commit();
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
        public void UpdateData(BECampTermCodeMapping oTermination)
        {
            DeleteData(oTermination);
            InsertData(oTermination);
        }
        public void DeleteData(BECampTermCodeMapping oTermination)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE_CAMPTERMINATIONMAP);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, oTermination.iCampaignID);
                db.ExecuteNonQuery(dbCommand);
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
