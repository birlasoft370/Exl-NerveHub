using BPA.AppConfig.BusinessEntity.ExternalRef.Security;
using BPA.AppConfig.BusinessEntity.Security;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Security;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Security;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Request;
using BPA.AppConfiguration.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;
using System.Data;
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class TeamManagementController : BaseController<TeamManagementController>
    {
        private readonly ITeamService _repositoryTeam;
        private readonly IPermissionService _repositoryPermission;

        public TeamManagementController(ILogger<TeamManagementController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ITeamService repositoryTeam, IPermissionService repositoryPermission) : base(logger, env, configuration)
        {
            _repositoryTeam = repositoryTeam;
            _repositoryPermission = repositoryPermission;
        }

        [ProducesResponseType(typeof(List<TeamManagementModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetTeamList")]
        public async Task<MessageResponse<List<TeamManagementModel>>> GetTeamList([FromQuery] string? searchText)
        {
            var result = new MessageResponse<List<TeamManagementModel>>();
            List<TeamManagementModel> objTeamManagementList = new List<TeamManagementModel>();
            List<BETeamInfo> listTeam = new List<BETeamInfo>();
            try
            {
                listTeam = await Task.Run(() => _repositoryTeam.GetTeamList(searchText, oUser.iUserID, true, base.oTenant));

                for (int i = 0; i < listTeam.Count; i++)
                {
                    objTeamManagementList.Add(new TeamManagementModel
                    {
                        TeamID = listTeam[i].iTeamID,
                        TeamName = Encoder.HtmlEncode(listTeam[i].sTeamName, false),
                        ClientID = listTeam[i].iClientID,
                        ProcessID = listTeam[i].iProcessID
                    });
                }

                result.Data = objTeamManagementList;
                result.TotalCount = objTeamManagementList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(TeamManagementModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetTeamById")]
        public async Task<MessageResponse<TeamManagementModel>> GetTeamById([FromQuery] int teamID)
        {
            var result = new MessageResponse<TeamManagementModel>();
            TeamManagementModel ObjTeam = new TeamManagementModel();
            try
            {
                var ObjTeamList = await Task.Run(() => _repositoryTeam.GetTeamList(teamID, base.oTenant));

                ObjTeam.ClientID = ObjTeamList[0].iClientID;
                ObjTeam.ProcessID = ObjTeamList[0].iProcessID;
                ObjTeam.TeamID = teamID;
                ObjTeam.TeamName = ObjTeamList[0].sTeamName;
                ObjTeam.Description = ObjTeamList[0].sTeamDesc;
                ObjTeam.IsClientLevelTeam = ObjTeamList[0].bClientLevel;
                ObjTeam.Disable = ObjTeamList[0].bDisabled;

                IList<BEUserInfo> ListUserInfo = new List<BEUserInfo>();
                ListUserInfo = ObjTeamList[0].iUserID;

                if (ListUserInfo.Count > 0)
                {

                    for (int i = 0; i < ListUserInfo.Count; i++)
                    {
                        ObjTeam.UserList.Add(new TeamManagementModel.User { UserId = ListUserInfo[i].iUserID, UserName = Encoder.HtmlEncode(ListUserInfo[i].Name, false) });
                    }
                }

                result.Data = ObjTeam;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(TeamManagementModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetSearchedUser")]
        public async Task<MessageResponse<List<TeamManagementModel.User>>> GetSearchedUser([FromQuery] string searchText, [FromQuery] string searchCondition)
        {
            var result = new MessageResponse<List<TeamManagementModel.User>>();
            TeamManagementModel ObjTeam = new TeamManagementModel();
            try
            {
                DataSet ds = await Task.Run(() =>
                    _repositoryPermission.GetUserListD(searchText, false, base.oUser.iUserID, 0, searchCondition, base.oTenant));

                var lstUser = new List<TeamManagementModel.User>();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        lstUser.Add(new TeamManagementModel.User
                        {
                            UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"]),
                            UserName = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]).Trim() + " " + Convert.ToString(ds.Tables[0].Rows[i]["MiddleName"]).Trim() + " " + Convert.ToString(ds.Tables[0].Rows[i]["LastName"]).Trim() + " (" + Convert.ToString(ds.Tables[0].Rows[i]["EmpID"]) + ")", @"\s+", " ")
                        });
                    }
                }
                result.Data = lstUser;
                result.TotalCount = lstUser.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddTeam")]
        public async Task<MessageResponse<string>> AddTeam([FromBody] TeamManagementModel teamManagementModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => this._repositoryTeam.InsertData(CatchRecord(teamManagementModel), 26, base.oTenant));

                result.Data = "2";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateTeam")]
        public async Task<MessageResponse<string>> UpdateTeam([FromBody] TeamManagementModel teamManagementModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => this._repositoryTeam.UpdateData(CatchRecord(teamManagementModel), 26, base.oTenant));

                result.Data = "1";

            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private BETeamInfo CatchRecord(TeamManagementModel TeamManagementModel)
        {
            BETeamInfo oTeam = new BETeamInfo();
            IList<BEUserInfo> lUserInfo = new List<BEUserInfo>();
            oTeam.iTeamID = TeamManagementModel.TeamID;
            oTeam.sTeamName = Encoder.HtmlEncode(TeamManagementModel.TeamName.Trim(), false);
            oTeam.sTeamDesc = TeamManagementModel.Description == null ? "" : TeamManagementModel.Description.Trim();
            oTeam.iProcessID = TeamManagementModel.ProcessID;
            oTeam.bDisabled = TeamManagementModel.Disable == false ? false : true;
            oTeam.iCreatedBy = oUser.iUserID;
            oTeam.bClientLevel = TeamManagementModel.IsClientLevelTeam == false ? false : true;
            if (TeamManagementModel.UserList.Count > 0)
            {
                for (int i = 0; i < TeamManagementModel.UserList.Count; i++)
                {
                    BEUserInfo objUser = new BEUserInfo();
                    objUser.iUserID = Convert.ToInt32(TeamManagementModel.UserList[i].UserId);
                    lUserInfo.Add(objUser);
                    objUser = null;
                }
            }
            oTeam.iUserID = lUserInfo;
            return oTeam;
        }
    }
}
