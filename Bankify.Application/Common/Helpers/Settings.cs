namespace Bankify.Application.Common.Helpers
{
    public class Settings
    {
        public EmailSettings EmailSettings { get; set; }=new EmailSettings();
    }

    public class EmailSettings
    {
        public string MailServer { get; set; }=String.Empty;
        public int MailPort { get; set; }
        public string SenderName { get; set; }=String.Empty;
        public string Sender { get; set; }=String.Empty;
        public string Password { get; set; }=String.Empty;
    }
}
