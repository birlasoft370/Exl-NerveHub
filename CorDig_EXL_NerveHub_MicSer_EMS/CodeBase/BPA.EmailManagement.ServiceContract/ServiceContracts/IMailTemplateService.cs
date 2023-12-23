using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.ServiceContract.ExternalRef.FaultContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.EmailManagement.ServiceContract.ServiceContracts
{
    [ServiceContract(Name = "MailTemplateContract")]
    public interface IMailTemplateService : IEmailGenericService<BEMailTemplate>, IDisposable
    {
        [OperationContract(Name = "GetMailTemplateAll")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailTemplate> GetMailTemplateAll(bool IsActive, BETenant oTenant, bool isAutoReply);

        [OperationContract(Name = "GetMailTemplateList")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailTemplate> GetMailTemplateList(int userID, int campaignId, BETenant oTenant);

        [OperationContract(Name = "GetMailTemplateDataList")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailTemplate> GetMailTemplateDataList(int iEmailTemplateID, int UserId, BETenant oTenant);
    }
}
