using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Config
{
    [Serializable]
    [DataContract]
    public class BEProcessSLA : ObjectBase
    {
        private DateTime _dStartDate;
        private DateTime _dEndDate;
        private DateTime _dPilotStartDate;
        private DateTime _dPilotEndDate;

        #region public Properties
        /// <summary>
        /// Gets or sets the startdate.
        /// </summary>
        /// <value>The startdate.</value>
        [DataMember]
        public DateTime dStartDate
        {
            get { return _dStartDate; }
            set { _dStartDate = value; }
        }

        /// <summary>
        /// Gets or sets the enddate.
        /// </summary>
        /// <value>The enddate.</value>
        [DataMember]
        public DateTime dEndDate
        {
            get { return _dEndDate; }
            set { _dEndDate = value; }
        }

        /// <summary>
        /// Gets or sets the pilot startdate.
        /// </summary>
        /// <value>The startdate.</value>
        [DataMember]
        public DateTime dPilotStartDate
        {
            get { return _dPilotStartDate; }
            set { _dPilotStartDate = value; }
        }

        /// <summary>
        /// Gets or sets the pilot enddate.
        /// </summary>
        /// <value>The enddate.</value>
        [DataMember]
        public DateTime dPilotEndDate
        {
            get { return _dPilotEndDate; }
            set { _dPilotEndDate = value; }
        }

        /// <summary>
        /// Gets or sets the  stage.
        /// </summary>
        /// <value>The  stage.</value>
        [DataMember]
        public string sStage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        [DataMember]
        public string sFileName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the process SLAID.
        /// </summary>
        /// <value>The process SLAID.</value>
        [DataMember]
        public int iProcessSLAID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the process ID.
        /// </summary>
        /// <value>The process ID.</value>
        [DataMember]
        public int iProcessID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ERP process ID.
        /// </summary>
        /// <value>The ERP process ID.</value>
        [DataMember]
        public int iERPProcessID
        {
            get;
            set;
        }

        /// <summary>
        /// Get Set File Contents
        /// </summary>
        [DataMember]
        public byte[] aryFileData
        {
            get;
            set;
        }

        #endregion

        #region Constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessSLAMaster"/> class.
        /// </summary>
        public BEProcessSLA()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessSLA"/> class.
        /// </summary>
        /// <param name="ProcessSLAID">The process SLAID.</param>
        /// <param name="ProcessID">The process ID.</param>
        /// <param name="StartDate">The start date.</param>
        /// <param name="EndDate">The end date.</param>
        /// <param name="Stage">The stage.</param>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="Disabled">if set to <c>true</c> [disabled].</param>
        /// <param name="CreatedBy">The created by.</param>
        public BEProcessSLA(int ProcessSLAID, int ProcessID, DateTime StartDate, DateTime EndDate, string Stage, string FileName, bool Disabled, int CreatedBy)
        {
            iProcessSLAID = ProcessSLAID;
            iProcessID = ProcessID;
            dStartDate = StartDate;
            dEndDate = EndDate;
            sStage = Stage;
            sFileName = FileName;
            base.bDisabled = Disabled;
            base.iCreatedBy = CreatedBy;
        }

        #endregion
    }
}
