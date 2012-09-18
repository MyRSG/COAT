using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using COAT.Extension;

namespace COAT.Util.Extension.ObjectParser
{
    class ValueParserFactory
    {
        public IValueParser GetValueParser(PropertyInfo prop)
        {
            if (prop.IsInteger())
                return new IntegerParser();

            if (prop.IsDouble())
                return new DoubleParser();

            if (prop.IsDateTime())
                return new DateTimeParser();

            if (prop.IsEntity())
                return null;

            return new DefaultParser();
        }
    }
}
