using BPA.Security.BusinessEntity;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.DataLayer.ExternalRef;
using BPA.Security.ServiceContract.ExternalRef;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.BusinessLayer.ExternalRef
{
    /// <summary>
    /// EXL Client
    /// </summary>
    //[ObjectPooling(MinPoolSize=0, MaxPoolSize=1), JustInTimeActivation(true)]
    public class BLClient : IClientService, IDisposable
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLClient"/> class.
        /// </summary>
        public BLClient()
        { }

        public void Dispose()
        { }
        ///// <summary>
        ///// This method is called by the infrastructure before the object is put back into the pool. Override this method to vote on whether the object is put back into the pool.
        ///// </summary>
        ///// <returns>
        ///// true if the serviced component can be pooled; otherwise, false.
        ///// </returns>
        //protected override bool CanBePooled()
        //{
        //    return true;
        //}


        #endregion

        #region Get Clients List
        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <returns></returns>
        //[AutoComplete]
        public List<BEClientInfo> GetClientList(int iLoggedinUserID, bool bActiveClient, BETenant oTenant)
        {
            return GetClientList(iLoggedinUserID, "", bActiveClient, oTenant);
        }
        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <returns></returns>
        public List<BEClientInfo> GetClientList(int iLoggedinUserID, string ClientName, bool bActiveClient, BETenant oTenant)
        {
            using (DLClient objClient = new DLClient(oTenant))
            {
                return objClient.GetClientList(iLoggedinUserID, ClientName, bActiveClient);
            }
        }
        /// <summary>
        /// Gets the client Access list.
        /// </summary>
        /// <returns></returns>
        public List<BEClientInfo> GetClientAccessList(int iLoggedinUserID, int iAgentID, bool bActiveClient, BETenant oTenant)
        {
            using (DLClient objClient = new DLClient(oTenant))
            {
                return objClient.GetClientAccessList(iLoggedinUserID, iAgentID, bActiveClient);
            }
        }
        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="Month">The month.</param>
        /// <param name="Year">The year.</param>
        /// <param name="bActiveClient">if set to <c>true</c> [b active client].</param>
        /// <returns></returns>
        public List<BEClientInfo> GetClientList(int iLoggedinUserID, string Month, string Year, bool bActiveClient, BETenant oTenant)
        {
            using (DLClient objClient = new DLClient(oTenant))
            {
                return objClient.GetClientList(iLoggedinUserID, Month, Year, bActiveClient);
            }
        }
        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="ClientName">Name of the client.</param>
        /// <param name="Month">The month.</param>
        /// <param name="Year">The year.</param>
        /// <param name="bActiveClient">if set to <c>true</c> [b active client].</param>
        /// <returns></returns>
        public List<BEClientInfo> GetClientList(int iLoggedinUserID, string ClientName, string Month, string Year, bool bActiveClient, BETenant oTenant)
        {
            using (DLClient objClient = new DLClient(oTenant))
            {
                return objClient.GetClientList(iLoggedinUserID, ClientName, Month, Year, bActiveClient);
            }
        }
        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <param name="Month">The month.</param>
        /// <param name="Year">The year.</param>
        /// <param name="bActiveClient">if set to <c>true</c> [b active client].</param>
        /// <returns></returns>
        public List<BEClientInfo> GetClientList(string Month, string Year, bool bActiveClient, BETenant oTenant)
        {
            using (DLClient objClient = new DLClient(oTenant))
            {
                return objClient.GetClientList(Month, Year, bActiveClient);
            }
        }

        public List<BEClientInfo> GetClientListDataUtility(int iLoggedinUserID, string ClientName, bool bActiveClient, BETenant oTenant)
        {
            using (DLClient objClient = new DLClient(oTenant))
            {
                return objClient.GetClientListDataUtility(iLoggedinUserID, ClientName, bActiveClient);
            }
        }
        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <param name="ClientID">The client ID.</param>
        /// <returns></returns>
        public List<BEClientInfo> GetClientList(int ClientID, BETenant oTenant)
        {
            using (DLClient objClient = new DLClient(oTenant))
            {
                return objClient.GetClientList(ClientID);
            }
        }
        /// <summary>
        /// Gets the ERP client.
        /// </summary>
        /// <param name="ClientID">The client ID.</param>
        /// <returns></returns>
        public DataTable GetERPClient(int ClientID, BETenant oTenant)
        {
            using (DLClient objClient = new DLClient(oTenant))
            {
                return objClient.GetERPClient(ClientID);
            }
        }
        #endregion

        #region Insert Client Details
        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="oClient">client.</param>
        /// <param name="FormID">form ID.</param>
        public void InsertData(BEClientInfo oClient, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oClient.iCreatedBy, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oClient.sClientName == string.Empty || oClient.sClientName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Client Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLClient objClient = new DLClient(oTenant))
            {
                objClient.ManageClientData(oClient, "Add");
            }
        }
        #endregion

        #region Update Client Details
        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="oClient">client.</param>
        /// <param name="FormID">form ID.</param>
        public void UpdateData(BEClientInfo oClient, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oClient.iCreatedBy, PermissionSet.UPDATE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (oClient.sClientName == string.Empty || oClient.sClientName == "")
            {
                //throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Client Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLClient objClient = new DLClient(oTenant))
            {
                objClient.ManageClientData(oClient, "Update");
            }
        }
        #endregion

        #region Delete Client Details
        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="oClient">client.</param>
        /// <param name="FormID">form ID.</param>
        public void DeleteData(BEClientInfo oClient, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oClient.iCreatedBy, PermissionSet.DELETE))
            {
                //throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLClient objClient = new DLClient(oTenant))
            {
                objClient.ManageClientData(oClient, "Delete");
            }
        }
        #endregion

    }
}
