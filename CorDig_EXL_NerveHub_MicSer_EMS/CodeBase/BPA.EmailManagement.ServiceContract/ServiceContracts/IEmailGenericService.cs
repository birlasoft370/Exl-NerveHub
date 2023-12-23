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
    [ServiceContract(Name = "EmailManagementContract")]
    public interface IEmailGenericService<U>
    {
        [OperationContract(Name = "InsData")]
        [FaultContract(typeof(ServiceFault))]
        void InsertData(U oEmailManagement, int iFormID, BETenant oTenant);



        [OperationContract(Name = "UpdData")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateData(U oEmailManagement, int iFormID, BETenant oTenant);

        [OperationContract(Name = "DelData")]
        [FaultContract(typeof(ServiceFault))]
        void DeleteData(U oEmailManagement, int iFormID, BETenant oTenant);

    }
}
