using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.ServiceContract.ExternalRef.FaultContracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.EmailManagement.ServiceContract.ServiceContracts
{
    [ServiceContract(Name = "MailContractGraph")]
    public interface IMailServiceGraph : IDisposable
    {
        /// <summary>
        /// Get mailbox folder list
        /// </summary>
        /// <param name="oMailConfig">business entity BEMailConfiguration</param>
        /// <param name="oTenant">business entity BETenant</param>
        /// <returns>list of folder in mailbox</returns>
        [OperationContract(Name = "GetMailFolderListGraph")]
        [FaultContract(typeof(ServiceFault))]
        Task<Hashtable> GetMailFolderListGraph(BEMailConfiguration oMailConfig, BETenant oTenant);

        Task<Hashtable> GetFolderListGraph(BEMailConfiguration oMailConfig, string folderID, string rootFolderName, BETenant oTenant);
    }
}
