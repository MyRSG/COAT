using System.Linq;
using COAT.Models;

namespace COAT.Helper
{
    public class SettingHelper
    {
        private readonly COATEntities db = new COATEntities();

        public string this[string index]
        {
            get
            {
                var firstOrDefault = db.Settings.FirstOrDefault(a => a.Name == index);
                if (firstOrDefault != null)
                    return firstOrDefault.Name;
                return null;
            }
        }
    }
}