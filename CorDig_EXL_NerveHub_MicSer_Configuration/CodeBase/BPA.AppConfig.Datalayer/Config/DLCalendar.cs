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
    public class DLCalendar : IDisposable
    {
        private BETenant _oTenant = null;

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLCalendar"/> class.
        /// </summary>
        public DLCalendar(BETenant oTenant)
        {
            _oTenant = oTenant;
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        private const string SQL_INSERT_CalendarMasterData = @"Usp_CDS_CalanderMasterData_Insert";
        private const string SQL_INSERT_CALANDERDATA = @"dbo.Usp_CDS_CalanderData_Insert";
        private const string SQL_UPDATE_CalendarData = @"[Config].[USP_CalanderData_Update]";

        private const string SQL_SELECT_Calendar = @"SELECT distinct CalenderID,Name,isnull(Description,'') as Description, Disabled FROM Config.tblCalenderMaster (nolock) WHERE Disabled=0 and Name Like @CalendarName ORDER BY Name";
        private const string SQL_SELECT_CalendarALL = @"SELECT distinct CalenderID,Name,isnull(Description,'') as Description, Disabled FROM Config.tblCalenderMaster (nolock) WHERE Name Like @CalendarName ORDER BY Name";
        private const string SQL_SELECT_CalendarID = @"SELECT CalenderID,Name,isnull(Description,'') as Description, Disabled FROM Config.tblCalenderMaster (nolock) WHERE CalenderID = @CalendarID";
        private const string SQL_UPDATE_Calendar = @"if not exists(select CalenderID from Config.tblCalenderMaster where CalenderID=@CalendarID and Name=@CalendarName)
                                                    Begin
	                                                    if exists(select processid from Config.tblProcessMaster where CalendarID=@CalendarID and disabled=0)
	                                                    Begin
		                                                    Raiserror('Calendar name can not be modified as this calendar is being used in processes.',15,1)
	                                                    End
	                                                    Else
	                                                    Begin
		                                                    UPDATE Config.tblCalenderMaster SET Name=@CalendarName,Description=@Description,Disabled=@Disabled,ModifiedBy=@ModifiedBy , ModifiedOn=GetDate() WHERE CalenderID=@CalendarID
	                                                    End
                                                    End
                                                    Else
                                                    Begin
	                                                    UPDATE Config.tblCalenderMaster SET Name=@CalendarName,Description=@Description,Disabled=@Disabled,ModifiedBy=@ModifiedBy , ModifiedOn=GetDate() WHERE CalenderID=@CalendarID
                                                    End";
        private const string SQL_SELECT_CalendarData1 = @"SELECT Distinct Name+' (Year - '+cast(Year as Varchar(5))+' Month - '+ case when Month=1 then 'January' when Month=2 then 'February' when Month=3 then 'March' when Month=4 then 'April' when Month=5 then 'May' when Month=6 then 'June' when Month=7 then 'July' when Month=8 then 'August' when Month=9 then 'September' when Month=10 then 'October' when Month=11 then 'Novemebr' when Month=12 then 'December' end  +')' as 'Name',  
                                                        cast(a.CalenderID as varchar(5))+'|'+cast(Year as varchar(5))+'|'+cast(Month as varchar(5)) as 'CalenderDataID' 
                                                        FROM Config.tblCalenderData (nolock) a inner join Config.tblCalenderMaster (nolock) b on a.CalenderID=b.CalenderID  
                                                        WHERE a.CalenderID = @CalendarID and Month=isnull(@Month,Month) and Year=isnull(@Year,Year) and a.Disabled=0 and b.Disabled=0 ORDER BY Name";
        private const string SQL_SELECT_CalendarDataALL1 = @"SELECT Distinct Name+' (Year - '+cast(Year as Varchar(5))+' Month - '+ case when Month=1 then 'January' when Month=2 then 'February' when Month=3 then 'March' when Month=4 then 'April' when Month=5 then 'May' when Month=6 then 'June' when Month=7 then 'July' when Month=8 then 'August' when Month=9 then 'September' when Month=10 then 'October' when Month=11 then 'Novemebr' when Month=12 then 'December' end  +')' as 'Name',  
                                                            cast(a.CalenderID as varchar(5))+'|'+cast(Year as varchar(5))+'|'+cast(Month as varchar(5)) as 'CalenderDataID' 
                                                            FROM Config.tblCalenderData (nolock) a inner join Config.tblCalenderMaster (nolock) b on a.CalenderID=b.CalenderID  
                                                            WHERE a.CalenderID = @CalendarID and Month=isnull(@Month,Month) and Year=isnull(@Year,Year) ORDER BY Name";
        private const string SQL_SELECT_CalendarData = @"SELECT Distinct Name+' (Year - '+cast(Year as Varchar(5))+' Month - '+ case when Month=1 then 'January' when Month=2 then 'February' when Month=3 then 'March' when Month=4 then 'April' when Month=5 then 'May' when Month=6 then 'June' when Month=7 then 'July' when Month=8 then 'August' when Month=9 then 'September' when Month=10 then 'October' when Month=11 then 'Novemebr' when Month=12 then 'December' end  +')' as 'Name',  cast(a.CalenderID as varchar(5))+'|'+cast(Year as varchar(5))+'|'+cast(Month as varchar(5)) as 'CalenderDataID' FROM Config.tblCalenderData (nolock) a inner join Config.tblCalenderMaster (nolock) b on a.CalenderID=b.CalenderID  WHERE b.Name Like @CalendarName and a.Disabled=0 and b.Disabled=0 ORDER BY Name";
        private const string SQL_SELECT_CalendarDataALL = @"SELECT Distinct Name+' (Year - '+cast(Year as Varchar(5))+' Month - '+ case when Month=1 then 'January' when Month=2 then 'February' when Month=3 then 'March' when Month=4 then 'April' when Month=5 then 'May' when Month=6 then 'June' when Month=7 then 'July' when Month=8 then 'August' when Month=9 then 'September' when Month=10 then 'October' when Month=11 then 'Novemebr' when Month=12 then 'December' end  +')' as 'Name',  cast(a.CalenderID as varchar(5))+'|'+cast(Year as varchar(5))+'|'+cast(Month as varchar(5)) as 'CalenderDataID' FROM Config.tblCalenderData (nolock) a inner join Config.tblCalenderMaster (nolock) b on a.CalenderID=b.CalenderID WHERE b.Name Like @CalendarName ORDER BY Name";
        private const string SQL_SELECT_CalendarDataID = @"SELECT Name, CalenderDataID, a.CalenderID, Month, Year, Week, StartDate, EndDate, a.Disabled FROM Config.tblCalenderData (nolock) a inner join Config.tblCalenderMaster (nolock) b on a.CalenderID=b.CalenderID  WHERE a.CalenderId = @CalendarID and Year=@Year and Month=@Month";
        private const string SQL_DELETE_CalendarData = @"if exists(select processid from Config.tblProcessMaster where CalendarID = (select calenderid from Config.tblCalenderData WHERE CalenderDataID=@CalendarDataID) and disabled=0)
	                                                    Begin
		                                                    Raiserror('Calendar data can not be modified as this calendar is being used in processes.',15,1)
	                                                    End
	                                                    Else
	                                                    Begin
                                                            DELETE FROM Config.tblCalenderData WHERE CalenderDataID=@CalendarDataID
                                                        End";
        private const string SQL_DELETE_Calendar = @"if exists(select processid from Config.tblProcessMaster where CalendarID in (select calenderid from Config.tblCalenderData WHERE CalenderID=@CalendarID) and disabled=0)
	                                                    Begin
Raiserror('Calendar data can not be delete as this calendar is being used in processes.',15,1)
	                                                    End
	                                                    Else
	                                                    Begin
DELETE FROM Config.tblCalenderData WHERE CalenderID=@CalendarID
Delete from Config.tblCalenderMaster where Config.tblCalenderMaster.CalenderID=@CalendarID
                                                        End";

        private const string PARAM_CalendarID = "@CalendarID";
        private const string PARAM_CalendarNAME = "@CalendarName";
        private const string PARAM_CalendarDESC = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";

        private const string PARAM_CalendarDataID = "@CalendarDataID";
        private const string PARAM_Month = "@Month";
        private const string PARAM_Year = "@Year";
        private const string PARAM_Week = "@Week";
        private const string PARAM_STARTDATE = "@StartDate";
        private const string PARAM_ENDDATE = "@EndDate";
        private const string PARAM_OUT = "@ReturnValue";
        private const string PARAM_OUT_CALANDER = "@MaxValue";
        private const string SQL_GET_MAX_WEEK = @" Select isnull(Max(Week),0) FROM Config.tblCalenderData WHERE CalenderID=@CalendarID and Year=@Year";

        public IList<BECalendarInfo> GetCalendarList(string CalendarName, bool IsActiveCalendar)
        {
            IList<BECalendarInfo> lCalendar = new List<BECalendarInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (IsActiveCalendar)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_Calendar);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_CalendarALL);
            }
            db.AddInParameter(dbCommand, PARAM_CalendarNAME, DbType.String, "%" + CalendarName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BECalendarInfo objCalendar = new BECalendarInfo(Convert.ToInt32(rdr["CalenderID"]), rdr["Name"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lCalendar.Add(objCalendar);
                    objCalendar = null;
                }
            }
            return lCalendar;

        }
        public IList<BECalendarInfo> GetCalendarList(int CalendarID)
        {
            IList<BECalendarInfo> lCalendar = new List<BECalendarInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CalendarID);
            db.AddInParameter(dbCommand, PARAM_CalendarID, DbType.Int32, CalendarID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BECalendarInfo objCalendar = new BECalendarInfo(Convert.ToInt32(rdr["CalenderID"]), rdr["Name"].ToString(), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lCalendar.Add(objCalendar);
                    objCalendar = null;
                }
            }
            return lCalendar;

        }

        public int InsertData(BECalendarInfo oCalendar)
        {
            int iReturnValue = 0;
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbCommand = db.GetStoredProcCommand(SQL_INSERT_CalendarMasterData);
                db.AddInParameter(dbCommand, PARAM_CalendarNAME, DbType.String, oCalendar.sCalendarName);
                db.AddInParameter(dbCommand, PARAM_CalendarDESC, DbType.String, oCalendar.sDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oCalendar.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oCalendar.iCreatedBy);
                db.AddOutParameter(dbCommand, PARAM_OUT_CALANDER, DbType.Int32, 20);
                db.ExecuteNonQuery(dbCommand);
                iReturnValue = Convert.ToInt16(db.GetParameterValue(dbCommand, PARAM_OUT_CALANDER));
                return iReturnValue;
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

        public void UpdateData(BECalendarInfo oCalendar)
        {

            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_Calendar);
                db.AddInParameter(dbCommand, PARAM_CalendarID, DbType.Int32, oCalendar.iCalendarID);
                db.AddInParameter(dbCommand, PARAM_CalendarNAME, DbType.String, oCalendar.sCalendarName);
                db.AddInParameter(dbCommand, PARAM_CalendarDESC, DbType.String, oCalendar.sDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oCalendar.bDisabled);
                db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oCalendar.iModifiedBy);
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

        public IList<BECalendarInfo> GetCalendarDataList(int CalendarID, int Year, int Month, bool IsActiveCalendar)
        {
            IList<BECalendarInfo> lCalendar = new List<BECalendarInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (IsActiveCalendar)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_CalendarData1);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_CalendarDataALL1);
            }
            db.AddInParameter(dbCommand, PARAM_CalendarID, DbType.Int32, CalendarID);
            if (Year != 0)
                db.AddInParameter(dbCommand, PARAM_Year, DbType.Int32, Year);
            else
                db.AddInParameter(dbCommand, PARAM_Year, DbType.Int32, DBNull.Value);
            if (Month != 0)
                db.AddInParameter(dbCommand, PARAM_Month, DbType.Int32, Month);
            else
                db.AddInParameter(dbCommand, PARAM_Month, DbType.Int32, DBNull.Value);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BECalendarInfo objCalendar = new BECalendarInfo();
                    objCalendar.sCalendarName = rdr["Name"].ToString();
                    objCalendar.sDescription = rdr["CalenderDataID"].ToString();
                    lCalendar.Add(objCalendar);
                    objCalendar = null;
                }
            }
            return lCalendar;

        }
        public IList<BECalendarInfo> GetCalendarDataList(string CalendarName, bool IsActiveCalendar)
        {
            IList<BECalendarInfo> lCalendar = new List<BECalendarInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (IsActiveCalendar)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_CalendarData);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_CalendarDataALL);
            }
            db.AddInParameter(dbCommand, PARAM_CalendarNAME, DbType.String, "%" + CalendarName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BECalendarInfo objCalendar = new BECalendarInfo();
                    objCalendar.sCalendarName = rdr["Name"].ToString();
                    objCalendar.sDescription = rdr["CalenderDataID"].ToString();
                    lCalendar.Add(objCalendar);
                    objCalendar = null;
                }
            }
            return lCalendar;

        }
        public IList<BECalendarInfo> GetCalendarDataList(BECalendarInfo oCalendar)
        {
            IList<BECalendarInfo> lCalendar = new List<BECalendarInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CalendarDataID);
            db.AddInParameter(dbCommand, PARAM_CalendarID, DbType.Int32, oCalendar.iCalendarID);
            db.AddInParameter(dbCommand, PARAM_Year, DbType.Int32, oCalendar.iYear);
            db.AddInParameter(dbCommand, PARAM_Month, DbType.Int32, oCalendar.iMonth);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BECalendarInfo objCalendar = new BECalendarInfo(Convert.ToInt32(rdr["CalenderDataID"]), Convert.ToInt32(rdr["CalenderID"]), Convert.ToInt32(rdr["Month"]), Convert.ToInt32(rdr["Year"]), Convert.ToInt32(rdr["Week"]), Convert.ToDateTime(rdr["StartDate"]), Convert.ToDateTime(rdr["EndDate"]), Convert.ToBoolean(rdr["Disabled"]), 0, rdr["Name"].ToString() + " - Week (" + rdr["Week"].ToString() + ")");
                    lCalendar.Add(objCalendar);
                    objCalendar = null;
                }
            }
            return lCalendar;

        }

        public string ManageCalendarData(BECalendarInfo oCalendar)
        {
            string sReturnValue = "1";
            string sWeek = "0";
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = null;
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open

                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {

                    try
                    {
                        foreach (DataRow dr in oCalendar.dtCalanderData.Rows)
                        {
                            if (dr[3].ToString() == "insert")
                            {
                                dbCommand = db.GetStoredProcCommand(SQL_INSERT_CALANDERDATA);
                                db.AddInParameter(dbCommand, PARAM_CalendarID, DbType.Int32, oCalendar.iCalendarID);
                                db.AddInParameter(dbCommand, PARAM_Month, DbType.Int32, oCalendar.iMonth);
                                db.AddInParameter(dbCommand, PARAM_Year, DbType.Int32, oCalendar.iYear);
                                db.AddInParameter(dbCommand, PARAM_Week, DbType.Int32, Convert.ToInt16(dr[0].ToString()));
                                db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, Convert.ToDateTime(dr[1].ToString()));
                                db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, Convert.ToDateTime(dr[2].ToString()));
                                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oCalendar.bDisabled);
                                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oCalendar.iCreatedBy);
                                db.AddOutParameter(dbCommand, PARAM_OUT, DbType.Int32, 20);
                                db.ExecuteNonQuery(dbCommand, trans);
                                sReturnValue = Convert.ToString(db.GetParameterValue(dbCommand, PARAM_OUT));
                                sWeek = dr[0].ToString();
                                if (sReturnValue == "2" || sReturnValue == "3")
                                {
                                    trans.Rollback();
                                    break;
                                }
                            }
                            if (dr[3].ToString() == "update")
                            {
                                dbCommand = db.GetStoredProcCommand(SQL_UPDATE_CalendarData);
                                db.AddInParameter(dbCommand, PARAM_CalendarID, DbType.Int32, oCalendar.iCalendarID);
                                db.AddInParameter(dbCommand, PARAM_Month, DbType.Int32, oCalendar.iMonth);
                                db.AddInParameter(dbCommand, PARAM_Year, DbType.Int32, oCalendar.iYear);
                                db.AddInParameter(dbCommand, PARAM_Week, DbType.Int32, Convert.ToInt16(dr[0].ToString()));
                                db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, Convert.ToDateTime(dr[1].ToString()));
                                db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, Convert.ToDateTime(dr[2].ToString()));
                                db.AddInParameter(dbCommand, PARAM_CalendarDataID, DbType.Int32, Convert.ToInt32(dr[4].ToString()));
                                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oCalendar.bDisabled);
                                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oCalendar.iCreatedBy);
                                db.AddOutParameter(dbCommand, PARAM_OUT, DbType.Int32, 20);
                                db.ExecuteNonQuery(dbCommand, trans);
                                sReturnValue = Convert.ToString(db.GetParameterValue(dbCommand, PARAM_OUT));
                                sWeek = dr[0].ToString();
                                if (sReturnValue == "2" || sReturnValue == "3")
                                {
                                    trans.Rollback();
                                    break;
                                }
                            }
                            else if (dr[3].ToString() == "delete")
                            {
                                dbCommand = db.GetSqlStringCommand(SQL_DELETE_CalendarData);
                                db.AddInParameter(dbCommand, PARAM_CalendarDataID, DbType.Int32, Convert.ToInt32(dr[4].ToString()));
                                db.AddInParameter(dbCommand, PARAM_CalendarID, DbType.Int32, oCalendar.iCalendarID);
                                db.ExecuteNonQuery(dbCommand, trans);
                            }

                        }
                        if (sReturnValue == "1")
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
                        if (ex.Number == 2627 || ex.Number == 2601)
                        {
                            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                        }
                        if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_Calendar_used_processes))
                        {
                            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_Calendar_used_processes);
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

            sReturnValue = sReturnValue + "|" + sWeek;
            return sReturnValue.ToString();

        }

        public void DeleteData(BECalendarInfo oCalendar)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE_Calendar);
                db.AddInParameter(dbCommand, PARAM_CalendarID, DbType.Int32, oCalendar.iCalendarID);
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
        public int GetMaxWeek(BECalendarInfo oCalendar)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_GET_MAX_WEEK);
            db.AddInParameter(dbCommand, PARAM_CalendarID, DbType.Int16, oCalendar.iCalendarID);
            db.AddInParameter(dbCommand, PARAM_Year, DbType.Int16, oCalendar.iYear);
            return (int)db.ExecuteScalar(dbCommand);
        }
    }
}
