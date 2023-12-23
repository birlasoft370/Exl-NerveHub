using MicUI.Configuration.Helper;
using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.Security;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.ClientInfoSetup
{
    public interface IClientService
    {
        List<BEClientInfo> GetClientList(bool isActive, string? searchText = null);
        BEClientInfo GetClientById(int clientID);
        string InsertData(ClientModel oClient);
        string UpdateData(ClientModel oClient);
        List<BEClientInfo> GetClientAccessList(int iAgentID, bool bActiveClient);
    }

    public class ClientService : IClientService
    {
        private readonly IConfigApiService _configService;
        private readonly ISecurityApiService _securityService;
        public ClientService(IConfigApiService configService, ISecurityApiService securityService)
        {
            _configService = configService;
            _securityService = securityService;
        }

        public List<BEClientInfo> GetClientList(bool isActive, string? searchText = null)
        {
            var result = _configService.GetClientAsync(searchText, isActive).GetAwaiter().GetResult();
            return result.data;
        }

        public BEClientInfo GetClientById(int clientID)
        {
            var result = _configService.GetClientByIdAsync(clientID).GetAwaiter().GetResult();
            return result.data;
        }

        public string InsertData(ClientModel oClient)
        {
            var result = _configService.AddClientAsync(oClient).GetAwaiter().GetResult();

            if (result != null && result.status)
            {
                return result.data;
            }
            else
            {
                //if (ex.Number == 547)
                //{
                //    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                //}
                if (result.message.Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.Contains(GlobalConstant.ExlSpecificClientAlreadyDefined))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_ClientAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }

        }

        public string UpdateData(ClientModel oClient)
        {
            var result = _configService.UpdateClientAsync(oClient).GetAwaiter().GetResult();
            if (result != null && result.status)
            {
                return result.data;
            }
            else
            {
                //if (ex.Number == 547)
                //{
                //    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                //}
                if (result.message.Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.Contains(GlobalConstant.ExlSpecificClientAlreadyDefined))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_ClientAlready);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
        public List<BEClientInfo> GetClientAccessList(int iAgentID, bool bActiveClient)
        {
            var result = _securityService.GetClientAccessListAsync(iAgentID, bActiveClient).GetAwaiter().GetResult();
            return result.data ?? new List<BEClientInfo>();
        }
    }
}
