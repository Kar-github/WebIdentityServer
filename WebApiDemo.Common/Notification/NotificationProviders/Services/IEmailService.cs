using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Common.Models;
using WebApiDemo.Common.Options.Email;

namespace WebApiDemo.Common.Notification
{
    public  interface IEmailService
    {
        Task<bool> SendEmailAsync(MessageEmail message,IList<Attachment> attachments=null);
    }
}
