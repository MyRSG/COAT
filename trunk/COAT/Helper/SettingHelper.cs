using System.Linq;
using COAT.Models;

namespace COAT.Helper
{
    public class SettingHelper
    {
        private readonly COATEntities _db = new COATEntities();

        public string this[string index]
        {
            get
            {
                var firstOrDefault = _db.Settings.FirstOrDefault(a => a.Name == index);
                return firstOrDefault != null ? firstOrDefault.Name : null;
            }
        }
    }
}