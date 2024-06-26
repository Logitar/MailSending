namespace MailSending.Settings;

internal record GmailSettings
{
  public const string SectionKey = "Gmail";

  public string Host { get; set; }
  public ushort Port { get; set; }
  public bool EnableSsl { get; set; }

  public EmailSettings Sender { get; set; }
  public string Password { get; set; }

  public GmailSettings() : this("smtp.gmail.com", 587, enableSsl: true, new EmailSettings(), string.Empty)
  {
  }

  public GmailSettings(string host, ushort port, bool enableSsl, EmailSettings sender, string password)
  {
    Host = host;
    Port = port;
    EnableSsl = enableSsl;

    Sender = sender;
    Password = password;
  }
}
