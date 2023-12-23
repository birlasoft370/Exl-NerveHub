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
    public class DLBreakCode : IDisposable
    {
        private BETenant _oTenant = null;

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLBreakCode"/> class.
        /// </summary>
        public DLBreakCode(BETenant oTenant)
        { _oTenant = oTenant; }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        private const string SQL_SELECT_BREAKCODE = @"SELECT a.* FROM TM.tblBreakMaster a (NOLOCK) JOIN Config.tblProcessBreakMapping b (NOLOCK) on a.BreakID=b.BreakID JOIN Config.tblCampaignMaster c (NOLOCK) ON b.ProcessID=c.ProcessID WHERE c.CampaignID=@CampaignID AND b.Disabled=0";
        private const string PARAM_CAMPAIGNID = "@CampaignID";

        public List<BEBreakInfo> GetBreakCode(int CampaignID)
        {
            List<BEBreakInfo> lBreak = new List<BEBreakInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbSelectCommand = db.GetSqlStringCommand(SQL_SELECT_BREAKCODE);
            db.AddInParameter(dbSelectCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignID);
            using (IDataReader rdr = db.ExecuteReader(dbSelectCommand))
            {
                while (rdr.Read())
                {
                    BEBreakInfo objBreak = new BEBreakInfo
                    {
                        iBreakID = int.Parse(rdr["BreakID"].ToString()),
                        sBreakName = rdr["BreakName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""),
                        sBreakDescription = rdr["Description"].ToString(),
                    };
                    lBreak.Add(objBreak);
                    objBreak = null;
                }
            }
            return lBreak;
        }
    }
}
