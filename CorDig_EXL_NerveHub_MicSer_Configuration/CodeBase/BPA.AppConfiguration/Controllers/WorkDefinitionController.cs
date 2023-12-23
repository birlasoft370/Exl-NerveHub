using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace MicSer.Configuration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class WorkDefinitionController : BaseController<WorkDefinitionController>
    {
        private readonly IStoreService _repositoryStore;

        public WorkDefinitionController(ILogger<WorkDefinitionController> logger, IWebHostEnvironment env, IConfiguration configuration,
          IStoreService repositoryStore) : base(logger, env, configuration)
        {
            _repositoryStore = repositoryStore;
        }

        [ProducesResponseType(typeof(List<BEStoreInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetStoreListWithCampId")]
        public async Task<MessageResponse<List<BEStoreInfo>>> GetStoreListWithCampId([FromQuery] int campaignId, [FromQuery] string? storeName)
        {
            var result = new MessageResponse<List<BEStoreInfo>>();
            try
            {
                storeName = storeName ?? "";
                var lst = await Task.Run(() => _repositoryStore.GetStoreList(campaignId, storeName, true, base.oUser.iUserID, base.oTenant));
                result.Data = lst;
                result.TotalCount = lst.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
    }
}
