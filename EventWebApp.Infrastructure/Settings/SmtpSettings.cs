﻿namespace EventWebApp.Infrastructure.Settings
{
    public class SmtpSettings
    {
        public string Host { get; set; } = "";
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Sender { get; set; } = "";
    }
}
