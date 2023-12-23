using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.Utility;
using System.Collections;
using System.Data.SqlClient;

namespace BPA.EmailManagement.DataLayer.ExternalRef.WorkAllocation
{
    public class DLWorkUpload : IDisposable
    {
        private BETenant _oTenant = null;
        public DLWorkUpload(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SENDMAILFORBULKUPLOAD = @"[WM].[Usp_SendMailforBulkUpload]";
        private const string SP_GETBATCHID = @"WM.Usp_GetBatchID";
        private const string SP_MailConfig_UserOffSet = @"[EMS].[USP_CDS_MailConfig_UserOffSet]";
        private const string SQL_SP_GETCOLUMNNAME = @"WM.Usp_GetColumnName";
        private const string SQL_SP_CHECKNEEDETICKET = @"[EMS].[Usp_CheckNeedeTicket]";
        private const string SQL_BULKUPLOAD = @"[WM].[Usp_BulkUpload]";

        private const string PARAM_TABLE = "@TableName";
        private const string PARAM_STRERR = "@error";
        private const string PARAM_TOTALREC = "@Total";
        private const string PARAM_STARTDATE = "@StartDate";
        private const string PARAM_SUBJECT = "@Subject";

        private const string PARAM_CAMPAIGNID = "@CampaignId";
        private const string PARAM_BATCHCODE = "@BatchCode";
        private const string PARAM_RETURNVALUE = "@ReturnValue";
        private const string PARAM_TABLENAME = "@sWorkTable";

        public void SendErrorMail(string sWorkTable, string strErr, int totalRec, string StartDate, string Subject)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetStoredProcCommand(SQL_SENDMAILFORBULKUPLOAD);
            db.AddInParameter(dbCommand, PARAM_TABLE, DbType.String, sWorkTable);
            db.AddInParameter(dbCommand, PARAM_STRERR, DbType.String, RemoveSpecialCharacters(strErr));
            db.AddInParameter(dbCommand, PARAM_TOTALREC, DbType.Int64, totalRec);
            db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.String, StartDate);
            db.AddInParameter(dbCommand, PARAM_SUBJECT, DbType.String, Subject);
            db.ExecuteNonQuery(dbCommand);
        }

        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public int GetBatchIdValue(int CampaignId, string BatchCode)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            int val = 0;
            DbCommand dbInsertCommand = db.GetStoredProcCommand(SP_GETBATCHID);
            try
            {
                db.AddInParameter(dbInsertCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignId);
                db.AddInParameter(dbInsertCommand, PARAM_BATCHCODE, DbType.String, BatchCode);
                db.AddOutParameter(dbInsertCommand, PARAM_RETURNVALUE, DbType.Int32, 0);
                db.ExecuteNonQuery(dbInsertCommand);
                val = Convert.ToInt32(db.GetParameterValue(dbInsertCommand, PARAM_RETURNVALUE));
                return val;

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

        public DataTable GetUserOffSet()
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetStoredProcCommand(SP_MailConfig_UserOffSet);
            return db.ExecuteDataSet(dbCommand).Tables[0];
        }

        private void RemoveEmptyRows(DataTable dtTable)
        {
            DataRow dr = null;
            bool result;
            for (int j = dtTable.Rows.Count - 1; j >= 0; j--)
            {
                result = false;
                dr = dtTable.Rows[j];
                for (int i = 0; i < dtTable.Columns.Count; i++)
                {
                    result = dr.IsNull(dtTable.Columns[i]);
                    if (!result) break;
                }
                if (result)
                    dtTable.Rows.Remove(dr);
                else
                    break;
            }
        }

        public IList<string> CheckColumn(string TableName)
        {
            IList<string> lColumnName = new List<string>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetStoredProcCommand(SQL_SP_GETCOLUMNNAME);
            db.AddInParameter(dbCommand, PARAM_TABLE, DbType.String, TableName);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    lColumnName.Add(rdr["ColumnName"].ToString());

                }
            }
            return lColumnName;
        }

        private bool CheckNeedTicket(string TableName)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetStoredProcCommand(SQL_SP_CHECKNEEDETICKET);
            db.AddInParameter(dbCommand, PARAM_TABLE, DbType.String, TableName);
            int Count = Convert.ToInt32(db.ExecuteScalar(dbCommand));
            if (Count == 1)
                return true;
            else
                return false;
        }

        public string BulkUpload(DataTable dt, string sWorkTable)
        {
            string bcpmsg = "";
            string strErr = "";
            int i = 1;
            Hashtable objcol = new Hashtable();
            RemoveEmptyRows(dt);
            Database db = DL_Shared.dbFactory(_oTenant);

            //   string AppConnection = ConfigurationManager.ConnectionStrings["DBCDSConnectionString"].ToString();

            string strMsg = "";
            using (SqlConnection cn = new SqlConnection(EncryptDecrypt.Decrypt(_oTenant.DatabaseConnectionString)))
            {
                cn.Open();

                using (SqlTransaction trans = cn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        using (SqlBulkCopy bcp = new SqlBulkCopy(cn, SqlBulkCopyOptions.TableLock, trans))
                        {
                            bcp.BatchSize = 500;
                            bcp.DestinationTableName = "Trans." + sWorkTable;
                            IList<string> dbColumnName = CheckColumn(sWorkTable);
                            foreach (DataColumn dc in dt.Columns)
                            {
                                objcol.Add(i, dc.Caption);
                                i = i + 1;
                                if (dbColumnName.Contains(dc.ColumnName))
                                {
                                    bcp.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                                }
                                else
                                    strMsg += dc.ColumnName + ", ";


                            }
                            if (strMsg == "")
                            {
                                bcp.WriteToServer(dt);
                                // Add for need ticket
                                if (CheckNeedTicket(sWorkTable))
                                {
                                    if (dt != null)
                                    {
                                        if (dt.Rows.Count > 0)
                                        {
                                            SqlCommand cmdUpdateTopId = new SqlCommand("[EMS].[UPdateWorktranstableForNeedticket]", cn, trans);
                                            cmdUpdateTopId.CommandType = CommandType.StoredProcedure;
                                            cmdUpdateTopId.Parameters.Add(new SqlParameter("@TableName", sWorkTable));
                                            cmdUpdateTopId.ExecuteNonQuery();
                                        }
                                    }
                                }
                                else
                                {
                                    using (DbCommand dbCommand = db.GetStoredProcCommand(SQL_BULKUPLOAD))
                                    {
                                        db.AddInParameter(dbCommand, PARAM_TABLENAME, DbType.String, sWorkTable);
                                        db.ExecuteNonQuery(dbCommand, trans);
                                    }
                                }
                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }

                    }

                    catch (Exception ex)
                    {

                        if (ex != null)
                        {
                            if (ex.Message.Contains("Received an invalid column length"))
                            {
                                string[] str = ex.Message.Split(' ', '.');
                                int val = Convert.ToInt32(str[11]) - 3;
                                strErr = "Inavalid Column Length for " + "''" + objcol[val] + "''";
                                strMsg = "Inavalid Column Length for " + "''" + objcol[val] + "''";
                            }
                            else
                                strErr = ex.Message;
                            strMsg = ex.Message;

                        }

                        trans.Rollback();//Transaction RollBack

                        // throw new NotImplementedException(strErr);
                    }

                }
            }
            return strMsg;
        }
    }
}
