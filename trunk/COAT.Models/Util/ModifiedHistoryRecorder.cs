using System.Collections.Generic;
using System.Linq;
using COAT.Util.Extension;

namespace COAT.Models.Util
{
    internal class ModifiedHistoryRecorder
    {
        private readonly Dictionary<string, ModifiedHistoryRecord> _modifiedHistoryRecords =
            new Dictionary<string, ModifiedHistoryRecord>();

        private string _actionName = "Modified";

        public List<ModifiedHistoryRecord> Records
        {
            get { return _modifiedHistoryRecords.Values.ToList(); }
        }

        public void SaveOrignalPropertyValue(string property, object value)
        {
            SetActionName(value);

            var rec = new ModifiedHistoryRecord {PropertyName = property, OrignalValue = value};
            _modifiedHistoryRecords.Update(property, rec);
        }

        public void SaveCurrentPropertyValue(string property, object value)
        {
            ModifiedHistoryRecord rec = _modifiedHistoryRecords.SafeGet(property, null);

            if (rec == null)
                return;

            rec.CurrentValue = value;
            rec.ActinoName = _actionName;
        }

        private void SetActionName(object value)
        {
            _actionName = value == null ? "Add" : "Modified";
        }
    }
}