using BPA.AppConfig.BusinessEntity.Config;

namespace BPA.AppConfiguration.Models.Request
{
    public class TimeZoneModel
    {
        public int TimeZoneID { get; set; }
        public string TimeZoneName { get; set; }
        public string Description { get; set; }
        public string OffSetGMT { get; set; }
        public bool Disabled { get; set; }
    }
}
