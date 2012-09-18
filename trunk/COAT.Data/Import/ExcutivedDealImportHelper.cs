using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using COAT.Extension;
using COAT.Data.Generate;

namespace COAT.Data.Import
{
    public class ExcutivedDealImportHelper : ExcelImportHelper
    {
        public ExcutivedDealImportHelper(string path)
            : base(path)
        { }

        public void ImportRawData()
        {
            DataTable table = new DataTable();
            string[] tabNames = GetTableList();

            foreach (var name in tabNames)
            {
                try
                {
                    table = GetTableByName(name);
                    for (var index = 0; index < table.Rows.Count; index++)
                    {
                        if (table.Rows[index].IsEmptyRow())
                            continue;

                        GenerateRowData(table.Rows[index]);
                    }
                }
                catch
                {

                }
            }
        }

        private string Query(string tableName)
        {
            return string.Format("select * from [{0}]", tableName);
        }

        private DataTable GetTableByName(string tableName)
        {
            var query = Query(tableName);
            return GetTable(query);
        }

        private void GenerateRowData(DataRow row)
        {
            new ExcutivedDealGenerator(row).Generate();
        }
    }
}