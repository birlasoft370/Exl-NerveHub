using BPA.EmailConfiguration.BaseController;
using BPA.EmailConfiguration.Helper;
using BPA.EmailConfiguration.Models;
using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Config;
using BPA.EmailManagement.ServiceContract.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;
using MicSer.EmailConfiguration.Helper;
using MicSer.EmailConfiguration.Helper.Filter;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace BPA.EmailConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class MailConfigurationController : BaseController<MailConfigurationController>
    {
        private readonly IMailConfigurationService iMailConfigurationService;
        private readonly IMailTemplateService imailTemplateService;
        private readonly IMailServiceGraph iMailServiceGraph;
        private readonly IMailService iMailService;
        public MailConfigurationController(ILogger<MailConfigurationController> logger,
            IWebHostEnvironment env,
            IConfiguration configuration,
            IMailConfigurationService pMailConfigurationService,
                                           IMailTemplateService pmailTemplateService,
                                           IMailServiceGraph pMailServiceGraph,
                                           IMailService pMService) : base(logger, env, configuration)
        {
            this.iMailConfigurationService = pMailConfigurationService;
            this.imailTemplateService = pmailTemplateService;
            this.iMailServiceGraph = pMailServiceGraph;
            this.iMailService = pMService;
        }

        [ProducesResponseType(typeof(IEnumerable<BEMailConfiguration>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCampaignWiseList")]
        public async Task<MessageResponse<IEnumerable<BEMailConfiguration>>> GetCampaignWiseList([FromQuery] string StoreID)
        {
            var result = new MessageResponse<IEnumerable<BEMailConfiguration>>();
            try
            {
                IList<BEMailConfiguration> listMailConfiguration = new List<BEMailConfiguration>();
                listMailConfiguration = await Task.Run(() => iMailConfigurationService.GetCampaignWiseList(int.Parse(StoreID == "" ? "0" : StoreID), base.oTenant));
                result.Data = listMailConfiguration;
                result.TotalCount = listMailConfiguration.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(IEnumerable<BEMailConfiguration>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetMailTemplateList")]
        public async Task<MessageResponse<IList<BEMailTemplate>>> GetMailTemplateList([FromQuery] bool isDisabled, [FromQuery] bool isAutoReply)
        {
            var result = new MessageResponse<IList<BEMailTemplate>>();
            try
            {
                result.Data = await Task.Run(() => imailTemplateService.GetMailTemplateAll(isDisabled, base.oTenant, isAutoReply));
                result.TotalCount = result.Data.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(IList<BEMailConfiguration>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetMailConfigAllData")]
        public async Task<MessageResponse<IList<BEMailConfiguration>>> GetMailConfigAllData([FromQuery] int mailConfigId)
        {
            var result = new MessageResponse<IList<BEMailConfiguration>>();
            try
            {
                result.Data = await Task.Run(() => iMailConfigurationService.GetMailConfigAllData(mailConfigId, base.oTenant));
                result.TotalCount = result.Data.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(IEnumerable<TreeStructure>), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("TestConnection")]
        public async Task<MessageResponse<IEnumerable<TreeStructure>>> TestConnection([FromBody] TestConnectionModel model)
        {
            var result = new MessageResponse<IEnumerable<TreeStructure>>();
            Hashtable DataRoot = new Hashtable();
            List<TreeStructure> lst = new List<TreeStructure>();
            List<TreeStructure> recursiveObjects = new List<TreeStructure>();
            recursiveObjects.Add(new TreeStructure { id = "", text = "" });
            try
            {
                //var lststr = "[{\"parentId\":\"Without Ticketing\",\"text\":\"Without Ticketing\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAEJaEuuAAA=\",\"items\":[]},{\"parentId\":\"LM UAT Query\",\"text\":\"LM UAT Query\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAFd1UIjAAA=\",\"items\":[]},{\"parentId\":\"without ticketing Complete\",\"text\":\"without ticketing Complete\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAEJaEuvAAA=\",\"items\":[]},{\"parentId\":\"CNAInput\",\"text\":\"CNAInput\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAGDaNbnAAA=\",\"items\":[]},{\"parentId\":\"LM UAT\",\"text\":\"LM UAT\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAFd1UIlAAA=\",\"items\":[]},{\"parentId\":\"ZurichNA\",\"text\":\"ZurichNA\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAANYrWUAAA=\",\"items\":[]},{\"parentId\":\"MCSWMTest\",\"text\":\"MCSWMTest\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAEq-cE3AAA=\",\"items\":[]},{\"parentId\":\"CCTUpload\",\"text\":\"CCTUpload\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AALlJNHAAAA=\",\"items\":[]},{\"parentId\":\"DCCPending\",\"text\":\"DCCPending\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAF5EKt_AAA=\",\"items\":[]},{\"parentId\":\"Junk Email\",\"text\":\"Junk Email\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAAAAAEBAAA=\",\"items\":[]},{\"parentId\":\"Task2\",\"text\":\"Task2\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AALez3dyAAA=\",\"items\":[]},{\"parentId\":\"EMSdemo\",\"text\":\"EMSdemo\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AADYeWOvAAA=\",\"items\":[]},{\"parentId\":\"Drafts\",\"text\":\"Drafts\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQA7LQpYEa8DTZjdawPBFrolAAAApU9RAAA=\",\"items\":[]},{\"parentId\":\"Sync Issues\",\"text\":\"Sync Issues\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAIUxJj-AAA=\",\"items\":[{\"parentId\":null,\"text\":\"\",\"id\":\"\",\"items\":[]}]},{\"parentId\":\"9-Completed\",\"text\":\"9-Completed\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAFD_bMCAAA=\",\"items\":[]},{\"parentId\":\"CCTUploadCompleted\",\"text\":\"CCTUploadCompleted\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AALlJNHBAAA=\",\"items\":[]},{\"parentId\":\"Outbox\",\"text\":\"Outbox\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQA7LQpYEa8DTZjdawPBFrolAAAApU9HAAA=\",\"items\":[]},{\"parentId\":\"Deleted Items\",\"text\":\"Deleted Items\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQA7LQpYEa8DTZjdawPBFrolAAAApU9JAAA=\",\"items\":[]},{\"parentId\":\"PA Response\",\"text\":\"PA Response\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAFD_bMDAAA=\",\"items\":[]},{\"parentId\":\"Archive\",\"text\":\"Archive\",\"id\":\"AQMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMAA4ZDNiNTgALgAAAwhLkc8m-EBHh4DTCvO3Iv8BAAVXLYHbjepNuWj66kTr5TcAAAIBLwAAAA==\",\"items\":[]},{\"parentId\":\"Sent Items\",\"text\":\"Sent Items\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQA7LQpYEa8DTZjdawPBFrolAAAApU9IAAA=\",\"items\":[]},{\"parentId\":\"OneWest Pending\",\"text\":\"OneWest Pending\",\"id\":\"AQMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMAA4ZDNiNTgALgAAAwhLkc8m-EBHh4DTCvO3Iv8BAAVXLYHbjepNuWj66kTr5TcAAeHgJwAAAQ==\",\"items\":[]},{\"parentId\":\"RSS Feeds\",\"text\":\"RSS Feeds\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AALlI826AAA=\",\"items\":[]},{\"parentId\":\"Inbox\",\"text\":\"Inbox\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQA7LQpYEa8DTZjdawPBFrolAAAApU9GAAA=\",\"items\":[{\"parentId\":null,\"text\":\"\",\"id\":\"\",\"items\":[]}]},{\"parentId\":\"DCCCompleted\",\"text\":\"DCCCompleted\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAF5EKt-AAA=\",\"items\":[]},{\"parentId\":\"Birlasoft_Completed\",\"text\":\"Birlasoft_Completed\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AALez3ftAAA=\",\"items\":[]},{\"parentId\":\"EXL_Testing\",\"text\":\"EXL_Testing\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AABPT_qgAAA=\",\"items\":[]},{\"parentId\":\"Endorsement\",\"text\":\"Endorsement\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAAD2ItyAAA=\",\"items\":[]},{\"parentId\":\"HTask\",\"text\":\"HTask\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AALez3d0AAA=\",\"items\":[]},{\"parentId\":\"Task1\",\"text\":\"Task1\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AALez3dxAAA=\",\"items\":[]},{\"parentId\":\"BPA_Testing\",\"text\":\"BPA_Testing\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AABfZolmAAA=\",\"items\":[]},{\"parentId\":\"AdnicComplete\",\"text\":\"AdnicComplete\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AANS9uisAAA=\",\"items\":[]},{\"parentId\":\"Liberty Mutual\",\"text\":\"Liberty Mutual\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAAD2ItzAAA=\",\"items\":[]},{\"parentId\":\"Birlasoft_Inbox\",\"text\":\"Birlasoft_Inbox\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AALez3fsAAA=\",\"items\":[]},{\"parentId\":\"Adnic\",\"text\":\"Adnic\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAIUxKjBAAA=\",\"items\":[]},{\"parentId\":\"OneWest Completed\",\"text\":\"OneWest Completed\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAHh4CcBAAA=\",\"items\":[]},{\"parentId\":\"CNAPending\",\"text\":\"CNAPending\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAGDaNbpAAA=\",\"items\":[]},{\"parentId\":\"ZNAPending\",\"text\":\"ZNAPending\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAGJNLdgAAA=\",\"items\":[]},{\"parentId\":\"LDS\",\"text\":\"LDS\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AALez3c3AAA=\",\"items\":[]},{\"parentId\":\"9-Com\",\"text\":\"9-Com\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAFD_bMBAAA=\",\"items\":[{\"parentId\":null,\"text\":\"\",\"id\":\"\",\"items\":[]}]},{\"parentId\":\"HanoverTest\",\"text\":\"HanoverTest\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAEq-cE4AAA=\",\"items\":[]},{\"parentId\":\"Completed\",\"text\":\"Completed\",\"id\":\"AQMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMAA4ZDNiNTgALgAAAwhLkc8m-EBHh4DTCvO3Iv8BAAVXLYHbjepNuWj66kTr5TcAAAJYXwAAAA==\",\"items\":[]},{\"parentId\":\"CNACompleted\",\"text\":\"CNACompleted\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAGDaNboAAA=\",\"items\":[]},{\"parentId\":\"Pending\",\"text\":\"Pending\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AABlLCQqAAA=\",\"items\":[]},{\"parentId\":\"Conversation History\",\"text\":\"Conversation History\",\"id\":\"AQMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMAA4ZDNiNTgALgAAAwhLkc8m-EBHh4DTCvO3Iv8BAAVXLYHbjepNuWj66kTr5TcAAAIBJwAAAA==\",\"items\":[{\"parentId\":null,\"text\":\"\",\"id\":\"\",\"items\":[]}]},{\"parentId\":\"Task3\",\"text\":\"Task3\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AALez3dzAAA=\",\"items\":[]},{\"parentId\":\"ZNAInput\",\"text\":\"ZNAInput\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAGJNLdfAAA=\",\"items\":[]},{\"parentId\":\"DCCInbox\",\"text\":\"DCCInbox\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAF5EKt9AAA=\",\"items\":[]},{\"parentId\":\"OneWest UAT\",\"text\":\"OneWest UAT\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAHh4Cb-AAA=\",\"items\":[]},{\"parentId\":\"Ticketing\",\"text\":\"Ticketing\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAEJaEusAAA=\",\"items\":[]},{\"parentId\":\"ZNACompleted\",\"text\":\"ZNACompleted\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAGJNLdhAAA=\",\"items\":[]},{\"parentId\":\"OpenOutlookSRVC\",\"text\":\"OpenOutlookSRVC\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AANS9ukWAAA=\",\"items\":[]},{\"parentId\":\"LM UAT Completed\",\"text\":\"LM UAT Completed\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAFd1UIkAAA=\",\"items\":[]},{\"parentId\":\"Ticketing Complete\",\"text\":\"Ticketing Complete\",\"id\":\"AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAEJaEutAAA=\",\"items\":[]}]";// JsonConvert.SerializeObject(lst);
                //lst = JsonConvert.DeserializeObject<List<TreeStructure>>(lststr);
                //goto returnTypeResult;

                BEMailConfiguration objBEMailConfiguration = CatChRecordEmailService(model);
                if (objBEMailConfiguration.iMailServerTypeID.ToString().ToUpper() == "MicrosoftGraph".ToUpper())
                {
                    DataRoot = await iMailServiceGraph.GetMailFolderListGraph(objBEMailConfiguration, base.oTenant);
                }
                else
                {
                    if (objBEMailConfiguration.bUseServiceCredentialToPull == false)
                    {
                        // objBEMailConfiguration.sUserID = Encoder.HtmlEncode(objBEMailConfiguration.sUserID, false);
                        if (objBEMailConfiguration.sPassword != null && !string.IsNullOrWhiteSpace(objBEMailConfiguration.sPassword))
                        {
                            objBEMailConfiguration.sPassword = Encoder.HtmlEncode(BPA.Utility.EncryptDecrypt.Encrypt(objBEMailConfiguration.sPassword).ToString(), false);
                            DataRoot = iMailService.GetMailFolderList(objBEMailConfiguration, base.oTenant);
                        }
                    }
                    else
                    {
                        DataRoot = iMailService.GetMailFolderList(objBEMailConfiguration, base.oTenant);
                    }
                }


                if (objBEMailConfiguration.iMailServerTypeID.ToString() == "Dominos")
                {
                    foreach (DictionaryEntry item in DataRoot)
                    {
                        lst.Add(new TreeStructure { id = item.Key.ToString(), text = item.Value.ToString().Replace("(", "").Replace(")", ""), parentId = item.Value.ToString() });
                    }
                }
                else if (objBEMailConfiguration.iMailServerTypeID.ToString().ToUpper() == "MicrosoftGraph".ToUpper())
                {
                    foreach (DictionaryEntry item in DataRoot)
                    {
                        if ((item.Value as Microsoft.Graph.MailFolder).ChildFolderCount > 0)
                        {
                            lst.Add(new TreeStructure { id = item.Key.ToString(), text = (item.Value as Microsoft.Graph.MailFolder).DisplayName, parentId = (item.Value as Microsoft.Graph.MailFolder).DisplayName, items = recursiveObjects });
                        }
                        else
                        {
                            lst.Add(new TreeStructure { id = item.Key.ToString(), text = (item.Value as Microsoft.Graph.MailFolder).DisplayName, parentId = (item.Value as Microsoft.Graph.MailFolder).DisplayName });
                        }
                    }
                }
                else
                {
                    foreach (DictionaryEntry item in DataRoot)
                    {
                        if ((item.Value as Microsoft.Exchange.WebServices.Data.Folder).ChildFolderCount > 0)
                        {
                            lst.Add(new TreeStructure { id = item.Key.ToString(), text = (item.Value as Microsoft.Exchange.WebServices.Data.Folder).DisplayName, parentId = (item.Value as Microsoft.Exchange.WebServices.Data.Folder).DisplayName, items = recursiveObjects });
                        }
                        else
                        {
                            lst.Add(new TreeStructure { id = item.Key.ToString(), text = (item.Value as Microsoft.Exchange.WebServices.Data.Folder).DisplayName, parentId = (item.Value as Microsoft.Exchange.WebServices.Data.Folder).DisplayName });
                        }
                    }
                }

                //returnTypeResult:
                result.TotalCount = lst.Count;
                result.Data = lst;

            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(IEnumerable<TreeStructure>), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("GetNodeValue")]
        public async Task<MessageResponse<IEnumerable<TreeStructure>>> GetNodeValue([FromBody] TestConnectionModel model)
        {
            var result = new MessageResponse<IEnumerable<TreeStructure>>();

            TimeZone localZone = TimeZone.CurrentTimeZone;
            BETimeZoneInfo oBETimeZoneInfo = new BETimeZoneInfo();
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            //oBETimeZoneInfo.sTimeZoneID = localZone.StandardName;
            Hashtable Data = new Hashtable();
            List<TreeStructure> list = new List<TreeStructure>();
            List<TreeStructure> lst = new List<TreeStructure>();
            Hashtable DataRoot = new Hashtable();

            List<TreeStructure> recursiveObjects = new List<TreeStructure>();
            recursiveObjects.Add(new TreeStructure { id = string.Empty, text = "" });
            string strParent = model.ParentFolderName + "/";

            try
            {

                BEMailConfiguration objBEMailConfiguration = CatChRecordEmailService(model);

                if (objBEMailConfiguration.iMailServerTypeID.ToString().ToUpper() == "MicrosoftGraph".ToUpper())
                {
                    //string ParentFolderId = model.ParentFolderId;// "AAMkAGEzMGVjNjdkLWJhYjMtNDcxYy04N2U4LTEzZjkwMDhkM2I1OAAuAAAAAAAIS5HPJvxAR4eA0wrztyL-AQAFVy2B243qTblo_upE6_U3AAIUxJj-AAA=";
                    //string ParentFolderName = model.ParentFolderName;// "Sync Issues";

                    DataRoot = await iMailServiceGraph.GetFolderListGraph(objBEMailConfiguration, model.ParentFolderId.ToString(), model.ParentFolderName, base.oTenant);
                    //DataRoot = await iMailServiceGraph.GetFolderListGraph(objBEMailConfiguration, ParentFolderId, ParentFolderName, base.oTenant);

                    foreach (DictionaryEntry item in DataRoot)
                    {
                        if ((item.Value as Microsoft.Graph.MailFolder).ChildFolderCount > 0)
                        {
                            lst.Add(new TreeStructure { id = item.Key.ToString(), text = (item.Value as Microsoft.Graph.MailFolder).DisplayName, parentId = (item.Value as Microsoft.Graph.MailFolder).DisplayName, items = recursiveObjects });
                        }
                        else
                        {
                            lst.Add(new TreeStructure { id = item.Key.ToString(), text = (item.Value as Microsoft.Graph.MailFolder).DisplayName, parentId = (item.Value as Microsoft.Graph.MailFolder).DisplayName });
                        }
                    }
                }
                else
                {
                    DataRoot = iMailService.GetFolderList(objBEMailConfiguration, model.ParentFolderId.ToString(), model.ParentFolderName, base.oTenant);
                   
                    foreach (DictionaryEntry item in DataRoot)
                    {
                        if ((item.Value as Microsoft.Exchange.WebServices.Data.Folder).ChildFolderCount > 0)
                        {

                            lst.Add(new TreeStructure { id = item.Key.ToString(), text = (item.Value as Microsoft.Exchange.WebServices.Data.Folder).DisplayName, parentId = strParent + (item.Value as Microsoft.Exchange.WebServices.Data.Folder).DisplayName, items = recursiveObjects });
                        }
                        else
                        {
                            lst.Add(new TreeStructure { id = item.Key.ToString(), text = (item.Value as Microsoft.Exchange.WebServices.Data.Folder).DisplayName, parentId = strParent + (item.Value as Microsoft.Exchange.WebServices.Data.Folder).DisplayName });
                        }
                    }
                }

                result.TotalCount = lst.Count;
                result.Data = lst;

            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }

            return result;
        }

        private BEMailConfiguration CatChRecordEmailService(TestConnectionModel objMailConfigurationModel)
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            BETimeZoneInfo oBETimeZoneInfo = new BETimeZoneInfo();
            //  CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            oBETimeZoneInfo.sTimeZoneID = (localZone.StandardName == "Coordinated Universal Time") ? "UTC" : localZone.StandardName;

            BEMailConfiguration objBEMailConfiguration = new BEMailConfiguration();
            objBEMailConfiguration.iMailServerTypeID = (EmailServerType)int.Parse(objMailConfigurationModel.MailServerTypeID.ToString());
            objBEMailConfiguration.sUserID = Encoder.HtmlEncode(objMailConfigurationModel.UserID, false);
            objBEMailConfiguration.sAutoDiscoveryPath = Encoder.HtmlEncode(objMailConfigurationModel.AutoDiscoveryPath, false);
            objBEMailConfiguration.bEMSWebServerHosting = objMailConfigurationModel.EMSWebServerHostingAtClient;
            // added by ManishDwivedi
            objBEMailConfiguration.bEMSWebServerHosting = objMailConfigurationModel.EMSWebServerHostingAtClient;
            objBEMailConfiguration.ClinetID = objMailConfigurationModel.ClientID ?? "";
            objBEMailConfiguration.TenentID = objMailConfigurationModel.TenentID ?? "";
            objBEMailConfiguration.Scope = objMailConfigurationModel.Scope ?? "";
            objBEMailConfiguration.RedirectUrl = objMailConfigurationModel.RedirectUrl ?? "";
            objBEMailConfiguration.Instance = objMailConfigurationModel.Instance ?? "";
            objBEMailConfiguration.Authority = string.Format(CultureInfo.InvariantCulture, objMailConfigurationModel.Instance == null ? "" : objMailConfigurationModel.Instance, objMailConfigurationModel.TenentID == null ? "" : objMailConfigurationModel.TenentID);
            return objBEMailConfiguration;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("InsertMailConfiguration")]
        public async Task<MessageResponse<string>> InsertMailConfiguration([FromBody] EmailConfigurationModel oMailConfiguration, [FromQuery] int formID)
        {
            var result = new MessageResponse<string>();
            try
            {
                var jsonStr = JsonConvert.SerializeObject(oMailConfiguration);
                BEMailConfiguration bEMailConfiguration = CatchRecord(oMailConfiguration);
                await Task.Run(() => iMailConfigurationService.InsertData(bEMailConfiguration, formID, base.oTenant));
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                //var exMsg = ex.Message;
                //var stackTrace = ex.StackTrace;
                //var innerex = ex.InnerException;
                result.Message = _logger.Error(new LoggerInfo() { actionName = "InsertMailConfiguration", message = $"{ex?.Message} {ex?.InnerException?.Message}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateMailConfiguration")]
        public async Task<MessageResponse<string>> UpdateMailConfiguration([FromBody] EmailConfigurationModel oMailConfiguration, [FromQuery] int formID)
        {
            var result = new MessageResponse<string>();
            try
            {
                BEMailConfiguration bEMailConfiguration = CatchRecord(oMailConfiguration);
                await Task.Run(() => iMailConfigurationService.UpdateData(bEMailConfiguration, formID, base.oTenant));
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "UpdateMailConfiguration", message = $"{ex?.Message} {ex?.InnerException?.Message}", requestId = CorrelationId });
            }
            return result;
        }


        [ProducesResponseType(typeof(IEnumerable<BEMailConfiguration>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetAdvancedConfiguration")]
        public async Task<MessageResponse<IEnumerable<BEMailConfiguration>>> GetAdvancedConfiguration([FromQuery] int iCampaignID)
        {
            var result = new MessageResponse<IEnumerable<BEMailConfiguration>>();
            try
            {
                result.Data = await Task.Run(() => iMailConfigurationService.GetAdvancedConfiguration(iCampaignID, base.oTenant));
                result.TotalCount = result.Data.Count();
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "GetAdvancedConfiguration", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("InsertUpdateAdvancedConfiguration")]
        public async Task<MessageResponse<string>> InsertUpdateAdvancedConfiguration([FromBody] EmailAdvancedConfigurationModel oMailAdvConfiguration)
        {
            var result = new MessageResponse<string>();
            try
            {
                BEMailConfiguration bEMailConfiguration = CatchRecord2(oMailAdvConfiguration);
                await Task.Run(() => iMailConfigurationService.InsertUpdateAdvancedConfiguration(bEMailConfiguration, base.oTenant));
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "InsertAdvancedConfiguration", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        protected BEMailConfiguration CatchRecord(EmailConfigurationModel objMailConfigurationModel)
        {
            BEMailConfiguration obj = new BEMailConfiguration();
            obj.iStoreID = objMailConfigurationModel.StoreID;
            obj.iMailConfigID = objMailConfigurationModel.MailConfigID;
            obj.MailTemplateID = objMailConfigurationModel.AutoReplyTemplate;
            obj.iCampaignID = objMailConfigurationModel.CampaignID;
            obj.sMailBoxName = Encoder.HtmlEncode(objMailConfigurationModel.MailBoxName).Trim();
            obj.sEmailID = Encoder.HtmlEncode(objMailConfigurationModel.EmailID, false).Trim();
            obj.sUserID = Encoder.HtmlEncode(objMailConfigurationModel.UserID, false).Trim();
            obj.sPassword = objMailConfigurationModel.Password != null ? Encoder.HtmlEncode(BPA.Utility.EncryptDecrypt.Encrypt(objMailConfigurationModel.Password).ToString(), false) : null ?? "";
            obj.bUseServiceCredentialToPull = objMailConfigurationModel.UseServiceCredentialToPull;
            obj.bUseUserCredentialToSend = objMailConfigurationModel.UseUserCredentialToSend;
            obj.iMailServerTypeID = (EmailServerType)int.Parse(objMailConfigurationModel.MailServerTypeID.ToString());
            obj.iScheduleInterval = int.Parse(objMailConfigurationModel.ScheduleInterval.ToString());
            obj.sAutoDiscoveryPath = Encoder.HtmlEncode(objMailConfigurationModel.AutoDiscoveryPath, false);
            obj.sLotusServerPath = Encoder.HtmlEncode(objMailConfigurationModel.LotusServerPath, false);
            obj.sNFSFilePath = Encoder.HtmlEncode(objMailConfigurationModel.NFSFilePath, false);
            obj.bWebEnabled = objMailConfigurationModel.WebEnabled;
            obj.bEMSWebServerHosting = objMailConfigurationModel.EMSWebServerHostingAtClient;
            obj.EMSWebServerURL = objMailConfigurationModel.EMSWebServerURL ?? "";
            obj.isPasswordExpire = false;
            obj.AutoReply = objMailConfigurationModel.AutoReply;
            obj.PoolingValue = false;
            obj.IsReadMail = objMailConfigurationModel.IsReadMail;
            obj.iFolderType = objMailConfigurationModel.FolderType;
            obj.iCreatedBy = base.oUser.iUserID;
            obj.bDisabled = Convert.ToBoolean(objMailConfigurationModel.Disabled);
            obj.sLotusDomainName = Encoder.HtmlEncode(objMailConfigurationModel.LotusDomainName, false);
            obj.sLotusDomainPrefix = Encoder.HtmlEncode(objMailConfigurationModel.LotusDomainPrefix, false);
            obj.bOutofOfficeEnabled = objMailConfigurationModel.OutofOffice;
            obj.sOutofOffice = Encoder.HtmlEncode(objMailConfigurationModel.OutofOfficeText, false);
            obj.bImpersonation = objMailConfigurationModel.Impersonation;
            obj.sImpersonationIDType = string.IsNullOrEmpty(objMailConfigurationModel.ImpersonationIDType) ? 0 : (ImpersonationIDType)Enum.Parse(typeof(ImpersonationIDType), objMailConfigurationModel.ImpersonationIDType);
            obj.sImpersonationID = Encoder.HtmlEncode(objMailConfigurationModel.ImpersonationID, false);
            obj.bOutLookEnabled = objMailConfigurationModel.OutLook;
            obj.bTranslationEnabled = objMailConfigurationModel.TranslationEnabled;
            // added by ManishDwivedi
            obj.ClinetID = objMailConfigurationModel.ClientID ?? "";
            obj.TenentID = objMailConfigurationModel.TenentID ?? "";
            obj.Scope = objMailConfigurationModel.Scope ?? "";
            obj.RedirectUrl = objMailConfigurationModel.RedirectUrl ?? "";
            obj.Instance = objMailConfigurationModel.Instance ?? "";
            obj.IsForSWMIntegration = objMailConfigurationModel.IsForSWMIntegration;
            obj.bSWMEMSIntegration = objMailConfigurationModel.IsSWMEMSIntegration;
            //end

            DataTable table = new DataTable();
            table = HelperConversion.ToDataTable(objMailConfigurationModel.GridList.ToList());
            table.TableName = "XMLData";
            table.Columns.Add("MailConfigID", typeof(Int32));
            table.Columns.Add("CreatedBy", typeof(Int32));
            table.Columns.Add("CreatedOn", typeof(DateTime));
            table.AcceptChanges();
            obj.dtMailfolderdetails = table;

            /*
            obj.MailBoxType = (emailType)objMailConfigurationViewModel.FolderType;
            obj.isRunning = false;
            obj.dtMailfolderdetails = objMailConfigurationViewModel.dtMailConfigDetails;
            obj.bBCCEnabled = objMailConfigurationViewModel.bBCCEnabled;
            */
            return obj;
        }

        protected BEMailConfiguration CatchRecord2(EmailAdvancedConfigurationModel objMailAdvConfigurationModel)
        {
            BEMailConfiguration obj = new BEMailConfiguration();
            BETimeZoneInfo objBETimeZoneInfo = new BETimeZoneInfo();
            objBETimeZoneInfo.iTimeZoneID = objMailAdvConfigurationModel.TimeZoneID;
            obj.BatchFrequency = (BatchFrequencyType)Enum.Parse(typeof(BatchFrequencyType), objMailAdvConfigurationModel.BatchFrequency.ToString());
            obj.bScheduletoSameUser = objMailAdvConfigurationModel.Scheduletosameuser;
            obj.bSendmailQuiqueIdentified = objMailAdvConfigurationModel.Sendmailuniqueidentified;
            obj.oTimeZone = objBETimeZoneInfo;
            obj.iCampaignID = objMailAdvConfigurationModel.CampaignID;
            obj.iCreatedBy = base.oUser.iUserID;
            obj.bInlineEditing = objMailAdvConfigurationModel.InlineEditing;
            obj.bNeedPrintEnabled = objMailAdvConfigurationModel.NeedPrint;
            obj.bOutLookMailEnabled = objMailAdvConfigurationModel.OutLookMailEnabled;
            obj.beFileEnabled = objMailAdvConfigurationModel.NeedeFile;
            obj.bReadMailBodyEnabled = objMailAdvConfigurationModel.ReadMailBody;
            obj.bCFXEnabled = objMailAdvConfigurationModel.CFX;
            obj.bNeedTicketEnabled = objMailAdvConfigurationModel.NeedTicket;

            obj.iUploadBy = objMailAdvConfigurationModel.UploadBy;
            obj.bSensitivityEnabled = objMailAdvConfigurationModel.IsSensitivity;
            obj.strCEXlauncherPath = objMailAdvConfigurationModel.CEXlauncherPath;

            obj.bSubmitDisplayEnabled = objMailAdvConfigurationModel.IsSubmitDisplay;
            obj.strEFilePath = objMailAdvConfigurationModel.EFilePath;
            obj.strSubmitDisplay = objMailAdvConfigurationModel.SubmitDisplay;

            obj.iNeedTicketLenth = objMailAdvConfigurationModel.NeedTicketLength;
            obj.bFreshRequiredEnabled = objMailAdvConfigurationModel.IsFreshRequired;
            obj.strTicketName = objMailAdvConfigurationModel.TicketName;
            obj.bDuringUploadEnabled = objMailAdvConfigurationModel.DuringUpload;
            obj.sAssignType = objMailAdvConfigurationModel.LastAssignType;
            obj.IsAssignLast = objMailAdvConfigurationModel.IsAssignLast;
            return obj;
        }
    }
}
