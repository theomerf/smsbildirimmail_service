namespace NesInterface.Models
{
    public class Mail
    {
        public String? To { get; set; }
        public String? From { get; set; }
        public String? Subject { get; set; }
        public String? Body { get; set; }
        public String? Cc { get; set; }
        public String? Bcc { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
