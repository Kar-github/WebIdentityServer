using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Common.Models
{
    public class MessageEmail
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Template { get; set; }
    }
}
