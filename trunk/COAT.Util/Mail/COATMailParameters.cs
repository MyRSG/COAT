using System.Net.Mail;

namespace COAT.Util.Mail
{
    public class COATMailParameters
    {
        private static COATMailParameters _instance;

        public static COATMailParameters Instance
        {
            get { return _instance ?? (_instance = GetDefaltInstance()); }

            set
            {
                if (value != null)
                {
                    _instance = value;
                }
            }
        }

        public string Host { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public int Timeout { get; set; }
        public bool UseDefaultCredentials { get; set; }

        private static COATMailParameters GetDefaltInstance()
        {
            var rslt = new COATMailParameters
                           {
                               Host = "192.168.214.43",
                               FromAddress = "coat@symantec.com",
                               FromName = "COAT",
                               DeliveryMethod = SmtpDeliveryMethod.Network,
                               Timeout = 60*1000,
                               UseDefaultCredentials = true
                           };
            return rslt;
        }
    }
}