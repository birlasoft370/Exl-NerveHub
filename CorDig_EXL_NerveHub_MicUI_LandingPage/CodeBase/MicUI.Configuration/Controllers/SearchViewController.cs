using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Module.Configuration.CampaignInfoSetup;
using MicUI.Configuration.Module.Configuration.ClientInfoSetup;
using MicUI.Configuration.Module.Configuration.ProcessInfoSetup;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{
    public class SearchViewController : BaseController
    {
        private readonly IClientService _repositoryClient;
        private readonly IProcessService _repositoryProcess;
        private readonly ICampaignService _repositoryCampaign;
        private readonly IStoreService _repositoryStore;
        public SearchViewController(IClientService repositoryClient, IProcessService repositoryProcess, ICampaignService repositoryCampaign, IStoreService repositoryStore)
        {
            _repositoryClient = repositoryClient;
            _repositoryProcess = repositoryProcess;
            _repositoryCampaign = repositoryCampaign;
            _repositoryStore = repositoryStore;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public JsonResult GetCascadeClient()
        {
            if (Request.GetTypedHeaders().Referer != null)
            {
                return Json(_repositoryClient.GetClientList(true));
            }
            else
            {
                return Json(new List<BEClientInfo>());
            }
        }
        public JsonResult GetCascadeProcess(int iClientID)
        {
            if (Request.GetTypedHeaders().Referer != null)
            {
                return Json(_repositoryProcess.GetProcessListSearch(iClientID, ""));
            }
            else
            {
                return Json(new List<BEProcessInfo>());
            }
        }
        public JsonResult GetCascadeCamp(int iProcessID)
        {
            if (Request.GetTypedHeaders().Referer != null)
            {
                return Json(_repositoryCampaign.GetProcessWiseCampaignList(iProcessID));
            }
            else
            {
                return Json(new List<BECampaignInfo>());
            }
        }

        public JsonResult GetWorkObjList(string iCampaignName)
        {
            return Json(_repositoryStore.GetWorkObjList(int.Parse(iCampaignName == "" ? "0" : iCampaignName)));
        }
    }
}
