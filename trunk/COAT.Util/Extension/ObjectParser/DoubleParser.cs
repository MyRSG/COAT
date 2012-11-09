namespace COAT.Util.Extension.ObjectParser
{
    internal class DoubleParser : IValueParser
    {
        #region IValueParser Members

        public object Parse(object value)
        {
            double rslt;
            if (double.TryParse(value.ToString(), out rslt))
            {
                return rslt;
            }
            return null;
        }

        #endregion
    }
}