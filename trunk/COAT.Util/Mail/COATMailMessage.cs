using System.Net.Mail;

namespace COAT.Util.Mail
{
    public class COATMailMessage : MailMessage
    {
        public COATMailMessage()
        {
            From = new MailAddress(COATMailParameters.Instance.FromAddress, COATMailParameters.Instance.FromName);
        }
    }
}