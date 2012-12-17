using System;
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
            var tabNames = GetTableList();

            foreach (var name in tabNames)
            {
                try
                {
                    var table = GetTableByName(name);
                    for (var index = 0; index < table.Rows.Count; index++)
                    {
                        if (table.Rows[index].IsEmptyRow())
                            continue;

                        GenerateRowData(table.Rows[index]);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                }
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
            new UserGenertator(SystemRoleId, BusinessRoleId, MailMsg, row).Generate();
        }
    }
}