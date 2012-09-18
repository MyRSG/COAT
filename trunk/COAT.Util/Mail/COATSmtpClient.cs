using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Reflection;

namespace COAT.Mail
{
    internal class COATSmtpClient
    {
        const string OLD_HOST_FIELD_NAME = "localHostName";
        const string NEW_HOST_FIELD_NAME = "clientDomain";

        readonly SmtpClient _smtp;

        public COATSmtpClient()
        {
            _smtp = new SmtpClient();
            _smtp.Host = COATMailParameters.Instance.Host;
            _smtp.Timeout = COATMailParameters.Instance.Timeout;
            _smtp.DeliveryMethod = COATMailParameters.Instance.DeliveryMethod;
            _smtp.UseDefaultCredentials = COATMailParameters.Instance.UseDefaultCredentials;

            LocalHostName = "COAT";
        }

        public void Send(MailMessage message)
        {
            _smtp.Send(message);
        }

        private string LocalHostName
        {
            get
            {
                if (IsNewEnvoriment())
                {
                    return GetField(NEW_HOST_FIELD_NAME, _smtp).ToString();
                }

                if (IsOldEnvoriment())
                {
                    return GetField(OLD_HOST_FIELD_NAME, _smtp).ToString();
                }

                return System.Environment.MachineName;
            }
            set
            {
                string heloName = string.IsNullOrEmpty(value) ? System.Environment.MachineName : value;

                // member "localHostName" changed to "clientDomain" in .NET Fx 4.0
                if (IsNewEnvoriment())
                {
                    SetField(NEW_HOST_FIELD_NAME, _smtp, heloName);

                }
                else if (IsOldEnvoriment())
                {
                    SetField(OLD_HOST_FIELD_NAME, _smtp, heloName);
                }

            }
        }

        private static bool IsOldEnvoriment()
        {
            return System.Environment.Version.Major == 2 && System.Environment.Version.Build == 50727;
        }

        private static bool IsNewEnvoriment()
        {
            return System.Environment.Version.Major == 4
                || (System.Environment.OSVersion.Version.Major > 6)
                || (System.Environment.OSVersion.Version.Major == 6 && System.Environment.OSVersion.Version.Minor >= 1);
        }

        private object GetField(string fieldName, object obj)
        {
            return obj.GetType().InvokeMember(fieldName,
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.GetField,
                        null,
                        obj,
                        null);
        }

        private object SetField(string fieldName, object obj, object value)
        {
            return obj.GetType().InvokeMember(fieldName,
                       BindingFlags.DeclaredOnly |
                       BindingFlags.Public | BindingFlags.NonPublic |
                       BindingFlags.Instance | BindingFlags.SetField,
                       null,
                       obj,
                       new object[] { value });
        }
    }
}