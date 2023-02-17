using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Common
{
    public  class EmailOptions
    {
        public string EmailFrom { get; set; } = string.Empty;
        public string EmailSmtpServer { get; set; } = string.Empty;
        public int EmailPort { get; set; }
        public string EmailPassword { get; set; } = string.Empty;
        public string EmailUsername { get; set; } = string.Empty;
    }
}
