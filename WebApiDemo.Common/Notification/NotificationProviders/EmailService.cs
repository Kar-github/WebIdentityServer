using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Common.Models;
using WebApiDemo.Common.Notification;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using EmailAttachment= WebApiDemo.Common.Options.Email;

namespace WebApiDemo.Common.Notification
{
    public  class EmailService : IEmailService
    {
        private readonly IOptions<EmailOptions> _emailoptions;
        public EmailService(IOptions<EmailOptions> emailoptions)
        {
            _emailoptions= emailoptions;
        }
        public async  Task<bool> SendEmailAsync(MessageEmail message,IList<EmailAttachment.Attachment> attachments= null)
        {
            var mimemessage=CreateEmailMessage(message);
            return await SendAsync(mimemessage);
        }
        private MimeMessage CreateEmailMessage(MessageEmail message)
        {

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", _emailoptions.Value.EmailFrom));
            emailMessage.To.Add(new MailboxAddress("", message.EmailTo));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }
        private async Task<bool> SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailoptions.Value.EmailSmtpServer, _emailoptions.Value.EmailPort, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailoptions.Value.EmailUsername,_emailoptions.Value.EmailPassword);
                    await client.SendAsync(mailMessage);
                    return true;
                }
                catch
                {
                    Trace.WriteLine("Email not send");
                    return false;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
