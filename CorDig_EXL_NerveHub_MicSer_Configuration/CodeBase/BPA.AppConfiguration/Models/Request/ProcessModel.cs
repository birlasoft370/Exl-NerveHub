using System.Runtime.Serialization;

namespace BPA.AppConfiguration.Models.Request
{
    public class ProcessModel
    {
        public int Processid { get; set; }
        public int Clientid { get; set; }
        public string Processname { get; set; }
        public string Description { get; set; }
        public int Calendarid { get; set; }
        public int Sbuid { get; set; }
        public int Processtype { get; set; }
        public int Processworktype { get; set; }
        public string? Scope { get; set; }
        public DateTime Pilotstartdate { get; set; }
        public DateTime Pilotenddate { get; set; }
        //public DateTime Golivedate { get; set; }
        public bool Disabled { get; set; }
        //[JsonPropertyName("UserId")]
        //public int Createdby { get; set; }
        public DateTime StabilizationStartDate { get; set; }
        public DateTime StabilizationEndDate { get; set; }
        public DateTime ProductionStartDate { get; set; }
        public DateTime ProductionEndDate { get; set; }
        public int SupervisorFeedbackTargetFrequency { get; set; }
        public int SupervisorFeebackTragetPerWeek { get; set; }
        public int QCAFeebackTragetPerWeek { get; set; }
        public int TargetAuditPerMonth { get; set; }
        public int TargetQCAHrs { get; set; }
        public int ProcessComplexity { get; set; }
        public int Captype { get; set; }
        //public string comStatus { get; set; }

        //public string Stage { get; set; }
        //public string Filename { get; set; }
        //public byte[] Filedata { get; set; }
        //public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        //public string QCParamXML { get; set; }
        //public string ERPParamXML { get; set; }
        //public string PASProcessMonthYear { get; set; }
        //public string PASProcessType { get; set; }

        [DataMember]
        public List<ERPProcessGroup> lProcessGroup
        {
            get;
            set;
        }
        //public int UserId { get; set; }

    }

    public class ERPProcessGroup
    {
        public int ProcessGroupID
        {
            get;
            set;
        }

        public int ERPProcessID
        {
            get;
            set;
        }
        public int ERPCode
        {
            get;
            set;
        }

        public string Name { get; set; }

        public bool Disabled
        {
            get;
            set;
        }

    }
}
