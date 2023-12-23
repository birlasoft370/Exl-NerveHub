namespace BPA.AppConfiguration.Models.Request
{
    public class VerticalMasterModel
    {
        public int VerticalID { get; set; }
        public int? ERPID { get; set; }
        public string VerticaName { get; set; }
        public string VerticalDescription { get; set; }
        public bool? Disabled { get; set; }
        public int UserId { get; set; }
    }
}
