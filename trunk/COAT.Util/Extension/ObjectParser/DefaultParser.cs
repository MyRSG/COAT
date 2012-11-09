namespace COAT.Util.Extension.ObjectParser
{
    internal class DefaultParser : IValueParser
    {
        #region IValueParser Members

        public object Parse(object value)
        {
            return value;
        }

        #endregion
    }
}