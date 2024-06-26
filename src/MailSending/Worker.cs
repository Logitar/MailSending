using MailSending.Settings;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MailSending;

internal class Worker : BackgroundService
{
  private readonly IConfiguration _configuration;
  private readonly Encoding? _encoding;
  private readonly ILogger<Worker> _logger;

  public Worker(IConfiguration configuration, ILogger<Worker> logger)
  {
    _configuration = configuration;
    _logger = logger;

    string? encodingName = _configuration.GetValue<string>("Encoding");
    if (!string.IsNullOrWhiteSpace(encodingName))
    {
      _encoding = Encoding.GetEncoding(encodingName);
    }
  }

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    try
    {
      Stopwatch chrono = Stopwatch.StartNew();

      MessageSettings settings = _configuration.GetSection(MessageSettings.SectionKey).Get<MessageSettings>()
        ?? throw new InvalidOperationException($"The configuration section '{MessageSettings.SectionKey}' is required.");

      MailAddress recipient = _configuration.GetSection("Recipient").Get<EmailSettings>()?.ToMailAddress(_encoding)
        ?? throw new InvalidOperationException("The configuration 'Recipient' is required.");

      MailMessage message = new()
      {
        Body = settings.Body.Trim(),
        BodyEncoding = _encoding,
        IsBodyHtml = settings.IsBodyHtml,
        Subject = settings.Subject.Trim(),
        SubjectEncoding = _encoding
      };
      message.To.Add(recipient);

      await SendThroughMailCatcherAsync(message, cancellationToken);
      await SendThroughGmailAsync(message, cancellationToken);
      await SendThroughSendGridAsync(message, cancellationToken);

      chrono.Stop();
      _logger.LogInformation("Operation completed in {Elapsed}ms.", chrono.ElapsedMilliseconds);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception, "An unhandled exception occurred.");
    }
  }

  private async Task SendThroughMailCatcherAsync(MailMessage message, CancellationToken cancellationToken)
  {
    MailCatcherSettings? settings = _configuration.GetSection(MailCatcherSettings.SectionKey).Get<MailCatcherSettings>();
    if (settings == null)
    {
      _logger.LogWarning("No message send through MailCatcher, since its configuration is null.");
      return;
    }

    EmailSettings sender = new("no-reply@mailcatcher.me", "MailCatcher");
    message.SetSender(sender, _encoding);

    using SmtpClient client = new(settings.Host.Trim(), settings.Port);
    await client.SendMailAsync(message, cancellationToken);
    _logger.LogInformation("Successfully sent a message through MailCatcher.");
  }

  private async Task SendThroughGmailAsync(MailMessage message, CancellationToken cancellationToken)
  {
    GmailSettings? settings = _configuration.GetSection(GmailSettings.SectionKey).Get<GmailSettings>();
    if (settings == null)
    {
      _logger.LogWarning("No message send through Gmail, since its configuration is null.");
      return;
    }

    message.SetSender(settings.Sender, _encoding);

    using SmtpClient client = new(settings.Host.Trim(), settings.Port)
    {
      EnableSsl = settings.EnableSsl,
      DeliveryMethod = SmtpDeliveryMethod.Network,
      UseDefaultCredentials = false,
      Credentials = new NetworkCredential(settings.Sender.Address, settings.Password)
    };
    await client.SendMailAsync(message, cancellationToken);
    _logger.LogInformation("Successfully sent a message through Gmail.");
  }

  private async Task SendThroughSendGridAsync(MailMessage message, CancellationToken cancellationToken)
  {
    SendGridSettings? settings = _configuration.GetSection(SendGridSettings.SectionKey).Get<SendGridSettings>();
    if (settings == null)
    {
      _logger.LogWarning("No message send through SendGrid, since its configuration is null.");
      return;
    }

    message.SetSender(settings.Sender, _encoding);

    using SmtpClient client = new(settings.Host.Trim(), settings.Port)
    {
      EnableSsl = settings.EnableSsl,
      DeliveryMethod = SmtpDeliveryMethod.Network,
      UseDefaultCredentials = false,
      Credentials = new NetworkCredential("apikey", settings.ApiKey)
    };
    await client.SendMailAsync(message, cancellationToken);
    _logger.LogInformation("Successfully sent a message through SendGrid.");
  }
}
