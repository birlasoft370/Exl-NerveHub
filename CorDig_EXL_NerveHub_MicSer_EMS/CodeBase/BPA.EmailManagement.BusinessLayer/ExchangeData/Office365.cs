using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;

namespace BPA.EmailManagement.BusinessLayer.ExchangeData
{

    public class Office365 : AbstractOffice
    {
        public new bool DownloadBulkMail(BEMailConfiguration mailConfig, out int ExceptionValidation, BETenant oTenant)
        {
            bool isDownloadCompleted = false;
            isDownloadCompleted = base.DownloadBulkMail(mailConfig, out ExceptionValidation, oTenant);

            //if (iAuth.Authenticate())
            //{
            //    isDownloadCompleted = base.DownloadBulkMail(mailConfig, oTenant);
            //}
            //else
            //{
            //    new BPA.ExceptionHandler.ExceptionType.BusinessLogicCustomException("Password Missmatch Office 2007");
            //}

            return isDownloadCompleted;
        }
    }
}
