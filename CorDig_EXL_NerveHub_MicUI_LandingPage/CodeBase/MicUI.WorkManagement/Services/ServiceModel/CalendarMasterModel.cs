namespace MicUI.WorkManagement.Services.ServiceModel
{
    public class CalendarInfoMasterModel
    {
      
            public int CalenderID
            {
                get;
                set;
            }
            public string CalenderName
            { get; set; }

            public string Description
            { get; set; }

            public bool Disabled
            {
                get; set;
            }
            public int UserId { get; set; }
        }
    
}
