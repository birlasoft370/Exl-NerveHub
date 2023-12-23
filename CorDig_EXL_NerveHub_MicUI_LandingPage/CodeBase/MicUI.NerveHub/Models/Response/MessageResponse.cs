namespace MicUI.NerveHub.Models;

public class MessageResponse<T> : BaseMessageResponse<T>
{
    public int totalCount { get; set; }
}
