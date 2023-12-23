using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer.CacheData
{
    public class EmailObjectCache : IDisposable
    {
        public void Dispose()
        {
        }

        public ExchangeService ConnectExchnageServer(BEMailConfiguration oMailConfig, BETenant oTenant, string mailFrom = "")
        {
            ExchangeService service = null;
            try
            {
                if (!string.IsNullOrEmpty(mailFrom)) mailFrom = mailFrom.Trim();
                //string cacheName = "MailServiceConfig_" + oTenant.TenantID.ToString() + "_" + oMailConfig.iMailConfigID ;
                //if (oMailConfig.oUserCredential != null)
                //{
                //    cacheName += "_" + oMailConfig.oUserCredential.sUserLANID + oMailConfig.sEmailID;
                //}
                //else
                //{
                //    cacheName += "_" + oMailConfig.sUserID + "_" + oMailConfig.sEmailID;
                //}
                //if (_EmailServerObjectCache != null)
                //{
                //    service = (ExchangeService)_EmailServerObjectCache.GetFromCache(cacheName);
                //}
                if (service == null)
                {
                    ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    // ServicePointManager.ServerCertificateValidationCallback = CertificateCallback.CertificateValidationCallBack;
                    //  System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;
                    // ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                    TimeZoneInfo tz = TimeZoneInfo.Local;
                    if (!string.IsNullOrEmpty(oMailConfig.oTimeZone.sTimeZoneID))
                    {
                        tz = TimeZoneInfo.FindSystemTimeZoneById(oMailConfig.oTimeZone.sTimeZoneID);
                    }

                    //string logstr = "  Local TimeZone : " + tz.DisplayName +
                    //                "  Campaign : " + oMailConfig.iCampaignID;

                    //EHLogger.WriteLog(logstr, EHLogger.ApplicationLog.EMSServiceLog);

                    switch (oMailConfig.iMailServerTypeID)
                    {
                        case EmailServerType.Dominos:
                            break;
                        case EmailServerType.Exchange2007SP1:
                            service = new ExchangeService(ExchangeVersion.Exchange2007_SP1, tz);
                            break;
                        case EmailServerType.Exchange2010SP1:
                            service = new ExchangeService(ExchangeVersion.Exchange2010_SP1, tz);
                            break;
                        case EmailServerType.Exchange2010SP2:
                            service = new ExchangeService(ExchangeVersion.Exchange2010_SP2, tz);
                            break;
                        case EmailServerType.Office365:
                            service = new ExchangeService(ExchangeVersion.Exchange2013, tz);
                            break;
                        default:
                            break;
                    }

                    service.WebProxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                    service.WebProxy.Credentials = CredentialCache.DefaultCredentials;


                    if (oMailConfig.oUserCredential != null)
                    {
                        service.Credentials = new WebCredentials(oMailConfig.oUserCredential.sUserLANID, BPA.Utility.EncryptDecrypt.Decrypt(oMailConfig.oUserCredential.sPassword));
                    }
                    else if (oMailConfig.bUseServiceCredentialToPull)
                    {
                        service.UseDefaultCredentials = true;
                    }
                    else
                    {
                        if (oMailConfig.sPassword != null)
                            service.Credentials = new WebCredentials(oMailConfig.sUserID, BPA.Utility.EncryptDecrypt.Decrypt(oMailConfig.sPassword));
                    }
                    //}
                    //else
                    //{
                    //    service.Credentials = new WebCredentials();
                    //}
                    if (oMailConfig.sAutoDiscoveryPath.Trim() != null)
                        service.Url = new Uri(oMailConfig.sAutoDiscoveryPath.Trim());

                    if (oMailConfig.bImpersonation)
                    {

                        switch (oMailConfig.sImpersonationIDType)
                        {

                            case ImpersonationIDType.PrincipalName:
                                service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.PrincipalName, oMailConfig.sImpersonationID);
                                break;
                            case ImpersonationIDType.SID:
                                service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SID, oMailConfig.sImpersonationID);
                                break;
                            case ImpersonationIDType.SmtpAddress:
                                if (!string.IsNullOrEmpty(mailFrom))
                                {
                                    //  string logstr1 = "  mailFrom : " + mailFrom +
                                    //"  Campaign : " + oMailConfig.iCampaignID;

                                    //  EHLogger.WriteLog(logstr1, EHLogger.ApplicationLog.EMSServiceLog);
                                    service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, mailFrom.Trim());
                                }
                                else
                                {
                                    //   string logstr1 = "  oMailConfig.sImpersonationID : " + oMailConfig.sImpersonationID +
                                    //"  Campaign : " + oMailConfig.iCampaignID;

                                    //   EHLogger.WriteLog(logstr1, EHLogger.ApplicationLog.EMSServiceLog);
                                    service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, oMailConfig.sImpersonationID);
                                }
                                break;

                            default:
                                break;
                        }
                    }

                    service.HttpHeaders.Clear();
                    if (!string.IsNullOrEmpty(mailFrom))
                    {
                        service.HttpHeaders.Add("X-AnchorMailbox", mailFrom.Trim());
                    }
                    else
                    {
                        service.HttpHeaders.Add("X-AnchorMailbox", oMailConfig.sEmailID.Trim());
                    }

                    #region added for trace and other info


                    bool _bLogEmailEvent = (System.Configuration.ConfigurationManager.AppSettings["LogEmailEvent"] == null) ? false : bool.Parse(System.Configuration.ConfigurationManager.AppSettings["LogEmailEvent"]);
                    if (_bLogEmailEvent)
                    {
                        service.TraceEnabled = true;
                        service.TraceEnablePrettyPrinting = true;
                        service.TraceListener = new EwsTraceListener();
                        service.ReturnClientRequestId = true;  // This will give us more data back about the servers used in the response headers
                        service.SendClientLatencies = true;  // sends latency info which is used by Microsoft to improve EWS and Exchagne 365.
                        service.KeepAlive = false;
                    }

                    #endregion

                    // service.PreferredCulture = new System.Globalization.CultureInfo("en-AU", true);

                    // _EmailServerObjectCache.AddToCache(cacheName, service, MyCachePriority.Default);

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return service;
        }
    }
}
