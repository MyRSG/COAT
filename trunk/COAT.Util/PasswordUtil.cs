using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COAT.Util
{
    public class PasswordUtil
    {
        public static string NEW_PASSWORD_HEAD = "NW";

        public static string CreatePassword()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(DateTime.UtcNow.ToFileTimeUtc().ToString());
            string longPWD = new string(Convert.ToBase64String(bytes).Reverse().Take(6).ToArray());

            if (longPWD.Length < 6)
            {
                return string.Format("{0}{1}", NEW_PASSWORD_HEAD, longPWD);
            }

            return string.Format("{0}{1}", NEW_PASSWORD_HEAD, longPWD.Substring(0, 6));
        }
    }
}
