using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace COAT.Util
{
    public class PasswordUtil
    {
        public static string NewPasswordHead = "NW";

        public static string CreatePassword()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(DateTime.UtcNow.ToFileTimeUtc().ToString(CultureInfo.InvariantCulture));
            var longPwd = new string(Convert.ToBase64String(bytes).Reverse().Take(6).ToArray());

            return string.Format("{0}{1}", NewPasswordHead, Get6LengthPassword(longPwd));
        }

        private static string Get6LengthPassword(string longPwd)
        {
            return longPwd.Length < 6 ? longPwd : longPwd.Substring(0, 6);
        }
    }
}