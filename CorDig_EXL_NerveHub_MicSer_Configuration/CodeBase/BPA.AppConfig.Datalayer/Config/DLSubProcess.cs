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

namespace BPA.AppConfig.Datalayer.Config
{
    public class DLSubProcess : IDisposable
    {
        private BETenant _oTenant = null;

        private const string SQL_SELECT_SUBPROCESS = @"SELECT * FROM Config.tblSubProcess(NOLOCK) where ProcessID=isnull(@ProcessId,ProcessId) and Disabled=0 and SubProcessName like @SubProcessName order by SubProcessName ";
        private const string SQL_SELECT_SUBPROCESSALL = @"SELECT * FROM Config.tblSubProcess(NOLOCK) where ProcessID=isnull(@ProcessId,ProcessId) and SubProcessName like @SubProcessName order by SubProcessName ";
        private const string SQL_SELECT_SUBPROCESSID = @"SELECT PM.ClientID,SP.* FROM Config.tblSubProcess(NOLOCK) SP
                                                            INNER JOIN Config.tblProcessMaster(NOLOCK) PM
                                                            ON SP.ProcessID =PM.ProcessID where SubProcessID=@SubProcessID";
        private const string SQL_SELECT_SUBPROCESSPROCESSWISE = @"SELECT PM.ClientID,SP.* FROM Config.tblSubProcess(NOLOCK) SP
                                                            INNER JOIN Config.tblProcessMaster(NOLOCK) PM
                                                            ON SP.ProcessID =PM.ProcessID where PM.ProcessID=@ProcessID and SP.Disabled=0";

        private const string SQL_INSERT_SUBPROCESS = @"INSERT INTO Config.tblSubProcess(ProcessID,SubProcessName,Description,SubProcessStartDate,SubProcessEndDate,GoLiveDate,
                                                            StabilizationStartDate,StabilizationEndDate,ProductionStartDate,ProductionEndDate,Disabled,CreatedBy)
                                                            VALUES (@ProcessID,@SubProcessName,@Description,@SubProcessStartDate,@SubProcessEndDate,@GoLiveDate,
                                                            @StabilizationStartDate,@StabilizationEndDate,@ProductionStartDate,@ProductionEndDate,@Disabled,@CreatedBy)
                                                            ";
        private const string SQL_UPDATE_SUBPROCESS = @"UPDATE Config.tblSubProcess SET ProcessID=@ProcessID,SubProcessName=@SubProcessName,Description=@Description,
                                                            SubProcessStartDate=@SubProcessStartDate,SubProcessEndDate=@SubProcessEndDate,GoLiveDate=@GoLiveDate,
                                                            StabilizationStartDate=@StabilizationStartDate,StabilizationEndDate=@StabilizationEndDate,ProductionStartDate=@ProductionStartDate,
                                                            ProductionEndDate=@ProductionEndDate,Disabled=@Disabled,ModifiedBy=@ModifiedBy,ModifiedOn=GetDate()  WHERE SubProcessID=@SubProcessID";
        private const string SQL_DELETE_SUBPROCESS = @"DELETE FROM Config.tblSubProcess  WHERE SubProcessID=@SubProcessID";

        //private const string SQL_SP_SUBPROCESSLIST = @"Usp_CDS_GetClientList"; //Dead Code: Unused Field

        private const string SQL_SELECT_MULTIPLE_SUBPROCESSPROCESSWISE = @"SELECT PM.ClientID,sp.SubProcessID,sp.ProcessID,PM.ProcessName +' - '+ sp.SubProcessName as SubProcessName,
sp.Description,sp.SubProcessStartDate,sp.SubProcessEndDate,sp.GoLiveDate,sp.StabilizationStartDate,
sp.StabilizationEndDate,sp.ProductionStartDate,sp.ProductionEndDate,sp.Disabled,
sp.CreatedBy,sp.CreatedOn,sp.ModifiedBy,sp.ModifiedOn,sp.PDSubProcessID FROM Config.tblSubProcess(NOLOCK) SP
                                                            INNER JOIN Config.tblProcessMaster(NOLOCK) PM
                                                            ON SP.ProcessID =PM.ProcessID where PM.ProcessID IN (SELECT Items FROM dbo.fn_Split(@ProcessID,',')) and SP.Disabled=0";


        private const string PARAM_SUBPROCESSID = "@SubProcessID";
        private const string PARAM_PROCESSID = "@ProcessID";
        private const string PARAM_SUBPROCESSNAME = "@SubProcessName";
        private const string PARAM_DESCRIPTION = "@Description";
        private const string PARAM_SUBPROCESSSTARTDATE = "@SubProcessStartDate";
        private const string PARAM_SUBPROCESSENDDATE = "@SubProcessEndDate";
        private const string PARAM_GOLIVEDATE = "@GoLiveDate";
        private const string PARAM_STABILIZATIONSTARTDATE = "@StabilizationStartDate";
        private const string PARAM_STABILIZATIONENDDATE = "@StabilizationEndDate";
        private const string PARAM_PRODUCTIONSTARTDATE = "@ProductionStartDate";
        private const string PARAM_PRODUCTIONENDDATE = "@ProductionEndDate";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";



        public DLSubProcess(BETenant oTenant)
        { _oTenant = oTenant; }


        public void Dispose()
        { _oTenant = null; }
        public List<BESubProcess> GetSubProcessList(bool bActiveSubProcess)
        {
            return GetSubProcessList("", bActiveSubProcess);
        }

        public List<BESubProcess> GetSubProcessList(string SubProcessName, bool bActiveSubProcess)
        {
            List<BESubProcess> lSubProcess = new List<BESubProcess>();


            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbCommand;
            if (bActiveSubProcess)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_SUBPROCESS);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_SUBPROCESSALL);
            }
            db.AddInParameter(dbCommand, PARAM_SUBPROCESSNAME, DbType.String, "" + SubProcessName + "%");
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, DBNull.Value);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BESubProcess objSubProcess = new BESubProcess
                    {
                        iSubProcessID = int.Parse(rdr["SubProcessID"].ToString()),
                        sSubProcessName = rdr["SubProcessName"].ToString(),
                        sDescription = rdr["Description"].ToString(),
                        iProcessID = int.Parse(rdr["ProcessID"].ToString()),
                        dSubProcessStartDate = rdr["SubProcessStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["SubProcessStartDate"].ToString()),
                        dSubProcessEndDate = rdr["SubProcessEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["SubProcessEndDate"].ToString()),
                        dProductionStartDate = rdr["ProductionStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionStartDate"].ToString()),
                        dProductionEndDate = rdr["ProductionEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionEndDate"].ToString()),
                        dStabilizationStartDate = rdr["StabilizationStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationStartDate"].ToString()),
                        dStabilizationEndDate = rdr["StabilizationEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationEndDate"].ToString()),
                        dGoLiveDate = rdr["GoLiveDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["GoLiveDate"].ToString()),
                        bDisabled = bool.Parse(rdr["Disabled"].ToString())
                    };
                    lSubProcess.Add(objSubProcess);
                    objSubProcess = null;
                }
            }

            return lSubProcess;
        }

        public List<BESubProcess> GetSubProcessList(int iProcessID, string SubProcessName, bool bActiveSubProcess)
        {
            List<BESubProcess> lSubProcess = new List<BESubProcess>();

            Database db = DL_Shared.dbFactory(_oTenant); //DatabaseFactory.CreateDatabase(DL_Shared.Connection);
            DbCommand dbCommand;
            if (bActiveSubProcess)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_SUBPROCESS);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_SUBPROCESSALL);
            }
            db.AddInParameter(dbCommand, PARAM_SUBPROCESSNAME, DbType.String, "" + SubProcessName + "%");
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BESubProcess objSubProcess = new BESubProcess
                    {
                        iSubProcessID = int.Parse(rdr["SubProcessID"].ToString()),
                        sSubProcessName = rdr["SubProcessName"].ToString(),
                        sDescription = rdr["Description"].ToString(),
                        iProcessID = int.Parse(rdr["ProcessID"].ToString()),
                        dSubProcessStartDate = rdr["SubProcessStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["SubProcessStartDate"].ToString()),
                        dSubProcessEndDate = rdr["SubProcessEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["SubProcessEndDate"].ToString()),
                        dProductionStartDate = rdr["ProductionStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionStartDate"].ToString()),
                        dProductionEndDate = rdr["ProductionEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionEndDate"].ToString()),
                        dStabilizationStartDate = rdr["StabilizationStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationStartDate"].ToString()),
                        dStabilizationEndDate = rdr["StabilizationEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationEndDate"].ToString()),
                        dGoLiveDate = rdr["GoLiveDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["GoLiveDate"].ToString()),
                        bDisabled = bool.Parse(rdr["Disabled"].ToString())
                    };
                    lSubProcess.Add(objSubProcess);
                    objSubProcess = null;
                }
            }

            return lSubProcess;
        }

        public BESubProcess GetSubProcessList(int iSubProcess)
        {
            BESubProcess objSubProcess = null;

            Database db = DL_Shared.dbFactory(_oTenant); //DatabaseFactory.CreateDatabase(DL_Shared.Connection);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_SUBPROCESSID);
            db.AddInParameter(dbCommand, PARAM_SUBPROCESSID, DbType.Int32, iSubProcess);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    objSubProcess = new BESubProcess
                    {
                        iClientID = int.Parse(rdr["ClientID"].ToString()),
                        iSubProcessID = int.Parse(rdr["SubProcessID"].ToString()),
                        sSubProcessName = rdr["SubProcessName"].ToString(),
                        sDescription = rdr["Description"].ToString(),
                        iProcessID = int.Parse(rdr["ProcessID"].ToString()),
                        dSubProcessStartDate = rdr["SubProcessStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["SubProcessStartDate"].ToString()),
                        dSubProcessEndDate = rdr["SubProcessEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["SubProcessEndDate"].ToString()),
                        dProductionStartDate = rdr["ProductionStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionStartDate"].ToString()),
                        dProductionEndDate = rdr["ProductionEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionEndDate"].ToString()),
                        dStabilizationStartDate = rdr["StabilizationStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationStartDate"].ToString()),
                        dStabilizationEndDate = rdr["StabilizationEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationEndDate"].ToString()),
                        dGoLiveDate = rdr["GoLiveDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["GoLiveDate"].ToString()),
                        bDisabled = bool.Parse(rdr["Disabled"].ToString()),
                        iPDSubProcessID = Convert.ToInt64(rdr["PDSubProcessID"]),
                        //sPDSubProcessName = Convert.ToString(rdr["PDSubProcessName"])
                    };

                }
            }

            return objSubProcess;
        }

        public List<BESubProcess> GetSubProcessListProcessWise(int iProcessId)
        {
            List<BESubProcess> lSubProcess = new List<BESubProcess>();

            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_SUBPROCESSPROCESSWISE);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessId);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BESubProcess objSubProcess = new BESubProcess()
                    {
                        iClientID = int.Parse(rdr["ClientID"].ToString()),
                        iSubProcessID = int.Parse(rdr["SubProcessID"].ToString()),
                        sSubProcessName = rdr["SubProcessName"].ToString(),
                        sDescription = rdr["Description"].ToString(),
                        iProcessID = int.Parse(rdr["ProcessID"].ToString()),
                        dSubProcessStartDate = rdr["SubProcessStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["SubProcessStartDate"].ToString()),
                        dSubProcessEndDate = rdr["SubProcessEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["SubProcessEndDate"].ToString()),
                        dProductionStartDate = rdr["ProductionStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionStartDate"].ToString()),
                        dProductionEndDate = rdr["ProductionEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionEndDate"].ToString()),
                        dStabilizationStartDate = rdr["StabilizationStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationStartDate"].ToString()),
                        dStabilizationEndDate = rdr["StabilizationEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationEndDate"].ToString()),
                        dGoLiveDate = rdr["GoLiveDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["GoLiveDate"].ToString()),
                        bDisabled = bool.Parse(rdr["Disabled"].ToString())
                    };
                    lSubProcess.Add(objSubProcess);
                    objSubProcess = null;
                }
            }

            return lSubProcess;
        }
        public void InsertData(BESubProcess oSubprocess)
        {
            try
            {




                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_SUBPROCESS);
                db.AddInParameter(dbCommand, PARAM_SUBPROCESSID, DbType.Int32, oSubprocess.iSubProcessID);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oSubprocess.iProcessID);
                db.AddInParameter(dbCommand, PARAM_SUBPROCESSNAME, DbType.String, oSubprocess.sSubProcessName);
                db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oSubprocess.sDescription);

                db.AddInParameter(dbCommand, PARAM_SUBPROCESSSTARTDATE, DbType.DateTime);
                if (oSubprocess.dSubProcessStartDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_SUBPROCESSSTARTDATE, oSubprocess.dSubProcessStartDate);
                db.AddInParameter(dbCommand, PARAM_SUBPROCESSENDDATE, DbType.DateTime);
                if (oSubprocess.dSubProcessEndDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_SUBPROCESSENDDATE, oSubprocess.dSubProcessEndDate);
                db.AddInParameter(dbCommand, PARAM_STABILIZATIONSTARTDATE, DbType.DateTime);
                if (oSubprocess.dStabilizationStartDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_STABILIZATIONSTARTDATE, oSubprocess.dStabilizationStartDate);
                db.AddInParameter(dbCommand, PARAM_STABILIZATIONENDDATE, DbType.DateTime);
                if (oSubprocess.dStabilizationEndDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_STABILIZATIONENDDATE, oSubprocess.dStabilizationEndDate);
                db.AddInParameter(dbCommand, PARAM_PRODUCTIONSTARTDATE, DbType.DateTime);
                if (oSubprocess.dProductionStartDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_PRODUCTIONSTARTDATE, oSubprocess.dProductionStartDate);
                db.AddInParameter(dbCommand, PARAM_PRODUCTIONENDDATE, DbType.DateTime);
                if (oSubprocess.dProductionEndDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_PRODUCTIONENDDATE, oSubprocess.dProductionEndDate);
                db.AddInParameter(dbCommand, PARAM_GOLIVEDATE, DbType.DateTime);
                if (oSubprocess.dGoLiveDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_GOLIVEDATE, oSubprocess.dGoLiveDate);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oSubprocess.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oSubprocess.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oSubprocess.iCreatedBy);
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
        public void UpdateData(BESubProcess oSubprocess)
        {

            try
            {
                // Database db = DL_Shared.dbFactory(_oTenant);


                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_SUBPROCESS);
                db.AddInParameter(dbCommand, PARAM_SUBPROCESSID, DbType.Int32, oSubprocess.iSubProcessID);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oSubprocess.iProcessID);
                db.AddInParameter(dbCommand, PARAM_SUBPROCESSNAME, DbType.String, oSubprocess.sSubProcessName);
                db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oSubprocess.sDescription);

                db.AddInParameter(dbCommand, PARAM_SUBPROCESSSTARTDATE, DbType.DateTime);
                if (oSubprocess.dSubProcessStartDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_SUBPROCESSSTARTDATE, oSubprocess.dSubProcessStartDate);
                db.AddInParameter(dbCommand, PARAM_SUBPROCESSENDDATE, DbType.DateTime);
                if (oSubprocess.dSubProcessEndDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_SUBPROCESSENDDATE, oSubprocess.dSubProcessEndDate);
                db.AddInParameter(dbCommand, PARAM_STABILIZATIONSTARTDATE, DbType.DateTime);
                if (oSubprocess.dStabilizationStartDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_STABILIZATIONSTARTDATE, oSubprocess.dStabilizationStartDate);
                db.AddInParameter(dbCommand, PARAM_STABILIZATIONENDDATE, DbType.DateTime);
                if (oSubprocess.dStabilizationEndDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_STABILIZATIONENDDATE, oSubprocess.dStabilizationEndDate);
                db.AddInParameter(dbCommand, PARAM_PRODUCTIONSTARTDATE, DbType.DateTime);
                if (oSubprocess.dProductionStartDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_PRODUCTIONSTARTDATE, oSubprocess.dProductionStartDate);
                db.AddInParameter(dbCommand, PARAM_PRODUCTIONENDDATE, DbType.DateTime);
                if (oSubprocess.dProductionEndDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_PRODUCTIONENDDATE, oSubprocess.dProductionEndDate);
                db.AddInParameter(dbCommand, PARAM_GOLIVEDATE, DbType.DateTime);
                if (oSubprocess.dGoLiveDate.Year != 0001) db.SetParameterValue(dbCommand, PARAM_GOLIVEDATE, oSubprocess.dGoLiveDate);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oSubprocess.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oSubprocess.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oSubprocess.iCreatedBy);
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

        public List<BESubProcess> GetMultipleSubProcessListProcessWise(string iProcessId)
        {
            List<BESubProcess> lSubProcess = new List<BESubProcess>();

            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_MULTIPLE_SUBPROCESSPROCESSWISE);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.String, iProcessId);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BESubProcess objSubProcess = new BESubProcess()
                    {
                        iClientID = int.Parse(rdr["ClientID"].ToString()),
                        iSubProcessID = int.Parse(rdr["SubProcessID"].ToString()),
                        sSubProcessName = rdr["SubProcessName"].ToString(),
                        sDescription = rdr["Description"].ToString(),
                        iProcessID = int.Parse(rdr["ProcessID"].ToString()),
                        dSubProcessStartDate = rdr["SubProcessStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["SubProcessStartDate"].ToString()),
                        dSubProcessEndDate = rdr["SubProcessEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["SubProcessEndDate"].ToString()),
                        dProductionStartDate = rdr["ProductionStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionStartDate"].ToString()),
                        dProductionEndDate = rdr["ProductionEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionEndDate"].ToString()),
                        dStabilizationStartDate = rdr["StabilizationStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationStartDate"].ToString()),
                        dStabilizationEndDate = rdr["StabilizationEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationEndDate"].ToString()),
                        dGoLiveDate = rdr["GoLiveDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["GoLiveDate"].ToString()),
                        bDisabled = bool.Parse(rdr["Disabled"].ToString())
                    };
                    lSubProcess.Add(objSubProcess);
                    objSubProcess = null;
                }
            }

            return lSubProcess;
        }

    }
}
