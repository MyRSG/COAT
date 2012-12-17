using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace COAT.Data.Export
{
    public class TableFormatHelper
    {
        private IEnumerable<ColunmPropertyPair> _schema;

        public TableFormatHelper(object obj)
            : this(obj.GetType())
        {
        }

        public TableFormatHelper(Type type)
        {
            ExportType = type;
        }

        public Type ExportType { get; protected set; }

        public IEnumerable<ColunmPropertyPair> Schema
        {
            get { return _schema ?? (_schema = GetTableSchema()); }
        }

        public IEnumerable<ColunmPropertyPair> GetTableSchema()
        {
            var props = GetExportProperties();

            var rslt = (from p in props
                        let attr = GetExportAttribute(p)
                        select new ColunmPropertyPair
                                   {
                                       ColunmName = string.IsNullOrEmpty(attr.Name) ? p.Name : attr.Name, 
                                       Order = attr.Order, 
                                       PropertyName = p.Name
                                   }).ToList();

            return rslt.OrderBy(a => a.Order);
        }

        private IEnumerable<PropertyInfo> GetExportProperties()
        {
            var props = ExportType.GetProperties();
            return props.Where(p => GetExportAttribute(p) != null);
        }

        private ExportAttribute GetExportAttribute(PropertyInfo p)
        {
            var attrs = p.GetCustomAttributes(typeof (ExportAttribute), false);
            if (attrs.Length == 0)
                return null;

            return (ExportAttribute) attrs[0];
        }
    }
}