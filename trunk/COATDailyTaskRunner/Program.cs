using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using COAT.Models;
using COAT.Util.Mail;

namespace COATDailyTaskRunner
{
    internal class Program
    {
        private static readonly SenderHelper SHelper = new SenderHelper();
        private static readonly COATMailHelper MHelper = new COATMailHelper();
        private static readonly List<Exception> ExList = new List<Exception>();


        private static void Main()
        {
            ExList.Clear();
            SendChannelMail();
            SendSalesMail();
            SendChannelDirectorMail();
            SendToAdmin();
        }

        private static void SendToAdmin()
        {
            string[] mailList = SHelper.Admins.Select(u => u.Email).ToArray();
            try
            {
                MHelper.SendMail(mailList, null, "COAT Daily Report Notification",
                                 GetErrorMessage());
            }
            catch
            {
            }
        }


        private static string GetErrorMessage()
        {
            var sb = new StringBuilder();
            foreach (Exception ex in ExList)
            {
                sb.Append("Message:");
                sb.AppendLine(ex.Message);
                sb.Append("Trace");
                sb.AppendLine(ex.StackTrace);
                sb.AppendLine();
            }

            if (sb.Length == 0)
            {
                sb.Append("There is no error today.");
            }

            return sb.ToString();
        }

        private static void SendChannelMail()
        {
            User[] users = SHelper.ChannelApprovers;
            foreach (User user in users)
            {
                var adHelper = new ApproverDealHelper(user);
                SendMail(user.Email, new string[] {}, adHelper);
            }
        }

        private static void SendSalesMail()
        {
            User[] users = SHelper.SalesApprovers;
            foreach (User user in users)
            {
                var adHelper = new ApproverDealHelper(user);
                SendMail(user.Email, new[] {"peiye_wang@symantec.com"}, adHelper);
            }
        }

        private static void SendChannelDirectorMail()
        {
            User[] users = SHelper.ChannelDirectors;
            string[] cc = SHelper.ChannelApprovers.Select(a => a.Email).ToArray();

            foreach (User user in users)
            {
                SendMail(user.Email, cc, new DirectorDealHelper());
            }
        }

        private static void SendMail(string email, string[] cc, IDealHelper helper)
        {
            if (helper.Count <= 0)
                return;

            if (helper.MoreThan14Days.Any())
            {
                SendMailByFile(new[] {email}, cc, "COAT Portal Pending Deal Notification (Urgent! >15days)",
                               "MoreThan14Days.txt");
            }
            else
            {
                SendMailByFile(new[] {email}, null, "COAT Portal Pending Deal Notification", "Normal.txt");
            }
        }

        private static void SendMailByFile(string[] to, string[] cc, string subject, string filename)
        {
            try
            {
                string message = GetFileContent(filename);
                MHelper.SendMail(to, cc, subject, message);
            }
            catch (Exception ex)
            {
                ExList.Add(ex);
            }
        }

        private static string GetFileContent(string filename)
        {
            string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = appDir + @"\MailTemplete\" + filename;
            using (var reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }
    }
}