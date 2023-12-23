using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.ServiceContract.ExternalRef.FaultContracts;
using System.Collections;
using System.ServiceModel;

namespace BPA.EmailManagement.ServiceContract.ServiceContracts
{
    [ServiceContract(Name = "MailContract")]
    public interface IMailService : IDisposable
    {
        /// <summary>
        /// Get mailbox folder list
        /// </summary>
        /// <param name="oMailConfig">business entity BEMailConfiguration</param>
        /// <param name="oTenant">business entity BETenant</param>
        /// <returns>list of folder in mailbox</returns>
        /// 
        [OperationContract(Name = "GetMailFolderList")]
        [FaultContract(typeof(ServiceFault))]
        Hashtable GetMailFolderList(BEMailConfiguration oMailConfig, BETenant oTenant);

        [OperationContract(Name = "GetFolderList")]
        [FaultContract(typeof(ServiceFault))]
        Hashtable GetFolderList(BEMailConfiguration oMailConfig, string folderID, string rootFolderName, BETenant oTenant);
        // string test();

    }
}
