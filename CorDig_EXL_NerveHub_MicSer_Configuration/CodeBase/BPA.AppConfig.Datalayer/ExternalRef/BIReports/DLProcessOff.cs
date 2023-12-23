using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.BIReports;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.Datalayer.ExternalRef.BIReports
{
    public class DLProcessOff : IDisposable
    {
        private BETenant _oTenant = null;
        public DLProcessOff(BETenant oTenant)
        {
            _oTenant = oTenant;
        }
        public void Dispose()
        {
            _oTenant = null;
        }

        private const string SQL_SELECT_PROCESSOFF = @"select distinct A.ProcessId, B.ProcessName+' ( '+convert(varchar(3),StartDate,100) +' - '+convert(varchar(10),YEAR(EndDate))+' )' as ProcessOff  from Config.tblProcessOff (NoLock) A
                                                        inner join Config.tblProcessMaster (nolock) B on A.ProcessID=B.ProcessID
                                                        where A.Disabled=0 And A.ProcessId =@ProcessId and Year(A.StartDate)=isnull(@Year,Year(A.StartDate)) and Month(A.StartDate)=isnull(@Month,Month(A.StartDate)) 
                                                        ";
        private const string SQL_SELECT_PROCESSOFFALL = @"select distinct A.ProcessId, B.ProcessName+' ( '+convert(varchar(3),StartDate,100) +' - '+convert(varchar(10),YEAR(EndDate))+' )' as ProcessOff  from Config.tblProcessOff (NoLock) A
                                                        inner join Config.tblProcessMaster (nolock) B on A.ProcessID=B.ProcessID
                                                        where A.ProcessId =@ProcessId and Year(A.StartDate)=isnull(@Year,Year(A.StartDate)) and Month(A.StartDate)=isnull(@Month,Month(A.StartDate)) 
                                                        ";

        private const string SQL_SELECT_PROCESSOFFID = @"select distinct ProcessOff.ProcessId, Convert(varchar,MONTH(StartDate)) Month,
                                                        Convert(varchar,Year(StartDate)) Year, ProcessOff.Description,ProcessOff.Disabled,ProcessOff.CreatedBy, ClientId from Config.tblProcessOff (NoLock) ProcessOff inner join Config.tblProcessMaster (nolock) Process on ProcessOff.ProcessId=Process.ProcessId 
                                                        where ProcessOff.Processid = @Processid and ProcessOff.Disabled =0 and  MONTH(StartDate)=@Month and Year(StartDate)=@Year";
        private const string SQL_SELECT_FIRSTLASTDAYOFCALENDER = @"select convert(varchar,isnull(Min(StartDate),'1/1/1900'))+'|'+ convert(varchar,isnull(max(EndDate),'1/1/1900')) +'|'+convert(varchar,Datediff(dd,isnull(Min(StartDate),'1/1/1900'),isnull(Max(EndDate),'1/1/1900')))  from config.tblCalenderData (nolock) where CalenderID =(select CalendarId from Config.tblProcessMaster where processid=@processid) and month=@month and year=@year";

        private const string SQL_SELECT_PROCESSOFFIDDAYMONTHWISE = @"select distinct ProcessOff.ProcessId, Convert(varchar,MONTH(StartDate)) Month,Convert(varchar,DAY(StartDate)) Day,
                                                        Convert(varchar,Year(StartDate)) Year, ProcessOff.Description,ProcessOff.Disabled,ProcessOff.CreatedBy, ClientId from Config.tblProcessOff (NoLock) ProcessOff inner join Config.tblProcessMaster (nolock) Process on ProcessOff.ProcessId=Process.ProcessId 
                                                        where ProcessOff.Processid = @Processid and  MONTH(StartDate)=@Month and Year(StartDate)=@Year";



        private const string SQL_INSERT_PROCESSOFF = @"declare @error varchar(200)
                                                        if not exists(select calenderdataid from Config.tblCalenderData (nolock) cd 
                                                                        inner join Config.tblCalenderMaster (nolock) cm on cd.calenderid=cm.calenderid 
                                                                        inner join Config.tblProcessMaster (nolock) pm on cm.calenderid=pm.CalendarID 
                                                                        where pm.processid=@processid and (@StartDate>=StartDate and @StartDate<=EndDate))
                                                        Begin                                                             
                                                                set @error= 'Calendar information does not exists for Date ( '+convert(varchar,@startDate,101)+' )'          
                                                                raiserror(@error,15,1)
                                                        End
                                                        Else
                                                        Begin    
                                                            if not exists(select ProcessOffId from Config.tblProcessOff where ProcessId=@ProcessId and startdate=@startDate and disabled=0)
                                                            begin
                                                                insert into Config.tblProcessOff(ProcessId,StartDate, EndDate, Description,Disabled,CreatedBy) values(@ProcessId,@StartDate, @EndDate, @Description,@Disabled,@CreatedBy)
                                                            end
                                                            else
                                                            begin
                                                                set @error= 'Process Off already exists for Date ( '+convert(varchar,@startDate,101)+' )'          
                                                                raiserror(@error,15,1)
                                                            end
                                                        End";
        private const string SQL_UPDATE_PROCESSOFF = @"
                                                        declare @error varchar(200)
                                                        if not exists(select calenderdataid from Config.tblCalenderData (nolock) cd 
                                                                        inner join Config.tblCalenderMaster (nolock) cm on cd.calenderid=cm.calenderid 
                                                                        inner join Config.tblProcessMaster (nolock) pm on cm.calenderid=pm.CalendarID 
                                                                        where pm.processid=@processid and month=@month and year=@year and (@StartDate>=StartDate and @StartDate<=EndDate))
                                                        Begin                                                             
                                                                set @error= 'Calendar information does not exists for Date ( '+convert(varchar,@startDate,101)+' )'          
                                                                raiserror(@error,15,1)
                                                        End
                                                        Else
                                                        Begin   
                                                            if not exists(select ProcessOffId from Config.tblProcessOff 
                                                                        where ProcessId=@ProcessId and  Month(StartDate)=@Month and Year(StartDate)=@Year and Day(StartDate)=@Day)
                                                            begin                                                          
                                                                insert into Config.tblProcessOff(ProcessId,StartDate, EndDate, Description,Disabled,CreatedBy) values(@ProcessId,@StartDate, @EndDate, @Description,@Disabled,@ModifiedBy)
                                                            end
                                                            else if exists(select ProcessOffId from Config.tblProcessOff where ProcessId=@ProcessId and  Month(StartDate)=@Month and Year(StartDate)=@Year and Day(StartDate)=@Day)
                                                            begin
                                                                Update Config.tblProcessOff set Disabled=0, ModifiedBy=@ModifiedBy, ModifiedOn=getdate(),Description=@Description where Processid = @Processid  and  Month(StartDate)=@Month and Year(StartDate)=@Year and Month(EndDate)=@Month and Year(EndDate)=@Year and Day(StartDate)=@Day and Day(EndDate)=@Day
                                                            end
                                                            else
                                                             begin
                                                                set @error= 'Process Off already exists for Date ( '+convert(varchar,@startDate,101)+' )'          
                                                                raiserror(@error,15,1)
                                                            end
                                                        end";

        private const string SQL_DELETE_PROCESSOFF = @"Delete from Config.tblProcessOff where ProcessId=@ProcessId 
                                                    and Month(StartDate)=@Month and Year(StartDate)=@Year and Month(EndDate)=@Month and Year(EndDate)=@Year";

        private const string SQL_DISABLED_PROCESSOFF = @"update Config.tblProcessOff set Disabled=1, ModifiedOn=getdate(), ModifiedBy=@ModifiedBy where ProcessId=@ProcessId 
                                                    and Month(StartDate)=@Month and Year(StartDate)=@Year and Month(EndDate)=@Month and Year(EndDate)=@Year";

        private const string PARAM_PROCESSID = "@ProcessId";
        private const string PARAM_STARTDATE = "@StartDate";
        private const string PARAM_ENDDATE = "@EndDate";
        private const string PARAM_DESCRIPTION = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_PROCESS = "@Process";
        private const string PARAM_MONTH = "@Month";
        private const string PARAM_YEAR = "@Year";
        private const string PARAM_DAY = "@Day";

        public IList<BEProcessOff> GetProcessOffList(int iProcessId, int iYear, int iMonth, bool bGetActive)
        {
            IList<BEProcessOff> lProcessOff = new List<BEProcessOff>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = null;
            if (bGetActive)
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSOFF);
            else
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSOFFALL);

            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessId);
            if (iYear != 0)
                db.AddInParameter(dbCommand, PARAM_YEAR, DbType.Int32, iYear);
            else
                db.AddInParameter(dbCommand, PARAM_YEAR, DbType.Int32, System.DBNull.Value);
            if (iMonth != 0)
                db.AddInParameter(dbCommand, PARAM_MONTH, DbType.Int32, iMonth);
            else
                db.AddInParameter(dbCommand, PARAM_MONTH, DbType.Int32, System.DBNull.Value);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    //lProcessOff.Add(new BEProcessOff { iProcessOffId = Convert.ToInt32(rdr["ProcessOffId"]), sName = rdr["ProcessOff"].ToString() });
                    lProcessOff.Add(new BEProcessOff { iProcessId = Convert.ToInt32(rdr["ProcessId"]), sName = rdr["ProcessOff"].ToString() });
                }
            }
            return lProcessOff;
        }
        public IList<BEProcessOff> GetProcessOffListProcess(int iProcessID, int month, int Year)
        {
            IList<BEProcessOff> lProcessOff = new List<BEProcessOff>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSOFFID);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.String, iProcessID);
            db.AddInParameter(dbCommand, PARAM_MONTH, DbType.Int16, month);
            db.AddInParameter(dbCommand, PARAM_YEAR, DbType.Int16, Year);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    lProcessOff.Add(new BEProcessOff { iProcessId = Convert.ToInt32(rdr["ProcessId"]), iMonth = Convert.ToInt16(rdr["Month"]), iYear = Convert.ToInt16(rdr["Year"]), sDescription = rdr["Description"].ToString(), bDisabled = Convert.ToBoolean(rdr["Disabled"]), iClientId = Convert.ToInt32(rdr["ClientId"]) });
                }
            }
            return lProcessOff;
        }
        public string GetFirstLastDayOfCalender(int iProcessID, int Month, int Year)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_FIRSTLASTDAYOFCALENDER);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.String, iProcessID);
            db.AddInParameter(dbCommand, PARAM_MONTH, DbType.Int16, Month);
            db.AddInParameter(dbCommand, PARAM_YEAR, DbType.Int16, Year);
            return Convert.ToString(db.ExecuteScalar(dbCommand));
        }
        public IList<BEProcessOff> GetProcessOffListProcessDayWise(int iProcessID, int month, int Year)
        {
            IList<BEProcessOff> lProcessOff = new List<BEProcessOff>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSOFFIDDAYMONTHWISE);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.String, iProcessID);
            db.AddInParameter(dbCommand, PARAM_MONTH, DbType.Int16, month);
            db.AddInParameter(dbCommand, PARAM_YEAR, DbType.Int16, Year);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    lProcessOff.Add(new BEProcessOff { iProcessId = Convert.ToInt32(rdr["ProcessId"]), iMonth = Convert.ToInt16(rdr["Month"]), iYear = Convert.ToInt16(rdr["Year"]), iDay = Convert.ToInt16(rdr["Day"]), sDescription = rdr["Description"].ToString(), bDisabled = Convert.ToBoolean(rdr["Disabled"]), iClientId = Convert.ToInt32(rdr["ClientId"]) });
                }
            }
            return lProcessOff;
        }
        public void UpdateData(BEProcessOff oProcessOff)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open

                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        DbCommand dbDisableCommand = db.GetSqlStringCommand(SQL_DISABLED_PROCESSOFF);
                        db.AddInParameter(dbDisableCommand, PARAM_PROCESSID, DbType.Int32, oProcessOff.iProcessId);
                        db.AddInParameter(dbDisableCommand, PARAM_MONTH, DbType.String, oProcessOff.dtWeekInfo.Rows[0][4]);
                        db.AddInParameter(dbDisableCommand, PARAM_YEAR, DbType.String, oProcessOff.dtWeekInfo.Rows[0][5]);
                        db.AddInParameter(dbDisableCommand, PARAM_MODIFIEDBY, DbType.Int32, oProcessOff.iModifiedBy);
                        db.ExecuteNonQuery(dbDisableCommand, trans);
                        DbCommand dbCommand;
                        foreach (DataRow dr in oProcessOff.dtWeekInfo.Rows)
                        {
                            dbCommand = db.GetSqlStringCommand(SQL_UPDATE_PROCESSOFF);
                            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oProcessOff.iProcessId);
                            db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, dr[1].ToString());
                            db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, dr[1].ToString());
                            db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oProcessOff.sDescription);
                            db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, dr[2]);
                            db.AddInParameter(dbCommand, PARAM_DAY, DbType.String, dr[3]);
                            db.AddInParameter(dbCommand, PARAM_MONTH, DbType.String, dr[4]);
                            db.AddInParameter(dbCommand, PARAM_YEAR, DbType.String, dr[5]);
                            db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oProcessOff.iModifiedBy);
                            db.ExecuteNonQuery(dbCommand, trans);
                        }
                        trans.Commit();
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
                }
            }

        }
        public void InsertData(BEProcessOff oProcessOff)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open

                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        foreach (DataRow dr in oProcessOff.dtWeekInfo.Rows)
                        {
                            dbCommand = db.GetSqlStringCommand(SQL_INSERT_PROCESSOFF);
                            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oProcessOff.iProcessId);
                            db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, dr[1].ToString());
                            db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, dr[1].ToString());
                            db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oProcessOff.sDescription);
                            //db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oProcessOff.bDisabled);
                            db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, dr[2]);
                            db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oProcessOff.iCreatedBy);
                            db.ExecuteNonQuery(dbCommand, trans);
                        }
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
                }
                conn.Close();
            }
        }
    }
}
