using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.SharePointConfiguration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.FaultContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.SharePointConfiguration
{
    public interface ISharePointConfiguration
    {
        [OperationContract(Name = "InsData")]
        [FaultContract(typeof(ServiceFault))]
        void InsertData(BESharePointConfiguration oSharePointConfiguration, int iFormID, BETenant oTenant);

        [OperationContract(Name = "UpdData")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateData(BESharePointConfiguration oBESharePointConfiguration, int iFormID, BETenant oTenant);

        [OperationContract(Name = "GetSharepointSchedulerList")]
        [FaultContract(typeof(ServiceFault))]
        IList<BESharePointConfiguration> GetSharepointSchedulerList(int ShedulerID, BETenant oTenant);

        [OperationContract(Name = "GetSearchSharepointList")]
        [FaultContract(typeof(ServiceFault))]
        IList<BESharePointConfiguration> GetSearchSharepointList(int campaignId, BETenant oTenant);

        [OperationContract(Name = "CheckIfCampaignMappingExist")]
        [FaultContract(typeof(ServiceFault))]
        bool CheckIfCampaignMappingExist(string campaignId, BETenant oTenant);

    }
}
