using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Common.Options.Email
{
    public  class Attachment
    {
        public string Content { get; set; }
        public string Type { get; set; }   
        public string Filename { get; set; }
        public string Disposition { get; set; }
        public string ContentId { get; set; }
    }
}
