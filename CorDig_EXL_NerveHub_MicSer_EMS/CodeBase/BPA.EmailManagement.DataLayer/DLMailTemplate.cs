using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.DataLayer.ExternalRef;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.Utility;

namespace BPA.EmailManagement.DataLayer
{
    public class DLMailTemplate : IDisposable
    {
        public DLMailTemplate(BETenant oTenant) { _oTenant = oTenant; }
        private BETenant _oTenant = null;

        private const string SP_GETMAILTEMPLATE = @"select MT.MailTemplateID,MT.TemplateName,MT.[Disabled],MT.MailTemplate from EMS.tblMailTemplate MT where MT.CampaignID=@CampaignId and MT.Disabled=0";
        private const string SQL_GETIMAGE_DATA = @"SELECT  [MailTemplateImageID],[MailTemplateID],LTRIM(rtrim([MailImageName])) as MailImageName,[MailImageWidth],[MailImageHeight],[MailImageAlt],[MailImage]      
                                                   FROM  [EMS].[tblMailTemplateImage] where   [EMS].[tblMailTemplateImage].MailTemplateID=@MailTemplateId ";
        private const string SQL_GETMAILTEMPLATE_ACTIVE_INACTIVE_All = @"select MT.MailTemplateID,MT.TemplateName,MT.[Disabled] from EMS.tblMailTemplate MT where MT.Disabled=@Disabled and MT.IsAutoReplay=@IsAutoReplay";
        private const string SP_MAILTEMPLATE_INSERT = @"[EMS].[USP_InsertMailTemplate]";
        private const string SP_GETMAILTEMPLATEWITHID = @"select MT.MailTemplateID,MT.MailTemplate,MT.TemplateName,
                                                        MT.[Disabled],MT.IsAutoReplay,CL.ClientID,CL.ClientName,P.ProcessID,P.ProcessName,CA.CampaignID,CA.CampaignName 
                                                        from EMS.tblMailTemplate MT
                                                        INNER JOIN Config.tblCampaignMaster CA on CA.CampaignID = MT.CampaignID INNER JOIN  Config.tblProcessMaster P on P.ProcessID =CA.ProcessID
                                                        INNER JOIN Config.tblClientMaster CL on CL.ClientID = P.ClientID where MT.MailTemplateID=@MailTemplateId";


        private const string PARAM_CAMPAIGNID = "@CampaignId";
        private const string PARAM_MAILTEMPLATEID = "@MailTemplateId";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_ISAUTOREPLY = "@IsAutoReplay";
        private const string PARAM_MAILTEMPLATENAME = "@TemplateName";
        private const string PARAM_MAILTEMPLATE = "@MailTemplate";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        public IList<BEMailTemplate> GetMailTemplateList(int userID, int CampaignID)
        {
            IList<BEMailTemplate> lBEMailTemplate = new List<BEMailTemplate>();

            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SP_GETMAILTEMPLATE);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignID);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        List<MailTemplateImage> lsting = new List<MailTemplateImage>();
                        DbCommand dbCommand1 = db.GetSqlStringCommand(SQL_GETIMAGE_DATA);
                        db.AddInParameter(dbCommand1, PARAM_MAILTEMPLATEID, DbType.Int32, int.Parse(rdr["MailTemplateID"].ToString()));
                        using (IDataReader rdr1 = db.ExecuteReader(dbCommand1))
                        {
                            while (rdr1.Read())
                            {
                                byte[] bytes = (byte[])rdr1["MailImage"];
                                // byte[] bytes = Convert.FromBase64String(rdr1["MailImage"].ToString());
                                lsting.Add(new MailTemplateImage
                                {
                                    sImageName = rdr1["MailImageName"].ToString(),
                                    sImageWidth = rdr1["MailImageWidth"].ToString(),
                                    sImageHeight = rdr1["MailImageHeight"].ToString(),
                                    bImageMail = bytes,
                                    iMailImageTemplateId = Convert.ToInt32(rdr1["MailTemplateImageID"].ToString()),
                                    iTempImageID = Convert.ToInt32(rdr1["MailTemplateID"].ToString())
                                });
                            }
                        }
                        BEMailTemplate oJob = new BEMailTemplate
                        {
                            iMailTemplateId = int.Parse(rdr["MailTemplateID"].ToString()),
                            sMailTemplateName = Convert.ToString(rdr["TemplateName"].ToString()) + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""),
                            sMailTemplate = rdr["MailTemplate"].ToString(),
                            lstImgMailTemp = lsting
                        };
                        lBEMailTemplate.Add(oJob);
                        oJob = null;
                    }

                }
                return lBEMailTemplate;
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

        public IList<BEMailTemplate> GetMailTemplateAll(bool IsActive, bool isAutoReply)
        {
            IList<BEMailTemplate> lBEMailTemplate = new List<BEMailTemplate>();
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_GETMAILTEMPLATE_ACTIVE_INACTIVE_All);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, IsActive);
                db.AddInParameter(dbCommand, PARAM_ISAUTOREPLY, DbType.Boolean, isAutoReply);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BEMailTemplate oJob = new BEMailTemplate { iMailTemplateId = int.Parse(rdr["MailTemplateID"].ToString()), sMailTemplateName = Convert.ToString(rdr["TemplateName"].ToString()) + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : "") };
                        lBEMailTemplate.Add(oJob);
                        oJob = null;
                    }

                }
                return lBEMailTemplate;
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

        public void InsertData(BEMailTemplate oBEMailTemplate)
        {

            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SP_MAILTEMPLATE_INSERT);
                db.AddInParameter(dbCommand, PARAM_MAILTEMPLATEID, DbType.Int32, oBEMailTemplate.iMailTemplateId);
                db.AddInParameter(dbCommand, PARAM_MAILTEMPLATENAME, DbType.String, oBEMailTemplate.sMailTemplateName);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, oBEMailTemplate.iCampaignID);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oBEMailTemplate.bDisabled);
                db.AddInParameter(dbCommand, PARAM_ISAUTOREPLY, DbType.Boolean, oBEMailTemplate.bIsAutoReplay);
                db.AddInParameter(dbCommand, PARAM_MAILTEMPLATE, DbType.String, oBEMailTemplate.sMailTemplate);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oBEMailTemplate.iCreatedBy);
                db.ExecuteNonQuery(dbCommand);
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_AssessmentAlready))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_AssessmentAlready);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }

        public void UpdateData(BEMailTemplate oBEMailTemplate)
        {

            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SP_MAILTEMPLATE_INSERT);
                db.AddInParameter(dbCommand, PARAM_MAILTEMPLATEID, DbType.Int32, oBEMailTemplate.iMailTemplateId);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oBEMailTemplate.bDisabled);
                db.AddInParameter(dbCommand, PARAM_ISAUTOREPLY, DbType.Boolean, oBEMailTemplate.bIsAutoReplay);
                db.AddInParameter(dbCommand, PARAM_MAILTEMPLATE, DbType.String, oBEMailTemplate.sMailTemplate);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oBEMailTemplate.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_MAILTEMPLATENAME, DbType.String, oBEMailTemplate.sMailTemplateName);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, oBEMailTemplate.iCampaignID);
                db.ExecuteNonQuery(dbCommand);
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_AssessmentAlready))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_AssessmentAlready);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }

        public IList<BEMailTemplate> GetMailTemplateDataList(int userID, int iMailTemplateID)
        {
            IList<BEMailTemplate> lBEMailTemplate = new List<BEMailTemplate>();
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SP_GETMAILTEMPLATEWITHID);
                db.AddInParameter(dbCommand, PARAM_MAILTEMPLATEID, DbType.Int32, iMailTemplateID);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BEMailTemplate oJob = new BEMailTemplate
                        {
                            iMailTemplateId = int.Parse(rdr["MailTemplateID"].ToString()),
                            sMailTemplateName = Convert.ToString(rdr["TemplateName"].ToString()).DecodeHtmlString(), // Add DecodeHtmlString by manishdwivedi - bug 319
                            sMailTemplate = rdr["MailTemplate"].ToString().ToString().DecodeHtmlString(), // Add DecodeHtmlString by manishdwivedi - bug 319
                            bDisabled = Convert.ToBoolean(rdr["Disabled"]),
                            bIsAutoReplay = Convert.ToBoolean(rdr["IsAutoReplay"]),
                            iClientID = int.Parse(rdr["ClientID"].ToString()),
                            iCampaignID = int.Parse(rdr["CampaignID"].ToString()),
                            iProcessID = int.Parse(rdr["ProcessID"].ToString())
                        };
                        lBEMailTemplate.Add(oJob);
                        oJob = null;
                    }

                }
                return lBEMailTemplate;
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

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
