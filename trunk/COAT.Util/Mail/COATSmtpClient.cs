using System;
using System.Net.Mail;
using System.Reflection;

namespace COAT.Util.Mail
{
    internal class COATSmtpClient
    {
        private const string OldHostFieldName = "localHostName";
        private const string NewHostFieldName = "clientDomain";

        private readonly SmtpClient _smtp;

        public COATSmtpClient()
        {
            _smtp = new SmtpClient
                        {
                            Host = COATMailParameters.Instance.Host,
                            Timeout = COATMailParameters.Instance.Timeout,
                            DeliveryMethod = COATMailParameters.Instance.DeliveryMethod,
                            UseDefaultCredentials = COATMailParameters.Instance.UseDefaultCredentials
                        };

            LocalHostName = "COAT";
        }

        public string LocalHostName
        {
            get
            {
                if (IsNewEnvoriment())
                {
                    return GetField(NewHostFieldName, _smtp).ToString();
                }

                if (IsOldEnvoriment())
                {
                    return GetField(OldHostFieldName, _smtp).ToString();
                }

                return Environment.MachineName;
            }
            set
            {
                string heloName = string.IsNullOrEmpty(value) ? Environment.MachineName : value;

                // member "localHostName" changed to "clientDomain" in .NET Fx 4.0
                if (IsNewEnvoriment())
                {
                    SetField(NewHostFieldName, _smtp, heloName);
                }
                else if (IsOldEnvoriment())
                {
                    SetField(OldHostFieldName, _smtp, heloName);
                }
            }
        }

        public void Send(MailMessage message)
        {
            _smtp.Send(message);
        }

        private static bool IsOldEnvoriment()
        {
            return Environment.Version.Major == 2 && Environment.Version.Build == 50727;
        }

        private static bool IsNewEnvoriment()
        {
            return Environment.Version.Major == 4
                   || (Environment.OSVersion.Version.Major > 6)
                   || (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1);
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

        private void SetField(string fieldName, object obj, object value)
        {
            if (obj != null)
                obj.GetType().InvokeMember(fieldName,
                                           BindingFlags.DeclaredOnly |
                                           BindingFlags.Public | BindingFlags.NonPublic |
                                           BindingFlags.Instance | BindingFlags.SetField,
                                           null,
                                           obj,
                                           new[] {value});
        }
    }
}