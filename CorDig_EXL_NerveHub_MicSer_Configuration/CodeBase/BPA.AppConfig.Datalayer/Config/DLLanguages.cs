using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef;
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
    public class DLLanguages : IDisposable
    {
        private BETenant _oTenant = null;
        private const string SQL_SELECT_LanguageALL = @"SELECT LanguageID,Language,Culture, Disabled FROM Config.tblLanguage (nolock) WHERE Disabled=0";
        private const string SQL_SELECT_TranslationProviderALL = @"select ApiKey,ApiUrl,ProviderID,ProviderName,Disabled,ProfileID from Config.tblTranslationProvider (nolock) where Disabled=0";
        private const string SQL_INSERT_LANGUAGERECIEVED = @"INSERT INTO [WM].[tblLanguageSchedule] (CampaignID,LanguageConfigID,LanguageUploadTime,FileCreateTime,LanguageFilePath,FileName) VALUES (@CampaignId,@LanguageConfigID,GetDate(),@FileCreateTime,@FilePath,@FileName)";

        private const string SP_USP_InsertRecieveDateLangConfig = "[WM].[USP_InsertRecieveDateLangConfig]";

        private const string SP_USP_GetLanguageCreationTime = "[WM].[USP_GetLanguageRecieveDetails]";
        private const string SP_USP_GetLanguageConfiguration = "[WM].[USP_GetLanguageConfiguration]";
        private const string SP_USP_GetLanguageAllConfiguration = "[WM].[USP_GetLanguageAllConfiguration]";
        private const string SP_USP_GetEMSLanguageAllConfiguration = "[WM].[USP_GetEMSLanguageAllConfiguration]";

        private const string SP_USP_InsertUpdateLangConfig = "[WM].[USP_InsertUpdateLangConfig]";

        private const string SP_USP_InsertTranslationData = "[Lng].[Usp_Insert_LanguageTranslation_Data]";
        private const string SP_USP_GetTranslationData = "[WM].[USP_GetTranslationData]";

        private const string SP_USP_GetLanguageProfileID = "[EMS].[GetLanguageProfile]";

        private const string PARAM_LANGUAGERECIEVEDTIME = "@FileCreateTime";
        private const string PARAM_LANGUAGEFILENAME = "@FileName";
        private const string PARAM_LanguageConfigName = "@LanguageConfigName";
        private const string PARAM_LanguageName = "@LanguageName";
        private const string PARAM_LanguageConfigID = "@LanguageConfigID";
        private const string PARAM_CampaignId = "@CampaignId";
        private const string PARAM_Translateddocumentfolder = "@Translateddocumentfolder";
        private const string PARAM_Completedfolder = "@Completedfolder";
        private const string PARAM_Intakefolder = "@Intakefolder";
        private const string PARAM_Exceptionfolder = "@Exceptionfolder";
        private const string PARAM_BatchFrequencyType = "@BatchFrequencyType";
        private const string PARAM_BatchName = "@BatchName";
        private const string PARAM_bSWMWorkUpload = "@bSWMWorkUpload";
        private const string PARAM_FilePath = "@FilePath";
        private const string PARAM_Target = "@Target";
        private const string PARAM_Source = "@Source";
        private const string PARAM_ApiKey = "@ApiKey";
        private const string PARAM_ApiUrl = "@ApiUrl";
        private const string PARAM_BatchId = "@BatchId";
        private const string PARAM_Callback = "@Callback";
        private const string PARAM_Format = "@Format";
        private const string PARAM_ProfileID = "@ProfileID";
        private const string PARAM_WithSource = "@WithSource";
        private const string PARAM_WithAnnotations = "@WithAnnotations";
        private const string PARAM_WithDictionary = "@WithDictionary";
        private const string PARAM_WithCorpus = "@WithCorpus";
        private const string PARAM_Options = "@Options";
        private const string PARAM_Encoding = "@Encoding";
        private const string PARAM_Async = "@Async";
        private const string PARAM_BackTranslation = "@BackTranslation";
        private const string PARAM_CreatedBy = "@CreatedBy";
        private const string PARAM_ModifiedBy = "@ModifiedBy";
        private const string PARAM_Disabled = "@Disabled";
        private const string PARAM_IncomingMail = "@IncomingMail";
        private const string PARAM_CreateYearFolder = "@CreateYearFolder";
        private const string PARAM_iFormID = "@iFormID";
        private const string PARAM_STOREID = "@StoreID";
        private const string PARAM_ISACTIVE = "@IsActive";
        private const string PARAM_ScheduleInterval = "@ScheduleInterval";

        private const string PARAM_SpanishText = "@SpanishText";
        private const string PARAM_SystranText = "@SystranText";
        private const string PARAM_SMEChangesText = "@SMEChangesText";

        private const string PARAM_IsApproved = "@IsApproved";

        private const string PARAM_IsRejectedTech = "@IsRejectedTech";
        private const string PARAM_IsApprovedTech = "@IsApprovedTech";

        private const string PARAM_ApprovedBy = "@ApprovedBy";
        private const string PARAM_LngID = "@LngID";





      
        public DLLanguages(BETenant oTenant)
        { _oTenant = oTenant; }

   
        public void Dispose()
        { _oTenant = null; }
      
        public List<BELanguages> GetLanguageList(bool IsActiveLanguage)
        {
            return GetLanguageList("", IsActiveLanguage);

        }

        public void UpdateData(BEMailTranslatorConfiguration oBEMailTranslatorConfiguration, int iFormID, BETenant oTenant)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
        }

        public void DeleteData(BEMailTranslatorConfiguration oBEMailTranslatorConfiguration, int iFormID, BETenant oTenant)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
        }

        public void InsertRecieveDateTime(int iCampaignID, int iLanguageConfigID, string LanguageFilePath, DateTime FileCreateTime, string FileName)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbInsertUserCommand = db.GetStoredProcCommand(SP_USP_InsertRecieveDateLangConfig);
            db.AddInParameter(dbInsertUserCommand, PARAM_CampaignId, DbType.Int32, iCampaignID);
            db.AddInParameter(dbInsertUserCommand, PARAM_LanguageConfigID, DbType.Int32, iLanguageConfigID);
            db.AddInParameter(dbInsertUserCommand, PARAM_LANGUAGERECIEVEDTIME, DbType.DateTime, FileCreateTime);
            db.AddInParameter(dbInsertUserCommand, PARAM_LANGUAGEFILENAME, DbType.String, FileName);
            db.AddInParameter(dbInsertUserCommand, PARAM_FilePath, DbType.String, LanguageFilePath);
            db.ExecuteNonQuery(dbInsertUserCommand);

        }

        public IList<BELanguages> GetLanguageReceiveDateTime(int iCampaignID, int iLanguageConfigID)
        {
            IList<BELanguages> lReceiveDate = new List<BELanguages>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_USP_GetLanguageCreationTime);
            db.AddInParameter(dbCommand, PARAM_CampaignId, DbType.Int32, iCampaignID);
            db.AddInParameter(dbCommand, PARAM_LanguageConfigID, DbType.Int32, iLanguageConfigID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BELanguages obj = new BELanguages
                    {
                        dReceivedDateTime = DateTime.Parse(rdr["FileCreateTime"].ToString()),
                        FileName = rdr["FileName"].ToString(),
                        LanguageFilePath = rdr["LanguageFilePath"].ToString()

                    };
                    lReceiveDate.Add(obj);
                    obj = null;
                }
            }

            return lReceiveDate;
        }


        public IList<BEMailTranslatorConfiguration> GetEMSLanguageConfigAllData(int langConfigID, BETenant oTenant)
        {
            List<BEMailTranslatorConfiguration> lLangConfiguration = new List<BEMailTranslatorConfiguration>();

            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = null;
                dbCommand = db.GetStoredProcCommand(SP_USP_GetEMSLanguageAllConfiguration);
                db.AddInParameter(dbCommand, PARAM_LanguageConfigID, DbType.Int32, langConfigID);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BEMailTranslatorConfiguration BELangConfig = new BEMailTranslatorConfiguration
                        {
                            CampaignId = int.Parse(string.IsNullOrEmpty(rdr["CampaignId"].ToString()) ? "0" : rdr["CampaignId"].ToString()),
                            Format = string.IsNullOrEmpty(rdr["Format"].ToString()) ? "" : rdr["Format"].ToString(),
                            ProfileID = string.IsNullOrEmpty(rdr["ProfileID"].ToString()) ? "" : rdr["ProfileID"].ToString(),
                            Callback = string.IsNullOrEmpty(rdr["Callback"].ToString()) ? "" : rdr["Callback"].ToString(),
                            WithDictionary = string.IsNullOrEmpty(rdr["WithDictionary"].ToString()) ? "" : rdr["WithDictionary"].ToString(),
                            WithCorpus = string.IsNullOrEmpty(rdr["WithCorpus"].ToString()) ? "" : rdr["WithCorpus"].ToString(),
                            Options = new List<string> { string.IsNullOrEmpty(rdr["Options"].ToString()) ? "" : rdr["Options"].ToString() },
                            Encoding = string.IsNullOrEmpty(rdr["Encoding"].ToString()) ? "" : rdr["Encoding"].ToString(),
                            BatchId = string.IsNullOrEmpty(rdr["BatchId"].ToString()) ? "" : rdr["BatchId"].ToString(),
                            ApiUrl = string.IsNullOrEmpty(rdr["ApiUrl"].ToString()) ? "" : rdr["ApiUrl"].ToString(),
                            ApiKey = string.IsNullOrEmpty(rdr["ApiKey"].ToString()) ? "" : rdr["ApiKey"].ToString(),
                            Async = string.IsNullOrEmpty(rdr["Async"].ToString()) ? false : bool.Parse(rdr["Async"].ToString()),
                            BackTranslation = string.IsNullOrEmpty(rdr["BackTranslation"].ToString()) ? false : bool.Parse(rdr["BackTranslation"].ToString()),
                            LanguageConfigID = string.IsNullOrEmpty(rdr["LanguageConfigID"].ToString()) ? 0 : int.Parse(rdr["LanguageConfigID"].ToString()),
                            LanguageConfigName = string.IsNullOrEmpty(rdr["LanguageConfigName"].ToString()) ? "" : rdr["LanguageConfigName"].ToString(),
                            WithSource = string.IsNullOrEmpty(rdr["WithSource"].ToString()) ? false : bool.Parse(rdr["WithSource"].ToString()),
                            Target = string.IsNullOrEmpty(rdr["Target"].ToString()) ? "" : rdr["Target"].ToString(),
                            WithAnnotations = string.IsNullOrEmpty(rdr["WithAnnotations"].ToString()) ? false : bool.Parse(rdr["WithAnnotations"].ToString()),
                            Source = string.IsNullOrEmpty(rdr["Source"].ToString()) ? "" : rdr["Source"].ToString(),
                            CreatedBy = string.IsNullOrEmpty(rdr["Createdby"].ToString()) ? 0 : int.Parse(rdr["Createdby"].ToString()),
                            DStoreID = int.Parse(rdr["DStoreID"].ToString()),
                            Disabled = string.IsNullOrEmpty(rdr["Disabled"].ToString()) ? false : bool.Parse(rdr["Disabled"].ToString()),
                            IncomingMail = string.IsNullOrEmpty(rdr["IncomingMail"].ToString()) ? false : bool.Parse(rdr["IncomingMail"].ToString()),

                        };
                        lLangConfiguration.Add(BELangConfig);
                        BELangConfig = null;
                    }
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
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            return lLangConfiguration;

        }
        public IList<BEMailTranslatorConfiguration> GetLanguageConfigAllData(int langConfigID, BETenant oTenant)
        {
            List<BEMailTranslatorConfiguration> lLangConfiguration = new List<BEMailTranslatorConfiguration>();

            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = null;
                dbCommand = db.GetStoredProcCommand(SP_USP_GetLanguageAllConfiguration);
                db.AddInParameter(dbCommand, PARAM_LanguageConfigID, DbType.Int32, langConfigID);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BEMailTranslatorConfiguration BELangConfig = new BEMailTranslatorConfiguration
                        {
                            CampaignId = int.Parse(string.IsNullOrEmpty(rdr["CampaignId"].ToString()) ? "0" : rdr["CampaignId"].ToString()),
                            Format = string.IsNullOrEmpty(rdr["Format"].ToString()) ? "" : rdr["Format"].ToString(),
                            ProfileID = string.IsNullOrEmpty(rdr["ProfileID"].ToString()) ? "" : rdr["ProfileID"].ToString(),
                            Callback = string.IsNullOrEmpty(rdr["Callback"].ToString()) ? "" : rdr["Callback"].ToString(),
                            WithDictionary = string.IsNullOrEmpty(rdr["WithDictionary"].ToString()) ? "" : rdr["WithDictionary"].ToString(),
                            WithCorpus = string.IsNullOrEmpty(rdr["WithCorpus"].ToString()) ? "" : rdr["WithCorpus"].ToString(),
                            Options = new List<string> { string.IsNullOrEmpty(rdr["Options"].ToString()) ? "" : rdr["Options"].ToString() },
                            Encoding = string.IsNullOrEmpty(rdr["Encoding"].ToString()) ? "" : rdr["Encoding"].ToString(),
                            BatchId = string.IsNullOrEmpty(rdr["BatchId"].ToString()) ? "" : rdr["BatchId"].ToString(),
                            ApiUrl = string.IsNullOrEmpty(rdr["ApiUrl"].ToString()) ? "" : rdr["ApiUrl"].ToString(),
                            ApiKey = string.IsNullOrEmpty(rdr["ApiKey"].ToString()) ? "" : rdr["ApiKey"].ToString(),
                            Async = string.IsNullOrEmpty(rdr["Async"].ToString()) ? false : bool.Parse(rdr["Async"].ToString()),
                            BackTranslation = string.IsNullOrEmpty(rdr["BackTranslation"].ToString()) ? false : bool.Parse(rdr["BackTranslation"].ToString()),
                            LanguageConfigID = string.IsNullOrEmpty(rdr["LanguageConfigID"].ToString()) ? 0 : int.Parse(rdr["LanguageConfigID"].ToString()),
                            LanguageConfigName = string.IsNullOrEmpty(rdr["LanguageConfigName"].ToString()) ? "" : rdr["LanguageConfigName"].ToString(),
                            Translateddocumentfolder = string.IsNullOrEmpty(rdr["Translateddocumentfolder"].ToString()) ? "" : rdr["Translateddocumentfolder"].ToString(),
                            Intakefolder = string.IsNullOrEmpty(rdr["Intakefolder"].ToString()) ? "" : rdr["Intakefolder"].ToString(),
                            Exceptionfolder = string.IsNullOrEmpty(rdr["Exceptionfolder"].ToString()) ? "" : rdr["Exceptionfolder"].ToString(),
                            bSWMWorkUpload = string.IsNullOrEmpty(rdr["bSWMWorkUpload"].ToString()) ? false : bool.Parse(rdr["bSWMWorkUpload"].ToString()),
                            WithSource = string.IsNullOrEmpty(rdr["WithSource"].ToString()) ? false : bool.Parse(rdr["WithSource"].ToString()),
                            ScheduleInterval = string.IsNullOrEmpty(rdr["ScheduleInterval"].ToString()) ? 0 : int.Parse(rdr["ScheduleInterval"].ToString()),
                            BatchName = string.IsNullOrEmpty(rdr["BatchName"].ToString()) ? "" : rdr["BatchName"].ToString(),
                            Completedfolder = string.IsNullOrEmpty(rdr["Completedfolder"].ToString()) ? "" : rdr["Completedfolder"].ToString(),
                            Target = string.IsNullOrEmpty(rdr["Target"].ToString()) ? "" : rdr["Target"].ToString(),
                            WithAnnotations = string.IsNullOrEmpty(rdr["WithAnnotations"].ToString()) ? false : bool.Parse(rdr["WithAnnotations"].ToString()),
                            Source = string.IsNullOrEmpty(rdr["Source"].ToString()) ? "" : rdr["Source"].ToString(),
                            CreatedBy = string.IsNullOrEmpty(rdr["Createdby"].ToString()) ? 0 : int.Parse(rdr["Createdby"].ToString()),
                            LangBatchFrequency = (BatchFrequencyType)rdr["BatchFrequencyType"],
                            DStoreID = int.Parse(rdr["DStoreID"].ToString()),
                            Disabled = string.IsNullOrEmpty(rdr["Disabled"].ToString()) ? false : bool.Parse(rdr["Disabled"].ToString()),
                            CreateYearFolder = string.IsNullOrEmpty(rdr["CreateYearFolder"].ToString()) ? false : bool.Parse(rdr["CreateYearFolder"].ToString())
                        };
                        lLangConfiguration.Add(BELangConfig);
                        BELangConfig = null;
                    }
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
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            return lLangConfiguration;

        }

        public IList<BEMailTranslatorConfiguration> GetCampaignWiseLangList(int iStoreID, bool isActive, int iFormId)
        {
            List<BEMailTranslatorConfiguration> lLangConfiguration = new List<BEMailTranslatorConfiguration>();

            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = null;
                dbCommand = db.GetStoredProcCommand(SP_USP_GetLanguageConfiguration);
                db.AddInParameter(dbCommand, PARAM_STOREID, DbType.Int32, iStoreID);
                db.AddInParameter(dbCommand, PARAM_ISACTIVE, DbType.Boolean, isActive);
                db.AddInParameter(dbCommand, PARAM_iFormID, DbType.Int32, iFormId);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BEMailTranslatorConfiguration BELangConfig = new BEMailTranslatorConfiguration();

                        BELangConfig.CampaignId = int.Parse(string.IsNullOrEmpty(rdr["CampaignId"].ToString()) ? "0" : rdr["CampaignId"].ToString());
                        BELangConfig.Format = string.IsNullOrEmpty(rdr["Format"].ToString()) ? "" : rdr["Format"].ToString();
                        BELangConfig.ProfileID = string.IsNullOrEmpty(rdr["ProfileID"].ToString()) ? "" : rdr["ProfileID"].ToString();
                        BELangConfig.Callback = string.IsNullOrEmpty(rdr["Callback"].ToString()) ? "" : rdr["Callback"].ToString();
                        BELangConfig.WithDictionary = string.IsNullOrEmpty(rdr["WithDictionary"].ToString()) ? "" : rdr["WithDictionary"].ToString();
                        BELangConfig.WithCorpus = string.IsNullOrEmpty(rdr["WithCorpus"].ToString()) ? "" : rdr["WithCorpus"].ToString();
                        BELangConfig.Options = new List<string> { string.IsNullOrEmpty(rdr["Options"].ToString()) ? "" : rdr["Options"].ToString() };
                        BELangConfig.Encoding = string.IsNullOrEmpty(rdr["Encoding"].ToString()) ? "" : rdr["Encoding"].ToString();
                        BELangConfig.BatchId = string.IsNullOrEmpty(rdr["BatchId"].ToString()) ? "" : rdr["BatchId"].ToString();
                        BELangConfig.ApiUrl = string.IsNullOrEmpty(rdr["ApiUrl"].ToString()) ? "" : rdr["ApiUrl"].ToString();
                        BELangConfig.ApiKey = string.IsNullOrEmpty(rdr["ApiKey"].ToString()) ? "" : rdr["ApiKey"].ToString();
                        BELangConfig.Async = string.IsNullOrEmpty(rdr["Async"].ToString()) ? false : bool.Parse(rdr["Async"].ToString());
                        BELangConfig.BackTranslation = string.IsNullOrEmpty(rdr["BackTranslation"].ToString()) ? false : bool.Parse(rdr["BackTranslation"].ToString());
                        BELangConfig.LanguageConfigID = string.IsNullOrEmpty(rdr["LanguageConfigID"].ToString()) ? 0 : int.Parse(rdr["LanguageConfigID"].ToString());
                        BELangConfig.LanguageConfigName = string.IsNullOrEmpty(rdr["LanguageConfigName"].ToString()) ? "" : rdr["LanguageConfigName"].ToString();
                        if (iFormId == 1)
                        {
                            BELangConfig.Translateddocumentfolder = string.IsNullOrEmpty(rdr["Translateddocumentfolder"].ToString()) ? "" : rdr["Translateddocumentfolder"].ToString();
                            BELangConfig.Completedfolder = string.IsNullOrEmpty(rdr["Completedfolder"].ToString()) ? "" : rdr["Completedfolder"].ToString();
                            BELangConfig.Intakefolder = string.IsNullOrEmpty(rdr["Intakefolder"].ToString()) ? "" : rdr["Intakefolder"].ToString();
                            BELangConfig.Exceptionfolder = string.IsNullOrEmpty(rdr["Exceptionfolder"].ToString()) ? "" : rdr["Exceptionfolder"].ToString();
                            BELangConfig.bSWMWorkUpload = string.IsNullOrEmpty(rdr["bSWMWorkUpload"].ToString()) ? false : bool.Parse(rdr["bSWMWorkUpload"].ToString());
                            BELangConfig.CreateYearFolder = string.IsNullOrEmpty(rdr["CreateYearFolder"].ToString()) ? false : bool.Parse(rdr["CreateYearFolder"].ToString());
                            BELangConfig.LangBatchFrequency = (BatchFrequencyType)rdr["BatchFrequencyType"];
                            BELangConfig.ScheduleInterval = string.IsNullOrEmpty(rdr["ScheduleInterval"].ToString()) ? 0 : int.Parse(rdr["ScheduleInterval"].ToString());
                            BELangConfig.BatchName = string.IsNullOrEmpty(rdr["BatchName"].ToString()) ? "" : rdr["BatchName"].ToString();

                        }
                        BELangConfig.WithSource = string.IsNullOrEmpty(rdr["WithSource"].ToString()) ? false : bool.Parse(rdr["WithSource"].ToString());
                        BELangConfig.FilePath = string.IsNullOrEmpty(rdr["FilePath"].ToString()) ? "" : rdr["FilePath"].ToString();
                        BELangConfig.Target = string.IsNullOrEmpty(rdr["Target"].ToString()) ? "" : rdr["Target"].ToString();
                        BELangConfig.WithAnnotations = string.IsNullOrEmpty(rdr["WithAnnotations"].ToString()) ? false : bool.Parse(rdr["WithAnnotations"].ToString());
                        BELangConfig.Source = string.IsNullOrEmpty(rdr["Source"].ToString()) ? "" : rdr["Source"].ToString();
                        BELangConfig.CreatedBy = string.IsNullOrEmpty(rdr["Createdby"].ToString()) ? 0 : int.Parse(rdr["Createdby"].ToString());

                        BELangConfig.DStoreID = int.Parse(rdr["DStoreID"].ToString());
                        BELangConfig.Disabled = string.IsNullOrEmpty(rdr["Disabled"].ToString()) ? false : bool.Parse(rdr["Disabled"].ToString());
                        if (iFormId == 2)
                        {
                            BELangConfig.IncomingMail = string.IsNullOrEmpty(rdr["IncomingMail"].ToString()) ? false : bool.Parse(rdr["IncomingMail"].ToString());
                        }
                        lLangConfiguration.Add(BELangConfig);
                        BELangConfig = null;
                    }
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
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            return lLangConfiguration;
        }

        public void InsertUpdateData(BEMailTranslatorConfiguration oBEMailTranslatorConfiguration, int iFormID, BETenant oTenant)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = null;
                dbCommand = db.GetStoredProcCommand(SP_USP_InsertUpdateLangConfig);
                db.AddInParameter(dbCommand, PARAM_Translateddocumentfolder, DbType.String, oBEMailTranslatorConfiguration.Translateddocumentfolder);
                db.AddInParameter(dbCommand, PARAM_Completedfolder, DbType.String, oBEMailTranslatorConfiguration.Completedfolder);
                db.AddInParameter(dbCommand, PARAM_Intakefolder, DbType.String, oBEMailTranslatorConfiguration.Intakefolder);
                db.AddInParameter(dbCommand, PARAM_Exceptionfolder, DbType.String, oBEMailTranslatorConfiguration.Exceptionfolder);
                db.AddInParameter(dbCommand, PARAM_BatchFrequencyType, DbType.Int32, (int)oBEMailTranslatorConfiguration.LangBatchFrequency);
                db.AddInParameter(dbCommand, PARAM_BatchName, DbType.String, oBEMailTranslatorConfiguration.BatchName);
                db.AddInParameter(dbCommand, PARAM_bSWMWorkUpload, DbType.Boolean, oBEMailTranslatorConfiguration.bSWMWorkUpload);
                db.AddInParameter(dbCommand, PARAM_ScheduleInterval, DbType.Int32, oBEMailTranslatorConfiguration.ScheduleInterval);
                db.AddInParameter(dbCommand, PARAM_CreateYearFolder, DbType.Int32, oBEMailTranslatorConfiguration.CreateYearFolder);
                db.AddInParameter(dbCommand, PARAM_LanguageConfigID, DbType.Int32, oBEMailTranslatorConfiguration.LanguageConfigID);
                db.AddInParameter(dbCommand, PARAM_CampaignId, DbType.Int32, oBEMailTranslatorConfiguration.CampaignId);
                db.AddInParameter(dbCommand, PARAM_LanguageConfigName, DbType.String, oBEMailTranslatorConfiguration.LanguageConfigName);

                db.AddInParameter(dbCommand, PARAM_FilePath, DbType.String, oBEMailTranslatorConfiguration.FilePath);
                db.AddInParameter(dbCommand, PARAM_Target, DbType.String, oBEMailTranslatorConfiguration.Target);
                db.AddInParameter(dbCommand, PARAM_Source, DbType.String, oBEMailTranslatorConfiguration.Source);
                db.AddInParameter(dbCommand, PARAM_ApiKey, DbType.String, oBEMailTranslatorConfiguration.ApiKey);
                db.AddInParameter(dbCommand, PARAM_ApiUrl, DbType.String, oBEMailTranslatorConfiguration.ApiUrl);
                db.AddInParameter(dbCommand, PARAM_BatchId, DbType.String, oBEMailTranslatorConfiguration.BatchId);
                db.AddInParameter(dbCommand, PARAM_Callback, DbType.String, oBEMailTranslatorConfiguration.Callback);
                db.AddInParameter(dbCommand, PARAM_Format, DbType.String, oBEMailTranslatorConfiguration.Format);
                db.AddInParameter(dbCommand, PARAM_ProfileID, DbType.String, oBEMailTranslatorConfiguration.ProfileID);
                db.AddInParameter(dbCommand, PARAM_WithSource, DbType.Boolean, oBEMailTranslatorConfiguration.WithSource);
                db.AddInParameter(dbCommand, PARAM_WithAnnotations, DbType.String, oBEMailTranslatorConfiguration.WithAnnotations);
                db.AddInParameter(dbCommand, PARAM_WithDictionary, DbType.String, oBEMailTranslatorConfiguration.WithDictionary);
                db.AddInParameter(dbCommand, PARAM_WithCorpus, DbType.String, oBEMailTranslatorConfiguration.WithCorpus);
                db.AddInParameter(dbCommand, PARAM_Options, DbType.String, oBEMailTranslatorConfiguration.Options);
                db.AddInParameter(dbCommand, PARAM_Encoding, DbType.String, oBEMailTranslatorConfiguration.Encoding);
                db.AddInParameter(dbCommand, PARAM_Async, DbType.Boolean, oBEMailTranslatorConfiguration.Async);
                db.AddInParameter(dbCommand, PARAM_BackTranslation, DbType.Boolean, oBEMailTranslatorConfiguration.BackTranslation);
                db.AddInParameter(dbCommand, PARAM_CreatedBy, DbType.Int32, oBEMailTranslatorConfiguration.CreatedBy);
                if (oBEMailTranslatorConfiguration.LanguageConfigID != 0)
                    db.AddInParameter(dbCommand, PARAM_ModifiedBy, DbType.Int32, oBEMailTranslatorConfiguration.iUserID);
                db.AddInParameter(dbCommand, PARAM_Disabled, DbType.Boolean, oBEMailTranslatorConfiguration.Disabled);
                db.AddInParameter(dbCommand, PARAM_IncomingMail, DbType.Boolean, oBEMailTranslatorConfiguration.IncomingMail);

                db.AddInParameter(dbCommand, PARAM_iFormID, DbType.Int32, iFormID);


                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open


                    try
                    {
                        int iLanguageConfigID = Convert.ToInt32(db.ExecuteScalar(dbCommand));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
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
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
       
        public List<BELanguages> GetLanguageList(string LanguageName, bool IsActiveLanguage)
        {
            List<BELanguages> lLanguage = new List<BELanguages>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (IsActiveLanguage)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_LanguageALL);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_LanguageALL);
            }
            db.AddInParameter(dbCommand, PARAM_LanguageName, DbType.String, "%" + LanguageName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BELanguages objLanguage = new BELanguages(Convert.ToInt32(rdr["LanguageID"]), (rdr["Language"].ToString()), rdr["Culture"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lLanguage.Add(objLanguage);
                    objLanguage = null;
                }
            }
            return lLanguage;

        }


        public List<BEMailTranslatorConfiguration> GetProviderList(string ProviderName)
        {
            List<BEMailTranslatorConfiguration> lProviderList = new List<BEMailTranslatorConfiguration>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetSqlStringCommand(SQL_SELECT_TranslationProviderALL);
            db.AddInParameter(dbCommand, PARAM_LanguageName, DbType.String, "%" + ProviderName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEMailTranslatorConfiguration objProvider = new BEMailTranslatorConfiguration
                    {
                        ApiKey = rdr["ApiKey"].ToString(),
                        ApiUrl = rdr["ApiUrl"].ToString(),
                        ProviderID = Convert.ToInt32(rdr["ProviderID"]),
                        iLanguageProviderTypeID = rdr["ProviderName"].ToString().ToUpper() == "SYSTRAN" ? LanguageProvider.SysTran : LanguageProvider.MicroSoft,
                        Disabled = Convert.ToBoolean(rdr["Disabled"]),
                        ProfileID = rdr["ProfileID"] == null ? "" : rdr["ProfileID"].ToString(),
                    };

                    lProviderList.Add(objProvider);
                    objProvider = null;
                }
            }
            return lProviderList;

        }

        public List<BEMailTranslatorConfiguration> GetLanguageProfile(int iCampaignID)
        {
            List<BEMailTranslatorConfiguration> lLangConfiguration = new List<BEMailTranslatorConfiguration>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = null;
            try
            {
                dbCommand = db.GetStoredProcCommand(SP_USP_GetLanguageProfileID);
                db.AddInParameter(dbCommand, PARAM_CampaignId, DbType.Int32, iCampaignID);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BEMailTranslatorConfiguration BELangConfig = new BEMailTranslatorConfiguration();


                        BELangConfig.ProfileID = string.IsNullOrEmpty(rdr["ProfileID"].ToString()) ? "" : rdr["ProfileID"].ToString();
                        BELangConfig.LanguageConfigID = string.IsNullOrEmpty(rdr["LanguageConfigID"].ToString()) ? 0 : int.Parse(rdr["LanguageConfigID"].ToString());
                        lLangConfiguration.Add(BELangConfig);
                        BELangConfig = null;
                    }
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
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            return lLangConfiguration;
        }

        public DataTable GetIncomingTranslationData(int iCampaignID, int LanguageConfigID = 0)
        {
            try
            {
                DataSet ds = new DataSet();
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand;
                dbCommand = db.GetStoredProcCommand(SP_USP_GetTranslationData);
                db.AddInParameter(dbCommand, PARAM_CampaignId, DbType.Int32, iCampaignID);
                db.AddInParameter(dbCommand, PARAM_LanguageConfigID, DbType.Int32, LanguageConfigID);
                db.LoadDataSet(dbCommand, ds, "PendingLanguageObjectApproval");
                return ds.Tables[0];
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
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }

        public void InsertIncomingTranslationData(BEMailTranslatorConfiguration oBEMailTranslatorConfiguration)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = null;
            try
            {
                dbCommand = db.GetStoredProcCommand(SP_USP_InsertTranslationData);
                db.AddInParameter(dbCommand, PARAM_LngID, DbType.Int32, oBEMailTranslatorConfiguration.LngID);
                db.AddInParameter(dbCommand, PARAM_CampaignId, DbType.Int32, oBEMailTranslatorConfiguration.CampaignId);
                db.AddInParameter(dbCommand, PARAM_SpanishText, DbType.String, oBEMailTranslatorConfiguration.SpanishText);
                db.AddInParameter(dbCommand, PARAM_SystranText, DbType.String, oBEMailTranslatorConfiguration.SystranText);
                db.AddInParameter(dbCommand, PARAM_SMEChangesText, DbType.String, oBEMailTranslatorConfiguration.SMEChangesText);
                db.AddInParameter(dbCommand, PARAM_IsApproved, DbType.Boolean, oBEMailTranslatorConfiguration.IsApproved);
                db.AddInParameter(dbCommand, PARAM_IsRejectedTech, DbType.Boolean, oBEMailTranslatorConfiguration.IsRejectedTech);
                db.AddInParameter(dbCommand, PARAM_ApprovedBy, DbType.Int32, oBEMailTranslatorConfiguration.ApprovedBy);
                db.AddInParameter(dbCommand, PARAM_CreatedBy, DbType.Int32, oBEMailTranslatorConfiguration.CreatedBy);
                db.AddInParameter(dbCommand, PARAM_ModifiedBy, DbType.Int32, oBEMailTranslatorConfiguration.iUserID);
                db.AddInParameter(dbCommand, PARAM_Disabled, DbType.Boolean, oBEMailTranslatorConfiguration.Disabled);
                db.AddInParameter(dbCommand, PARAM_IncomingMail, DbType.Boolean, oBEMailTranslatorConfiguration.IncomingMail);
                db.AddInParameter(dbCommand, PARAM_LanguageConfigID, DbType.Int32, oBEMailTranslatorConfiguration.LanguageConfigID);
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
