namespace NotificationService.Models
{
    public class NotificationLog
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime SentDate { get; set; }
    }
}
