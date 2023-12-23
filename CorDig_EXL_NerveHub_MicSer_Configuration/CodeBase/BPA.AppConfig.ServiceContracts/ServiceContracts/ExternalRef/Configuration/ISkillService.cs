using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.FaultContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration
{
    [ServiceContract(Name = "SkillServiceContract")]
    public interface ISkillService : IDisposable
    {
        [OperationContract(Name = "GetSkillListByName")]
        [FaultContract(typeof(ServiceFault))]
        IList<BESkillInfo> GetSkillListByName(string sSkillName, bool bGetAll, BETenant oTenant);

        [OperationContract(Name = "GetSkillListByID")]
        [FaultContract(typeof(ServiceFault))]
        IList<BESkillInfo> GetSkillList(int iSkillID, BETenant oTenant);


        [OperationContract(Name = "GetActiveSkillList")]
        [FaultContract(typeof(ServiceFault))]
        List<BESkillInfo> GetActiveSkillList(BETenant oTenant);

        [OperationContract(Name = "GetCampaignSkillList")]
        [FaultContract(typeof(ServiceFault))]
        List<BESkillInfo> GetCampaignSkillList(int iCampaignId, BETenant oTenant);

        [OperationContract(Name = "InsertData")]
        [FaultContract(typeof(ServiceFault))]
        string InsertData(BESkillInfo oSkill, int iFormID, BETenant oTenant);

        [OperationContract(Name = "UpdateData")]
        [FaultContract(typeof(ServiceFault))]
        string UpdateData(BESkillInfo oSkill, int iFormID, BETenant oTenant);

        [OperationContract(Name = "UpdateCampSkillMap")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateCampSkillMap(int iCampaignId, string sSkillId, int iCreatedBy, BETenant oTenant);

        [OperationContract(Name = "GetMapSkill")]
        [FaultContract(typeof(ServiceFault))]
        List<BEUserSkillMap> GetMapSkill(int iCampaignId, BETenant oTenant);

        [OperationContract(Name = "InsertUserSkillMap")]
        [FaultContract(typeof(ServiceFault))]
        void InsertUserSkillMap(string strXml, BETenant oTenant);
    }
}
