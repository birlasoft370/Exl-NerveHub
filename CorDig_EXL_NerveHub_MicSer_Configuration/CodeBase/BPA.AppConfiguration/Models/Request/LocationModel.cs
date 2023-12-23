namespace BPA.AppConfiguration.Models.Request
{
    public class LocationModel
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string LocationDesc { get; set; }
        public bool Disabled { get; set; }
        public int UserId { get; set; }
    }
}
