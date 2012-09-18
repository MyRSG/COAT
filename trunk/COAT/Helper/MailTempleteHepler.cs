using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;

namespace COAT.Helper
{
    public class MailTempleteHepler
    {
        public const string NewAccount = "NewAccount.txt";
        public const string SalesCofirm = "SalesConfirm.txt";
        public const string DealApproved = "DealApproved.txt";
        public const string DealRejected = "DealRejected.txt";

        public string GetTemplete(string filename, string defaultVal = "")
        {
            try
            {
                var dir = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "MailTemplete");
                var filePath = Path.Combine(dir, filename);
                using (var reader = new StreamReader(filePath))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                return defaultVal;
            }
        }
    }
}