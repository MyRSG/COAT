using System.Linq;
using System.Web.Mvc;
using COAT.Models;

namespace COAT.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly COATEntities _db = new COATEntities();

        //
        // GET: /Setting/
        [Authorize]
        public ActionResult Index()
        {
            return View(_db.Settings);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(Setting[] settings)
        {
            foreach (Setting setting in settings)
            {
                var firstOrDefault = _db.Settings.FirstOrDefault(s => s.Name == setting.Name);
                if (firstOrDefault != null)
                    firstOrDefault.Value = setting.Value;
            }

            _db.SaveChanges();

            return View(_db.Settings);
        }
    }
}