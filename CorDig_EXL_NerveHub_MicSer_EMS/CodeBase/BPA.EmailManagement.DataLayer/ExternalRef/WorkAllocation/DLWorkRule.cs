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
    public class DLWorkRule : IDisposable
    {
        private BETenant _oTenant = null;

        public DLWorkRule(BETenant oTenant)
        { _oTenant = oTenant; }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }

        private const string SP_EVENTRULES = @"WM.USP_GetEventRules"; //Usp_CDS_GetEventRules

        private const string PARAM_DSTOREID = "@DStoreId";
        public List<BEWorkRule> GetEventRule(int StoreId)
        {
            List<BEWorkRule> lRule = new List<BEWorkRule>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetStoredProcCommand(SP_EVENTRULES);
            db.AddInParameter(dbCommand, PARAM_DSTOREID, DbType.Int32, StoreId);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEWorkRule Rule = new BEWorkRule(Convert.ToInt32(rdr["WorkRuleId"]), Convert.ToInt32(rdr["TDSObjId1"]), rdr["TDSObjPrefix1"].ToString(), Convert.ToInt32(rdr["TDSObjId2"]), rdr["TDSObjPrefix2"].ToString(), Convert.ToInt32(rdr["TDSObjId3"]), rdr["TDSObjPrefix3"].ToString(), Convert.ToInt32(rdr["ActionId"]), Convert.ToInt32(rdr["ActionOn"]), rdr["ActionOnValue"].ToString(), Convert.ToInt32(rdr["RuleTypeId"]), Convert.ToInt32(rdr["EventObjectId"]), rdr["EventObjectPrefix"].ToString(), rdr["DefaultValue"].ToString(), Convert.ToBoolean(rdr["bFormula"]), rdr["ConditionExpression"].ToString());

                    lRule.Add(Rule);
                    Rule = null;
                }
            }
            return lRule;
        }
    }
}
