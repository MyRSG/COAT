using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace COAT.Extension
{
    public static class DataRowExtension
    {
        public static void FillObject(this DataRow row, object obj, IEnumerable<ColunmPropertyPair> pairCollection)
        {
            foreach (var pair in pairCollection)
            {
                var val = row.GetTableValueByPair(pair);
                if (val == null || val == DBNull.Value)
                    continue;

                obj.SetPropertyValue(pair.PropertyName, val);

            }
        }

        public static object GetTableValueByPair(this DataRow row, ColunmPropertyPair pair)
        {
            if (pair.ColunmIndex >= 0)
                return row[pair.ColunmIndex];

            if (pair.IsOptional && !row.ContainColunm(pair.ColunmName))
                return null;

            return row[pair.ColunmName];
        }

        public static bool IsEmptyRow(this DataRow row)
        {
            return row.ItemArray.Count(i => i != null && i != DBNull.Value) < 1;
        }

        public static bool ContainColunm(this DataRow row, string colName)
        {
            return row.Table.Columns.Contains(colName);
        }

    }

}