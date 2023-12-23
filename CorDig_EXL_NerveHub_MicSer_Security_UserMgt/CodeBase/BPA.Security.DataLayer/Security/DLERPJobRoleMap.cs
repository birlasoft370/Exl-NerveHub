
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace BPA.Security.Datalayer.Security
{
    /// <summary>
    /// ERP Job Role Mapping
    /// </summary>
    public class DLERPJobRoleMap : IDisposable
    {
        #region Field
        private BETenant _oTenant = null;

        private const string SQL_SELECT_JOBROLE = @"Select JRM.Disabled,RoleName,RM.RoleID,JD.JOBID,JOBCode,JobDesc,RoleJobID 
                                                    ,JRM.MappedON,JRM.DefaultRole
                                                    from Config.tblERPJobRoleMapping (NOLOCK) JRM
                                                    INNER JOIN Config.tblRoleMaster(NOLOCK) RM
	                                                    ON RM.ROLEID = JRM.ROLEID
                                                    INNER JOIN Config.tblJobDesc (NOLOCK)JD
	                                                    ON JD.JobID=JRM.JOBID
                                                    ";
        private const string SQL_SELECT_JOB = @"select * from Config.tblJobDesc(NOLOCK) where Disabled=0 order by JobDesc";

        private const string SQL_SELECT_JOBDESC = @"Select JRM.Disabled,RoleName,RM.RoleID,JD.JOBID,JOBCode,JobDesc,RoleJobID 
                                                    ,JRM.MappedON,JRM.DefaultRole
                                                    from Config.tblERPJobRoleMapping (NOLOCK) JRM
                                                    INNER JOIN Config.tblRoleMaster(NOLOCK) RM
	                                                    ON RM.ROLEID = JRM.ROLEID
                                                    INNER JOIN Config.tblJobDesc (NOLOCK)JD
	                                                    ON JD.JobID=JRM.JOBID
                                                     where JD.JobDesc like @JobDesc   
                                                    ";

        private const string SQL_SELECT_JOBROLEID = @"Select JRM.Disabled,RoleName,RM.RoleID,JD.JOBID,JOBCode,JobDesc,RoleJobID 
                                                    ,JRM.MappedON,JRM.DefaultRole
                                                    from Config.tblERPJobRoleMapping (NOLOCK) JRM
                                                    INNER JOIN Config.tblRoleMaster(NOLOCK) RM
	                                                    ON RM.ROLEID = JRM.ROLEID
                                                    INNER JOIN Config.tblJobDesc (NOLOCK)JD
	                                                    ON JD.JobID=JRM.JOBID
                                                     where RoleJobID=@RoleJobID   
                                                    ";

        private const string SQL_SELECT_MultiJOBROLEID = @"Select JRM.Disabled,RoleName,RM.RoleID,JD.JOBID,JOBCode,JobDesc,RoleJobID 
                                                    ,JRM.MappedON,JRM.DefaultRole,
  JRM.CreatedOn  as CreatedOn,  JRM.ModifiedOn as ModifiedOn
                                                    from Config.tblERPJobRoleMapping (NOLOCK) JRM
                                                    INNER JOIN Config.tblRoleMaster(NOLOCK) RM
	                                                    ON RM.ROLEID = JRM.ROLEID
                                                    INNER JOIN Config.tblJobDesc (NOLOCK)JD
	                                                    ON JD.JobID=JRM.JOBID
                                                     where RoleJobID in (SELECT * FROM dbo.fn_Split(@RoleJobID, ',')) 
                                                     ";


        private const string SQL_SELECT_JOBID = @"  Select JRM.Disabled, RoleName,RM.RoleID,JD.JOBID,JOBCode,JobDesc,RoleJobID 
                                                    ,JRM.MappedON,JRM.DefaultRole
                                                    from Config.tblERPJobRoleMapping (NOLOCK) JRM
                                                    INNER JOIN Config.tblRoleMaster(NOLOCK) RM
	                                                    ON RM.ROLEID = JRM.ROLEID
                                                    INNER JOIN Config.tblJobDesc (NOLOCK)JD
	                                                    ON JD.JobID=JRM.JOBID
                                                    Where JRM.JobID=@JobID";

        private const string SQL_SELECT_ROLEID = @" Select JRM.Disabled,RoleName,RM.RoleID,JD.JOBID,JOBCode,JobDesc,RoleJobID 
                                                    ,JRM.MappedON,JRM.DefaultRole
                                                    from Config.tblERPJobRoleMapping (NOLOCK) JRM
                                                    INNER JOIN Config.tblRoleMaster(NOLOCK) RM
	                                                    ON RM.ROLEID = JRM.ROLEID
                                                    INNER JOIN Config.tblJobDesc (NOLOCK)JD
	                                                    ON JD.JobID=JRM.JOBID
                                                    Where JRM.RoleID=@RoleID";

        private const string SQL_INSERT_JOBROLE = @"insert into Config.tblERPJobRoleMapping(ROLEID,JOBID,DISABLED,CREATEDBY,MAPPEDON,DefaultRole)
                                                    VALUES (@RoleID,@JobID,@Disabled,@CreatedBy,@MappedON,@DefaultRole)";

        private const string SQL_UPDATE_JOBROLE = @"UPDATE Config.tblERPJobRoleMapping SET ROLEID=@RoleID,JOBID=@JobID,DISABLED=@Disabled,MODIFIEDBY=@CreatedBy,DefaultRole=@DefaultRole,
                                                    MappedON=@MappedON
                                                    Where RoleJobID=@RoleJobID";

        //added by santosh 20200717
        private const string SQL_SELECT_ROLES = @"SELECT ROLEID,ROLENAME,Description,isnull(LevelID,0) as LevelID, disabled,isnull(IsClientRole,0) as IsClientRole,createdby FROM [Config].[tblRoleMaster] (Nolock) WHERE Disabled=0   Order By ROLENAME";
        private const string SQL_SELECT_ROLESALL = @"SELECT ROLEID,ROLENAME,Description,isnull(LevelID,0) as LevelID, disabled,isnull(IsClientRole,0) as IsClientRole,createdby FROM [Config].[tblRoleMaster] (Nolock) WHERE RoleName like @RoleName  Order By ROLENAME";
        private const string SQL_SELECT_ROLESBYID = @"SELECT ROLEID,ROLENAME,Description, isnull(LevelID,0) as LevelID, isnull(SecurityGroup,0) as SecurityGroup,disabled,isnull(IsClientRole,0) as IsClientRole,createdby FROM [Config].[tblRoleMaster] (Nolock) WHERE RoleID = @RoleID";
        //code ended by santosh
        private const string SQL_DELETE_JOBROLE = @"DELETE FROM Config.tblERPJobRoleMapping Where RoleJobID=@RoleJobID";

        //added by santosh 20200717
        private const string SP_SELECT_ROLEAPPROVERLIST = @"Config.USP_GetRoleApproverList";

        private const string SP_GET_ERPJOBROLE_REQUESTSTATUS = @"[Config].[Usp_GetERPRoleJobMapRequestStatus]";
        //code ended by santosh
        private const string SP_GET_ERPJOBROLE_APPROVALLIST = @"[Config].[Usp_GetERPJobRoleApprovalList]";
        // private const string SP_APPROVE_ERPJOBROLEREQUEST = @"dbo.USP_CDS_ApproveERPJobRoleRequest";
        //code ended by santosh
        private const string SP_APPROVE_ERPJOBROLEREQUEST = @"[Config].[USP_ApproveERPJobRoleRequest]";

        private const string SP_REJECT_ERPJOBROLEREQUEST = @"[Config].[USP_RejectERPRoleJobRequest]";
        // private const string SP_REJECT_ERPJOBROLEREQUEST = @"dbo.USP_CDS_RejectERPRoleJobRequest";
        //added  by santosh
        private const string SP_CANCEL_ERPJOBROLEREQUEST = @"[Config].[Usp_Cancel_ERPJobRoleRequest]";
        // private const string SP_INSERT_ERPJOBROLEREQUEST = @"dbo.Usp_CDS_Insert_ERPJobRoleMapping";

        //added by santosh 20200718
        private const string SP_INSERT_ERPJOBROLEREQUEST = @"[Config].[Usp_Insert_ERPJobRoleMapping]";
        private const string SP_GET_ERPJOBROLEMAPREQUEST = @"[Config].[Usp_RPT_GetRoleFormsAccess]";


        //added by santosh 20200717
        private const string PARAM_ROLENAME = "@RoleName";
        private const string PARAM_FORMID = "@FormID";
        //code ended by santosh
        private const string PARAM_ISDEFAULTROLE = "@DefaultRole";
        private const string PARAM_ROLEJOBID = "@RoleJobID";
        private const string PARAM_ROLE = "@Role";
        private const string PARAM_JOBDESC = "@JobDesc";
        private const string PARAM_ROLEID = "@RoleID";
        private const string PARAM_JOBID = "@JobID";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MAPPEDON = "@MappedON";
        private const string PARAM_APPROVERID = "@ApproverId";
        private const string PARAM_MODE = "@Mode";
        private const string PARAM_USERID = "@UserId";
        private const string PARAM_FROMDATE = "@FromDate";
        private const string PARAM_TODATE = "@ToDate";
        private const string PARAM_REQUESTID = "@RequestId";
        #endregion

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLPermission"/> class.
        /// </summary>
        public DLERPJobRoleMap(BETenant oTenant)
        { _oTenant = oTenant; }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <returns></returns>
        public List<BEJobCodeInfo> GetJob()
        {
            List<BEJobCodeInfo> lJob = new List<BEJobCodeInfo>();


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_JOB);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEJobCodeInfo oJob = new BEJobCodeInfo { iJOBID = int.Parse(rdr["JobID"].ToString()), iJobCode = int.Parse(rdr["JOBCode"].ToString()), sJobDesc = rdr["JobDesc"].ToString() };

                    lJob.Add(oJob);
                    oJob = null;
                }

            }

            return lJob;
        }

        #region Get the job role map.
        /// <summary>
        /// Gets the job role map.
        /// </summary>
        /// <param name="JobID">The job ID.</param>
        /// <param name="RoleID">The role ID.</param>
        /// <returns></returns>
        public List<BEErpJobRoleMap> GetJobRoleMap(int JobID, int RoleID)
        {
            List<BEErpJobRoleMap> lJobRole = new List<BEErpJobRoleMap>();


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (JobID == 0 && RoleID == 0) dbCommand = db.GetSqlStringCommand(SQL_SELECT_JOBROLE);
            else if (RoleID > 0) dbCommand = db.GetSqlStringCommand(SQL_SELECT_ROLEID);
            else dbCommand = db.GetSqlStringCommand(SQL_SELECT_JOBID);

            db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.String, RoleID);
            db.AddInParameter(dbCommand, PARAM_JOBID, DbType.Int32, JobID);


            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEErpJobRoleMap oRoleJob = new BEErpJobRoleMap
                    {
                        oJob = new BEJobCodeInfo { iJOBID = int.Parse(rdr["JobID"].ToString()), iJobCode = int.Parse(rdr["JOBCode"].ToString()), sJobDesc = rdr["JobDesc"].ToString() },
                        oRole = new BERoleInfo { iRoleID = int.Parse(rdr["RoleID"].ToString()), sRoleName = rdr["RoleName"].ToString() }
                        ,
                        iMappedOn = int.Parse(rdr["MappedOn"].ToString()),
                        bDefaultRole = bool.Parse(rdr["DefaultRole"].ToString())
                    };

                    lJobRole.Add(oRoleJob);
                    oRoleJob = null;
                }

            }

            return lJobRole;
        }

        /// <summary>
        /// Gets the job role map.
        /// </summary>
        /// <param name="JObDesc">The Job desc.</param>
        /// <returns></returns>
        public List<BEErpJobRoleMap> GetJobRoleMap(string JObDesc)
        {
            List<BEErpJobRoleMap> lJobRole = new List<BEErpJobRoleMap>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = dbCommand = db.GetSqlStringCommand(SQL_SELECT_JOBDESC);
            db.AddInParameter(dbCommand, PARAM_JOBDESC, DbType.String, "" + JObDesc + "%");

            try
            {
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BEErpJobRoleMap oRoleJob = new BEErpJobRoleMap
                        {

                            iRoleJobID = int.Parse(rdr["RoleJobID"].ToString()),

                            //iRoleJobID = int.Parse(rdr["RoleJobID"].ToString()),
                            oJob = new BEJobCodeInfo { iJOBID = int.Parse(rdr["JobID"].ToString()), iJobCode = int.Parse(rdr["JOBCode"].ToString()), sJobDesc = rdr["JobDesc"].ToString() },
                            oRole = new BERoleInfo { iRoleID = int.Parse(rdr["RoleID"].ToString()), sRoleName = rdr["RoleName"].ToString() },
                            //,
                            //iMappedOn = int.Parse(rdr["MappedOn"].ToString())
                            //,
                            bDisabled = bool.Parse(rdr["Disabled"].ToString()),
                            //,
                            //bDefaultRole = bool.Parse(rdr["DefaultRole"].ToString())


                        };

                        lJobRole.Add(oRoleJob);
                        oRoleJob = null;
                    }

                }
            }
            catch (Exception ex)
            {
                string strerrromsg = ex.Message;
            }

            return lJobRole;
        }

        /// <summary>
        /// Gets the Report for Role Form Access.
        /// </summary>
        /// <param name="RoleJobID">The role job ID.</param>
        /// <returns></returns>
        public List<RoleFormAccessModel> GetRoleFormMapReport(string RoleJobID)
        {
            List<RoleFormAccessModel> lJobRole = new List<RoleFormAccessModel>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = dbCommand = db.GetStoredProcCommand(SP_GET_ERPJOBROLEMAPREQUEST);
            db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.String, RoleJobID);
            DataSet ds = new DataSet();
            ds = db.ExecuteDataSet(dbCommand);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string add = "";
                string delete = "";
                string view = "";                    
                string approve = "";
                string modify = "";
                if (!string.IsNullOrEmpty(dr["Approve"].ToString()) && dr["Approve"].ToString() !="0")
                {
                    approve = "Yes";
                }
                if (!string.IsNullOrEmpty(dr["Add"].ToString()) && dr["Add"].ToString() != "0")
                {
                    add = "Yes";
                }
                if (!string.IsNullOrEmpty(dr["Delete"].ToString()) && dr["Delete"].ToString() != "0")
                {
                    delete = "Yes";
                }
                if (!string.IsNullOrEmpty(dr["Modify"].ToString()) && dr["Modify"].ToString() != "0")
                {
                    modify = "Yes";
                }
                if (!string.IsNullOrEmpty(dr["View"].ToString()) && dr["View"].ToString() != "0")
                {
                    view = "Yes";
                }

                RoleFormAccessModel oRoleJob = new RoleFormAccessModel
                {

                    RoleId = int.Parse(dr["RoleId"].ToString()),
                    RoleName = dr["RoleName"].ToString(),
                    FormId = dr["FormId"].ToString(),
                    Description = dr["Description"].ToString(),
                    FormName = dr["Form Name"].ToString(),
                    Add = add,
                    Approve = approve,
                    Delete = delete,
                    Modify = modify,
                    View = view

                };                               
                lJobRole.Add(oRoleJob);
                oRoleJob = null;
            }
            
            return lJobRole;
        }


        /// <summary>
        /// Gets the Multiple job role map.
        /// </summary>
        /// <param name="RoleJobID">The role job ID.</param>
        /// <returns></returns>
        public List<BEErpJobRoleMap> GetMultiJobRoleMap(string RoleJobID)
        {
            List<BEErpJobRoleMap> lJobRole = new List<BEErpJobRoleMap>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = dbCommand = db.GetSqlStringCommand(SQL_SELECT_MultiJOBROLEID);
            db.AddInParameter(dbCommand, PARAM_ROLEJOBID, DbType.String, RoleJobID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEErpJobRoleMap oRoleJob = new BEErpJobRoleMap
                    {
                        iRoleJobID = int.Parse(rdr["RoleJobID"].ToString()),
                        oJob = new BEJobCodeInfo
                        {
                            iJOBID = int.Parse(rdr["JobID"].ToString()),
                            iJobCode = int.Parse(rdr["JOBCode"].ToString()),
                            sJobDesc = rdr["JobDesc"].ToString()
                        },
                        oRole = new BERoleInfo
                        {
                            iRoleID = int.Parse(rdr["RoleID"].ToString()),
                            sRoleName = rdr["RoleName"].ToString()
                        }
                        ,
                        iMappedOn = int.Parse(rdr["MappedOn"].ToString())
                        ,
                        bDisabled = bool.Parse(rdr["Disabled"].ToString())
                       ,
                        bDefaultRole = bool.Parse(rdr["DefaultRole"].ToString())
                        ,
                        dCreateDate = Convert.ToDateTime(rdr["CreatedOn"].ToString())
                        ,
                        dModifyDate = Convert.ToDateTime(rdr["ModifiedOn"].ToString())
                    };
                    //rdr["DefaultRole"].ToString()=="0"?false:true
                    lJobRole.Add(oRoleJob);
                    oRoleJob = null;
                }

            }

            return lJobRole;
        }



        /// <summary>
        /// Gets the job role map.
        /// </summary>
        /// <param name="RoleJobID">The role job ID.</param>
        /// <returns></returns>
        public List<BEErpJobRoleMap> GetJobRoleMap(int RoleJobID)
        {
            List<BEErpJobRoleMap> lJobRole = new List<BEErpJobRoleMap>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = dbCommand = db.GetSqlStringCommand(SQL_SELECT_JOBROLEID);
            db.AddInParameter(dbCommand, PARAM_ROLEJOBID, DbType.String, RoleJobID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEErpJobRoleMap oRoleJob = new BEErpJobRoleMap
                    {
                        iRoleJobID = int.Parse(rdr["RoleJobID"].ToString()),
                        oJob = new BEJobCodeInfo { iJOBID = int.Parse(rdr["JobID"].ToString()), iJobCode = int.Parse(rdr["JOBCode"].ToString()), sJobDesc = rdr["JobDesc"].ToString() },
                        oRole = new BERoleInfo { iRoleID = int.Parse(rdr["RoleID"].ToString()), sRoleName = rdr["RoleName"].ToString() }
                        ,
                        iMappedOn = int.Parse(rdr["MappedOn"].ToString())
                        ,
                        bDisabled = bool.Parse(rdr["Disabled"].ToString())
                        ,
                        bDefaultRole = bool.Parse(rdr["DefaultRole"].ToString())
                    };

                    lJobRole.Add(oRoleJob);
                    oRoleJob = null;
                }

            }

            return lJobRole;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oJobRole"></param>
        //public void InsertData(BEErpJobRoleMap oJobRole)
        //{
        //    try
        //    {
        //        Database db = DL_Shared.dbFactory(_oTenant);

        //        DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_JOBROLE);
        //        db.AddInParameter(dbCommand, PARAM_ROLE, DbType.String, oJobRole.oRole.sRoleName);
        //        db.AddInParameter(dbCommand, PARAM_JOBDESC, DbType.String, oJobRole.oJob.sJobDesc);
        //        db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, oJobRole.oRole.iRoleID);
        //        db.AddInParameter(dbCommand, PARAM_JOBID, DbType.String, oJobRole.oJob.iJOBID);
        //        db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.String, oJobRole.bDisabled);
        //        db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oJobRole.iCreatedBy);
        //        db.AddInParameter(dbCommand, PARAM_MAPPEDON, DbType.Int32, oJobRole.iMappedOn);
        //        db.AddInParameter(dbCommand, PARAM_ISDEFAULTROLE, DbType.Boolean, oJobRole.bDefaultRole);
        //        db.ExecuteNonQuery(dbCommand);
        //    }
        //    catch (System.Data.SqlClient.SqlException ex)
        //    {
        //        if (ex.Number == 547)
        //        {
        //            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
        //        }
        //        if (ex.Number == 2627)
        //        {
        //            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
        //        }
        //        throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
        //    }
        //}

        /// <summary>
        /// Insert ERP JOB Role Mapping Data
        /// </summary>
        /// <param name="oJobRole"></param>
        /// <param name="iApprover"></param>
        /// <param name="iMode"></param>
        //public void InsertData(BEErpJobRoleMap oJobRole, int iApprover, int iMode)
        //{
        //    try
        //    {
        //        Database db = DL_Shared.dbFactory(_oTenant);

        //        DbCommand dbCommand = db.GetStoredProcCommand(SP_INSERT_ERPJOBROLEREQUEST);
        //        db.AddInParameter(dbCommand, PARAM_ROLE, DbType.String, oJobRole.oRole.sRoleName);
        //        db.AddInParameter(dbCommand, PARAM_JOBDESC, DbType.String, oJobRole.oJob.sJobDesc);
        //        db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, oJobRole.oRole.iRoleID);
        //        db.AddInParameter(dbCommand, PARAM_JOBID, DbType.String, oJobRole.oJob.iJOBID);
        //        db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.String, oJobRole.bDisabled);
        //        db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oJobRole.iCreatedBy);
        //        db.AddInParameter(dbCommand, PARAM_MAPPEDON, DbType.Int32, oJobRole.iMappedOn);
        //        db.AddInParameter(dbCommand, PARAM_ISDEFAULTROLE, DbType.Boolean, oJobRole.bDefaultRole);
        //        db.AddInParameter(dbCommand, PARAM_APPROVERID, DbType.Int32, iApprover);
        //        db.AddInParameter(dbCommand, PARAM_MODE, DbType.Int32, iMode);
        //        db.ExecuteNonQuery(dbCommand);
        //    }
        //    catch (System.Data.SqlClient.SqlException ex)
        //    {
        //        if (ex.Number == 547)
        //        {
        //            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
        //        }
        //        if (ex.Number == 2627)
        //        {
        //            throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
        //        }
        //        throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
        //    }
        //}

        public void InsertData(BEErpJobRoleMap oJobRole)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbCommand = db.GetStoredProcCommand(SP_INSERT_ERPJOBROLEREQUEST);
                db.AddInParameter(dbCommand, PARAM_ROLE, DbType.String, oJobRole.oRole.sRoleName);
                db.AddInParameter(dbCommand, PARAM_JOBDESC, DbType.String, oJobRole.oJob.sJobDesc);
                db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, oJobRole.oRole.iRoleID);
                db.AddInParameter(dbCommand, PARAM_JOBID, DbType.String, oJobRole.oJob.iJOBID);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.String, oJobRole.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oJobRole.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_MAPPEDON, DbType.Int32, oJobRole.iMappedOn);
                db.AddInParameter(dbCommand, PARAM_ISDEFAULTROLE, DbType.Boolean, oJobRole.bDefaultRole);
                db.AddInParameter(dbCommand, PARAM_APPROVERID, DbType.Int32, oJobRole.iApprover);
                db.AddInParameter(dbCommand, PARAM_MODE, DbType.Int32, Convert.ToInt32(oJobRole.iMode));
                db.ExecuteNonQuery(dbCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                if (ex.Number == 50000)
                {
                   // throw new DataAccessCustomException(ex.Message);
                }
                throw ex;
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="oJobRole">The o job role.</param>
        public void UpdateData(BEErpJobRoleMap oJobRole)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_JOBROLE);
                db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, oJobRole.oRole.iRoleID);
                db.AddInParameter(dbCommand, PARAM_JOBID, DbType.String, oJobRole.oJob.iJOBID);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.String, oJobRole.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oJobRole.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_ROLEJOBID, DbType.Int32, oJobRole.iRoleJobID);
                db.AddInParameter(dbCommand, PARAM_MAPPEDON, DbType.Int32, oJobRole.iMappedOn);
                db.AddInParameter(dbCommand, PARAM_ISDEFAULTROLE, DbType.Boolean, oJobRole.bDefaultRole);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }


        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="oJobRole">The object job role.</param>
        public void DeleteData(BEErpJobRoleMap oJobRole)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE_JOBROLE);
                db.AddInParameter(dbCommand, PARAM_ROLEJOBID, DbType.Int32, oJobRole.iRoleJobID);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        #region ERP Job Role Mapping Request
        /// <summary>
        ///// Gets the ERPJobRole Request status.
        ///// </summary>
        ///// <param name="iUser">The i user.</param>
        ///// <param name="dtFromDate">The dt from date.</param>
        ///// <param name="dtToDate">The dt to date.</param>
        ///// <returns></returns>
        //public DataSet GetERPJobRoleRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate)
        //{
        //    DataSet ds = new DataSet();
        //    Database db = DL_Shared.dbFactory(_oTenant);
        //    DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_ERPJOBROLE_REQUESTSTATUS);
        //    db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUser);
        //    db.AddInParameter(dbCommand, PARAM_FROMDATE, DbType.DateTime, dtFromDate);
        //    db.AddInParameter(dbCommand, PARAM_TODATE, DbType.DateTime, dtToDate);
        //    db.LoadDataSet(dbCommand, ds, "User");
        //    return ds;
        //}

        public List<BEErpJobRoleMap> GetERPJobRoleRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate)
        {
            List<BEErpJobRoleMap> lJobRole = new List<BEErpJobRoleMap>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_ERPJOBROLE_REQUESTSTATUS);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUser);
            db.AddInParameter(dbCommand, PARAM_FROMDATE, DbType.DateTime, dtFromDate);
            db.AddInParameter(dbCommand, PARAM_TODATE, DbType.DateTime, dtToDate);
            //db.LoadDataSet(dbCommand, ds, "User");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEErpJobRoleMap oRoleJob = new BEErpJobRoleMap
                    {
                        RequestId = int.Parse(rdr["RequestId"].ToString()),
                        RequestedBy = Convert.ToString(rdr["RequestedBy"]),
                        RequestedOn = Convert.ToDateTime(rdr["RequestedOn"]),
                        Approver = Convert.ToString(rdr["Approver"]),
                        RequestStatus = Convert.ToString(rdr["RequestStatus"]).ToString(),
                        //Cancelable = int.Parse(rdr["Cancelable"].ToString()),
                        RequestDesc = Convert.ToString(rdr["RequestDesc"]).ToString()

                    };

                    lJobRole.Add(oRoleJob);
                    oRoleJob = null;
                }

            }
            return lJobRole;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUser"></param>
        /// <returns></returns>
        public List<BEErpJobRoleMap> GetERPJobRoleApprovalList(int iUser)
        {
            DataSet ds = new DataSet();
            List<BEErpJobRoleMap> lJobRole = new List<BEErpJobRoleMap>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_ERPJOBROLE_APPROVALLIST);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUser);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEErpJobRoleMap oRoleJob = new BEErpJobRoleMap
                    {
                        RequestId = int.Parse(rdr["RequestId"].ToString()),
                        RequestedBy = Convert.ToString(rdr["RequestedBy"]),
                        RequestedOn = Convert.ToDateTime(rdr["RequestedOn"]),
                        Approver = Convert.ToString(rdr["Approver"]),
                        RequestStatus = Convert.ToString(rdr["RequestStatus"]).ToString(),
                        //Cancelable = int.Parse(rdr["Cancelable"].ToString()),
                        RequestDesc = Convert.ToString(rdr["RequestDesc"]).ToString()

                    };

                    lJobRole.Add(oRoleJob);
                    oRoleJob = null;
                }

            }
            return lJobRole;
            //db.LoadDataSet(dbCommand, ds, "User");
            //return ds;
        }


        ///// <summary>
        ///// Gets the ERPJobRole approval list.
        ///// </summary>
        ///// <param name="iUser">The i user.</param>
        ///// <returns></returns>
        //public DataSet GetERPJobRoleApprovalList(int iUser)
        //{
        //    DataSet ds = new DataSet();
        //    Database db = DL_Shared.dbFactory(_oTenant);
        //    DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_ERPJOBROLE_APPROVALLIST);
        //    db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUser);
        //    db.LoadDataSet(dbCommand, ds, "User");
        //    return ds;
        //}
        /// <summary>
        /// Rejects ERPJobRole Request
        /// </summary>
        /// <param name="iRequestID"></param>
        /// <param name="iUserID"></param>
        public void RejectERPJobRoleRequest(int iRequestID, int iUserID)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_REJECT_ERPJOBROLEREQUEST);
            db.AddInParameter(dbCommand, PARAM_REQUESTID, DbType.Int32, iRequestID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        /// <summary>
        /// Approve ERPJobRole request
        /// </summary>
        /// <param name="iRequestID"></param>
        /// <param name="iUserID"></param>
        public void ApproveERPJobRoleRequest(int iRequestID, int iUserID)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_APPROVE_ERPJOBROLEREQUEST);
            db.AddInParameter(dbCommand, PARAM_REQUESTID, DbType.Int32, iRequestID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                if (ex.Number == 50000)
                {
                   // throw new DataAccessCustomException(ex.Message);
                }
                throw ex;
                //throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        /// <summary>
        /// Cancels ERP Job Role Request
        /// </summary>
        /// <param name="iRequestID"></param>
        /// <param name="iUserID"></param>
        public void CancelERPJobRoleRequest(int iRequestID, int iUserID)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_CANCEL_ERPJOBROLEREQUEST);
            db.AddInParameter(dbCommand, PARAM_REQUESTID, DbType.Int32, iRequestID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw;// new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw;// new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                //throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }

        //



        //added by santosh 20200717
        /// <summary>
        /// GetRoleList
        /// </summary>
        /// <param name="bActiveRoles"></param>
        /// <param name="sRoleName"></param>
        /// <returns></returns>
        public List<BERoleInfo> GetRoleList(bool bActiveRoles, string sRoleName)
        {
            List<BERoleInfo> lRoleInfo = new List<BERoleInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (bActiveRoles)
            { dbCommand = db.GetSqlStringCommand(SQL_SELECT_ROLES); }
            else
            { dbCommand = db.GetSqlStringCommand(SQL_SELECT_ROLESALL); }

            db.AddInParameter(dbCommand, PARAM_ROLENAME, DbType.String, "%" + sRoleName + "%");
            //DataSet ds = new DataSet();
            //db.LoadDataSet(dbCommand, ds, "Roles");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    try
                    {
                        BERoleInfo item = new BERoleInfo(Convert.ToInt32(rdr["ROLEID"]), rdr["ROLENAME"].ToString() + " " + (Convert.ToBoolean(rdr["disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToInt32(rdr["LevelID"]), Convert.ToBoolean(rdr["disabled"]), Convert.ToInt32(rdr["createdby"]));
                        lRoleInfo.Add(item);
                    }catch{}
                  
                }
            }
            return lRoleInfo;
        }

        /// <summary>
        /// GetUserRoleApproverList
        /// </summary>
        /// <param name="iRoleID"></param>
        /// <param name="iFormID"></param>
        /// <returns></returns>
        public List<BERoleInfo> GetUserRoleApproverList(int iRoleID, int iFormID)
        {
            List<BERoleInfo> lUserRoleApprover = new List<BERoleInfo>();
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_ROLEAPPROVERLIST);
            db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, iRoleID);
            db.AddInParameter(dbCommand, PARAM_FORMID, DbType.Int32, iFormID);
            db.LoadDataSet(dbCommand, ds, "Approvers");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BERoleInfo item = new BERoleInfo(Convert.ToInt32(rdr["Userid"]), rdr["Agent"].ToString() + " " + (Convert.ToBoolean(rdr["disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToInt32(rdr["LevelID"]), Convert.ToBoolean(rdr["disabled"]), Convert.ToInt32(rdr["createdby"]));
                    lUserRoleApprover.Add(item);
                }
            }
            return lUserRoleApprover;

        }

        #endregion

    }
}
