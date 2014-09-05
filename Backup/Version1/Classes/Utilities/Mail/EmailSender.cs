using System;
using System.Net.Mail;

namespace Csbs.Utilities
{
    public static class EmailSender
    {
        #region Public methods

        public static void Send(EmailItem item)
        {
            if (Settings.EmailEnabled)
            {
                MailMessage mail = new MailMessage();

                if (!String.IsNullOrEmpty(item.From))
                    mail.From = new MailAddress(item.From);

                AddMailAddress(mail.To, item.To);
                AddMailAddress(mail.CC, item.Cc);
                AddMailAddress(mail.Bcc, item.Bcc);

                mail.Subject = item.Subject;
                mail.IsBodyHtml = true;
                mail.Body = item.Body;

                SmtpClient client = new SmtpClient();
                client.Send(mail);
            }
        }

        static void AddMailAddress(MailAddressCollection collection, String address)
        {
            if (String.IsNullOrEmpty(address))
                return;

            if (address.Contains(";"))
            {
                String[] recipients = address.Split(';');
                foreach (String recipient in recipients)
                    collection.Add(recipient);
            }
            else
            {
                collection.Add(address);
            }
        }

        #endregion
    }
}