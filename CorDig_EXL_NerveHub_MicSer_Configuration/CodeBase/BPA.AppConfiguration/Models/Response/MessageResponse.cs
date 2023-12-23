namespace BPA.AppConfiguration.Models.Response
{
    public class MessageResponse<T> : BaseMessageResponse<T>
    {
        public int TotalCount { get; set; }
    }
}
