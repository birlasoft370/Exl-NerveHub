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

namespace BPA.EmailManagement.DataLayer
{
    public class DLMailScheduler : IDisposable
    {
        public DLMailScheduler(BETenant oTenant) { _oTenant = oTenant; }

        private BETenant _oTenant = null;

        public void Dispose()
        {

        }

        private const string SP_GETWORKIDDETAILS = @"[EMS].[Usp_GetWorkTopID]";
        private const string SP_GETWORKID_AUTOMAIL = @"[EMS].[Usp_GetWorkID_AutoMail]";
        private const string SP_UPDATEWOKRDETAILS = @"[EMS].[Usp_UpdateWork]";

        private const string PARAM_WORKIDLIST = "@WorkIDList";
        private const string PARAM_TABLENAME = "@TableName";
        private const string PARAM_COLUMNLIST = "@ColumnList";

        public DataTable GetWorkDetails(string WorkList, string TableName, string sColnameList)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GETWORKIDDETAILS);
            db.AddInParameter(dbCommand, PARAM_WORKIDLIST, DbType.String, WorkList);
            db.AddInParameter(dbCommand, PARAM_TABLENAME, DbType.String, TableName);
            db.AddInParameter(dbCommand, PARAM_COLUMNLIST, DbType.String, sColnameList);
            db.LoadDataSet(dbCommand, ds, "WorkDetails");
            return ds.Tables[0];

        }

        public DataTable GetWorkAutoMailID(string WorkList, string TableName)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GETWORKID_AUTOMAIL);
            db.AddInParameter(dbCommand, PARAM_WORKIDLIST, DbType.String, WorkList);
            db.AddInParameter(dbCommand, PARAM_TABLENAME, DbType.String, TableName);
            db.LoadDataSet(dbCommand, ds, "AutoMailDetails");
            return ds.Tables[0];

        }

        public void UpdateWorkTable(string WorkList, string TableName)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_UPDATEWOKRDETAILS);
            db.AddInParameter(dbCommand, PARAM_WORKIDLIST, DbType.String, WorkList);
            db.AddInParameter(dbCommand, PARAM_TABLENAME, DbType.String, TableName);
            db.ExecuteNonQuery(dbCommand);
        }
    }
}
