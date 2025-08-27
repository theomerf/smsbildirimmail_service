namespace NesInterface.Models
{
    public class Stat
    {
        public required String To { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public int MailCount { get; set; }
        public int SmsCount { get; set; }
        public int NotificationCount { get; set; }
    }
}
