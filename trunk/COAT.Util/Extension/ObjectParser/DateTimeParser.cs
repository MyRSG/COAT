using System;
using System.Globalization;

namespace COAT.Util.Extension.ObjectParser
{
    internal class DateTimeParser : IValueParser
    {
        #region IValueParser Members

        public object Parse(object value)
        {
            string dataString = value.ToString();

            DateTime rslt;
            if (DateTime.TryParse(dataString, new CultureInfo("en-GB"), DateTimeStyles.None, out rslt))
            {
                return rslt;
            }

            if (DateTime.TryParse(dataString, new CultureInfo("en-US"), DateTimeStyles.None, out rslt)
                )
            {
                return rslt;
            }
            return null;
        }

        #endregion
    }
}