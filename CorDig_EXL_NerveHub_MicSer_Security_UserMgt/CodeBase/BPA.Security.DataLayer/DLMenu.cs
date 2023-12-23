using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.Datalayer;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using BPA.Security.BusinessEntity;

namespace BPA.Security.DataLayer
{
    public class DLMenu : IDisposable
    {
        private BETenant _oTenant = null;
        private const string SQL_SELECTMENU = @"dbo.Usp_CDS_GetMenuItems";
        private const string SP_GETMENU = @"dbo.Usp_CDS_GetMenuItems_RoleWise";

        private const string CONST_ROLEID = "@RoleID";
        private const string CONST_USERID = "@UserID";
        private const string PARAM_Type = "@Type";

        private const string PARAM_LANDINGPAGEAction = @"[Config].[Usp_GetPageLanding]";


        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLMenu"/> class.
        /// </summary>
        public DLMenu(BETenant oTenant)
        { _oTenant = oTenant; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        /// <summary>
        /// Gets the menu data.
        /// </summary>
        /// <param name="UserID">The user ID.</param>
        /// <param name="isMossApplicationMenu">if set to <c>true</c> [is moss application menu].</param>
        /// <returns></returns>
        public DataSet GetMenuData(int UserID, bool isMossApplicationMenu)
        {

            DataSet MenuItems = new DataSet();
            string dTables = "MenuItems";
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SELECTMENU);
            db.AddInParameter(dbCommand, CONST_USERID, DbType.Int32, UserID);
            db.LoadDataSet(dbCommand, MenuItems, dTables);
            return MenuItems;

        }

        /// <summary>
        /// Gets the smart menu data.
        /// </summary>
        /// <param name="UserID">The user ID.</param>
        /// <param name="isMossApplicationMenu">if set to <c>true</c> [is moss application menu].</param>
        /// <returns></returns>
        public DataSet GetSmartMenuData(int UserID, bool isMossApplicationMenu)
        {

            DataSet MenuItems = new DataSet();
            string dTables = "MenuItems";
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SELECTMENU);
            db.AddInParameter(dbCommand, CONST_USERID, DbType.Int32, UserID);
            db.AddInParameter(dbCommand, PARAM_Type, DbType.Int32, 2);
            db.LoadDataSet(dbCommand, MenuItems, dTables);
            return MenuItems;

        }

        /// <summary>
        /// Gets the role wise menu.
        /// </summary>
        /// <param name="UserID">The user ID.</param>
        /// <param name="isMossApplicationMenu">if set to <c>true</c> [is moss application menu].</param>
        /// <returns></returns>
        public List<BEMenuItems> GetRoleWiseMenu(int UserID, bool isMossApplicationMenu)
        {
            List<BEMenuItems> lMenuItems = new List<BEMenuItems>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GETMENU);
            db.AddInParameter(dbCommand, CONST_ROLEID, DbType.Int32, UserID);
            db.AddInParameter(dbCommand, PARAM_Type, DbType.Int32, isMossApplicationMenu);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEMenuItems oMenu = new BEMenuItems
                    {
                        iFormID = int.Parse(rdr["FormID"].ToString()),
                        MenuOrder = int.Parse(rdr["Displayorder"].ToString()),
                        iFormType = int.Parse(rdr["FormType"].ToString()),
                        NodeID = int.Parse(rdr["MenuId"].ToString()),
                        NodeName = rdr["Text"].ToString(),
                        ParentID = int.Parse(rdr["ParentID"].ToString()),
                        URL = rdr["URL"].ToString(),
                        Flag = rdr["Flag"].ToString(),
                        bHasPermission = Convert.ToBoolean(rdr["HasPermission"]),
                        sModuleName = rdr["ModuleName"].ToString(),
                        sController = rdr["Controller"].ToString(),
                        sAction = rdr["Action"].ToString(),
                        sIconClass = rdr["IconClass"].ToString()
                    };
                    lMenuItems.Add(oMenu);
                    oMenu = null;
                }
            }
            return lMenuItems;
        }


        public DataSet GetLandingData()
        {

            DataSet MenuItems = new DataSet();
            string dTables = "MenuItems";
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(PARAM_LANDINGPAGEAction);
            db.LoadDataSet(dbCommand, MenuItems, dTables);
            return MenuItems;

        }
    }
}