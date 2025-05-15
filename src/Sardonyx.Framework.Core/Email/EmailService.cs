using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Sardonyx.Framework.Core.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public string SMTPServer { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }

        public EmailService(IConfiguration config)
        {
            _config = config;
            SMTPServer = _config.GetValue<string?>("SMTPServer") ?? string.Empty;
            SMTPUsername = _config.GetValue<string?>("SMTPUsername") ?? string.Empty;
            SMTPPassword = _config.GetValue<string?>("SMTPPassword") ?? string.Empty;

            if (String.IsNullOrEmpty(SMTPServer) || String.IsNullOrEmpty(SMTPUsername) || String.IsNullOrEmpty(SMTPPassword))
            {
                throw new ApplicationException("Missing configuration for email service. Check appsettings.json or .env for correct configuration entries.");
            }
        }

        public void SendEmail(Email email)
        {
            using (var mailMessage = new MailMessage(email.FromEmail, email.ToEmail, email.Subject, email.Message))
            {
                mailMessage.IsBodyHtml = true;

                if (!String.IsNullOrWhiteSpace(email.CCs))
                {
                    foreach(var cc in email.CCs.Split(','))
                    {
                        mailMessage.CC.Add(cc);
                    }
                }

                using (var smtp = new SmtpClient(SMTPServer, 587))
                {
                    NetworkCredential credential = new NetworkCredential(SMTPUsername, SMTPPassword);

                    smtp.Credentials = credential;
                    smtp.EnableSsl = true;

                    smtp.Send(mailMessage);
                }
            }
        }
    }
}
