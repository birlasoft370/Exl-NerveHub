using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;
using BPA.AppConfig.Datalayer.ExternalRef.Configuration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration;

namespace BPA.AppConfig.BusinessLayer.ExternalRef.Configuration
{
    //[ExceptionShielding("WCF Exception Shielding")]
    public class BLSkill : ISkillService, IDisposable
    {
        public void Dispose()
        { }

        public BLSkill()
        { }

        public IList<BESkillInfo> GetSkillList(int iSkillID, BETenant oTenant)
        {
            using (DLSkill oSkill = new DLSkill(oTenant))
            {
                return oSkill.GetSkillList(iSkillID);
            }
        }

        public string InsertData(BESkillInfo oSkill, int iFormID, BETenant oTenant)
        {
            /*
            if (oSkill.sSkillName == string.Empty || oSkill.sSkillName == "")
            {
                throw new BPA.ExceptionHandler.ExceptionType.BusinessLogicCustomException("Skill " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(iFormID, oSkill.iCreatedBy, PermissionSet.ADD))
            {
                throw new BPA.ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }*/
            using (DLSkill objSkill = new DLSkill(oTenant))
            {
                return objSkill.InsertData(oSkill);
            }
        }

        public string UpdateData(BESkillInfo oSkill, int iFormID, BETenant oTenant)
        {
            /*
            if (oSkill.sSkillName == string.Empty || oSkill.sSkillName == "")
            {
                throw new BPA.ExceptionHandler.ExceptionType.BusinessLogicCustomException("Skill " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(iFormID, oSkill.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new BPA.ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }*/
            using (DLSkill objSkill = new DLSkill(oTenant))
            {
                return objSkill.UpdateData(oSkill);
            }
        }

        public IList<BESkillInfo> GetSkillListByName(string sSkillName, bool bGetAll, BETenant oTenant)
        {
            using (DLSkill objSkill = new DLSkill(oTenant))
            {
                return objSkill.GetSkillListByName(sSkillName, bGetAll);
            }
        }

        public List<BESkillInfo> GetActiveSkillList(BETenant oTenant)
        {
            using (DLSkill objSkill = new DLSkill(oTenant))
            {
                return objSkill.GetActiveSkillList();
            }
        }

        public List<BESkillInfo> GetCampaignSkillList(int iCampaignId, BETenant oTenant)
        {
            using (DLSkill objSkill = new DLSkill(oTenant))
            {
                return objSkill.GetCampaignSkillList(iCampaignId);
            }
        }

        public void UpdateCampSkillMap(int CampaignId, string strSkillID, int CreatedBy, BETenant oTenant)
        {
            using (DLSkill objSkill = new DLSkill(oTenant))
            {
                objSkill.UpdateCampSkillMap(CampaignId, strSkillID, CreatedBy);
            }
        }

        public List<BEUserSkillMap> GetMapSkill(int iCampaignId, BETenant oTenant)
        {
            using (DLSkill objSkill = new DLSkill(oTenant))
            {
                return objSkill.GetMapSkill(iCampaignId);
            }
        }
        public void InsertUserSkillMap(string strXml, BETenant oTenant)
        {
            using (DLSkill objSkill = new DLSkill(oTenant))
            {
                objSkill.InsertUserSkillMap(strXml);
            }
        }
    }
}
