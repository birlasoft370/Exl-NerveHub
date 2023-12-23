using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.BIReports
{
    [Serializable]
    [DataContract]
    public class BEProcessOff : ObjectBase
    {


        /// <summary>
        /// Gets or sets the i process off id.
        /// </summary>
        /// <value>The i process off id.</value>
        [DataMember]
        public int iProcessOffId { get; set; }


        /// <summary>
        /// Gets or sets the i process id.
        /// </summary>
        /// <value>The i process id.</value>
        [DataMember]
        public int iProcessId { get; set; }


        /// <summary>
        /// Gets or sets the dt start date.
        /// </summary>
        /// <value>The dt start date.</value>
        [DataMember]
        public DateTime dtStartDate { get; set; }


        /// <summary>
        /// Gets or sets the dt end date.
        /// </summary>
        /// <value>The dt end date.</value>
        [DataMember]
        public DateTime dtEndDate { get; set; }


        /// <summary>
        /// Gets or sets the s description.
        /// </summary>
        /// <value>The s description.</value>
        [DataMember]
        public string sDescription { get; set; }

        /// <summary>
        /// Gets or sets the i client id.
        /// </summary>
        /// <value>The i client id.</value>
        [DataMember]
        public int iClientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the s.
        /// </summary>
        /// <value>The name of the s.</value>
        [DataMember]
        public string sName { get; set; }


        /// <summary>
        /// Gets or sets the dt week info.
        /// </summary>
        /// <value>The dt week info.</value>
        [DataMember]
        public DataTable dtWeekInfo { get; set; }


        /// <summary>
        /// Gets or sets the i month.
        /// </summary>
        /// <value>The i month.</value>
        [DataMember]
        public int iMonth { get; set; }

        /// <summary>
        /// Gets or sets the i year.
        /// </summary>
        /// <value>The i year.</value>
        [DataMember]
        public int iYear { get; set; }

        /// <summary>
        /// Gets or sets the i day.
        /// </summary>
        /// <value>The i day.</value>
        [DataMember]
        public int iDay { get; set; }

    }
}
