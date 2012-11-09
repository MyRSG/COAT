using System.Data;
using COAT.Data.Generate;
using COAT.Util.Extension;

namespace COAT.Data.Import
{
    public class ExcutivedDealImportHelper : ExcelImportHelper
    {
        public ExcutivedDealImportHelper(string path)
            : base(path)
        {
        }

        public void ImportRawData()
        {
            string[] tabNames = GetTableList();

            foreach (string name in tabNames)
            {
                try
                {
                    DataTable table = GetTableByName(name);
                    for (int index = 0; index < table.Rows.Count; index++)
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
            string query = Query(tableName);
            return GetTable(query);
        }

        private void GenerateRowData(DataRow row)
        {
            new ExcutivedDealGenerator(row).Generate();
        }
    }
}