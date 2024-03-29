﻿using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.Datalayer;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace BPA.Security.DataLayer.ExternalRef
{
    /// <summary>
    ///  DL Facility 
    /// </summary>
    public class DLFacility : IDisposable
    {
        #region Field
        private BETenant _oTenant = null;

        private const string SQL_SELECT_FACILITYALL = @"SELECT FacilityID,LocationID,FacilityName,Description,Disabled FROM Config.tblFacilities WHERE FacilityName like @FacilityName";
        private const string SQL_SELECT_ACTIVEFACILITY = @"SELECT FacilityID,LocationID,FacilityName,Description,Disabled FROM Config.tblFacilities WHERE FacilityName like @FacilityName AND Disabled=0";

        private const string SQL_SELECT_FACILITYBYLOCATION = @"SELECT FacilityID,LocationID,FacilityName,Description,Disabled FROM Config.tblFacilities WHERE LocationID = @LocationID";
        private const string SQL_SELECT_FACILITYID = @"SELECT FacilityID,LocationID,FacilityName,Description,Disabled FROM Config.tblFacilities WHERE FacilityID = @FacilityID";

        private const string SQL_MANAGE_FACILITY = @"Config.Usp_ManageFacility";


        private const string PARAM_LOCATIONID = "@LocationID";
        private const string PARAM_FACILITYID = "@FacilityID";
        private const string PARAM_FACILITYNAME = "@FacilityName";
        private const string PARAM_FACILITYDESC = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";

        private const string PARAM_ACTION = "@Action";
        #endregion

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLLocation"/> class.
        /// </summary>
        public DLFacility(BETenant oTenant)
        { _oTenant = oTenant; }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        #region GetFacility List

        /// <summary>
        /// Gets the Facility list.
        /// </summary>
        /// <param name="bActiveFacility">if set to <c>true</c> [b active Facility].</param>
        /// <returns>
        /// If bStatus is true
        /// return List of all the Facility (Active or Inactive)
        /// else List of all the Active Facility
        /// </returns>
        public List<BEFacility> GetFacilityList(bool bActiveFacility)
        {
            return GetFacilityList("", bActiveFacility);
        }


        /// <summary>
        /// Gets the Facility list.
        /// </summary>
        /// <param name="FacilityName">Name of the Facility.</param>
        /// <param name="bActiveFacility">if set to <c>true</c> [b active Facility].</param>
        /// <returns>
        /// If bStatus is true,retunr List of all the Active Facility
        /// </returns>
        public List<BEFacility> GetFacilityList(string FacilityName, bool bActiveFacility)
        {
            List<BEFacility> lFacility = new List<BEFacility>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (bActiveFacility)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_ACTIVEFACILITY);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_FACILITYALL);
            }
            db.AddInParameter(dbCommand, PARAM_FACILITYNAME, DbType.String, "%" + FacilityName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEFacility objFacility = new BEFacility(Convert.ToInt32(rdr["FacilityID"]), Convert.ToInt32(rdr["LocationID"]), rdr["FacilityName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lFacility.Add(objFacility);
                    objFacility = null;
                }

            }
            return lFacility;
        }
        /// <summary>
        /// Get The Facility Data
        /// </summary>
        public List<BEFacility> GetFacilityList(int iFacilityID)
        {
            List<BEFacility> lFacility = new List<BEFacility>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_FACILITYID);
            db.AddInParameter(dbCommand, PARAM_FACILITYID, DbType.Int32, iFacilityID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEFacility objFacility = new BEFacility(Convert.ToInt32(rdr["FacilityID"]), Convert.ToInt32(rdr["LocationID"]), rdr["FacilityName"].ToString(), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lFacility.Add(objFacility);
                    objFacility = null;
                }
            }
            return lFacility;

        }


        /// <summary>
        /// Gets the Location wise Facility list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="iLocationID">The Location ID.</param>
        /// <returns></returns>
        public List<BEFacility> GetLocationWiseFacilityList(int iLocationID)
        {
            List<BEFacility> lFacility = new List<BEFacility>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_FACILITYBYLOCATION);
            db.AddInParameter(dbCommand, PARAM_LOCATIONID, DbType.Int32, iLocationID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEFacility objFacility = new BEFacility(Convert.ToInt32(rdr["FacilityID"]), Convert.ToInt32(rdr["LocationID"]), rdr["FacilityName"].ToString(), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lFacility.Add(objFacility);
                    objFacility = null;
                }
            }
            return lFacility;

        }
        #endregion

        #region Manage Facility
        /// <summary>
        /// Insert Facility data.
        /// </summary>
        /// <param name="oFacility">Facility.</param>
        public void ManageFacility(BEFacility oFacility, string Action)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SQL_MANAGE_FACILITY);
                db.AddInParameter(dbCommand, PARAM_FACILITYID, DbType.Int32, oFacility.iFacilityID);
                db.AddInParameter(dbCommand, PARAM_LOCATIONID, DbType.Int32, oFacility.iLocationID);
                db.AddInParameter(dbCommand, PARAM_FACILITYNAME, DbType.String, oFacility.sFacilityName);
                db.AddInParameter(dbCommand, PARAM_FACILITYDESC, DbType.String, oFacility.sFacilityDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oFacility.bDisabled);
                if (Action == "Add" || Action == "Delete")
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oFacility.iCreatedBy);
                else
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oFacility.iModifiedBy);
                db.AddInParameter(dbCommand, PARAM_ACTION, DbType.String, Action);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                  //  throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                //if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_FacilityAlready))
                //{
                //  //  throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_FacilityAlready);
                //}
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        #endregion




    }
}

