using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Extension;

namespace COAT.Models.Util
{
    internal class ModifiedHistoryRecorder
    {
        string _ActionName = "Modified";
        Dictionary<string, ModifiedHistoryRecord> modifiedHistoryRecords = new Dictionary<string, ModifiedHistoryRecord>();

        public List<ModifiedHistoryRecord> Records
        {
            get { return modifiedHistoryRecords.Values.ToList(); }
        }

        public void SaveOrignalPropertyValue(string property, object value)
        {
            SetActionName(value);

            var rec = new ModifiedHistoryRecord { PropertyName = property, OrignalValue = value };
            modifiedHistoryRecords.Update(property, rec);
        }

        public void SaveCurrentPropertyValue(string property, object value)
        {
            var rec = modifiedHistoryRecords.SafeGet(property, null);

            if (rec == null)
                return;

            rec.CurrentValue = value;
            rec.ActinoName = _ActionName;
        }

        private void SetActionName(object value)
        {
            if (value == null)
                _ActionName = "Add";
            else
                _ActionName = "Modified";
        }
    }
}