using System.IO;
using System.Web.Hosting;

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
                string dir = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "MailTemplete");
                string filePath = Path.Combine(dir, filename);
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