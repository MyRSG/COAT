using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace COAT.Mail
{
    public class COATMailParameters
    {
        static COATMailParameters _Instance;

        public static COATMailParameters Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = GetDefaltInstance();
                }
                return _Instance;
            }

            set
            {
                if (value as COATMailParameters != null)
                {
                    _Instance = value;
                }
            }
        }

        private static COATMailParameters GetDefaltInstance()
        {
            var rslt = new COATMailParameters();
            rslt.Host = "192.168.214.43";
            rslt.FromAddress = "coat@symantec.com";
            rslt.FromName = "COAT";
            rslt.DeliveryMethod = SmtpDeliveryMethod.Network;
            rslt.Timeout = 60 * 1000;
            rslt.UseDefaultCredentials = true;
            return rslt;
        }

        public string Host { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public int Timeout { get; set; }
        public bool UseDefaultCredentials { get; set; }



    }
}