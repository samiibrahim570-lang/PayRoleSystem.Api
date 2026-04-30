using PayRoleSystem.Data;
using PayRoleSystem.Models;
using PayRoleSystem.Services;

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace PayRoleSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _context;
        private readonly SystemSetting _systemSetting;

        public EmailService(ApplicationDbContext context)
        {
            _context = context;

            // Fetch SystemSetting from the database
                            _systemSetting = _context.SystemSetting
                .FirstOrDefault();

            if (_systemSetting == null)
            {
                throw new Exception("System settings are not configured in the database.");
            }
        }

        public async Task<bool> SendEmailAsync(string to, string subject = null, string body = null)
        {
            try
            {
                subject = string.IsNullOrWhiteSpace(subject) ? "Create a New Password" : subject;
                body = string.IsNullOrWhiteSpace(body)
                    ? "Please click the link below to create your password:<br>" +
                      "<a href='https://example.com/create-password'>Create Password</a>"
                    : body;

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Persistent Solutions", _systemSetting.EmailAddress));
                emailMessage.To.Add(new MailboxAddress("", to));
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
            <h2>{subject}</h2>
            <p>{body}</p>
            <h4>If you have any questions, contact us at <a href='mailto:info@persistentsolutions.co'>info@persistentsolutions.co</a>.</h4>
            <p style='color:gray; font-size: small;'>© 2024 Persistent Solutions. All rights reserved.</p>
            "
                };

                emailMessage.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_systemSetting.SmtpHost, 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_systemSetting.SmtpUsername, _systemSetting.SmtpPassword);
                    await client.SendAsync(emailMessage);
                }

                return true; // Success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false; // Failure
            }
        }



    }
}
