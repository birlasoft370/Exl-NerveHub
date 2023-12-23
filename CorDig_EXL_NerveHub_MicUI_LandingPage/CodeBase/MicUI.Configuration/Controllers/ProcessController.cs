using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Models;
using MicUI.Configuration.Module.Configuration.ClientInfoSetup;
using MicUI.Configuration.Module.Configuration.ProcessInfoSetup;
using MicUI.Configuration.Services.ServiceModel;
using System.Collections;
using System.Text.Json;

namespace MicUI.Configuration.Controllers
{
    public class ProcessController : BaseController
    {
        private readonly IClientService _repositoryClient;
        private readonly ICalenderService _repositoryCalender;
        private readonly IMasterTableService _repositoryMasterTable;
        private readonly ISBUInfoService _repositorySBUInfo;
        private readonly ILocationService _repositoryLocation;
        private readonly IProcessService _repositoryProcess;

        public ProcessController(IClientService repositoryClient, ICalenderService repositoryCalender, IMasterTableService repositoryMasterTable, ISBUInfoService repositorySBUInfo, ILocationService repositoryLocation, IProcessService repositoryProcess)
        {
            _repositoryClient = repositoryClient;
            _repositoryCalender = repositoryCalender;
            _repositoryMasterTable = repositoryMasterTable;
            _repositorySBUInfo = repositorySBUInfo;
            _repositoryLocation = repositoryLocation;
            _repositoryProcess = repositoryProcess;
        }

        public IActionResult Index()
        {
            int iProcessId = 0;

            if (TempData["ProcessId"] != null)
            {
                iProcessId = Convert.ToInt32(TempData["ProcessId"]);
            }

            ProcessViewModel oProvessViewModel = new()
            {
                sTanatNameWeb = GlobalConstant.AppTenantName.ToString().ToUpper()
            };

            if (iProcessId == 0)
            {
                TempData["lProcessGroup"] = null;
                oProvessViewModel.PASProcessType = "1";
                return View(oProvessViewModel);
            }
            else
            {
                IList<BEProcessInfo> objBEProcessInfoList = new List<BEProcessInfo>();
                BEProcessInfo objBEProcessInfo = _repositoryProcess.GetProcessDetails(iProcessId);
                oProvessViewModel.ProcessID = iProcessId;
                oProvessViewModel.ClientId = objBEProcessInfo.iClientID;
                oProvessViewModel.ClientName = objBEProcessInfo.iClientID.ToString();
                oProvessViewModel.ProcessName = objBEProcessInfo.sProcessName;
                oProvessViewModel.Description = objBEProcessInfo.sProcessDescription;
                oProvessViewModel.sCalendarId = objBEProcessInfo.iCalendarID.ToString();
                oProvessViewModel.ProcessTypeId = objBEProcessInfo.iProcessType;
                oProvessViewModel.ProcessType = objBEProcessInfo.iProcessType.ToString();
                oProvessViewModel.ProcessWorkTypeId = objBEProcessInfo.iProcessWorkType;
                oProvessViewModel.SBUID = objBEProcessInfo.iSBUID;
                oProvessViewModel.Scope = objBEProcessInfo.sScope;
                oProvessViewModel.StabilizationStartDate = objBEProcessInfo.dStabilizationStartDate;
                oProvessViewModel.StabilizationEndDate = objBEProcessInfo.dStabilizationEndDate;
                oProvessViewModel.PilotStartDate = objBEProcessInfo.oProcessSLA.dPilotStartDate;
                oProvessViewModel.PilotEndDate = objBEProcessInfo.oProcessSLA.dPilotEndDate;
                oProvessViewModel.ProductionStartDate = objBEProcessInfo.dProductionStartDate;
                oProvessViewModel.ProductionEndDate = objBEProcessInfo.dProductionEndDate;
                oProvessViewModel.FrequencyQCA = objBEProcessInfo.iQCAFeebackTragetPerWeek < 0 ? 0 : objBEProcessInfo.iQCAFeebackTragetPerWeek;

                oProvessViewModel.FrequencyFeedbackSupervisorId = objBEProcessInfo.iSupervisorFeedbackTargetFrequency;
                oProvessViewModel.FrequencyFeedbackSupervisor = objBEProcessInfo.iSupervisorFeebackTragetPerWeek < 0 ? 0 : objBEProcessInfo.iSupervisorFeebackTragetPerWeek;

                oProvessViewModel.TargetAuditPerMonth = objBEProcessInfo.iTargetAuditPerMonth < 0 ? 0 : objBEProcessInfo.iTargetAuditPerMonth;
                oProvessViewModel.TargetQCAPerMonth = objBEProcessInfo.iTargetQCAHrs < 0 ? 0 : objBEProcessInfo.iTargetQCAHrs;


                oProvessViewModel.ProcessComplexityId = objBEProcessInfo.iProcessComplexity;
                oProvessViewModel.ERSCAPTypeId = objBEProcessInfo.iCAPType;

                oProvessViewModel.Disable = objBEProcessInfo.bDisabled;


                oProvessViewModel.ProcessSLAId = objBEProcessInfo.oProcessSLA.iProcessSLAID;

                oProvessViewModel.lProcessGroup = objBEProcessInfo.lProcessGroup;

                oProvessViewModel.sPASProcessMonth = objBEProcessInfo.sPASProcessMonth;
                oProvessViewModel.PASProcessType = objBEProcessInfo.sPASProcessType;
                oProvessViewModel.sTanatName = base.oTenant.ClientName;
                oProvessViewModel.sPASProcessFlagActionType = objBEProcessInfo.sPASProcessFlagActionType;

                TempData["lProcessGroup"] = JsonSerializer.Serialize(objBEProcessInfo.lProcessGroup);
                TempData["lProcessFTE"] = JsonSerializer.Serialize(objBEProcessInfo.lProcessFTE);

                return View(oProvessViewModel);
            }
        }

        public JsonResult GetCascadeClient()
        {
            return Json(_repositoryClient.GetClientList(true));
        }
        public JsonResult FillCalendarList()
        {
            return Json(_repositoryCalender.GetCalendarList());
        }
        public JsonResult FillProcessType()
        {
            List<SelectListItem> ObjProcessList = new();
            ObjProcessList.Insert(0, (new SelectListItem { Text = "Client Organization", Value = "3" }));
            ObjProcessList.Insert(1, (new SelectListItem { Text = "Organization", Value = "2" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "Client", Value = "1" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "Support", Value = "4" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "Client Level Process", Value = "5" }));
            return Json(ObjProcessList);
        }
        public JsonResult FillProcessWorkType()
        {
            var OmasterTable = _repositoryMasterTable.GetMasterList();
            return Json(OmasterTable);
        }
        public JsonResult FillSUBName(string iClientID)
        {
            return Json(_repositorySBUInfo.GetSBUListbasedONClient(Convert.ToInt32(iClientID)));
        }
        public JsonResult FillFrequencyFeedbacksupervisor()
        {
            List<SelectListItem> ObjProcessList = new();
            ObjProcessList.Insert(0, (new SelectListItem { Text = "Daily", Value = "1" }));
            ObjProcessList.Insert(1, (new SelectListItem { Text = "Weekly", Value = "2" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "Fortnightly", Value = "3" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "Monthly", Value = "4" }));
            return Json(ObjProcessList);
        }
        public JsonResult FillProcessComplexity()
        {
            return Json(_repositoryMasterTable.GetProcessComplexityList());
        }
        public JsonResult FillERSCAPType()
        {
            List<SelectListItem> ObjProcessList = new List<SelectListItem>();
            ObjProcessList.Insert(0, (new SelectListItem { Text = "CAP 1", Value = "1" }));
            ObjProcessList.Insert(1, (new SelectListItem { Text = "CAP 2", Value = "2" }));
            return Json(ObjProcessList);
        }
        public JsonResult FillLocation()
        {
            return Json(_repositoryLocation.GetLocationList());
        }
        public ActionResult FillERPGridWithSearch([DataSourceRequest] DataSourceRequest request, string ErpProcessName, string iLocation)
        {
            List<BEERPProcess> lErpProcess = null;
            iLocation = String.IsNullOrWhiteSpace(iLocation) ? "0" : iLocation;
            ProcessViewModel objvm = new();
            lErpProcess = _repositoryProcess.GetERPProcessList(ErpProcessName, int.Parse(iLocation));
            objvm.lErpProcess = lErpProcess;
            return Json(objvm.lErpProcess.ToDataSourceResult(request));
        }
        public JsonResult fillErpGroupGridRead([DataSourceRequest] DataSourceRequest request, string[] iERPProcessIDList)
        {
            ProcessViewModel objProcessViewModel = new();
            objProcessViewModel = FillERPProcessMappingGrid(iERPProcessIDList);
            return Json(new { lProcessGroup = objProcessViewModel.lProcessGroup });
        }
        public ProcessViewModel FillERPProcessMappingGrid(string[] iERPProcessIDList)
        {
            IList<BEERPProcess> lERPProcess = new List<BEERPProcess>();
            ArrayList aryDistinctERPProcessId = new();
            ProcessViewModel oProvessViewModel = new()
            {
                lProcessGroup = new List<BEProcessGroup>(),
                lProcessFTE = new List<BEProcessFTE>()
            };

            if (TempData["lProcessGroup"] != null)
            {
                oProvessViewModel.lProcessGroup = JsonSerializer.Deserialize<List<BEProcessGroup>>(TempData["lProcessGroup"].ToString());
                TempData.Keep("lProcessGroup");
            }

            if (iERPProcessIDList == null)
                aryDistinctERPProcessId.Add("0");
            else
                aryDistinctERPProcessId.AddRange(iERPProcessIDList);


            lERPProcess = _repositoryProcess.GetERPProcessList(aryDistinctERPProcessId);

            for (int i = 0; i < lERPProcess.Count; i++)
            {
                int iProcessGroupPrev = oProvessViewModel.lProcessGroup.Count(s => s.oERPProcess.iERPProcessID == lERPProcess[i].iERPProcessID);
                if (iProcessGroupPrev <= 0)
                {
                    BEProcessGroup oProcessGroup = new();
                    BEERPProcess oERPProcessNew = new BEERPProcess
                    {
                        oLocation = new BELocation(),
                        iERPProcessID = lERPProcess[i].iERPProcessID,
                        iERPCode = lERPProcess[i].iERPCode,
                        sName = lERPProcess[i].sName
                    };
                    oProcessGroup.oERPProcess = oERPProcessNew;
                    oProcessGroup.oRowState = RowState.NEW;
                    oERPProcessNew.oLocation = new BELocation(lERPProcess[i].oLocation.iLocationID, lERPProcess[i].oLocation.sLocationName, "", false, 0);
                    oProcessGroup.oERPProcess = oERPProcessNew;
                    oProvessViewModel.lProcessGroup.Add(oProcessGroup);
                }
            }

            TempData["lProcessGroup"] = JsonSerializer.Serialize(oProvessViewModel.lProcessGroup);
            //var deserialize = System.Text.Json.JsonSerializer.Deserialize<List<BEProcessGroup>>(TempData["lProcessGroup"].ToString());
            TempData.Keep("lProcessGroup");
            return oProvessViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteErpGroupGridRead(string[] iERPProcessIDList)
        {
            ProcessViewModel objProcessViewModel = new ProcessViewModel();
            objProcessViewModel = DeleteERPProcessMappingGrid(iERPProcessIDList);

            return Json(new { objProcessViewModel.lProcessGroup, objProcessViewModel.lProcessFTE });
        }

        public ProcessViewModel DeleteERPProcessMappingGrid(string[] iERPProcessIDList)
        {

            IList<BEERPProcess> lERPProcess = new List<BEERPProcess>();
            ArrayList aryDistinctERPProcessId = new ArrayList();
            ProcessViewModel oProvessViewModel = new ProcessViewModel();
            oProvessViewModel.lProcessGroup = new List<BEProcessGroup>();
            oProvessViewModel.lProcessFTE = new List<BEProcessFTE>();

            if (TempData["lProcessGroup"] != null)
            {
                var result = JsonSerializer.Deserialize<List<BEProcessGroup>>(TempData["lProcessGroup"].ToString());
                oProvessViewModel.lProcessGroup = result;
                TempData.Keep("lProcessGroup");
            }

            if (iERPProcessIDList == null)
                aryDistinctERPProcessId.Add("0");
            else
                aryDistinctERPProcessId.AddRange(iERPProcessIDList);

            lERPProcess = _repositoryProcess.GetERPProcessList(aryDistinctERPProcessId);


            int iProcessGroupPrev = oProvessViewModel.lProcessGroup.Count(s => s.oERPProcess.iERPProcessID == lERPProcess[0].iERPProcessID);
            if (iProcessGroupPrev > 0)
            {
                var itemToRemove = oProvessViewModel.lProcessGroup.SingleOrDefault(r => r.oERPProcess.iERPProcessID == Convert.ToInt32(iERPProcessIDList[0]));
                if (itemToRemove != null)
                    oProvessViewModel.lProcessGroup.Remove(itemToRemove);
            }
            int iProcessFTEPrev = oProvessViewModel.lProcessFTE.Count(s => s.oLocation.iLocationID == lERPProcess[0].oLocation.iLocationID);
            if (iProcessGroupPrev > 0)
            {
                var itemToRemoveProcessFTE = oProvessViewModel.lProcessFTE.SingleOrDefault(r => r.oLocation.iLocationID == lERPProcess[0].oLocation.iLocationID);
                if (itemToRemoveProcessFTE != null)
                    oProvessViewModel.lProcessFTE.Remove(itemToRemoveProcessFTE);
            }
            TempData["lProcessGroup"] = null;
            TempData["lProcessGroup"] = JsonSerializer.Serialize(oProvessViewModel.lProcessGroup);
            TempData.Keep("lProcessGroup");

            return oProvessViewModel;
        }

        public JsonResult PASProcessMonth(string PASProcessMonth, string iProcessID, string TenantName, string PASProcessFlagActionType)
        {
            TempData["PASProcessMonth"] = PASProcessMonth;
            TempData["PASTenantName"] = TenantName;
            TempData["PASProcessFlagActionType"] = PASProcessFlagActionType;
            TempData.Keep("PASProcessFlagActionType");
            TempData.Keep("PASProcessMonth");
            TempData.Keep("PASTenantName");
            string ProcessMonthDraft = "OK";
            string PASProcessMonthYear = "";
            return Json(new { sProcessMonthDraft = ProcessMonthDraft, dPASProcessMonthYear = PASProcessMonthYear });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string SaveUpdateProcess(ProcessViewModel processViewModel)
        {
            string ret;
            string PASProcessMonthYear = "YES";
            string PASProcessActionType = "0";
            string PASFlagActionType = "YES";
            string ret_1 = "";
            ProcessViewModel objProcessViewModelNew = new();

            int iCalendarId = processViewModel.sCalendarId == null ? 0 : Convert.ToInt32(processViewModel.sCalendarId);
            int iClientID = processViewModel.ClientName == null ? 0 : Convert.ToInt32(processViewModel.ClientName);
            int iProcessID = processViewModel.ProcessID;

            if (processViewModel.ProcessID > 0)
            {
                iClientID = processViewModel.ClientId;
            }

            if (TempData["PASTenantName"] != null)
            {
                string TenantName = TempData["PASTenantName"].ToString();
                TempData.Keep("PASTenantName");

                string sPASProcessMonth = TempData["PASProcessMonth"].ToString();
                TempData.Keep("PASProcessMonth");

                if (TenantName == GlobalConstant.AppTenantName.ToString().ToUpper())
                {
                    PASProcessActionType = TempData["PASProcessFlagActionType"].ToString() == "" ? "0" : TempData["PASProcessFlagActionType"].ToString();
                    switch (PASProcessActionType)
                    {
                        case "0":
                            PASFlagActionType = processViewModel.PASProcessType == "1" ? "YES" : "NO";
                            ret_1 = processViewModel.PASProcessType == "1" ? "0" : "-5";  // Create New
                            break;
                        case "1":
                            PASFlagActionType = processViewModel.PASProcessType == "2" ? "YES" : "NO";
                            ret_1 = processViewModel.PASProcessType == "2" ? "0" : "-6"; // Amend
                            break;
                        case "2":
                            PASFlagActionType = processViewModel.PASProcessType == "2" ? "YES" : "NO";
                            ret_1 = processViewModel.PASProcessType == "2" ? "0" : "-7"; // Amend
                            break;
                        case "3":
                            PASFlagActionType = processViewModel.PASProcessType == "1" ? "YES" : "NO";
                            ret_1 = processViewModel.PASProcessType == "1" ? "0" : "-8"; // Create New
                            break;
                        default:
                            break;
                    }
                }
            }

            if (PASFlagActionType == "NO")
            {
                ret = ret_1;
            }
            else if (PASProcessMonthYear == "NOT")
            {
                ret = "-4";
            }
            else if (_repositoryProcess.CheckRoleForOrgProcess() == 0)
            {
                ret = "-2";
            }
            else if (_repositoryProcess.CheckProcessByClientForUniqueness(processViewModel.ProcessName, iClientID, iProcessID))
            {
                ret = "-3";
            }
            else
            {
                if (processViewModel.ProcessID > 0)
                {
                    _repositoryProcess.UpdateData(CatchRecord(processViewModel));
                    ret = "2";
                }
                else
                {
                    _repositoryProcess.InsertData(CatchRecord(processViewModel));
                    ret = "1";
                }

            }
            TempData["lProcessGroup"] = null;
            return ret;
        }

        private ProcessModel CatchRecord(ProcessViewModel ObjvmProcess)
        {
            Services.ServiceModel.BEProcessInfo oProcessNew = new Services.ServiceModel.BEProcessInfo();

            if (TempData["lProcessGroup"] != null)
            {
                oProcessNew.lProcessGroup = JsonSerializer.Deserialize<List<BEProcessGroup>>(TempData["lProcessGroup"].ToString());
                TempData.Keep("lProcessGroup");
            }

            if (ObjvmProcess.ClientName == null)
            {
                if (ObjvmProcess.ClientId != null)
                {
                    ObjvmProcess.ClientName = ObjvmProcess.ClientId.ToString();
                }
            }

            ProcessModel process = new()
            {
                Processid = ObjvmProcess.ProcessID,
                Processname = ObjvmProcess.ProcessName,
                Description = ObjvmProcess.Description ?? "",
                Clientid = int.Parse(ObjvmProcess.ClientName == null ? "0" : ObjvmProcess.ClientName),
                Processtype = ObjvmProcess.ProcessType == null ? ObjvmProcess.ProcessTypeId == null ? 0 : ObjvmProcess.ProcessTypeId : Convert.ToInt32(ObjvmProcess.ProcessType),
                Sbuid = ObjvmProcess.SBUID,
                Calendarid = ObjvmProcess.sCalendarId == null ? 0 : Convert.ToInt32(ObjvmProcess.sCalendarId),
                Processworktype = ObjvmProcess.ProcessWorkTypeId,
                Scope = ObjvmProcess.Scope,
                Pilotstartdate = ObjvmProcess.PilotStartDate,
                Pilotenddate = ObjvmProcess.PilotEndDate,
                Disabled = Convert.ToBoolean(ObjvmProcess.Disable),
                StabilizationStartDate = ObjvmProcess.StabilizationStartDate,
                StabilizationEndDate = ObjvmProcess.StabilizationEndDate,
                ProductionStartDate = ObjvmProcess.ProductionStartDate,
                ProductionEndDate = ObjvmProcess.ProductionEndDate,
                ProcessComplexity = ObjvmProcess.ProcessComplexityId,
                Captype = ObjvmProcess.ERSCAPTypeId,
                QCAFeebackTragetPerWeek = ObjvmProcess.FrequencyQCA,
                SupervisorFeedbackTargetFrequency = ObjvmProcess.FrequencyFeedbackSupervisorId,
                SupervisorFeebackTragetPerWeek = ObjvmProcess.FrequencyFeedbackSupervisor,
                TargetAuditPerMonth = ObjvmProcess.TargetAuditPerMonth,
                TargetQCAHrs = ObjvmProcess.TargetQCAPerMonth,
                lProcessGroup = oProcessNew.lProcessGroup.Select(x => new ERPProcessGroup()
                {
                    ERPCode = x.oERPProcess.iERPCode,
                    Disabled = x.bDisabled,
                    ERPProcessID = x.oERPProcess.iERPProcessID,
                    Name = x.oERPProcess.sName,
                    ProcessGroupID = x.iProcessGroupID
                }).ToList()
            };
            return process;
        }

        public ActionResult SearchView()
        {
            return View(new ProcessViewModel());
        }
        public ActionResult GetProcessList([DataSourceRequest] DataSourceRequest request, int? clientName, string processName)
        {
            ProcessViewModel objProcessViewModel = new();
            IList<BEProcessInfo> objBEProcessInfoList = new List<BEProcessInfo>();

            if (clientName != 0)
            {
                objProcessViewModel.lBEProcessInfo = _repositoryProcess.GetProcessListSearch(clientName == null ? 0 : int.Parse(Convert.ToString(clientName)), processName);
                return Json(objProcessViewModel.lBEProcessInfo.ToDataSourceResult(request));
            }
            else
            {
                return Json(objProcessViewModel.lBEProcessInfo.ToDataSourceResult(request));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public int SetEditableProcessId(string tempProcessId)
        {

            TempData["ProcessId"] = tempProcessId;
            TempData.Keep("ProcessId");

            return 1;
        }
    }

}
