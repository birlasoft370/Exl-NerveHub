using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Security
{
    [Serializable]
    [DataContract]
    public class BELocalization
    {
        [DataMember]
        public string CultureInfo { get; set; }
        [DataMember]
        public string TimeZone { get; set; }

    }
}
