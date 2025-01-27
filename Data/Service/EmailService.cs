using System;
using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EG_ERP.Data.Service;

public class EmailService : IEmailService
{
    private readonly IConfiguration config;
    private readonly ILogger<EmailService> logger;
    private readonly SendGridClient client;
    private readonly EmailAddress from;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        this.config = config;
        this.logger = logger;
        string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? throw new ArgumentNullException("SENDGRID_API_KEY is not set in environment variables");
        client = new SendGridClient(apiKey);
        from = new EmailAddress("yousef.sendgrid@gmail.com", "Yousef");
    }

    public async Task SendEmailAsync(string recipient, string subject, string body, bool isHtml = false)
    {
        try
        {
            if (string.IsNullOrEmpty(recipient))
                throw new ArgumentNullException(nameof(recipient));
            EmailAddress to = new EmailAddress(recipient);
            SendGridMessage mes;
            if (isHtml)
                mes = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            else
                mes = MailHelper.CreateSingleEmail(from, to, subject, body, "");
    
            Response response = await client.SendEmailAsync(mes);
            if (response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Failed to send email to {recipient}, StatusCode: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            throw ex;  // Log exception
        }
    }

    public Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath, bool isHtml = false)
    {
        throw new NotImplementedException(); // need file service
    }
}
