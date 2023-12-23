using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.SharePointConfiguration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.Utility;

namespace BPA.AppConfig.Datalayer.ExternalRef.SharePointConfiguration
{
    public class DLSharePointConfiguration : IDisposable
    {
        private BETenant _oTenant = null;

        public void Dispose()
        {
            _oTenant = null;
        }
        public DLSharePointConfiguration(BETenant oTenant)
        {
            _oTenant = oTenant;
        }

        private const string USP_CDS_INSERTUPDATESHAREPOINTSCHEDULER = @"[WM].[Usp_CDS_InsertUpdateSharepointScheduler]";
        private const string SQL_DELETE_SHAREPOINTSCHEDULER = @"DELETE FROM [WM].[SharePointAD_Schedular] WHERE SchedulerID=@SchedulerID";
        private const string USP_CDS_GETSHAREPOINTSCHEDULEDLIST = @"[WM].[Usp_CDS_GetSharepointScheduledList]";
        private const string USP_CDS_GETSHAREPOINTSCHEDULERLISTBYID = @"[WM].[Usp_CDS_GetSharepointSchedulerListById]";
        private const string USP_CDS_CHECKSHAREPOINTMAPPINGEXIST = @"[WM].[Usp_CDS_CheckSharepointMappingExist]";

        private const string USP_CDS_SEARCHSHAREPOINT = @"[WM].[Usp_CDS_GetSharepointSearchById]";

        private const string PARAM_SCHEDULERID = "@SchedulerID";
        private const string PARAM_CAMPAIGNID = "@CampaignID";
        private const string PARAM_SHAREPOINTPATH = "@SharePointPath";
        private const string PARAM_SHAREPOINTNAME = "@SharePointName";
        private const string PARAM_SROOTFOLDERNAME = "@RootFolderName";
        private const string PARAM_DATAFOLDERLOCATION = "@DataFolderLocation";
        private const string PARAM_ERRORFOLDERLOCATION = "@ErrorFolderLocation";
        private const string PARAM_ARCHIVALFOLDERLOCATION = "@ArchivalFolderLocation";
        private const string PARAM_DURATION = "@Duration";
        private const string PARAM_FILEEXTENTION = "@FileExtention";
        private const string PARAM_FREQUENCY = "@Frequency";
        private const string PARAM_DELIMITER = "@Delimiter";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_USERID = "@UserId";
        private const string PARAM_CLIENTID = "@ClientId";
        private const string PARAM_ISACTIVE = "@IsActive";
        private const string PARAM_USERPD = "@UserPwd";

        public void InsertData(BESharePointConfiguration oSharepoint)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(USP_CDS_INSERTUPDATESHAREPOINTSCHEDULER);
                db.AddInParameter(dbCommand, PARAM_SCHEDULERID, DbType.Int32, oSharepoint.iSchedulerID);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, oSharepoint.iCampaignID);
                db.AddInParameter(dbCommand, PARAM_SHAREPOINTPATH, DbType.String, oSharepoint.sSharePointPath);
                db.AddInParameter(dbCommand, PARAM_SHAREPOINTNAME, DbType.String, oSharepoint.sSharePointName);
                db.AddInParameter(dbCommand, PARAM_SROOTFOLDERNAME, DbType.String, oSharepoint.sRootFolderName);
                db.AddInParameter(dbCommand, PARAM_DATAFOLDERLOCATION, DbType.String, oSharepoint.sDataFolderLocation);
                db.AddInParameter(dbCommand, PARAM_ERRORFOLDERLOCATION, DbType.String, oSharepoint.sErrorFolderLocation);
                db.AddInParameter(dbCommand, PARAM_ARCHIVALFOLDERLOCATION, DbType.String, oSharepoint.sArchivalFolderLocation);
                db.AddInParameter(dbCommand, PARAM_DURATION, DbType.String, oSharepoint.iDuration);
                db.AddInParameter(dbCommand, PARAM_FREQUENCY, DbType.String, oSharepoint.sFrequency);
                db.AddInParameter(dbCommand, PARAM_FILEEXTENTION, DbType.String, oSharepoint.sFileExtention);
                db.AddInParameter(dbCommand, PARAM_DELIMITER, DbType.String, oSharepoint.sDelimiter);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.String, oSharepoint.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.String, oSharepoint.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.String, oSharepoint.iModifiedBy);
                db.AddInParameter(dbCommand, PARAM_USERID, DbType.String, oSharepoint.sUserID);
                db.AddInParameter(dbCommand, PARAM_USERPD, DbType.String, oSharepoint.sPassword);
                //*************************************
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbCommand, trans);
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
        public IList<BESharePointConfiguration> GetSharePointData(int iShedulerID)
        {
            IList<BESharePointConfiguration> lSharepointScheduler = new List<BESharePointConfiguration>();
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);


                DbCommand dbCommand = db.GetStoredProcCommand(USP_CDS_GETSHAREPOINTSCHEDULERLISTBYID);
                db.AddInParameter(dbCommand, PARAM_SCHEDULERID, DbType.Int32, iShedulerID);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BESharePointConfiguration oSP = new BESharePointConfiguration();
                        oSP.iClientID = Convert.ToInt32(rdr["ClientID"]);
                        oSP.iProcessID = Convert.ToInt32(rdr["ProcessID"]);
                        oSP.sSharePointName = Convert.ToString(rdr["SharePointName"]);
                        oSP.iSchedulerID = Convert.ToInt32(rdr["SchedulerID"]);
                        oSP.iCampaignID = Convert.ToInt32(rdr["CampaignID"]);
                        oSP.sSharePointPath = Convert.ToString(rdr["SharePointPath"]);
                        oSP.sRootFolderName = Convert.ToString(rdr["RootFolderName"]);
                        oSP.sDataFolderLocation = Convert.ToString(rdr["DataFolderLocation"]);
                        oSP.sErrorFolderLocation = Convert.ToString(rdr["ErrorFolderLocation"]);
                        oSP.sArchivalFolderLocation = Convert.ToString(rdr["ArchivalFolderLocation"]);
                        oSP.iDuration = Convert.ToInt32(rdr["Duration"]);
                        oSP.sFrequency = Convert.ToString(rdr["Frequency"]);
                        oSP.sFileExtention = Convert.ToString(rdr["FileExtention"]);
                        oSP.sDelimiter = Convert.ToString(rdr["Delimiter"]);
                        if (rdr.HasColumn("Disabled"))
                            oSP.bDisabled = Convert.ToBoolean(rdr["Disabled"]);
                        if (rdr.HasColumn("UserId"))
                            oSP.sUserID = Convert.ToString(rdr["UserId"]);
                        if (rdr.HasColumn("Password"))
                            oSP.sPassword = Convert.ToString(rdr["Password"]);
                        lSharepointScheduler.Add(oSP);
                        oSP = null;
                    }
                }
                return lSharepointScheduler;
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
                throw;
            }
            //return lstShare;
        }


        public IList<BESharePointConfiguration> GetSearchSharePoint(int CampaignID)
        {
            IList<BESharePointConfiguration> lSharepointScheduler = new List<BESharePointConfiguration>();
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);


                DbCommand dbCommand = db.GetStoredProcCommand(USP_CDS_SEARCHSHAREPOINT);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignID);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BESharePointConfiguration oSP = new BESharePointConfiguration();
                        oSP.sSharePointName = Convert.ToString(rdr["SharePointName"]) + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : "");
                        oSP.iSchedulerID = Convert.ToInt32(rdr["SchedulerID"]);
                        lSharepointScheduler.Add(oSP);
                        oSP = null;
                    }
                }
                return lSharepointScheduler;
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
                throw;
            }
            //return lstShare;
        }

        public bool CheckIfCampaignMappingExist(string campaignId)
        {
            IList<int> lRet = new List<int>();
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbCommand = db.GetStoredProcCommand(USP_CDS_CHECKSHAREPOINTMAPPINGEXIST);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, Int32.Parse(campaignId));

                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {

                        lRet.Add(Convert.ToInt32(rdr["ret"]));

                    }
                }
                return lRet[0] == -1 ? true : false;
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
