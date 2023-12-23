using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.Datalayer.ExternalRef
{
    public class DLWATReport : IDisposable
    {
        private BETenant _oTenant = null;
        public DLWATReport(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }
        private const string SP_SAVE_PROCESSFAMILY = @"USP_Insert_PocessFamily";
        private const string SP_SAVE_PROCESSFAMILYMAP = @"Config.USP_SavePocessFamilyMap";
        private const string SQL_SELECT_FIELDNAME = @"USP_CDS_GetAgentFields";
        private const string SP_GET_PROCESSFAMILY = @"Config.USP_GetPocessFamily";
        private const string SQL_SELECT_PROCESSFAMILYLIST = @"Select ProcessFamilyId, ProcessFamilyName From  Config.tblReport_ProcessFamily(NOLOCK) Where ProcessFamilyName like @ProcessFamilyName And ISNULL(Disabled,0)=0";
        private const string SQL_SELECT_INPUTSEARCHDATA = @"Usp_CDS_GetSearchAgentField";
        private const string SQL_INSERTUPDATE_INPUTFIELD = @"Usp_CDS_InsertUpDateUserFieldValue";

        private const string SQL_DISABLE_PROCESSFAMILYMAP = @"Update  Config.tblReport_ProcessFamilyProcessMapping Set Disabled=1, ModifiedBy=@ModifiedBy, ModifiedOn=GetDate()  Where ProcessFamilyId = @ProcessFamilyID And ISNULL(Disabled,0)=0";
        private const string SQL_DISABLE_PROCESSFAMILY = @"Update Config.tblReport_ProcessFamily Set Disabled=1, ModifiedBy=@ModifiedBy, ModifiedOn=GetDate()  Where ProcessFamilyId = @ProcessFamilyID And ISNULL(Disabled,0)=0";


        private const string PARAM_PROCESSID = "@ProcessID";
        private const string PARAM_PROCESSFAMILYID = "@ProcessFamilyID";
        private const string PARAM_PROCESSFAMILYNAME = "@ProcessFamilyName";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";

        private const string PARAM_INPUTDATE = "@Date";
        private const string PARAM_STRINGXML = "@xml";

        public void SaveProcessFamily(List<BEProcessFamilyMap> lstProcessFamily)
        {
            int iProcessFamilyID = 0;
            DbCommand dbCommand = null;

            Database db = DL_Shared.dbFactory(_oTenant);
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open
                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        dbCommand = db.GetStoredProcCommand(SP_SAVE_PROCESSFAMILY);
                        db.AddInParameter(dbCommand, PARAM_PROCESSFAMILYNAME, DbType.String, lstProcessFamily[0].ProcessFamilyName);
                        db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, lstProcessFamily[0].iCreatedBy);

                        iProcessFamilyID = Convert.ToInt32(db.ExecuteScalar(dbCommand));

                        foreach (BEProcessFamilyMap item in lstProcessFamily)
                        {
                            {
                                dbCommand = db.GetStoredProcCommand(SP_SAVE_PROCESSFAMILYMAP);
                                db.AddInParameter(dbCommand, PARAM_PROCESSFAMILYID, DbType.Int32, iProcessFamilyID);
                                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, item.ProcessId);
                                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, item.iCreatedBy);
                                db.ExecuteNonQuery(dbCommand);
                            }
                        }

                        trans.Commit(); //Commit Transaction
                    }
                    /*
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        trans.Rollback();//Transaction RollBack
                        if (ex.Number == 547)
                        {
                            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                        }
                        if (ex.Number == 2627)
                        {
                            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                        }
                        if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_ProcessFamilyAlready))
                        {
                            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_ProcessFamilyAlready);
                        }

                        throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }*/
                    catch (Exception ex)
                    {
                        trans.Rollback();//Transaction RollBack
                        throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public void UpdateProcessFamily(List<BEProcessFamilyMap> lstProcessFamily, int iProcessFamilyID)
        {
            DbCommand dbCommand = null;

            Database db = DL_Shared.dbFactory(_oTenant);
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open
                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        dbCommand = db.GetSqlStringCommand(SQL_DISABLE_PROCESSFAMILYMAP);
                        db.AddInParameter(dbCommand, PARAM_PROCESSFAMILYID, DbType.Int32, iProcessFamilyID);
                        db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, lstProcessFamily[0].iCreatedBy);
                        db.ExecuteNonQuery(dbCommand);
                        foreach (BEProcessFamilyMap item in lstProcessFamily)
                        {
                            {
                                dbCommand = db.GetStoredProcCommand(SP_SAVE_PROCESSFAMILYMAP);
                                db.AddInParameter(dbCommand, PARAM_PROCESSFAMILYID, DbType.Int32, iProcessFamilyID);
                                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, item.ProcessId);
                                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, item.iCreatedBy);
                                db.ExecuteNonQuery(dbCommand);
                            }
                        }

                        trans.Commit(); //Commit Transaction
                    }
                    /*
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        trans.Rollback();//Transaction RollBack
                        if (ex.Number == 547)
                        {
                            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                        }
                        if (ex.Number == 2627)
                        {
                            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                        }
                        throw ex;
                    }*/
                    catch (Exception ex)
                    {
                        trans.Rollback();//Transaction RollBack
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        public void DisableProcessFamily(int iProcessFamilyID, int iUserID)
        {
            DbCommand dbCommand = null;

            Database db = DL_Shared.dbFactory(_oTenant);
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open
                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        dbCommand = db.GetSqlStringCommand(SQL_DISABLE_PROCESSFAMILY);
                        db.AddInParameter(dbCommand, PARAM_PROCESSFAMILYID, DbType.Int32, iProcessFamilyID);
                        db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, iUserID);
                        db.ExecuteNonQuery(dbCommand);

                        dbCommand = db.GetSqlStringCommand(SQL_DISABLE_PROCESSFAMILYMAP);
                        db.AddInParameter(dbCommand, PARAM_PROCESSFAMILYID, DbType.Int32, iProcessFamilyID);
                        db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, iUserID);
                        db.ExecuteNonQuery(dbCommand);
                        trans.Commit(); //Commit Transaction
                    }
                    /*
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        trans.Rollback();//Transaction RollBack
                        if (ex.Number == 547)
                        {
                            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                        }
                        if (ex.Number == 2627)
                        {
                            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                        }
                        throw ex;
                    }*/
                    catch (Exception ex)
                    {
                        trans.Rollback();//Transaction RollBack
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        public List<BEProcessFamilyMap> GetProcessFamilyList(int iProcessFamilyID)
        {

            List<BEProcessFamilyMap> lProcessFamily = new List<BEProcessFamilyMap>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_PROCESSFAMILY);
            db.AddInParameter(dbCommand, PARAM_PROCESSFAMILYID, DbType.Int32, iProcessFamilyID);
            // db.LoadDataSet(dbCommand, dsProcessFamily, "ProcessFamily");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessFamilyMap objProcessFamily = new BEProcessFamilyMap();
                    objProcessFamily.ProcessFamilyId = Convert.ToInt32(rdr["ProcessFamilyID"]);
                    objProcessFamily.ProcessFamilyName = rdr["ProcessFamilyName"].ToString();
                    objProcessFamily.ClientID = Convert.ToInt32(rdr["ClientID"]);
                    objProcessFamily.ProcessName = rdr["ProcessId"].ToString();
                    lProcessFamily.Add(objProcessFamily);
                    objProcessFamily = null;
                }
            }
            return lProcessFamily;
        }
        public List<BEProcessFamilyMap> GetProcessFamilyList(string sProcessFamilyName)
        {
            List<BEProcessFamilyMap> lstProcessFamily = new List<BEProcessFamilyMap>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSFAMILYLIST);
            db.AddInParameter(dbCommand, PARAM_PROCESSFAMILYNAME, DbType.String, "%" + sProcessFamilyName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessFamilyMap objProcessFamily = new BEProcessFamilyMap();
                    objProcessFamily.ProcessFamilyId = Convert.ToInt32(rdr["ProcessFamilyId"]);
                    objProcessFamily.ProcessFamilyName = rdr["ProcessFamilyName"].ToString();
                    lstProcessFamily.Add(objProcessFamily);
                    objProcessFamily = null;
                }
            }
            return lstProcessFamily;
        }
    }
}
