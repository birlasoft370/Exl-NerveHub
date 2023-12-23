using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace BPA.AppConfig.Datalayer.Config
{
    public class DLUserPreference : IDisposable
    {
        private BETenant _oTenant = null;
        public void Dispose()
        { }
        public DLUserPreference(BETenant oTenant)
        {
            _oTenant = oTenant;
        }

        // SQL Procesdure and Query Variables
        private const string USP_INSERTUPDATEUSERPEREFERNCE = @"[Config].[Usp_InsertUpdateUserPerefernce]";
        private const string USP_GETUSERPREFERNCEDETAIL = @"[Config].[Usp_GetUserPrefernceDetail]";

        //Parameter Variables
        private const string PARAM_USERID = "@UserID";
        private const string PARAM_TIMEZONEID = "@TimeZoneID";
        private const string PARAM_LANGUAGE = "@Language";
        private const string PARAM_DISABLE = "@Disable";
        private const string PARAM_STARTDATE = "@startDate";
        private const string PARAM_ENDDATE = "@EndDate";

        public BEUserPreference GetUserPerefernceDetail(int UserId)
        {
            BEUserPreference objBEUserPreference = new BEUserPreference();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(USP_GETUSERPREFERNCEDETAIL);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    objBEUserPreference = new BEUserPreference(Convert.ToInt32(rdr["UserID"]), Convert.ToInt32(rdr["TimeZoneID"]), Convert.ToString(rdr["Language"]), Convert.ToBoolean(rdr["Disabled"]));
                }
            }
            return objBEUserPreference;
        }

        public int SaveUpdateUserPreference(BusinessEntity.Config.BEUserPreference objBEUserPreference)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(USP_INSERTUPDATEUSERPEREFERNCE);
                db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, objBEUserPreference.iUserID);
                db.AddInParameter(dbCommand, PARAM_TIMEZONEID, DbType.Int32, objBEUserPreference.oTimezone.iTimeZoneID);
                db.AddInParameter(dbCommand, PARAM_LANGUAGE, DbType.String, objBEUserPreference.sLanguage);
                db.AddInParameter(dbCommand, PARAM_DISABLE, DbType.String, objBEUserPreference.bDisabled);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbCommand, trans);
                            trans.Commit(); //Commit Transaction
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();//Transaction RollBack
                            throw ex;
                        }
                    }
                    conn.Close();
                }
                return 1;
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
