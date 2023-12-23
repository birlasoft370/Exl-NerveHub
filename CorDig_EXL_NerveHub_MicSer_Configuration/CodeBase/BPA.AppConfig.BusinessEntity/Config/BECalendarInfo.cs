using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Config
{
    [Serializable]
    public class BECalendarInfo : ObjectBase
    {
        private int _iCalendarID;
        private string _sCalendarName;
        private string _sDescription;

        private int _iCalendarDataId;
        private int _iMonth;
        private int _iYear;
        private int _iWeek;
        private DateTime _dtStartDate;
        private DateTime _dtEndDate;
        private DataTable _dtCalanderData;

        /// <summary>
        /// Initializes a new instance of the <see cref="BECalendarInfo"/> class.
        /// </summary>
        public BECalendarInfo()
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="BECalendarInfo"/> class.
        /// </summary>
        /// <param name="iCalendarID">The i Calendar ID.</param>
        /// <param name="sCalendarName">Name of the s Calendar.</param>
        /// <param name="sDescription">The s description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [bdisabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BECalendarInfo(int iCalendarID, string sCalendarName, string sDescription, bool bDisabled, int iCreatedBy)
        {
            _iCalendarID = iCalendarID;
            _sCalendarName = sCalendarName;
            _sDescription = sDescription;

            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BECalendarInfo"/> class.
        /// </summary>
        /// <param name="iCalendarDataId">The i calendar data id.</param>
        /// <param name="iCalendarId">The i calendar id.</param>
        /// <param name="iMonth">The i month.</param>
        /// <param name="iYear">The i year.</param>
        /// <param name="iWeek">The i week.</param>
        /// <param name="dtStartDate">The dt start date.</param>
        /// <param name="dtEndDate">The dt end date.</param>
        /// <param name="bDisabled">if set to <c>true</c> [b disabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        /// <param name="sCalendarName">Name of the s calendar.</param>
        public BECalendarInfo(int iCalendarDataId, int iCalendarId, int iMonth, int iYear, int iWeek, DateTime dtStartDate, DateTime dtEndDate, bool bDisabled, int iCreatedBy, string sCalendarName)
        {
            _iCalendarDataId = iCalendarDataId;
            _iCalendarID = iCalendarId;
            _iMonth = iMonth;
            _iYear = iYear;
            _iWeek = iWeek;
            _dtStartDate = dtStartDate;
            _dtEndDate = dtEndDate;

            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
            _sCalendarName = sCalendarName;
        }

        /// <summary>
        /// Gets or sets  Calendar ID.
        /// </summary>
        /// <value> Calendar ID.</value>

        public int iCalendarID
        {
            get
            {
                return _iCalendarID;
            }
            set
            {
                _iCalendarID = value;
            }
        }

        /// <summary>
        /// Gets or sets  Calendar Name.
        /// </summary>
        /// <value> Calendar Name.</value>

        public string sCalendarName
        {
            get
            {
                return _sCalendarName;
            }
            set
            {
                _sCalendarName = value;
            }
        }

        /// <summary>
        /// Gets or sets  Calendar Decription.
        /// </summary>
        /// <value>Calendar Description.</value>


        public string sDescription
        {
            get
            {
                return _sDescription;
            }
            set
            {
                _sDescription = value;
            }
        }


        /// <summary>
        /// Gets or sets the i calendar data id.
        /// </summary>
        /// <value>The i calendar data id.</value>
        public int iCalendarDataId
        {
            get
            {
                return _iCalendarDataId;
            }
            set
            {
                _iCalendarDataId = value;
            }
        }


        /// <summary>
        /// Gets or sets the i month.
        /// </summary>
        /// <value>The i month.</value>
        public int iMonth
        {
            get
            {
                return _iMonth;
            }
            set
            {
                _iMonth = value;
            }
        }

        /// <summary>
        /// Gets or sets the i year.
        /// </summary>
        /// <value>The i year.</value>
        public int iYear
        {
            get
            {
                return _iYear;
            }
            set
            {
                _iYear = value;
            }
        }

        /// <summary>
        /// Gets or sets the i week.
        /// </summary>
        /// <value>The i week.</value>
        public int iWeek
        {
            get
            {
                return _iWeek;
            }
            set
            {
                _iWeek = value;
            }
        }

        /// <summary>
        /// Gets or sets the dt start date.
        /// </summary>
        /// <value>The dt start date.</value>
        public DateTime dtStartDate
        {
            get
            {
                return _dtStartDate;
            }
            set
            {
                _dtStartDate = value;
            }
        }


        /// <summary>
        /// Gets or sets the dt end date.
        /// </summary>
        /// <value>The dt end date.</value>
        public DateTime dtEndDate
        {
            get
            {
                return _dtEndDate;
            }
            set
            {
                _dtEndDate = value;
            }
        }
        /// <summary>
        /// Gets or sets the dt calander data.
        /// </summary>
        /// <value>The dt calander data.</value>
        public DataTable dtCalanderData
        {
            get { return _dtCalanderData; }
            set { _dtCalanderData = value; }
        }
    }
}
