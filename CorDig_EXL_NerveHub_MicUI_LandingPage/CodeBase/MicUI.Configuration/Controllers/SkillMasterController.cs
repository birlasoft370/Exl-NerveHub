using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Administration.SkillMaster;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{

    public class SkillMasterController : BaseController
    {
        private readonly ISkillService _repositorySkill;
        public SkillMasterController(ISkillService skillService)
        {
            _repositorySkill = skillService;
        }
        public IActionResult Index()
        {
            SkillMasterViewModel oSkillViewModel = new();
            if (TempData["iSkillID"] != null)
            {
                oSkillViewModel = GetModel(_repositorySkill.GetSkillById(int.Parse(TempData["iSkillID"].ToString())));
            }
            return View(oSkillViewModel);
        }
        public ActionResult SearchView()
        {
            return View(new SkillMasterViewModel());
        }
        [HttpPost]
        public ActionResult Index(SkillMasterViewModel SkillModel)
        {

            if (SkillModel.SkillID == 0)
            {
                string Output = _repositorySkill.InsertData(GetSkillInfo(SkillModel));
                if (string.IsNullOrEmpty(Output))
                {
                    ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_SkillMaster.display_SaveMsg.ToString();
                    ModelState.Clear();
                    SkillModel = new SkillMasterViewModel();
                }
                else
                {
                    ViewData["Message"] = Output;
                }
            }
            else
            {
                string Output = _repositorySkill.UpdateData(GetSkillInfo(SkillModel));
                if (string.IsNullOrEmpty(Output))
                {
                    ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_SkillMaster.display_UpdateMsg.ToString();
                    ModelState.Clear();
                    SkillModel = new SkillMasterViewModel();
                }
                else
                {
                    ViewData["Message"] = Output;
                }
            }
            return View("Index", SkillModel);
        }
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public JsonResult EditingCustom_Edit(string iSkillID)
        {
            TempData["iSkillID"] = iSkillID;
            TempData.Keep("iSkillID");
            return Json("OK");
        }

        private SkillMasterViewModel GetModel(SkillMasterModel info)
        {
            SkillMasterViewModel SBUObj = new()
            {
                SkillID = info.SkillID,
                SkillName = info.SkillName,
                SkillDescription = info.SkillDescription,
                IsDisable = info.Disabled
            };

            return SBUObj;
        }
        [HttpPost]
        public ActionResult SearchView(SkillMasterViewModel skillMaster)
        {
            IList<SkillMasterModel> skillMasterList = _repositorySkill.GetSkillListByName(skillMaster.SearchSkillName);
            
            
            foreach (var item in skillMasterList)
            {
                BESkillInfo skillinfo = new BESkillInfo();
                skillinfo.iSkillID = item.SkillID;
                skillinfo.sSkillName = item.SkillName;
                skillinfo.sSkillDescription = item.SkillDescription;
                skillinfo.bDisabled = item.Disabled;
                skillMaster.SkillList.Add(skillinfo);
            }

            if (skillMaster.SkillList.Count == 0)
            {
                ViewData["Message"] = @BPA.GlobalResources.UI.Resources_common.dispNoRecordFound;
            }
            return View("SearchView", skillMaster);
        }
        private SkillMasterModel GetSkillInfo(SkillMasterViewModel SkillMaster)
        {
            SkillMasterModel skillInfo = new SkillMasterModel
            {
                SkillName = SkillMaster.SkillName,
                SkillDescription = SkillMaster.SkillDescription,
                SkillID = SkillMaster.SkillID,
                Disabled = SkillMaster.IsDisable
            };
            return skillInfo;
        }
    }
}
