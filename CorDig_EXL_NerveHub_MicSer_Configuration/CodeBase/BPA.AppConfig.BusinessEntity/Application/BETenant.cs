using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Application
{
    [Serializable]
    public class BETenant
    {
        public int TenantID { get; set; }

        public int ClientID { get; set; }

        public string ClientName { get; set; }

        public string ApplicationHostName { get; set; }

        public string DatabaseName { get; set; }

        public string DatabaseInstanceIP { get; set; }

        public string DatabaseConnectionString { get; set; }

        public string DatabaseConnectionBIQueryString
        {
            get
            {
                return @"Server=BPUATSQLVS01;Database=ConfigurationDB;Trusted_Connection=True;Min Pool Size=5;Max Pool Size=300;MultipleActiveResultSets=True;";
            }
        }
        public bool ClientMultiLanguage { get; set; }

        public string DatabaseConnectionBIDatawareHouseString
        {
            get
            {
                return @"";
            }
        }



        //HBM Properties
        public string HBMReportDatabaseInstanceName { get; set; }
        public string HBMReportDatabaseInstanceIP { get; set; }
        public string HBMReportDatabaseConnectionString { get; set; }

        //DataUtility 
        public string DataUtilityDatabaseInstanceName { get; set; }
        public string DataUtilityDatabaseInstanceIP { get; set; }
        public string DataUtilityDatabaseConnectionString { get; set; }

        //EXLDCCDW Agent Daily Summary 
        public string ADSDatabaseInstanceName { get; set; }
        public string ADSDatabaseInstanceIP { get; set; }
        public string ADSDatabaseConnectionString { get; set; }
        public string XMLfilepath { get; set; }

        public string AppTenantName { get; set; } = "EXLPAS";

    }
}
