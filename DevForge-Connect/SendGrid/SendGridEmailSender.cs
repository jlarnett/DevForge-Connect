using DevForge_Connect.SendGrid.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
namespace DevForge_Connect.SendGrid;

public class SendGridEmailSender : IEmailSender
{
    private readonly ILogger<ILogger> _logger;

    public SendGridEmailSender(IOptions<SendGridEmailSenderOptions> options, ILogger<ILogger> logger)
    {
        Options = options.Value;
        _logger = logger;
    }

    public SendGridEmailSenderOptions Options { get; set; }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var response = await Execute(Options.ApiKey, subject, message, email);
        _logger.Log(LogLevel.Information, "Sent request to SendGrid API, returned status code - {0}", response.StatusCode);
    }


    /// <summary>
    /// Execute Sendgrid email process and returns response. 
    /// </summary>
    /// <param name="apiKey">Sendgrid API Key</param>
    /// <param name="subject">Email Subject</param>
    /// <param name="message">Main email section. Contains text, links, whatever in the body of email.</param>
    /// <param name="email">Email address to send email to. </param>
    /// <returns></returns>
    private async Task<Response> Execute(string apiKey, string subject, string message, string email)
    {
        var client = new SendGridClient(apiKey);

        var msg = new SendGridMessage()
        {
            From = new EmailAddress(Options.SenderEmail, Options.SenderName),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(email));

        // disable tracking settings
        // ref.: https://sendgrid.com/docs/User_Guide/Settings/tracking.html

        msg.SetClickTracking(false, false);
        msg.SetOpenTracking(false);
        msg.SetGoogleAnalytics(false);
        msg.SetSubscriptionTracking(false);

        return await client.SendEmailAsync(msg);

    }
}
