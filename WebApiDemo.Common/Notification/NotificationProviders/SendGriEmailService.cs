using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Common.Models;
using EmailAttachment = WebApiDemo.Common.Options.Email;

namespace WebApiDemo.Common.Notification
{
    public class SendGriEmailService : IEmailService
    {
        private readonly IOptions<EmailOptions> _emailoptions;
        public SendGriEmailService(IOptions<EmailOptions> emailoptions)
        {
            _emailoptions= emailoptions;
        }
        public async Task<bool> SendEmailAsync(MessageEmail message, IList<EmailAttachment.Attachment> attachments = null)
        {
            var from = new EmailAddress(_emailoptions.Value.EmailFrom); 
            var to = new EmailAddress(message.EmailTo);
            var templateId = message.Template;
            var msg = MailHelper.CreateSingleTemplateEmail(from,to,templateId,message.Content);
            return await SendEmailGrid(msg, attachments);
        }
        async Task<bool> SendEmailGrid(SendGridMessage message,IList<EmailAttachment.Attachment> attachments=null)
        {
            //For implementing needs SendGrid Account
            var client = new SendGridClient("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");
            if(attachments!=null && attachments.Count>0)
            {
                message.AddAttachments(attachments.Select(attachment => new SendGrid.Helpers.Mail.Attachment
                {
                    Filename = attachment.Filename,
                    Type = attachment.Type,
                    Content = attachment.Content,
                    ContentId = attachment.ContentId,
                    Disposition = attachment.Disposition
                }));
            }
            var response = await client.SendEmailAsync(message);
            return response.StatusCode == System.Net.HttpStatusCode.Accepted;
        }
    }
}
