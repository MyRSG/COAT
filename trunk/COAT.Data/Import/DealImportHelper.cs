using System;
using System.Data;
using System.Linq;
using COAT.Data.Generate;
using COAT.Util.Extension;

namespace COAT.Data.Import
{
    public class DealImportHelper : ExcelImportHelper
    {
        public DealImportHelper(string path)
            : base(path)
        { }

        public void ImportRawData()
        {
            var tabNames = GetTableList();
            foreach (var table in tabNames.Select(SafeGetTable).Where(table => table != null))
            {
                GenerateTableData(table);
            }
        }

        private DataTable SafeGetTable(string name)
        {
            try
            {
                return GetTableByName(name);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }

            return null;
        }

        private void GenerateTableData(DataTable table)
        {
            for (var index = 0; index < table.Rows.Count; index++)
            {
                if (table.Rows[index].IsEmptyRow())
                    continue;

                GenerateRowData(table.Rows[index]);
            }
        }

        private static string Query(string tableName)
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
            try
            {
                new DealProductGenerator(row).Generate();
            }
            catch (ArgumentException e)
            {
                Logger.Error(e.Message);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
                throw;
            }
        }




















    }
}