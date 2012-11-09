namespace COAT.Util.Extension.ObjectParser
{
    internal class IntegerParser : IValueParser
    {
        #region IValueParser Members

        public object Parse(object value)
        {
            int rslt;
            if (int.TryParse(value.ToString(), out rslt))
            {
                return rslt;
            }
            return null;
        }

        #endregion
    }
}