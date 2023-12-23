namespace BPA.AppConfiguration.Models.Request
{
    public class ShiftMasterModel
    {
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string Description { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public bool Disabled { get; set; }
        public int UserId { get; set; }
    }
}
