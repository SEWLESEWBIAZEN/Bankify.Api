﻿namespace Bankify.Application.Common.Helpers
{
    public class Settings
    {
        public EmailSettings EmailSettings { get; set; }
    }

    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
    }
}
