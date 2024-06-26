namespace MailSending.Settings;

internal record EmailSettings
{
  public string Address { get; set; }
  public string? DisplayName { get; set; }

  public EmailSettings() : this(string.Empty)
  {
  }

  public EmailSettings(string address, string? emailAddress = null)
  {
    Address = address;
    DisplayName = emailAddress;
  }
}
