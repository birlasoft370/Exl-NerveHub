namespace MicSer.Security.UserMgt.Models;

public abstract class BaseMessageResponse<T>
{
    public BaseMessageResponse()
    {
        this.timeStamp = DateTime.UtcNow.AddDays(10);
    }

    public T? data { get; set; }
    public string? message { get; set; }
    public DateTime timeStamp { get; set; }
    public bool status => data != null ? true : false;
}