﻿using System.Net.Mail;

namespace COAT.Util.Mail
{
    public class COATMailHelper
    {
        public void SendMail(string[] to, string[] cc, string[] replyTo, string subject, string body)
        {
            var builder = new COATMailMessageBuilder {Subject = subject, Body = body, To = to, CC = cc};
            if (replyTo != null)
            {
                builder.ReplyTo = replyTo;
            }

            SendMail(builder.GetMailMessage());
        }

        public void SendMail(string[] to, string[] cc, string subject, string body)
        {
            SendMail(to, cc, null, subject, body);
        }

        public void SendMail(MailMessage msg)
        {
            var client = new COATSmtpClient();
            client.Send(msg);
        }
    }
}