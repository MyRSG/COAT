using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace COAT.Util.Mail
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

            var msg = new COATMailMessage {Subject = Subject, Body = Body};

            AddMailAddress(msg.To, To);
            AddMailAddress(msg.ReplyToList, ValidateSingleAddressList(ReplyTo) ? ReplyTo : To);
            AddMailAddress(msg.CC, CC);
            AddMailAddress(msg.Bcc, Bcc);

            return msg;
        }

        private void AddMailAddress(MailAddressCollection collection, IEnumerable<string> addressList)
        {
            if (collection == null || addressList == null)
                return;

            foreach (string add in addressList)
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

        private bool ValidateSingleAddressList(IEnumerable<string> addressList)
        {
            if (addressList == null)
                return false;

            return addressList.Count(a => !String.IsNullOrEmpty(a)) > 0;
        }
    }
}