using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.EmailManagement.DataLayer.ExternalRef.WorkAllocation;
using BPA.EmailManagement.ServiceContract.ServiceContracts.WorkAllocation;

namespace BPA.EmailManagement.BusinessLayer.ExternalRef.WorkAllocation
{
    //[ExceptionShielding("WCF Exception Shielding")]
    // [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BLStore : IStoreService, IDisposable //, IDataOperation<BEStoreInfo>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BLStore"/> class.
        /// </summary>
        public BLStore()
        { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }

        #endregion Constructor

        public List<BEWorkObjectTAB> GetTabMasterList(int iStoreID, BETenant oTenant)
        {
            using (DLStore oStore = new DLStore(oTenant))
            {
                return oStore.GetTabMasterList(iStoreID);
            }
        }


    }
}
