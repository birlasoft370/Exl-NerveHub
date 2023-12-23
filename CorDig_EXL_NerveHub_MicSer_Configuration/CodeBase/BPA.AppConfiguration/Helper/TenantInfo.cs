using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfiguration.Models;

namespace BPA.AppConfiguration.Helper
{
    public static class TenantInfo
    {
        public static BETenant GetTanentInfo(TokenDetail _tokenDetails)
        {

            BETenant bETenant = new BETenant();
            bETenant.TenantID = Convert.ToInt32(_tokenDetails.TenantID);
            bETenant.ClientID = Convert.ToInt32(_tokenDetails.ClientID); ;
            bETenant.ClientName = _tokenDetails.ClientID;
            bETenant.ApplicationHostName = _tokenDetails.ApplicationHostName;
            bETenant.DatabaseName = _tokenDetails.DatabaseName;
            bETenant.DatabaseInstanceIP = _tokenDetails.DatabaseInstanceIP;
            bETenant.DatabaseConnectionString = _tokenDetails.DatabaseConnectionString;
            bETenant.ClientMultiLanguage = Convert.ToBoolean(_tokenDetails.ClientMultiLanguage);
            bETenant.HBMReportDatabaseInstanceName = _tokenDetails.HBMReportDatabaseInstanceName;
            bETenant.HBMReportDatabaseInstanceIP = _tokenDetails.HBMReportDatabaseInstanceIP;
            bETenant.HBMReportDatabaseConnectionString = _tokenDetails.HBMReportDatabaseConnectionString;
            bETenant.DataUtilityDatabaseInstanceName = _tokenDetails.DataUtilityDatabaseInstanceName;
            bETenant.DataUtilityDatabaseInstanceIP = _tokenDetails.HBMReportDatabaseInstanceIP;
            bETenant.DataUtilityDatabaseConnectionString = _tokenDetails.DataUtilityDatabaseConnectionString;
            bETenant.ADSDatabaseInstanceName = _tokenDetails.ADSDatabaseInstanceName;
            bETenant.ADSDatabaseInstanceIP = _tokenDetails.ADSDatabaseInstanceIP;
            bETenant.ADSDatabaseConnectionString = _tokenDetails.ADSDatabaseConnectionString;
            bETenant.XMLfilepath = _tokenDetails.XMLfilepath;
            return bETenant;

        }
    }
}
