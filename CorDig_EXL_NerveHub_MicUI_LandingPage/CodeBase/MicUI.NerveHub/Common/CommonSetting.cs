using Microsoft.AspNetCore.Mvc.Rendering;

namespace MicUI.NerveHub.Common
{
    public static class CommonSetting
    {
        #region "Properties"


        /// <summary>
        ///  This property is return the site url
        /// </summary>
        public static string SiteUrl
        {
            get;
            

        }

        /// <summary>
        ///  This is the static list of the columns to bind the search column on package advance search popup page
        /// </summary>
        public static List<SelectListItem> PackageSearchColumns
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Package Name", Value = "PackageName" });
                items.Add(new SelectListItem { Text = "Package Description", Value = "PackageDescription" });
                return items;
            }
        }

        /// <summary>
        ///  This is the static list of the columns to bind the search column on Subscription List page in advance search popup page
        /// </summary>
        public static List<SelectListItem> SubscripionSearchColumns
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Client Name", Value = "ClientName" });
                items.Add(new SelectListItem { Text = "Package Name", Value = "PackageName" });
                items.Add(new SelectListItem { Text = "License Users", Value = "LicenseUsers" });
                items.Add(new SelectListItem { Text = "License Validity", Value = "LicenseValiditty" });
                items.Add(new SelectListItem { Text = "License Users", Value = "LicenseUsers" });
                items.Add(new SelectListItem { Text = "License Start Date", Value = "LicenseStartDate" });
                items.Add(new SelectListItem { Text = "License End Date", Value = "LicenseEndDate" });
                items.Add(new SelectListItem { Text = "License Notification", Value = "LicenseNotification" });
                return items;
            }
        }


        /// <summary>
        ///  This is the static list of the columns to bind the search column on Subscription List page in advance search popup page
        /// </summary>
        public static List<SelectListItem> PackageLicenseSearchColumns
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Package Name", Value = "PackageName" });
                items.Add(new SelectListItem { Text = "Module Names", Value = "ModuleNames" });
                return items;
            }
        }

        /// <summary>
        ///  This is the static list of the columns to bind the search column on package advance search popup page
        /// </summary>
        public static List<SelectListItem> PackageDeploymentSearchColumns
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Client Name", Value = "ClientName" });
                items.Add(new SelectListItem { Text = "Package Name", Value = "PackageName" });
                items.Add(new SelectListItem { Text = "Application Deployment Mode", Value = "ApplicationDeploymentMode" });
                items.Add(new SelectListItem { Text = "PDatabase Configuration Mode", Value = "DatabaseConfigurationMode" });
                items.Add(new SelectListItem { Text = "Application Host Name", Value = "ApplicationHostName" });
                items.Add(new SelectListItem { Text = "Appliction WebServer IP", Value = "ApplictionWebServerIP" });
                items.Add(new SelectListItem { Text = "Database Instance Name", Value = "DatabaseInstanceName" });
                items.Add(new SelectListItem { Text = "Database Instance IP", Value = "DatabaseInstanceIP" });
                return items;
            }
        }

        public static List<SelectListItem> DataArchivalSearchColumns
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Client Name", Value = "ClientName" });
                items.Add(new SelectListItem { Text = "Data Archival Scope", Value = "DataArchivalScope" });
                items.Add(new SelectListItem { Text = "Data Archival Start Date", Value = "DataArchivalStartDate" });
                items.Add(new SelectListItem { Text = "Data Archival End Date", Value = "DataArchivalEndDate" });
                items.Add(new SelectListItem { Text = "Data Archival Storage Mode", Value = "DataArchivalStorageMode" });
                items.Add(new SelectListItem { Text = "Database Instance Name", Value = "DatabaseInstanceName" });
                items.Add(new SelectListItem { Text = "Database Instance IP", Value = "DatabaseInstanceIP" });

                return items;
            }
        }


        /*To get list of column for advance search on client provisining page*/
        public static List<SelectListItem> CleintSearchColumns
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Client Name", Value = "ClientName" });
                items.Add(new SelectListItem { Text = "Description", Value = "ClientDescription" });
                items.Add(new SelectListItem { Text = "Client Domain", Value = "ClientDomain" });
                items.Add(new SelectListItem { Text = "Client Country", Value = "ClientCountry" });
                items.Add(new SelectListItem { Text = "Client Address", Value = "ClientAddress" });
                items.Add(new SelectListItem { Text = "SPOC Name", Value = "ClientSPOCName" });
                items.Add(new SelectListItem { Text = "SPOC EmailID", Value = "ClientSPOCEmailID" });
                items.Add(new SelectListItem { Text = "SPOC Contact Number", Value = "ClientSPOCContactNumber" });
                return items;
            }
        }
        /*To get list of column for advance search based on column*/
        public static List<SelectListItem> GetColumnList(string table)
        {
            List<SelectListItem> ColumnList = new List<SelectListItem>();
            switch (table)
            {
                case "Client":
                    ColumnList = CleintSearchColumns;
                    break;

                case "deployment":
                    ColumnList = PackageDeploymentSearchColumns;
                    break;
                case "archival":
                    ColumnList = DataArchivalSearchColumns;
                    break;
            }
            return ColumnList;
        }

        /// <summary>
        ///  This property is return the site url
        /// </summary>
        public static int DefaultPageNumber
        {
            get
            {
                return 1;
            }

        }
        /// <summary>
        ///  This property is return the site url
        /// </summary>
        public static int DefaultPageSize
        {
            get
            {
                return 10;
            }

        }

        /// <summary>
        ///  This property is return the site url
        /// </summary>
        public static string DefaultSortDir
        {
            get
            {
                return "ASC";
            }

        }

        #endregion


    }
}

