
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;



namespace BPA.Security.ServiceContract.FaultContracts
{
    [DataContract(Name = "ServiceFault")]
    public class ServiceFault : ValidationFault
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
