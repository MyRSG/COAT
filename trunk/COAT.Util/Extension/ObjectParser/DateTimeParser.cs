using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace COAT.Util.Extension.ObjectParser
{
    class DateTimeParser : IValueParser
    {
        public object Parse(object value)
        {
            var dataString = value.ToString();

            DateTime rslt;
            if (DateTime.TryParse(dataString, new CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out rslt))
            {
                return rslt;
            }

            if (DateTime.TryParse(dataString, new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out rslt)
            )
            {
                return rslt;
            }
            return null;
        }
    }
}
