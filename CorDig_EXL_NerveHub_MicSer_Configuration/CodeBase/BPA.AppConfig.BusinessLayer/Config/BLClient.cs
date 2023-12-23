using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.Datalayer.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;

namespace BPA.AppConfig.BusinessLayer.Config
{
    public class BLClient : IClientService, IDisposable
    {

        public BLClient()
        { }

        public void Dispose()
        { }

        public List<BEClientInfo> GetClientList(int iLoggedinUserID, string ClientName, bool bActiveClient, BETenant oTenant)
        {
            using (DLClient objClient = new DLClient(oTenant))
            {
                return objClient.GetClientList(iLoggedinUserID, ClientName, bActiveClient);
            }
        }

        public List<BEClientInfo> GetClientList(int iLoggedinUserID, bool bActiveClient, BETenant oTenant)
        {
            return GetClientList(iLoggedinUserID, "", bActiveClient, oTenant);
        }

        public void InsertData(BEClientInfo oClient, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oClient.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oClient.sClientName == string.Empty || oClient.sClientName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Client Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLClient objClient = new DLClient(oTenant))
            {
                objClient.ManageClientData(oClient, "Add");
            }
        }

        public void UpdateData(BEClientInfo oClient, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oClient.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (oClient.sClientName == string.Empty || oClient.sClientName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Client Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLClient objClient = new DLClient(oTenant))
            {
                objClient.ManageClientData(oClient, "Update");
            }
        }

        public List<BEClientInfo> GetClientList(int ClientID, BETenant oTenant)
        {
            using (DLClient objClient = new DLClient(oTenant))
            {
                return objClient.GetClientList(ClientID);
            }
        }

    }
}
