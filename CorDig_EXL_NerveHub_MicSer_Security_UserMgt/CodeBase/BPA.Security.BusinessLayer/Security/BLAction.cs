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

using BPA.Security.BusinessEntity;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.Datalayer.Security;
using BPA.Security.ServiceContracts.Security;
using System;
using System.Collections.Generic;

namespace BPA.Security.BusinessLayer.Security
{
    /// <summary>
    /// Permission Action
    /// </summary>
    /// <seealso cref="BPA.Security.ServiceContracts.Security.IActionService" />
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="BPA.Security.BusinessEntity.IDataOperation{BPA.Security.BusinessEntity.Security.BEActionInfo}" />
    public class BLAction : IActionService, IDisposable, IDataOperation<BEActionInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BLAction" /> class.
        /// </summary>
        public BLAction()
        { }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }

        /// <summary>
        /// Gets the action list.
        /// </summary>
        /// <param name="oTenant">The tenant.</param>
        /// <returns></returns>
        public List<BEActionInfo> GetActionList(BETenant oTenant)
        {
            return GetActionList("",oTenant);
        }

        /// <summary>
        /// Gets the action list.
        /// </summary>
        /// <param name="ActionName">Name of the action.</param>
        /// <param name="oTenant">The tenant.</param>
        /// <returns></returns>
        public List<BEActionInfo> GetActionList(string ActionName, BETenant oTenant)
        {
            using (DLAction oAction = new DLAction(oTenant))
            {
                return oAction.GetActionList(ActionName);
            }
        }

        /// <summary>
        /// Inserts the record.
        /// </summary>
        /// <param name="objAction">action object.</param>
        /// <param name="iFormID">The form identifier.</param>
        /// <param name="oTenant">The tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">Action Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField</exception>
        public void InsertData(BEActionInfo objAction, int iFormID, BETenant oTenant)
        {
            if (objAction.sActionName == string.Empty || objAction.sActionName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Action Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLAction obAction = new DLAction(oTenant))
            {
                obAction.InsertData(objAction);
            }

        }

        /// <summary>
        /// Updates the record.
        /// </summary>
        /// <param name="objAction">Action Object.</param>
        /// <param name="iFormID">The form identifier.</param>
        /// <param name="oTenant">The tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void UpdateData(BEActionInfo objAction, int iFormID, BETenant oTenant)
        {
            if (objAction.sActionName == string.Empty || objAction.sActionName == "")
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLAction obAction = new DLAction(oTenant))
            {
                obAction.UpdateData(objAction);
            }

        }

        /// <summary>
        /// Delete Action Record
        /// </summary>
        /// <param name="oAction">Action Object</param>
        /// <param name="iFormID">The form identifier.</param>
        /// <param name="oTenant">The tenant.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void DeleteData(BEActionInfo oAction, int iFormID, BETenant oTenant)
        {
            throw new System.NotImplementedException();
        }
    }
}
