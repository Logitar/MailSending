namespace MailSending.Settings;

internal record MailCatcherSettings
{
  public const string SectionKey = "MailCatcher";

  public string Host { get; set; }
  public ushort Port { get; set; }

  public MailCatcherSettings() : this("localhost", 1025)
  {
  }

  public MailCatcherSettings(string host, ushort port)
  {
    Host = host;
    Port = port;
  }
}
