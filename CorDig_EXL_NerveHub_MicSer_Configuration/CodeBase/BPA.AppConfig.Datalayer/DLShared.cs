using BPA.AppConfig.BusinessEntity.Application;
using BPA.Utility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.Datalayer
{
    public sealed class DL_Shared
    {

        static DatabaseProviderFactory dataFactory = new DatabaseProviderFactory();

        private DL_Shared()
        { }

        public const string Connection = "DBCDSConnectionString";
        public const string Connection_DC = "DBCDSConnectionString";
        public static Database dbFactory(string connectionString)
        {
            return dataFactory.Create(connectionString);
        }
        public static Database dbFactory(BETenant oTenant)
        {
            return new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(EncryptDecrypt.Decrypt(oTenant.DatabaseConnectionString));
        }

        public static Database dbFactoryHBM(BETenant oTenant)
        {
            return new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(EncryptDecrypt.Decrypt(oTenant.HBMReportDatabaseConnectionString));
        }
        public static Database dbFactoryDataUtility(BETenant oTenant)
        {
            return new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(EncryptDecrypt.Decrypt(oTenant.DataUtilityDatabaseConnectionString));
        }
        public static Database dbFactoryADS(BETenant oTenant)
        {
            return new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(EncryptDecrypt.Decrypt(oTenant.ADSDatabaseConnectionString));
        }

        #region Remove Single Quote from String with double Single Quote
        /// <summary>
		/// Remove single quote from the string before inserting 
		/// it to database.
		/// </summary>
		/// <param name="StringValue">String </param>
		/// <returns></returns>
		public static string RemoveSingleQuote(string StringValue)
        {
            return StringValue.Replace("'", "''");
        }
        #endregion

        #region Convert arraylist of int to int[]
        public static Int32[] AddListToIntarray(ArrayList ArrayLst)
        {
            Int32[] i = new Int32[ArrayLst.Count];
            for (int j = 0; j < ArrayLst.Count; j++)
            {
                i[j] = Convert.ToInt32(ArrayLst[j]);
            }
            return i;
        }
        #endregion

        #region Convert arraylist of string to string[]
        public static string[] AddListToStringArray(ArrayList ArrayLst)
        {
            string[] str = new string[ArrayLst.Count];
            for (int j = 0; j < ArrayLst.Count; j++)
            {
                str[j] = Convert.ToString(ArrayLst[j]);
            }
            return str;
        }
        #endregion

    }
}
