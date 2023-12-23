using BPA.GlobalResources;
using BPA.GlobalResources.UI.WorkManagement;
using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Module.Administration;
using MicUI.WorkManagement.Services.ServiceModel;
using Telerik.SvgIcons;

namespace MicUI.WorkManagement.Controllers
{
    public class TerminationCodeController : BaseController
    {
        private readonly ITerminationCodeService _repositoryTerminationCode;
        public TerminationCodeController
         (
           ITerminationCodeService repositoryTerminationCode
         )
        {
            this._repositoryTerminationCode = repositoryTerminationCode;
        }
        public IActionResult Index()
        {
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.TerminationCode)))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
          
            TerminationCodeViewModel objTerminationCode = new TerminationCodeViewModel();
            if (TempData["TerminationCodeID"] != null)
            {
                var data = (_repositoryTerminationCode.GetTerminationCodeById(int.Parse(TempData.Peek("TerminationCodeID").ToString())));
                objTerminationCode.mTerminationCodeID = data.TerminationCodeID;
                objTerminationCode.mTerminationCodeName = data.TerminationCodeName.ToString();
                objTerminationCode.mDescription = data.Description.ToString();
                objTerminationCode.mIsdisable = data.Disabled;
            }
            return View("Index", objTerminationCode);
        }
        [HttpPost]
        public JsonResult SetTerminationCodeID(string id)
        {
            int returnValue = 0;
            if (id != null)
            {
                TempData["TerminationCodeID"] = id;
                TempData.Keep("TerminationCodeID");
                returnValue = 1;
            }
            return Json(returnValue);
        }
        [HttpPost]
        public ActionResult Index(TerminationCodeViewModel objTerminationCode)
        {

            string Flag = string.Empty;

            //string.IsNullOrEmpty(objCalendar.mCalenderID.ToString()) &&
            //
            if (objTerminationCode.mTerminationCodeID != 0)
            {
                if (string.IsNullOrEmpty(objTerminationCode.mTerminationCodeName))
                {
                    ModelState.AddModelError("mTerminationCodeName", @BPA.GlobalResources.UI.WorkManagement.Resources_TerminationCode.required_TermCodeName);
                }
                else
                {
                    Update(objTerminationCode);
                    ModelState.Clear();
                    ViewData["Message"] = Resources_TerminationCode.display_Update_Message;
                    objTerminationCode = new TerminationCodeViewModel();
                }

            }
            else
            {
                if (string.IsNullOrEmpty(objTerminationCode.mTerminationCodeName))
                {
                    ModelState.AddModelError("mTerminationCodeName", @BPA.GlobalResources.UI.WorkManagement.Resources_TerminationCode.required_TermCodeName);
                }
                else
                {


                    Flag = _repositoryTerminationCode.AddTerminationCode(CatchRecord(objTerminationCode));
                    if (Flag == "" || Flag == string.Empty)
                    {
                        ModelState.Clear();
                        ViewData["Message"] = Resources_TerminationCode.display_Save_Message;
                        objTerminationCode = new TerminationCodeViewModel();
                    }
                    else
                    {
                        ViewData["Message"] = Flag;
                    }


                }

            }
            return View("Index", objTerminationCode);
        }
        public void Update(TerminationCodeViewModel objTerminationCode)
        {
            _repositoryTerminationCode.UpdateTerminationCode(CatchRecord(objTerminationCode));

        }
        public ActionResult ShowTerminationCode()
        {
            return View("ShowTerminationCode", new TerminationCodeViewModel());
        }
        [HttpPost]
       
        public ActionResult ShowTerminationCode(TerminationCodeViewModel ObjTerminationCode)
        {
            ObjTerminationCode.ValidationFilter(ModelState, "btnSearchTerminationCode");

            if (!string.IsNullOrEmpty(ObjTerminationCode.mTerminationCodeSearchName))
            {
                ObjTerminationCode.BETerminationCodeList = DisplayRecord(_repositoryTerminationCode.GetTerminationCodeList(ObjTerminationCode.mTerminationCodeSearchName));
            }
            else
            {
                ObjTerminationCode.BETerminationCodeList = DisplayRecord(_repositoryTerminationCode.GetTerminationCodeList(ObjTerminationCode.mTerminationCodeSearchName));

            }
            if (ObjTerminationCode.BETerminationCodeList.Count <= 0)
            {
                ViewData["Message"] = @BPA.GlobalResources.UI.Resources_common.dispNoRecordFound;
            }
            return View("ShowTerminationCode", ObjTerminationCode);
        }
        private TerminationCodeModel CatchRecord(TerminationCodeViewModel objTerminationCode)
        {
            TerminationCodeModel objBETerminationCodeInfo = new();
                objBETerminationCodeInfo.TerminationCodeID = objTerminationCode.mTerminationCodeID;
                objBETerminationCodeInfo.TerminationCodeName =objTerminationCode.mTerminationCodeName.ToString().TrimStart().TrimEnd().Trim();
                objBETerminationCodeInfo.Description = objTerminationCode.mDescription;
                objBETerminationCodeInfo.Disabled = objTerminationCode.mIsdisable;
                objBETerminationCodeInfo.UserId = base.oUser.iUserID;
                return objBETerminationCodeInfo;
            
        }
        private List<BETerminationCodeInfo> DisplayRecord(List<TerminationCodeModel> terminationCodeModelList)
        {
            List<BETerminationCodeInfo> list= new();
            foreach (var item in terminationCodeModelList)
            {
                BETerminationCodeInfo bETerminationCodeInfo= new();
                bETerminationCodeInfo.bDisabled = item.Disabled;
                bETerminationCodeInfo.iTerminationCodeID = item.TerminationCodeID;
                bETerminationCodeInfo.sTermCodeName = item.TerminationCodeName;
                bETerminationCodeInfo.sTermCodeDesc = item.Description;
                bETerminationCodeInfo.iCreatedBy = item.UserId;
                bETerminationCodeInfo.iModifiedBy = item.UserId;
                list.Add(bETerminationCodeInfo);
            }
            return list;
          
        }

    }
}
