using System;
using System.Web.Mvc;
using System.Data.Linq;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using COAT.Data.Export;
using COAT.Extension;


namespace COAT.COATExtension
{
    /// <summary>
    /// 提供将泛型集合数据导出Excel文档。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExcelResult<T> : ActionResult where T : new()
    {
        public ExcelResult(IList<T> entity, string fileName)
        {
            this.Entity = entity;
            this.FileName = fileName;
        }

        public ExcelResult(IList<T> entity)
        {
            this.Entity = entity;

            DateTime time = DateTime.Now;
            this.FileName = string.Format("{0}_{1}_{2}_{3}",
                time.Month, time.Day, time.Hour, time.Minute);
        }

        public IList<T> Entity
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (Entity == null)
            {
                new EmptyResult().ExecuteResult(context);
                return;
            }

            SetResponse(context);
        }

        public string GetContent()
        {
            StringBuilder sBuilder = ConvertEntity();
            return sBuilder.ToString();
        }

        /// <summary>
        /// 设置并向客户端发送请求响应。
        /// </summary>
        /// <param name="context"></param>
        private void SetResponse(ControllerContext context)
        {
            var content = GetContent();
            var encoder = System.Text.Encoding.GetEncoding("gb2312");
            byte[] bytestr = encoder.GetBytes(content);

            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ClearContent();
            context.HttpContext.Response.Charset = "GB2312";
            context.HttpContext.Response.ContentEncoding = encoder;
            context.HttpContext.Response.ContentType = "application/vnd.ms-excel";
            context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ".xls");
            context.HttpContext.Response.AddHeader("Content-Length", bytestr.Length.ToString());
            context.HttpContext.Response.Write(content);
            context.HttpContext.Response.End();
        }

        /// <summary>
        /// 把泛型集合转换成组合Excel表格的字符串。
        /// </summary>
        /// <returns></returns>
        private StringBuilder ConvertEntity()
        {
            Type t = typeof(T);
            StringBuilder sb = new StringBuilder();

            if (!HasProperty() || IsEmpty())
                return sb;

            if (t == typeof(ExportObject))
            {
                var helper = new TableFormatHelper(t);
                AddExportTableHead(sb, helper);
                AddExportTableData(sb, helper);

            }
            else
            {
                AddTableHead(sb);
                AddTableBody(sb);
            }

            return sb;
        }

        private void AddExportTableData(StringBuilder sb, TableFormatHelper helper)
        {
            var schema = helper.Schema.ToArray();
            for (int i = 0; i < Entity.Count; i++)
            {
                for (var j = 0; j < schema.Count(); j++)
                {
                    var sign = GetSign(j, schema.Count());
                    var obj = Entity[i].GetPropertyValue(schema[j].PropertyName);
                    var val = obj == null ? string.Empty : obj.ToString();
                    sb.Append(val + sign);
                }
            }
        }
        private void AddExportTableHead(StringBuilder sb, TableFormatHelper helper)
        {
            var schema = helper.Schema.ToArray();
            for (var index = 0; index < schema.Count(); index++)
            {
                var sign = GetSign(index, schema.Count());
                sb.Append(schema[index].ColunmName + sign);
            }
        }

        /// <summary>
        /// 根据IList泛型集合中的每项的属性值来组合Excel表格。
        /// </summary>
        /// <param name="sb"></param>
        private void AddTableBody(StringBuilder sb)
        {
            PropertyDescriptorCollection properties = FindProperties();
            for (int i = 0; i < Entity.Count; i++)
            {
                for (int j = 0; j < properties.Count; j++)
                {
                    string sign = GetSign(j, properties.Count);
                    object obj = properties[j].GetValue(Entity[i]);
                    obj = obj == null ? string.Empty : obj.ToString();
                    sb.Append(obj + sign);
                }
            }
        }

        /// <summary>
        /// 根据指定类型T的所有属性名称来组合Excel表头。
        /// </summary>
        /// <param name="sb"></param>
        private void AddTableHead(StringBuilder sb)
        {
            PropertyDescriptorCollection properties = FindProperties();
            for (int i = 0; i < properties.Count; i++)
            {
                string sign = GetSign(i, properties.Count);
                sb.Append(properties[i].Name + sign);
            }
        }

        /// <summary>
        /// 返回指定类型T的属性集合。
        /// </summary>
        /// <returns></returns>
        private static PropertyDescriptorCollection FindProperties()
        {
            return TypeDescriptor.GetProperties(typeof(T));
        }

        private bool HasProperty()
        {
            return FindProperties().Count > 0;

        }

        private bool IsEmpty()
        {
            return Entity == null || Entity.Count <= 0;

        }

        private string GetSign(int colIndex, int colCount)
        {
            return colIndex == colCount - 1 ? "\n" : "\t";
        }
    }
}
