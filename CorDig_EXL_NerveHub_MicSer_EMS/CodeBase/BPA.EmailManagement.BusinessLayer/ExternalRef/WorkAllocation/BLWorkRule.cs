using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.EmailManagement.DataLayer.ExternalRef.WorkAllocation;
using BPA.EmailManagement.ServiceContract.ServiceContracts.WorkAllocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer.ExternalRef.WorkAllocation
{
    public class BLWorkRule : IWorkRuleService, IDisposable//, IDataOperation<BEWorkRule>
    {
        public void Dispose()
        { }

        public List<BEWorkRule> GetEventRule(int StoreId, BETenant oTenant)
        {
            using (DLWorkRule objRule = new DLWorkRule(oTenant))
            {
                return objRule.GetEventRule(StoreId);
            }
        }
    }
}
