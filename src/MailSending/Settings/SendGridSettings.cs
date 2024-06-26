namespace MailSending.Settings;

internal record SendGridSettings
{
  public const string SectionKey = "SendGrid";

  public string Host { get; set; }
  public ushort Port { get; set; }
  public bool EnableSsl { get; set; }

  public EmailSettings Sender { get; set; }
  public string ApiKey { get; set; }

  public SendGridSettings() : this("smtp.sendgrid.net", 587, enableSsl: true, new EmailSettings(), string.Empty)
  {
  }

  public SendGridSettings(string host, ushort port, bool enableSsl, EmailSettings sender, string apiKey)
  {
    Host = host;
    Port = port;
    EnableSsl = enableSsl;

    Sender = sender;
    ApiKey = apiKey;
  }
}
