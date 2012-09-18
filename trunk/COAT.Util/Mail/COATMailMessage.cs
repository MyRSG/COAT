using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace COAT.Mail
{
    public class COATMailMessage : MailMessage
    {
        public COATMailMessage()
        {
            From = new MailAddress(COATMailParameters.Instance.FromAddress, COATMailParameters.Instance.FromName);
        }
    }
}