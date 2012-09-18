using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace COAT.Mail
{
    public class COATMailMessageBuilder
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] To { get; set; }
        public string[] ReplyTo { get; set; }
        public string[] CC { get; set; }
        public string[] Bcc { get; set; }

        public MailMessage GetMailMessage()
        {
            if (!ValidateAllAddressList())
            {
                throw new InvalidOperationException("Invalid Mail Address List");
            }

            var msg = new COATMailMessage();
            msg.Subject = Subject;
            msg.Body = Body;

            AddMailAddress(msg.To, To);
            if (ValidateSingleAddressList(ReplyTo))
            {
                AddMailAddress(msg.ReplyToList, ReplyTo);
            }
            else
            {
                AddMailAddress(msg.ReplyToList, To);
            }
            AddMailAddress(msg.CC, CC);
            AddMailAddress(msg.Bcc, Bcc);

            return msg;
        }

        private void AddMailAddress(MailAddressCollection collection, string[] addressList)
        {
            if (collection == null || addressList == null)
                return;

            foreach (var add in addressList)
            {
                collection.Add(add);
            }
        }

        private bool ValidateAllAddressList()
        {
            return ValidateSingleAddressList(To) ||
                ValidateSingleAddressList(CC) ||
                ValidateSingleAddressList(Bcc);
        }

        private bool ValidateSingleAddressList(string[] addressList)
        {
            if (addressList == null)
                return false;

            return addressList.Count(a => !String.IsNullOrEmpty(a)) > 0;
        }



    }
}