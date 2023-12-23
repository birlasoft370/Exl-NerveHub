namespace BPA.AppConfiguration.Models.Request
{
    public class ClientModel
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int VerticalID { get; set; }
        public string? Description { get; set; }
        public bool EXLSpecificClient { get; set; }
        public bool Disabled { get; set; }
        public string[] ListSBU { get; set; }
    }
}
