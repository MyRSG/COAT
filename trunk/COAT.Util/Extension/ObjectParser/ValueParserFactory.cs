using System.Reflection;

namespace COAT.Util.Extension.ObjectParser
{
    internal class ValueParserFactory
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