using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COAT.Util.Extension.ObjectParser
{
    class DoubleParser : IValueParser
    {
        public object Parse(object value)
        {
            double rslt;
            if (double.TryParse(value.ToString(), out rslt))
            {
                return rslt;
            }
            return null;
        }
    }
}
