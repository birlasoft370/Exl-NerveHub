using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.ServiceContract.ExternalRef.FaultContracts
{
    [DataContract(Name = "ServiceFault")]
    public class ServiceFault //: ValidationFault
    {
        public ServiceFault()
        {

        }

        private string message;
        private IDictionary data;
        private Guid id;

        [DataMember]
        public string MessageText
        {
            get { return message; }
            set { message = value; }
        }

        [DataMember]
        public IDictionary Data
        {
            get { return data; }
            set { data = value; }
        }

        [DataMember]
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
