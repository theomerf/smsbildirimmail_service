namespace NesInterface.Models
{
    public class Request
    {
        public int Id { get; set; }
        public required String To { get; set; }
        public required String From { get; set; }
        public required String Subject { get; set; }
        public required String Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public String? Cc { get; set; }
        public String? Bcc { get; set; }
    }
}
