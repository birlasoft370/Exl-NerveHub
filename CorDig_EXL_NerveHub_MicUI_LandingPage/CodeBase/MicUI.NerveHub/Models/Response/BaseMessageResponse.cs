namespace MicUI.NerveHub.Models;

public abstract class BaseMessageResponse<T>
{
    public T? data { get; set; }
    public string? message { get; set; }
    public DateTime timeStamp { get; set; }
    public bool status { get; set; } 
}
