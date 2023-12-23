using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.BusinessEntity
{
    [Serializable]
    [DataContract]
    public class BEWCFHeaderInfo
    {
        [DataMember]
        public string UserName { get; set; }
    }
}
