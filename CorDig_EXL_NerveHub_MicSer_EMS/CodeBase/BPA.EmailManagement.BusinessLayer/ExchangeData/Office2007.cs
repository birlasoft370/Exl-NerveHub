using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;

namespace BPA.EmailManagement.BusinessLayer.ExchangeData
{
    public class Office2007 : AbstractOffice
    {

        public new bool DownloadBulkMail(BEMailConfiguration mailConfig, out int ExceptionValidation, BETenant oTenant)
        {
            bool isDownloadCompleted = false;
            //using (IAuthenticate iAuth = new LDAPUser(mailConfig.sUserID, mailConfig.sPassword,""))
            //{
            //    if (iAuth.Authenticate())
            //    {
            isDownloadCompleted = base.DownloadBulkMail(mailConfig, out ExceptionValidation, oTenant);
            //}
            //else
            //{
            //    new BPA.ExceptionHandler.ExceptionType.BusinessLogicCustomException("Password Missmatch Office 2007");
            //}
            //}
            return isDownloadCompleted;
        }
    }
}
