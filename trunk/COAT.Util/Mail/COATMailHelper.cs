using System.Net.Mail;

namespace COAT.Util.Mail
{
    public class COATMailHelper
    {

        public bool IsTest;

        public COATMailHelper()
            : this(false)
        {
        }

        public COATMailHelper(bool isTest)
        {
            IsTest = isTest;
        }

        public void SendMail(string[] to, string[] cc, string[] replyTo, string subject, string body)
        {
            COATMailMessageBuilder builder;
            if (IsTest)
            {
                //Curent user are always in CC, So change to TO
                var to2 = cc == null || cc.Length == 0 ? to : cc;
                builder = new COATMailMessageBuilder { Subject = subject, Body = body, To = to2 };
                SendMail(builder.GetMailMessage());
                return;
            }

            builder = new COATMailMessageBuilder { Subject = subject, Body = body, To = cc, CC = cc };
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