using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COAT.Util.Extension.ObjectParser
{
    class DefaultParser : IValueParser
    {
        public object Parse(object value)
        {
            return value;
        }
    }
}
