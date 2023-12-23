namespace MicUI.NerveHub.Models
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int SortOrder { get; set; }
        public string Url { get; set; }

        public string Flag { get; set; }
        public string Areas { get; set; }
        public string RouterAction { get; set; }
        public string RouterController { get; set; }
        public int FormID { get; set; }

        public string IconClass { get; set; }
    }
}
