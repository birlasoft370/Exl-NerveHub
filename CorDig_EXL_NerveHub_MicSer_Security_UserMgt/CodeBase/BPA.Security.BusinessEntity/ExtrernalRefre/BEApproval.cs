using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.BusinessEntity.ExtrernalRefre
{
    [Serializable]
    [DataContract]
    public class BEApproval
    {
        //[DataMember]
        //public string Text { get; set; }

        ///// <summary>
        ///// Gets or sets the i calibration id.
        ///// </summary>
        ///// <value>The i calibration id.</value>
        //[DataMember]
        //public int Value { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string UserName { get; set; }

    }
}
