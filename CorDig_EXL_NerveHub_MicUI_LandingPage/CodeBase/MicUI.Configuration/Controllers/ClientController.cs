using BPA.GlobalResources;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Models;
using MicUI.Configuration.Module.Configuration.ClientInfoSetup;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{
    public class ClientController : BaseController
    {
        private readonly IVerticalService _repositoryVertical;
        private readonly ISBUInfoService _repositorySBU;
        private readonly IClientService _repositoryClient;
        public ClientController(IVerticalService repositoryVertical, ISBUInfoService repositorySBU, IClientService repositoryClient)
        {
            _repositoryVertical = repositoryVertical;
            _repositorySBU = repositorySBU;
            _repositoryClient = repositoryClient;
        }
        //[HttpGet("Client/Index")]
        public IActionResult Index()
        {
            ClientViewModel CM = new();

            if (!base.CheckViewPermission(int.Parse(Resources_FormID.Client)))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
            if (TempData["ClientID"] == null)
            {
                TempData["ClientID"] = 0;
            }
            string id = (TempData["ClientID"]).ToString();

            if (int.Parse(id) != 0)
            {
                var EnumClientListBind = _repositoryClient.GetClientById(int.Parse(id));

                CM.iClientID = EnumClientListBind.iClientID;
                CM.ClientName = EnumClientListBind.sClientName;
                CM.VerticalID = EnumClientListBind.iVerticalID;
                CM.Description = EnumClientListBind.sClientDescription;
                CM.EXLSpecificClient = EnumClientListBind.bEXLSpecific;
                CM.Disabled = EnumClientListBind.bDisabled;
            }

            return View("Index", CM);
        }

        public JsonResult GetVerticalList()
        {
            return Json(_repositoryVertical.GetVerticalList());
        }

        public JsonResult GetSbuList()
        {
            var SBUList = _repositorySBU.GetSBUList(true);

            return Json(new { lSBUInfo = SBUList });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Index(ClientViewModel ObjClient, string[] chkSelected)
        {
            string result = string.Empty;

            try
            {
                if (ObjClient.iClientID == 0)
                {
                    result = _repositoryClient.InsertData(CatchRecord(ObjClient, chkSelected));
                }
                else
                {
                    result = _repositoryClient.UpdateData(CatchRecord(ObjClient, chkSelected));
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result);
        }

        [NonAction]
        private ClientModel CatchRecord(ClientViewModel ObjClient, string[] chkClientid)
        {
            ClientModel objClientEntity = new()
            {
                ClientID = ObjClient.iClientID,
                ClientName = ObjClient.ClientName,
                VerticalID = ObjClient.VerticalID,
                Description = ObjClient.Description,
                EXLSpecificClient = ObjClient.EXLSpecificClient,
                Disabled = ObjClient.Disabled,
                ListSBU = chkClientid
            };
            return objClientEntity;
        }

        [HttpGet]
        public ActionResult ShowClient()
        {
            return View(new ClientViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowClient(ClientViewModel ObjClient)
        {
            ObjClient.ValidationFilter(ModelState, "btnSearch");
            ObjClient.SEARCHNAME = ObjClient.SEARCHNAME is null ? null : ObjClient.SEARCHNAME.ToString().Trim();
            ObjClient.ClientList = _repositoryClient.GetClientList(false, ObjClient.SEARCHNAME);
            if (ObjClient.ClientList.Count <= 0)
            {
                ViewData["result"] = @BPA.GlobalResources.UI.Resources_common.display_msgNotFound;
            }
            return View("ShowClient", ObjClient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetClientID(string id)
        {
            int returnValue = 0;
            if (id != null)
            {
                TempData["ClientID"] = id;
                TempData.Keep("ClientID");
                returnValue = 1;
            }
            return Json(returnValue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetErpSbuListById(string ClientID)
        {
            List<BESBUInfo> lSBUInfo = _repositorySBU.GetSBUListbasedONClient(int.Parse(ClientID));
            return Json(new { lSBUInfo });
        }
    }
}
