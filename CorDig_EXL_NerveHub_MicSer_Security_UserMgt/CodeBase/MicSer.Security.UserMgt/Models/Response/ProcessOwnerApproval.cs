namespace MicSer.Security.UserMgt.Models.Response
{
    public class ProcessOwnerApproval
    {
       public int RequestId { get; set; }
        public string ClientName { get; set; }
        public string ProcessName { get; set; }
        public string Creater { get; set; }
        public string Approver { get; set; }
        public string CreateDate { get; set; }
        public string ForUser { get; set; }
        public string ForApprover { get; set; }
        public string ForCancel { get; set; }
        public string TransStatus { get; set; }

    }
}
