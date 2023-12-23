using System.Runtime.Serialization;

namespace MicUI.Configuration.Services.ServiceModel
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

