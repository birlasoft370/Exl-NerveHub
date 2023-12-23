namespace BPA.EmailConfiguration
{
    public class MessageResponse<T> : BaseMessageResponse<T>
    {
        public int TotalCount { get; set; }
    }
}
