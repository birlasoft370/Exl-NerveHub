namespace MicUI.Configuration.Services.ServiceModel
{
    public class FacilityModel
    {
        public int LocationID { get; set; }
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
        public string? FacilityDescription { get; set; }
        public bool Disabled { get; set; }
        public int UserId { get; set; }
    }
}
