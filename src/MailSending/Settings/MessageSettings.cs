namespace MailSending.Settings;

internal record MessageSettings
{
  public const string SectionKey = "Message";

  public string Subject { get; set; }

  public bool IsBodyHtml { get; set; }
  public string Body { get; set; }

  public MessageSettings() : this(string.Empty, false, string.Empty)
  {
  }

  public MessageSettings(string subject, bool isBodyHtml, string body)
  {
    Subject = subject;

    IsBodyHtml = isBodyHtml;
    Body = body;
  }
}
