using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace COAT.Data.Import
{
    public class ExcelImportHelper
    {
        public const string TableNameHeader = "TABLE_NAME";

        //HDR=NO and IMEX=1 force all value to string [first row must be a string title]
        protected const string XlsProvideStrFormat =
            "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0; HDR=NO; IMEX=1;\"";

        protected const string XlsxProvideStrFormat =
            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=NO; IMEX=1;\"";

        private readonly Dictionary<string, int> _headerCount = new Dictionary<string, int>();

        public ExcelImportHelper(string path)
        {
            Path = path;
        }

        protected string Path { get; set; }

        public DataTable GetTable(string query)
        {
            DataTable dt = GetFilledTable(query);
            ReColunmTable(dt);
            return dt;
        }

        public string[] GetTableList()
        {
            string[] rslt = null;
            SaveOpenConnection(conn =>
                                   {
                                           rslt = conn
                                               .GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null)
                                               .AsEnumerable()
                                               .Select(r => r[TableNameHeader].ToString())
                                               .ToArray();
                                   });

            return rslt;
        }

        private OleDbConnection GetConnection()
        {
            string connString = GetConnectionString();
            return new OleDbConnection(connString);
        }

        private string GetConnectionString()
        {
            var info = new FileInfo(Path);
            if (info.Extension == ".xls")
                return String.Format(XlsProvideStrFormat, Path);
            if (info.Extension == ".xlsx")
                return String.Format(XlsxProvideStrFormat, Path);

            return null;
        }

        private DataTable GetFilledTable(string query)
        {
            var dt = new DataTable();
            SaveOpenConnection(conn =>
                                   {
                                       using (var cmd = new OleDbCommand(query, conn))
                                       {
                                           var ad = new OleDbDataAdapter(cmd);
                                           ad.Fill(dt);
                                       }
                                   });
            return dt;
        }

        private void ReColunmTable(DataTable dt)
        {
            //Rename columns and delete first row
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    if (!string.IsNullOrWhiteSpace(dr[col].ToString()))
                        dt.Columns[col].ColumnName = GetColunmName(dr[col].ToString());
                }

                dt.Rows[0].Delete();
                dt.AcceptChanges();
            }
        }

        private string GetColunmName(string headerString)
        {
            if (_headerCount.ContainsKey(headerString))
            {
                _headerCount[headerString]++;
                return string.Format("{0}{1}", headerString, _headerCount[headerString]);
            }

            _headerCount.Add(headerString, 0);
            return headerString;
        }

        private void SaveOpenConnection(Action<OleDbConnection> action)
        {
            OleDbConnection conn = GetConnection();
            try
            {
                conn.Open();
                action(conn);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}