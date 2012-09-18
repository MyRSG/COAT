using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using COAT.Extension;
using COAT.Data.Generate;
using System.Net.Mail;

namespace COAT.Data.Import
{
    public class UserImportHelper : ExcelImportHelper
    {
        public int SystemRoleId { get; set; }
        public int BusinessRoleId { get; set; }
        public MailMessage MailMsg { get; set; }

        public UserImportHelper(int systemRoleId, int bussinessRoleId, MailMessage mailMsg, string path)
            : base(path)
        {
            SystemRoleId = systemRoleId;
            BusinessRoleId = bussinessRoleId;
            MailMsg = mailMsg;
        }

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
            new UserGenertator(SystemRoleId, BusinessRoleId, MailMsg, row).Generate();
        }
    }
}