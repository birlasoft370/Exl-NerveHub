namespace BPA.AppConfiguration.Models.Request
{
    public class LOBModel
    {
        public int LOBID { get; set; }
        public int ERPID { get; set; }
        public string LOBName { get; set; }
        public string Description { get; set; }
        public bool Disabled { get; set; }
        public int UserId { get; set; }
    }
}
