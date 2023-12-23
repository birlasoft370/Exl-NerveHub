namespace MicUI.WorkManagement.Services.ServiceModel
{
    public class GetGridObject_TempModel
    {
        public int GridID { get; set; }
        public int StoreID { get; set; }
        public string DSObjId { get; set; } = "0";
        public string? GridObjectName { get; set; }
        public string? GridTable { get; set; }
        public bool DisableGridObject { get; set; }
        public bool IsGrdEditable { get; set; }
    }
}
