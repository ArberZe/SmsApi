namespace SmsApi.Entities;
public class Message
{
    public int Id { get; set; }
    public string RecipientNumber { get; set; }
    public string Text { get; set; }
    public MessageStatus Status { get; set; }
}
public enum MessageStatus
{
    Pending,
    Sent,
    Failed
}
