using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.WorkAllocation;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.DataLayer.ExternalRef.WorkAllocation
{
    public class DLStore : IDisposable
    {
        private BETenant _oTenant = null;

        #region Constructor and Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="DLStore"/> class.
        /// </summary>
        public DLStore(BETenant oTenant)
        { _oTenant = oTenant; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        private const string SQL_SELECT_TABMASTER = @"Select DSObjTABID,DSObjID,TabObjectID,TabObjectValue,DSOrderID,Disabled from WM.tblDStoreObjTAB where DSObjID=@StoreID   order by DSOrderID ASC";
        private const string PARAM_STOREID = "@StoreID";

        public List<BEWorkObjectTAB> GetTabMasterList(int StoreID)
        {
            List<BEWorkObjectTAB> lSttabmaster = new List<BEWorkObjectTAB>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_TABMASTER);
            db.AddInParameter(dbCommand, PARAM_STOREID, DbType.Int32, StoreID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BEWorkObjectTAB item = new BEWorkObjectTAB
                    {
                        iObjectChoiceID = Convert.ToInt32(rdr["DSObjTABID"]),
                        iObjID = Convert.ToInt32(rdr["DSObjID"]),
                        sChoiceValue = Convert.ToString(rdr["TabObjectID"]),
                        sTABNameValue = Convert.ToString(rdr["TabObjectValue"]),
                        bDisabled = Convert.ToBoolean(rdr["Disabled"]),
                        iOrder = Convert.ToInt32(rdr["DSOrderID"])

                    };
                    lSttabmaster.Add(item);
                    item = null;
                }
            }
            return lSttabmaster;
        }
    }
}
