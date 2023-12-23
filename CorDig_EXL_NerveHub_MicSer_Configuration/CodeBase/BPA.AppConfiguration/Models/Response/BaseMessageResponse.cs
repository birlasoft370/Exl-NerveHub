namespace BPA.AppConfiguration.Models.Response
{
    public abstract class BaseMessageResponse<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public DateTime TimeStamp => DateTime.UtcNow;
        public bool Status => Data != null ? true : false;
    }
}
