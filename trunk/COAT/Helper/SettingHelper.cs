using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;

namespace COAT.Helper
{
    public class SettingHelper
    {
        COATEntities db = new COATEntities();
        public string this[string index]
        {
            get
            {
                return db.Settings.FirstOrDefault(a => a.Name == index).Name;
            }
        }
    }
}