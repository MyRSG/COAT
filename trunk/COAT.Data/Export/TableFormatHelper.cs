using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace COAT.Data.Export
{
    public class TableFormatHelper
    {
        public Type ExportType { get; protected set; }

        public IEnumerable<ColunmPropertyPair> Schema
        {
            get
            {
                if (_Schema == null)
                    _Schema = GetTableSchema();
                return _Schema;
            }
        }

        public TableFormatHelper(object obj)
            : this(obj.GetType())
        { }

        public TableFormatHelper(Type type)
        {
            ExportType = type;
        }

        public IEnumerable<ColunmPropertyPair> GetTableSchema()
        {
            var rslt = new List<ColunmPropertyPair>();
            var props = GetExportProperties();

            ExportAttribute attr = null;
            foreach (var p in props)
            {
                attr = GetExportAttribute(p);
                rslt.Add(new ColunmPropertyPair
                {
                    ColunmName = string.IsNullOrEmpty(attr.Name) ? p.Name : attr.Name,
                    Order = attr.Order,
                    PropertyName = p.Name
                });
            }

            return rslt.OrderBy(a => a.Order);
        }

        private IEnumerable<PropertyInfo> GetExportProperties()
        {
            var props = ExportType.GetProperties();
            return props.Where(p => GetExportAttribute(p) != null);
        }

        private ExportAttribute GetExportAttribute(PropertyInfo p)
        {
            var attrs = p.GetCustomAttributes(typeof(ExportAttribute), false);
            if (attrs.Length == 0)
                return null;

            return (ExportAttribute)attrs[0];
        }

        private IEnumerable<ColunmPropertyPair> _Schema;
    }
}
