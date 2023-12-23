using Microsoft.AspNetCore.Mvc;
using MicUI.EmailManagement.Module.Common;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Controllers
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

        public IActionResult Index()
        {
            return View();
        }

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
                return Json(_repositoryProcess.GetProcessList(iClientID, "", true));
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
