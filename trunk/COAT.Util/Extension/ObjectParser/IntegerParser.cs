using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COAT.Util.Extension.ObjectParser
{
    class IntegerParser : IValueParser
    {
        public object Parse(object value)
        {
            int rslt;
            if (int.TryParse(value.ToString(), out rslt))
            {
                return rslt;
            }
            return null;
        }
    }
}
