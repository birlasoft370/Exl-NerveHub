using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace BPA.AppConfig.BusinessLayer.ExternalRef.WorkAllocation
{
   // [ExceptionShielding("WCF Exception Shielding")]
    // [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BLWorkObject : IWorkObjectService, IDisposable//, IDataOperation<BEWorkObject>
    {
        public BLWorkObject()
        { }
        public void Dispose()
        { }

        public int CheckUserIsSuperOrFunctionalAdmin(int UserId, BETenant oTenant)
        {
            using (DLWorkObject objWorkObjApproval = new DLWorkObject(oTenant))
            {
                return objWorkObjApproval.CheckUserIsSuperOrFunctionalAdmin(UserId);
            }
        }
    }
}
