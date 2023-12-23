
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.DataLayer.ExternalRef.Utitlity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BPA.Security.Datalayer
{
    public class BPADatabase : IDisposable
    {

        const string SQL_GETTENANT = @"Select ClientName,ClientDeploymentID,DP.ClientID,DatabaseConfigurationMode,ApplicationHostName,ApplictionWebServerIP,DatabaseInstanceName,DatabaseInstanceIP
                                            from [dbo].[tblAdminClientPackageDeployment] (NOLOCK) DP
                                            INNER JOIN [dbo].[tblAdminClient] DC
                                            ON DP.ClientID=DC.ClientID
                                            where DP.IsActive=1 and ApplicationDeploymentMode='EXL'";

        public IList<BETenant> GetTenantList()
        {
            bool readfromXML = false;
            IList<BETenant> ITenant = new List<BETenant>();
            DataSet ds = new DataSet();
            readfromXML = true;//bool.Parse(AppSettingsValues.GetSetting("readfromXML"));
            if (readfromXML)
            {
                System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
                string codeBase = System.IO.Path.GetDirectoryName(ass.CodeBase);
                System.Uri uri = new Uri(codeBase);
                var XMLDirectoryPath = uri.LocalPath.Replace(@"\bin", "").Replace(@"\Debug", "");
                XMLDirectoryPath = XMLDirectoryPath + @"\Tenant.xml";
                ds.ReadXml(XMLDirectoryPath);
            }
            else
            {
                DatabaseProviderFactory dbFactory = new DatabaseProviderFactory();
                string str = ConfigurationManager.ConnectionStrings["DBadminConnection"].ConnectionString;
                Database db = new SqlDatabase(str);

                DbCommand dbCommand = db.GetSqlStringCommand(SQL_GETTENANT);


                ds = db.ExecuteDataSet(dbCommand);
            }
            BETenant oTenant = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    oTenant = new BETenant()
                    {
                        ApplicationHostName = dr["ApplicationHostName"].ToString(),
                        ClientID = int.Parse(dr["ClientID"].ToString()),
                        ClientName = dr["ClientName"].ToString(),
                        DatabaseInstanceIP = dr["DatabaseInstanceIP"].ToString(),
                        DatabaseName = dr["DatabaseInstanceName"].ToString(),
                        TenantID = int.Parse(dr["ClientDeploymentID"].ToString()),
                        ClientMultiLanguage = dr["ClientMultiLanguage"].ToString().ToUpper() == "TRUE" ? true : false,
                    };
                    oTenant.DatabaseConnectionString = EncryptDecrypt.Encrypt(@"server=" + dr["DatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + dr["DatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;");
                    ITenant.Add(oTenant);
                    oTenant = null;
                }
            }

            return ITenant;
        }
        public BETenant GetTenantInfo(string Url, string TenantName)
        {
            bool readfromXML = false;
            var XMLDirectoryPath = "";
          //  bool.TryParse(AppSettingsValues.GetSetting("readfromXML"), out readfromXML);
            BETenant oTenant = null;
            DataSet ds = new DataSet();
            if (readfromXML)
            {
                System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
                string codeBase = System.IO.Path.GetDirectoryName(ass.CodeBase);
                System.Uri uri = new Uri(codeBase);
                XMLDirectoryPath = uri.LocalPath.Replace(@"\bin", "").Replace(@"\Debug", "");
                XMLDirectoryPath = XMLDirectoryPath + @"\Tenant.xml";
                ds.ReadXml(XMLDirectoryPath);
            }
            else
            {

                Database db = DL_Shared.dbFactory("DBadminConnection");
                //new SqlDatabase("DBadminConnection");
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_GETTENANT);
                //db.AddInParameter(dbCommand, "@HostName", DbType.String, Url);
                ds = db.ExecuteDataSet(dbCommand);
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].CaseSensitive = false;

                var T = ds.Tables[0].Select("ClientName='" + TenantName.ToUpper() + "' and ApplicationHostName='" + Url.ToUpper() + "'");

                if (T != null && T.Count() > 0)
                {
                    oTenant = new BETenant()
                    {
                        ApplicationHostName = T[0]["ApplicationHostName"].ToString(),
                        ClientID = int.Parse(T[0]["ClientID"].ToString()),
                        ClientName = T[0]["ClientName"].ToString(),
                        DatabaseInstanceIP = EncryptDecrypt.Encrypt(T[0]["DatabaseInstanceIP"].ToString()),
                        DatabaseName = EncryptDecrypt.Encrypt(T[0]["DatabaseInstanceName"].ToString()),
                        TenantID = int.Parse(T[0]["ClientDeploymentID"].ToString()),
                        ClientMultiLanguage = T[0]["ClientMultiLanguage"].ToString().ToUpper() == "TRUE" ? true : false,
                        HBMReportDatabaseInstanceIP = EncryptDecrypt.Encrypt(T[0]["HBMReportDatabaseInstanceIP"].ToString()),
                        HBMReportDatabaseInstanceName = EncryptDecrypt.Encrypt(T[0]["HBMReportDatabaseInstanceName"].ToString()),
                        DataUtilityDatabaseInstanceName = EncryptDecrypt.Encrypt(T[0]["DataUtilityDatabaseInstanceName"].ToString()),
                        DataUtilityDatabaseInstanceIP = EncryptDecrypt.Encrypt(T[0]["DataUtilityDatabaseInstanceIP"].ToString()),
                        ADSDatabaseInstanceName = EncryptDecrypt.Encrypt(T[0]["ADSDatabaseInstanceName"].ToString()),
                        ADSDatabaseInstanceIP = EncryptDecrypt.Encrypt(T[0]["ADSDatabaseInstanceIP"].ToString()),
                        XMLfilepath = XMLDirectoryPath
                    };
                    if (T[0]["azureconnection"].ToString() != "NO")
                    {
                        string Constring = T[0]["azureconnection"].ToString();
                        
                        oTenant.DatabaseConnectionString = EncryptDecrypt.Encrypt(Constring);
                    }
                    else
                    {
                        oTenant.DatabaseConnectionString = EncryptDecrypt.Encrypt(@"Server=" + T[0]["DatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + T[0]["DatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;");
                    }
                    // oTenant.DatabaseConnectionString = EncryptDecrypt.Encrypt(@"Server=" + T[0]["DatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + T[0]["DatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;");
                   // oTenant.DatabaseConnectionString = EncryptDecrypt.Encrypt(@"Server=" + T[0]["DatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + T[0]["DatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;");
                    oTenant.HBMReportDatabaseConnectionString = EncryptDecrypt.Encrypt(@"Server=" + T[0]["HBMReportDatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + T[0]["HBMReportDatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;");
                    oTenant.DataUtilityDatabaseConnectionString = EncryptDecrypt.Encrypt(@"Server=" + T[0]["DataUtilityDatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + T[0]["DataUtilityDatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;");
                    oTenant.ADSDatabaseConnectionString = EncryptDecrypt.Encrypt(@"Server=" + T[0]["ADSDatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + T[0]["ADSDatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;");
                }
            }
            return oTenant;
        }
        public string ToXml(DataSet ds)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(DataSet));
                    xmlSerializer.Serialize(streamWriter, ds);
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }


        public void Dispose()
        {

        }
    }
}
