using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using TravelStar.Business.Interfaces;
using TravelStar.Business.Option;
using TravelStar.Entities;
using TravelStar.Model;

namespace TravelStar.Business.Implements;
public class EmailService : IEmailService
{
    private readonly IResourceReader _resourceReader;
    private readonly EmailOptions _emailSettings;

    public EmailService(IOptions<EmailOptions> emailOptions, IResourceReader resourceReader)
    {
        _resourceReader = resourceReader;
        _emailSettings = emailOptions.Value;
    }

    public async Task SendConfirmBookingEmailAsync(string to, string subject, string confirmCode)
    {
        try
        {
            string resourceName = $"TravelStar.Business.Resources.ConfirmBookingEmailTemplate.html";
            var content = await _resourceReader.GetConfirmBookingEmailResourceAsync(resourceName);
            content = content.Replace("{{confirm_code}}", confirmCode);



            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailSettings.SmtpUser));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = content };

            // send email
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {

        }
    }

    public async Task SendInfoBookingSuccessEmailAsync(string to, string subject)
    {
        try
        {
            string resourceName = $"TravelStar.Business.Resources.InfoBookingSuccessEmailTemplate.html";
            var content = await _resourceReader.GetConfirmBookingEmailResourceAsync(resourceName);

            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailSettings.SmtpUser));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = content };

            // send email
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {

        }

    }
}
