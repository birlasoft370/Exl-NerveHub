/* Copyright © 2012 ExlService (I) Pvt. Ltd.
 * project Name                 :   
 * Class Name                   :   
 * Namespace                    :   
 * Purpose                      :
 * Description                  :
 * Dependency                   :   
 * Related Table                :
 * Related Class                :
 * Related StoreProcdure        :
 * Author                       :   
 * Created on                   :   
 * Reviewed on                  :          
 * Reviewed by                  :
 * Tested on                    :
 * Tested by                    :
 * Modification history         :
 * modify1 on                   :
 * modify1 By                   :
 * Overall effect               :
 */


using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.Datalayer.Security;
using BPA.Security.ServiceContracts.Security;
using System;
namespace BPA.Security.BusinessLayer.Security
{
    public class BLSession : ISessionService, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BLSession"/> class.
        /// </summary>
        public BLSession() { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }

        /// <summary>
        /// Inserts the sesssion start.
        /// </summary>
        /// <param name="oBESession">The o BE session.</param>
        /// <returns></returns>
        public int InsertSessionStart(BESession oBESession, BETenant oTenant)
        {
            using (DLSession oDLSession = new DLSession(oTenant))
            {
              return  oDLSession.InsertSessionStart(oBESession);
            }
        }
        /// <summary>
        /// Inserts the sesssion end.
        /// </summary>
        /// <param name="SessionID">The session ID.</param>
        public void InsertSessionEnd(int SessionID, BETenant oTenant)
        {
            using (DLSession oDLSession = new DLSession(oTenant))
            {
                oDLSession.InsertSessionEnd (SessionID);
            }
        }
        /// <summary>
        /// Gets the user ID.
        /// </summary>
        /// <param name="SessionID">The session ID.</param>
        /// <returns></returns>
        public int GetUserID(int SessionID, BETenant oTenant)
       {
           using (DLSession oDLSession = new DLSession(oTenant))
           {
               return oDLSession.GetUserID(SessionID);
           }
       }
       /// <summary>
       /// Gets the session ID.
       /// </summary>
       /// <param name="oBESession">The o BE session.</param>
       /// <returns></returns>
        public int GetSessionID(BESession oBESession, BETenant oTenant)
       {
           using (DLSession oDLSession = new DLSession(oTenant))
           {
               return oDLSession.GetSessionID(oBESession);
           }
       }
       /// <summary>
       /// Finals the logout.
       /// </summary>
       /// <param name="UserID">The user ID.</param>
        public void FinalLogout(int UserID, BETenant oTenant)
       {
           using (DLSession oDLSession = new DLSession(oTenant))
           {
               oDLSession.FinalLogout(UserID);
           }
       }
        public string InsertErorLog(string ErrorMessage, BETenant oTenant)
        {
            using (DLSession oDLSession = new DLSession(oTenant))
            {
              return  oDLSession.InsertErorLog(ErrorMessage);
            }
        }
    }
   
}
