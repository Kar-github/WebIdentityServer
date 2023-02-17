using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Services.Infrastructure
{
    public  class CommandValidationException:Exception
    {
        public CommandValidationException(string message) : base(message) { }
        public CommandValidationException(string message, Exception innerexception)
            : base(message, innerexception) { }
        
    }
}
