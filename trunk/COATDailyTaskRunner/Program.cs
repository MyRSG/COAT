using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COAT.Models;
using COAT.Mail;
using System.IO;

namespace COATDailyTaskRunner
{
    class Program
    {
        static SenderHelper sHelper = new SenderHelper();
        static COATMailHelper mHelper = new COATMailHelper();
        static List<Exception> exList = new List<Exception>();


        static void Main(string[] args)
        {
            exList.Clear();
            SendChannelMail();
            SendSalesMail();
            SendChannelDirectorMail();
            SendToMe();
        }

        private static void SendToMe()
        {
            try
            {
                mHelper.SendMail(new string[] { "chao_zhou@symantec.com" }, null, "COAT Daily Report Notification", GetErrorMessage());
            }
            catch { }
        }

        private static string GetErrorMessage()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var ex in exList)
            {
                sb.Append("Message:");
                sb.AppendLine(ex.Message);
                sb.Append("Trace");
                sb.AppendLine(ex.StackTrace);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static void SendChannelMail()
        {
            var users = sHelper.ChannelApprovers;
            foreach (var user in users)
            {
                var adHelper = new ApproverDealHelper(user);
                SendMail(user.Email, new string[] { }, adHelper);
            }
        }

        private static void SendSalesMail()
        {
            var users = sHelper.SalesApprovers;
            foreach (var user in users)
            {
                var adHelper = new ApproverDealHelper(user);
                SendMail(user.Email, new string[] { "peiye_wang@symantec.com" }, adHelper);
            }
        }

        private static void SendChannelDirectorMail()
        {
            var users = sHelper.ChannelDirectors;
            var cc = sHelper.ChannelApprovers.Select(a => a.Email).ToArray();

            foreach (var user in users)
            {
                SendMail(user.Email, cc, new DirectorDealHelper());
            }
        }

        private static void SendMail(string email, string[] cc, IDealHelper helper)
        {
            if (helper.Count <= 0)
                return;

            if (helper.MoreThan14Days.Count() > 0)
            {
                SendMailByFile(new string[] { email }, cc, "COAT Portal Pending Deal Notification (Urgent! >15days)", "MoreThan14Days.txt");
            }
            else
            {
                SendMailByFile(new string[] { email }, null, "COAT Portal Pending Deal Notification", "Normal.txt");
            }

        }

        private static void SendMailByFile(string[] to, string[] cc, string subject, string filename)
        {
            try
            {
                var message = GetFileContent(filename);
                mHelper.SendMail(to, cc, subject, message);
            }
            catch (Exception ex)
            {
                exList.Add(ex);
            }
        }

        private static string GetFileContent(string filename)
        {
            string appDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = appDir + @"\MailTemplete\" + filename;
            using (var reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

    }
}
