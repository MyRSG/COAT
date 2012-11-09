using System.Data;
using System.Net.Mail;
using COAT.Data.Generate;
using COAT.Util.Extension;

namespace COAT.Data.Import
{
    public class UserImportHelper : ExcelImportHelper
    {
        public UserImportHelper(int systemRoleId, int bussinessRoleId, MailMessage mailMsg, string path)
            : base(path)
        {
            SystemRoleId = systemRoleId;
            BusinessRoleId = bussinessRoleId;
            MailMsg = mailMsg;
        }

        public int SystemRoleId { get; set; }
        public int BusinessRoleId { get; set; }
        public MailMessage MailMsg { get; set; }

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
            new UserGenertator(SystemRoleId, BusinessRoleId, MailMsg, row).Generate();
        }
    }
}