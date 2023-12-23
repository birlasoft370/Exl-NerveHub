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
    /// CDS Appliction Form and Action
    /// </summary>
    /// <seealso cref="BPA.Security.ServiceContracts.Security.IFormActionService" />
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="BPA.Security.BusinessEntity.IDataOperation{BPA.Security.BusinessEntity.Security.BEFormAction}" />
    public class BLFormAction : IFormActionService, IDisposable, IDataOperation<BEFormAction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BLFormAction" /> class.
        /// </summary>
        public BLFormAction()
        { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }

        /// <summary>
        /// Gets the form list.
        /// </summary>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEFormAction> GetFormList(BETenant oTenant)
        {
            return GetFormList("", oTenant);
        }
        /// <summary>
        /// Gets the form list.
        /// </summary>
        /// <param name="sFormName">Name of the s form.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEFormAction> GetFormList(string sFormName, BETenant oTenant)
        {
            using (DLFormAction oform = new DLFormAction(oTenant))
            {
                return oform.GetFormList(sFormName);
            }
        }

        /// <summary>
        /// Gets the form list.
        /// </summary>
        /// <param name="iFormID">The i form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEFormAction> GetFormList(int iFormID, BETenant oTenant)
        {
            using (DLFormAction oform = new DLFormAction(oTenant))
            {
                return oform.GetFormList(iFormID);
            }
        }

        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="objFormAction">The object form action.</param>
        /// <param name="FormID">The form identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">
        /// Form Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField
        /// or
        /// </exception>
        public void InsertData(BEFormAction objFormAction, int FormID, BETenant oTenant)
        {
            if (objFormAction.sFormName == string.Empty || objFormAction.sFormName == "")
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Form Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(FormID, objFormAction.iCreatedBy, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLFormAction oFormAction = new DLFormAction(oTenant))
            {
                oFormAction.InsertData(objFormAction);
            }

        }

        /// <summary>
        /// Updates the record.
        /// </summary>
        /// <param name="objFormAction">The obj form action.</param>
        /// <param name="FormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">
        /// </exception>
        public void UpdateData(BEFormAction objFormAction, int FormID, BETenant oTenant)
        {
            if (objFormAction.sFormName == string.Empty || objFormAction.sFormName == "")
            {
                //throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(FormID, objFormAction.iCreatedBy, PermissionSet.UPDATE))
            {
                //throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            using (DLFormAction oFormAction = new DLFormAction(oTenant))
            {
                oFormAction.UpdateData(objFormAction);
            }

        }
        /// <summary>
        /// Deletes the record.
        /// </summary>
        /// <param name="objFormAction">The obj form action.</param>
        /// <param name="FormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void DeleteData(BEFormAction objFormAction, int FormID, BETenant oTenant)
        {
          
            if (!CheckPermission.hasPermission(FormID, objFormAction.iCreatedBy, PermissionSet.DELETE))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLFormAction oFormAction = new DLFormAction(oTenant))
            {
                oFormAction.DeleteData(objFormAction);
            }

        }
    }
}
