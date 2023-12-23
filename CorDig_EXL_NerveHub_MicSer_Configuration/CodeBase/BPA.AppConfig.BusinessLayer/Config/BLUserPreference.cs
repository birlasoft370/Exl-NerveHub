using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.Datalayer.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;

namespace BPA.AppConfig.BusinessLayer.Config
{
    public class BLUserPreference : IUserPreferenceService, IDisposable
    {
        #region Dispose
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BLTimeZone"/> class.
        /// </summary>
        public BLUserPreference()
        { }
        #endregion

        public int SaveUpdateUserPreference(BEUserPreference objBEUserPreference, BETenant oTenant)
        {
            using (DLUserPreference objDLUserPreference = new DLUserPreference(oTenant))
            {
                return objDLUserPreference.SaveUpdateUserPreference(objBEUserPreference);
            }
        }

        public BEUserPreference GetUserPerefernceDetail(int UserId, BETenant bETenant)
        {
            using (DLUserPreference objDLUserPreference = new DLUserPreference(bETenant))
            {
                return objDLUserPreference.GetUserPerefernceDetail(UserId);
            }
        }
    }
}
