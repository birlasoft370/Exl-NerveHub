using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessLayer.Dominos;
using BPA.EmailManagement.BusinessLayer.ExchangeData;
using BPA.EmailManagement.ServiceContract.ServiceContracts;
using System.Collections;

namespace BPA.EmailManagement.BusinessLayer
{
    // [ExceptionShielding("WCF Exception Shielding")]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BLEmailFactory : IMailService
    {
        IMailService IMService = null;
        public void Dispose()
        {
            IMService = null;
        }

        private IMailService getclass(EmailServerType Exchangeserverversion)
        {

            switch (Exchangeserverversion)
            {
                case BusinessEntity.EmailServerType.Dominos:
                    IMService = new LotusNotes();
                    break;

                case BusinessEntity.EmailServerType.Exchange2007SP1:
                    IMService = new Office2007();
                    break;

                case BusinessEntity.EmailServerType.Exchange2010SP1:
                    IMService = new Office2010();
                    break;

                case BusinessEntity.EmailServerType.Exchange2010SP2:
                    IMService = new Office2010();
                    break;

                case BusinessEntity.EmailServerType.Office365:
                    IMService = new Office365();
                    break;
                case BusinessEntity.EmailServerType.MicrosoftGraph:
                    //    //IMService = new AbstractOffice();
                    break;

                default:
                    break;
            }
            return IMService;
        }
        public Hashtable GetMailFolderList(BEMailConfiguration oMailConfig, BETenant oTenant)
        {
            /*
            //Lotus
            var obj = getclass(oMailConfig.iMailServerTypeID);
            // var obj1 = new LotusNotes();
            //var res = obj1.GetMailFolderList(oMailConfig, oTenant);
            var resdd = obj.test();

            //Office2007
            oMailConfig.iMailServerTypeID = (EmailServerType)int.Parse("1");
            var objOffice2007 = getclass(oMailConfig.iMailServerTypeID);
            var resOffice2007 = objOffice2007.test();

            //Office2010
            oMailConfig.iMailServerTypeID = (EmailServerType)int.Parse("2");
            var objOffice2010= getclass(oMailConfig.iMailServerTypeID);
            var resOffice2010 = objOffice2010.test();

            //Office2010
            oMailConfig.iMailServerTypeID = (EmailServerType)int.Parse("3");
            var objOffice2010V2 = getclass(oMailConfig.iMailServerTypeID);
            var resOffice2010V2 = objOffice2010V2.test();

            //Office365
            oMailConfig.iMailServerTypeID = (EmailServerType)int.Parse("4");
            var objOffice365 = getclass(oMailConfig.iMailServerTypeID);
            var resOffice365 = objOffice365.test();

            var result = objOffice365.GetMailFolderList(oMailConfig, oTenant);             
            return result;   */
            return getclass(oMailConfig.iMailServerTypeID).GetMailFolderList(oMailConfig, oTenant);
        }

        public System.Collections.Hashtable GetFolderList(BusinessEntity.BEMailConfiguration oMailConfig, string folderID, string rootFolderName, BETenant oTenant)
        {
            return getclass(oMailConfig.iMailServerTypeID).GetFolderList(oMailConfig, folderID, rootFolderName, oTenant);
        }
    }
}
