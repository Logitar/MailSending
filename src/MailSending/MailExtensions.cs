using MailSending.Settings;
using System.Net.Mail;
using System.Text;

namespace MailSending;

internal static class MailExtensions
{
  public static void SetSender(this MailMessage message, EmailSettings email, Encoding? encoding = null)
  {
    MailAddress sender = email.ToMailAddress(encoding);

    message.From = sender;
    message.Sender = sender;

    message.ReplyToList.Clear();
    message.ReplyToList.Add(sender);
  }

  public static MailAddress ToMailAddress(this EmailSettings email, Encoding? encoding = null) => new(email.Address.Trim(), email.DisplayName?.Trim(), encoding);
}
