using System.Data;
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
            string[] tabNames = GetTableList();

            foreach (var name in tabNames)
            {
                try
                {
                    DataTable table = GetTableByName(name);
                    for (var index = 0; index < table.Rows.Count; index++)
                    {
                        if (table.Rows[index].IsEmptyRow())
                            continue;

                        GenerateRowData(table.Rows[index]);
                    }
                }
                catch { }
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
            new DealProductGenerator(row).Generate();
        }




















    }
}