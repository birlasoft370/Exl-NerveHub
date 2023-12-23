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
using System.Data;

namespace BPA.Security.BusinessLayer.Security
{
    ///// <summary>
    ///// ERP JOBRole Mapping
    ///// </summary>
    ///// <seealso cref="BPA.Security.ServiceContracts.Security.IERPJobRoleMapService" />
    ///// <seealso cref="System.IDisposable" />
    ///// <seealso cref="BPA.Security.BusinessEntity.IDataOperation{BPA.Security.BusinessEntity.Security.BEErpJobRoleMap}" />
    ///// 
    /// <summary>
    /// 
    /// </summary>
    public class BLERPJobRoleMap : IERPJobRoleMapService, IDisposable, IDataOperation<BEErpJobRoleMap>
    {

        #region Constructor and Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="BLERPJobRoleMap" /> class.
        /// </summary>
        public BLERPJobRoleMap()
        { }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        #endregion

        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="oJobRole">The object job role.</param>
        /// <param name="FormID">The form identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        //public void InsertData(BEErpJobRoleMap oJobRole, int FormID, BETenant oTenant)
        //{
        //    if (!CheckPermission.hasPermission(FormID, oJobRole.iCreatedBy, PermissionSet.ADD))
        //    {
        //        throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
        //    }

        //    using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
        //    {
        //        oRole.InsertData(oJobRole);
        //    }
        //}
        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name = "oJobRole" > The object job role.</param>
        /// <param name = "iApprover" > The approver.</param>
        /// <param name = "FormID" > The form identifier.</param>
        /// <param name = "oTenant" > The Tenant.</param>
        /// <exception cref = "ExceptionHandler.ExceptionType.BusinessLogicCustomException" ></ exception >
        //public void InsertData(BEErpJobRoleMap oJobRole, int iApprover, int FormID, BETenant oTenant)
        //{
        //    if (!CheckPermission.hasPermission(FormID, oJobRole.iCreatedBy, PermissionSet.ADD))
        //    {
        //        throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
        //    }

        //    using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
        //    {
        //        oRole.InsertData(oJobRole, iApprover, 1);
        //    }
        //}

        public void InsertData(BEErpJobRoleMap oJobRole, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oJobRole.iCreatedBy, PermissionSet.ADD))
            {
                //throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
            {
                oRole.InsertData(oJobRole);
            }
        }
        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="oJobRole">The object job role.</param>
        /// <param name="iApprover">The approver.</param>
        /// <param name="FormID">The form identifier.</param>
        /// <param name="oTenant">The tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        //public void UpdateData(BEErpJobRoleMap oJobRole, int iApprover, int FormID, BETenant oTenant)
        //{
        //    if (!CheckPermission.hasPermission(FormID, oJobRole.iCreatedBy, PermissionSet.UPDATE))
        //    {
        //        throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
        //    }
        //    using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
        //    {
        //        oRole.InsertData(oJobRole, iApprover, 2);
        //    }
        //}
        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="oJobRole">The object job role.</param>
        /// <param name="FormID">The form identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void UpdateData(BEErpJobRoleMap oJobRole, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oJobRole.iCreatedBy, PermissionSet.UPDATE))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
            {
                oRole.UpdateData(oJobRole);
            }
        }

        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="oJobRole">The object job role.</param>
        /// <param name="iApprover">The i approver.</param>
        /// <param name="FormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        //public void DeleteData(BEErpJobRoleMap oJobRole, int iApprover, int FormID, BETenant oTenant)
        //{
        //    if (!CheckPermission.hasPermission(FormID, oJobRole.iCreatedBy, PermissionSet.DELETE))
        //    {
        //        throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
        //    }
        //    using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
        //    {
        //        oRole.InsertData(oJobRole, iApprover, 3);
        //    }
        //}
        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="oJobRole">The object job role.</param>
        /// <param name="FormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void DeleteData(BEErpJobRoleMap oJobRole, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oJobRole.iCreatedBy, PermissionSet.DELETE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
            {
                oRole.DeleteData(oJobRole);
            }
        }
        ///// <summary>
        ///// Gets the job role map.
        ///// </summary>
        ///// <param name="JobID">The job ID.</param>
        ///// <param name="RoleID">The role ID.</param>
        ///// <param name="oTenant">The Tenant.</param>
        ///// <returns></returns>
        //public List<BEErpJobRoleMap> GetJobRoleMap(int JobID, int RoleID, BETenant oTenant)
        //{
        //    using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
        //    {
        //        return oRole.GetJobRoleMap(JobID, RoleID);
        //    }
        //}

        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEJobCodeInfo> GetJob(BETenant oTenant)
        {
            using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
            {
                return oRole.GetJob();
            }
        }

        //added by santosh 20200717
        /// <summary>
        /// GetRoleList
        /// </summary>
        /// <param name="oTenant"></param>
        /// <returns></returns>
        public List<BERoleInfo> GetRoleList(BETenant oTenant)
        {

            using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
            {
                return oRole.GetRoleList(true,"");
            }
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="RoleId"></param>
       /// <param name="FormID"></param>
       /// <param name="oTenant"></param>
       /// <returns></returns>
        public List<BERoleInfo> GetUserRoleApproverList(int RoleId, int FormID,BETenant oTenant)
        {
            using (DLERPJobRoleMap oPer = new DLERPJobRoleMap(oTenant))
            {
                return oPer.GetUserRoleApproverList(RoleId, FormID);
            }
        }

        ///// <summary>
        ///// Gets the job role map.
        ///// </summary>
        ///// <param name="JobDesc">The job desc.</param>
        ///// <param name="oTenant">The Tenant.</param>
        ///// <returns></returns>
        //public List<BEErpJobRoleMap> GetJobRoleMap(string JobDesc, BETenant oTenant)
        //{
        //    using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
        //    {
        //        return oRole.GetJobRoleMap(JobDesc);
        //    }
        //}

        /// <summary>
        /// Gets the job role map.
        /// </summary>
        /// <param name="JobDesc">The job desc.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEErpJobRoleMap> GetJobRoleMap(string JobDesc, BETenant oTenant)
        {
            using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
            {
                return oRole.GetJobRoleMap(JobDesc);
            }
        }

        /// <summary>
        /// Gets the job role map.
        /// </summary>
        /// <param name="RoleJobID">The role job ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEErpJobRoleMap> GetJobRoleMap(int RoleJobID, BETenant oTenant)
        {
            using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
            {
                return oRole.GetJobRoleMap(RoleJobID);
            }
        }
        

        /// <summary>
        /// Gets the Multi job role map.
        /// </summary>
        /// <param name="RoleJobID">The role job ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<RoleFormAccessModel> GetRoleFormMap(string RoleJobID, BETenant oTenant)
        {
            using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
            {
                return oRole.GetRoleFormMapReport(RoleJobID);
            }
        }

        /// <summary>
        /// Gets the Multi job role map.
        /// </summary>
        /// <param name="RoleJobID">The role job ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEErpJobRoleMap> GetMultiJobRoleMap(string RoleJobID, BETenant oTenant)
        {
            using (DLERPJobRoleMap oRole = new DLERPJobRoleMap(oTenant))
            {
                return oRole.GetMultiJobRoleMap(RoleJobID);
            }
        }



        ///// <summary>
        ///// Gets the ERPJobRole Request status.
        ///// </summary>
        ///// <param name="iUser">The user.</param>
        ///// <param name="dtFromDate">The from date.</param>
        ///// <param name="dtToDate">The to date.</param>
        ///// <param name="oTenant">The Tenant.</param>
        ///// <returns></returns>
        //public DataSet GetERPJobRoleRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate, BETenant oTenant)
        //{
        //    using (DLERPJobRoleMap oPer = new DLERPJobRoleMap(oTenant))
        //    {
        //        return oPer.GetERPJobRoleRequestStatus(iUser, dtFromDate, dtToDate);
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUser"></param>
        /// <param name="dtFromDate"></param>
        /// <param name="dtToDate"></param>
        /// <param name="oTenant"></param>
        /// <returns></returns>
        public List<BEErpJobRoleMap> GetERPJobRoleRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate, BETenant oTenant)
        {
            using (DLERPJobRoleMap oPer = new DLERPJobRoleMap(oTenant))
            {
                return oPer.GetERPJobRoleRequestStatus(iUser, dtFromDate, dtToDate);
            }
        }
        ///// <summary>
        ///// Gets the ERPJobRole approval list.
        ///// </summary>
        ///// <param name="iUser">The i user.</param>
        ///// <param name="oTenant">The Tenant.</param>
        ///// <returns></returns>
        //public DataSet GetERPJobRoleApprovalList(int iUser, BETenant oTenant)
        //{
        //    using (DLERPJobRoleMap oPer = new DLERPJobRoleMap(oTenant))
        //    {
        //        return oPer.GetERPJobRoleApprovalList(iUser);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUser"></param>
        /// <param name="oTenant"></param>
        /// <returns></returns>
        public List<BEErpJobRoleMap> GetERPJobRoleApprovalList(int iUser, BETenant oTenant)
        {
            using (DLERPJobRoleMap oPer = new DLERPJobRoleMap(oTenant))
            {
                return oPer.GetERPJobRoleApprovalList(iUser);
            }
        }
        /// <summary>
        /// Rejects ERPJobRole Request
        /// </summary>
        /// <param name="iRequestID">The request identifier.</param>
        /// <param name="iUserID">The user identifier.</param>
        /// <param name="iFormID">The form identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void RejectERPJobRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, iUserID, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLERPJobRoleMap oPer = new DLERPJobRoleMap(oTenant))
            {
                oPer.RejectERPJobRoleRequest(iRequestID, iUserID);
            }
        }
        /// <summary>
        /// Approve ERPJobRole request
        /// </summary>
        /// <param name="iRequestID">The request identifier.</param>
        /// <param name="iUserID">The user identifier.</param>
        /// <param name="iFormID">The form identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void ApproveERPJobRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, iUserID, PermissionSet.ADD))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLERPJobRoleMap oPer = new DLERPJobRoleMap(oTenant))
            {
                oPer.ApproveERPJobRoleRequest(iRequestID, iUserID);
            }
        }
        /// <summary>
        /// Cancels ERP Job Role Request
        /// </summary>
        /// <param name="iRequestID">The request identifier.</param>
        /// <param name="iUserID">The user identifier.</param>
        /// <param name="iFormID">The  form identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void CancelERPJobRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, iUserID, PermissionSet.DELETE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLERPJobRoleMap oPer = new DLERPJobRoleMap(oTenant))
            {
                oPer.CancelERPJobRoleRequest(iRequestID, iUserID);
            }
        }
    }
}
