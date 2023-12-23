namespace MicUI.WorkManagement.Models.Response
{
    public class MessageResponse<T> : BaseMessageResponse<T>
    {
        public int totalCount { get; set; }
    }
}
