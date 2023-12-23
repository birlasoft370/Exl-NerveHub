namespace MicSer.Tenant.Models
{
    public class BETenant
    {
        public string ApplicationHostName { get; set; }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string DatabaseInstanceIP { get; set; }
        public string DatabaseName { get; set; }
        public int TenantID { get; set; }
        public string XMLfilepath { get; set; }
        public bool ClientMultiLanguage { get; set; }
        public string HBMReportDatabaseInstanceIP { get; set; }
        public string HBMReportDatabaseInstanceName { get; set; }
        public string DataUtilityDatabaseInstanceName { get; set; } = "";
        public string DataUtilityDatabaseInstanceIP { get; set; } = "";
        public string ADSDatabaseInstanceName { get; set; } = "";
        public string ADSDatabaseInstanceIP { get; set; } = "";

        public string DatabaseConnectionString { get; set; } = "";
        public string HBMReportDatabaseConnectionString { get; set; } = "";
        public string DataUtilityDatabaseConnectionString { get; set; } = "";
        public string ADSDatabaseConnectionString { get; set; } = "";
    }
}
