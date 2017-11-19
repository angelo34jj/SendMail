using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web
{
    public class MailRequest
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}