using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.DataLayer;
using BPA.EmailManagement.ServiceContract.ServiceContracts;

namespace BPA.EmailManagement.BusinessLayer
{
    //[ExceptionShielding("WCF Exception Shielding")]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BLMailTemplate : IMailTemplateService
    {
        public IList<BEMailTemplate> GetMailTemplateAll(bool IsActive, BETenant oTenant, bool isAutoReply)
        {
            using (DLMailTemplate oDLMailTemplate = new DLMailTemplate(oTenant))
            {
                return oDLMailTemplate.GetMailTemplateAll(IsActive, isAutoReply);
            }
        }

        public void InsertData(BEMailTemplate oEmailManagement, int iFormID, BETenant oTenant)
        {
            using (DLMailTemplate oDLMailTemplate = new DLMailTemplate(oTenant))
            {
                oDLMailTemplate.InsertData(oEmailManagement);
            }
        }

        public void UpdateData(BEMailTemplate oEmailManagement, int iFormID, BETenant oTenant)
        {
            using (DLMailTemplate oDLMailTemplate = new DLMailTemplate(oTenant))
            {
                oDLMailTemplate.UpdateData(oEmailManagement);
            }
        }

        public void DeleteData(BEMailTemplate oEmailManagement, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }

        public IList<BEMailTemplate> GetMailTemplateList(int userID, int campaignId, BETenant oTenant)
        {
            using (DLMailTemplate oDLMailTemplate = new DLMailTemplate(oTenant))
            {
                return oDLMailTemplate.GetMailTemplateList(userID, campaignId);
            }
        }

        public IList<BEMailTemplate> GetMailTemplateDataList(int iEmailTemplateID, int UserId, BETenant oTenant)
        {
            using (DLMailTemplate oDLMailTemplate = new DLMailTemplate(oTenant))
            {
                return oDLMailTemplate.GetMailTemplateDataList(UserId, iEmailTemplateID);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BLMailTemplate() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        //User  Select and save Template code
        //public void DeleteUserTemplate(BEMailTemplate oEmailManagement, int UserID, BETenant oTenant)
        //{
        //    using (DLMailTemplate oDLMailTemplate = new DLMailTemplate(oTenant))
        //    {
        //        oDLMailTemplate.DeleteUserTemplate(oEmailManagement, UserID);
        //    }
        //}

        //public void InsertUserTemplate(BEMailTemplate oEmailManagement, int UserID, BETenant oTenant)
        //{
        //    using (DLMailTemplate oDLMailTemplate = new DLMailTemplate(oTenant))
        //    {
        //        oDLMailTemplate.InsertUserTemplate(oEmailManagement, UserID);
        //    }
        //}

        //public IList<BEMailTemplate> GetUserTemplate(int userID, int campaignId, BETenant oTenant)
        //{
        //    using (DLMailTemplate oDLMailTemplate = new DLMailTemplate(oTenant))
        //    {
        //        return oDLMailTemplate.GetUserTemplate(userID, campaignId);
        //    }
        //}
        #endregion
    }
}
