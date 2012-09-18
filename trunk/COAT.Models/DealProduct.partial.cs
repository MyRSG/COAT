using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using COAT.Models.Util;
using COAT.Extension;

namespace COAT.Models
{
    public partial class DealProduct
    {
        //ModifiedHistoryRecorder recorder = new ModifiedHistoryRecorder();

        //public List<ModifiedHistory> GetModifiedHistoryList()
        //{
        //    var list = new List<ModifiedHistory>();
        //    foreach (var rec in recorder.Records)
        //    {
        //        list.Add(createModifiedHistory(rec));
        //    }

        //    return list;
        //}

        //protected override void OnPropertyChanging(string property)
        //{
        //    recorder.SaveOrignalPropertyValue(property, this.GetPropertyValue(property));
        //    base.OnPropertyChanging(property);
        //}

        //protected override void OnPropertyChanged(string property)
        //{
        //    recorder.SaveCurrentPropertyValue(property, this.GetPropertyValue(property));
        //    base.OnPropertyChanged(property);
        //}

        //private ModifiedHistory createModifiedHistory(ModifiedHistoryRecord rec)
        //{
        //    return new ModifiedHistory
        //    {
        //        Action = rec.ActinoName,
        //        DealId = this.DealId,
        //        DealProductId = this.Id,
        //        Comment = rec.ToString()
        //    };
        //}

    }
}