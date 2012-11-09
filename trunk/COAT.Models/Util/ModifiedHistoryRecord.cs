using System;

namespace COAT.Models.Util
{
    internal class ModifiedHistoryRecord
    {
        public string PropertyName { get; set; }
        public object OrignalValue { get; set; }
        public object CurrentValue { get; set; }
        public string ActinoName { get; set; }

        public override string ToString()
        {
            return String.Format("'{0}' was modified from '{1}' to '{2}'",
                                 new[] {PropertyName, OrignalValue, CurrentValue});
        }
    }
}