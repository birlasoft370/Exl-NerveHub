using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation
{
    public class DLTerminationCode : IDisposable
    {
        private BETenant _oTenant = null;
        public DLTerminationCode(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SELECT_TERMINATIONCODE_CAMPID = @"  SELECT distinct TBM.TerminationID,TerminationName,isnull(TBM.Description,'') as DESCRIPTION,TBM.DISABLED,
TBM.createdOn FROM WM.tblTerminationCodeMaster TBM (NoLock)  inner join 
  WM.tblCampaignTermCodeMap   TPBM (NoLock) on
  TBM.TerminationID=TPBM.TerminationID    where TBM.Disabled=0  and TPBM.CampaignID=@CampaignID Order By TerminationName";
        private const string SQL_SELECT_TERMINATIONCODE = @"SELECT TerminationID,TerminationName,isnull(Description,'') as DESCRIPTION,DISABLED,createdOn FROM WM.tblTerminationCodeMaster WHERE TerminationName Like @TerminationName and Disabled=0 ORDER BY TerminationName ";
        private const string SQL_SELECT_TERMINATIONCODEAll = @"SELECT TerminationID,TerminationName,isnull(Description,'') as DESCRIPTION,DISABLED,createdOn FROM WM.tblTerminationCodeMaster WHERE TerminationName Like @TerminationName ORDER BY TerminationName ";

        private const string SQL_SELECT_TERMINATIONCODEID = @"SELECT TerminationID,TerminationName,isnull(Description,'') as DESCRIPTION,DISABLED,createdOn FROM WM.tblTerminationCodeMaster WHERE TerminationID = @TerminationID";


        private const string SQL_INSERT_TERMINATIONCODE = @"if exists(select TerminationName from WM.tblTerminationCodeMaster where TerminationName=@TerminationName)
                                                                 Begin
                                                                 select 'Termination Name already exists'
                                                                 End
                                                                 else
                                                                 Begin
                                                                 INSERT INTO WM.tblTerminationCodeMaster (TerminationName,Description,Disabled,CreatedBy)
                                                                  VALUES(@TerminationName,@TerminationDesc,@Disabled,@CreatedBy)
                                                                  select ''  
                                                                  End";
        private const string SQL_UPDATE_TERMINATIONCODE = @"UPDATE WM.tblTerminationCodeMaster SET TerminationName=@TerminationName,Description=@TerminationDesc,Disabled=@Disabled,MODIFIEDBY=@ModifiedBy , MODIFIEDON=GetDate()  WHERE TerminationID=@TerminationID";
        private const string SQL_DELETE_TERMINATIONCODE = @"DELETE FROM WM.tblTerminationCodeMaster WHERE TerminationID=@TerminationID";
        private const string SQL_SELECT_TERMINATION_NAME = @"[WM].[Usp_GetTerminationkNameBySearch]";
        private const string PARAM_TerminationID = "@TerminationID";
        private const string PARAM_TerminationName = "@TerminationName";
        private const string PARAM_TerminationDesc = "@TerminationDesc";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_CAMPID = "@CampaignID";


        public List<BETerminationCodeInfo> GetTermCodeList(int iTermCodeID)
        {
            //throw new System.NotImplementedException();

            List<BETerminationCodeInfo> ITerm = new List<BETerminationCodeInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_TERMINATIONCODEID);
            db.AddInParameter(dbCommand, PARAM_TerminationID, DbType.Int32, iTermCodeID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BETerminationCodeInfo objTerm = new BETerminationCodeInfo(Convert.ToInt32(rdr["TerminationID"]), rdr["TerminationName"].ToString(), rdr["DESCRIPTION"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0, Convert.ToDateTime(rdr["CreatedOn"]));

                    ITerm.Add(objTerm);
                    objTerm = null;

                }
            }
            return ITerm;
        }
        public List<BETerminationCodeInfo> GetTermCodeList(string sTermCodeName, bool bGetActive)
        {
            //throw new System.NotImplementedException();
            List<BETerminationCodeInfo> ITerm = new List<BETerminationCodeInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = null;

            if (bGetActive)
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_TERMINATIONCODE);
            else
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_TERMINATIONCODEAll);

            db.AddInParameter(dbCommand, PARAM_TerminationName, DbType.String, "" + sTermCodeName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BETerminationCodeInfo objTerm = new BETerminationCodeInfo(Convert.ToInt32(rdr["TerminationID"]), rdr["TerminationName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["DESCRIPTION"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0, Convert.ToDateTime(rdr["CreatedOn"]));
                    ITerm.Add(objTerm);
                    objTerm = null;
                }
            }
            return ITerm;
        }
        public string InsertData(BETerminationCodeInfo oTermincationCode)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_TERMINATIONCODE);
                db.AddInParameter(dbCommand, PARAM_TerminationID, DbType.Int32, oTermincationCode.iTerminationCodeID);
                db.AddInParameter(dbCommand, PARAM_TerminationName, DbType.String, oTermincationCode.sTermCodeName);
                db.AddInParameter(dbCommand, PARAM_TerminationDesc, DbType.String, oTermincationCode.sTermCodeDesc);

                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oTermincationCode.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oTermincationCode.iCreatedBy);
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
        public void UpdateData(BETerminationCodeInfo oTermincationCode)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_TERMINATIONCODE);
                db.AddInParameter(dbCommand, PARAM_TerminationID, DbType.Int32, oTermincationCode.iTerminationCodeID);
                db.AddInParameter(dbCommand, PARAM_TerminationName, DbType.String, oTermincationCode.sTermCodeName);
                db.AddInParameter(dbCommand, PARAM_TerminationDesc, DbType.String, oTermincationCode.sTermCodeDesc);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oTermincationCode.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oTermincationCode.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oTermincationCode.iModifiedBy);

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

        public List<BETerminationCodeInfo> GetTermCodeListByCamp(int iCampID)
        {
            //throw new System.NotImplementedException();
            List<BETerminationCodeInfo> ITerm = new List<BETerminationCodeInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = null;
            dbCommand = db.GetSqlStringCommand(SQL_SELECT_TERMINATIONCODE_CAMPID);
            db.AddInParameter(dbCommand, PARAM_CAMPID, DbType.Int32, iCampID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BETerminationCodeInfo objTerm = new BETerminationCodeInfo(Convert.ToInt32(rdr["TerminationID"]), rdr["TerminationName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["DESCRIPTION"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0, Convert.ToDateTime(rdr["CreatedOn"]));
                    ITerm.Add(objTerm);
                    objTerm = null;
                }
            }
            return ITerm;
        }

        public List<BETerminationCodeInfo> GetTermCodeListByCamp(string sTermName, int iCampID)
        {
            //throw new System.NotImplementedException();

            List<BETerminationCodeInfo> ITerm = new List<BETerminationCodeInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SELECT_TERMINATION_NAME);
            db.AddInParameter(dbCommand, PARAM_TerminationName, DbType.String, sTermName);
            db.AddInParameter(dbCommand, PARAM_CAMPID, DbType.Int32, iCampID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BETerminationCodeInfo objTerm = new BETerminationCodeInfo(Convert.ToInt32(rdr["TerminationID"]), rdr["TerminationName"].ToString(), rdr["DESCRIPTION"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0, Convert.ToDateTime(rdr["CreatedOn"]));

                    ITerm.Add(objTerm);
                    objTerm = null;

                }
            }
            return ITerm;
        }
    }
}
