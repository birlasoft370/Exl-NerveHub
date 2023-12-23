/* Copyright © 2012 ExlService (I) Pvt. Ltd.
 * project Name                 :   
 * Class Name                   :   
 * Namespace                    :   
 * Purpose                      :
 * Description                  :
 * Dependency                   :   
 * Related Table                :
 * Related Class                :
 * Related StoreProcdure        :
 * Author                       :   
 * Created on                   :   
 * Reviewed on                  :          
 * Reviewed by                  :
 * Tested on                    :
 * Tested by                    :
 * Modification history         :
 * modify1 on                   :
 * modify1 By                   :
 * Overall effect               :
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.ComponentModel;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using Microsoft.Practices.EnterpriseLibrary.Data;
using BPA.Security.DataLayer.ExternalRef.Utitlity;

namespace BPA.Security.Datalayer
{
	/// <summary>
	/// Summary description for ConnectionInfo.
	/// </summary>
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
