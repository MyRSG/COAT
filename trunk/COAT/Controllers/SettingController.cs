using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAT.Models;

namespace COAT.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        COATEntities db = new COATEntities();

        //
        // GET: /Setting/
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Settings);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(Setting[] settings)
        {
            foreach (var setting in settings)
            {
                db.Settings.FirstOrDefault(s => s.Name == setting.Name).Value = setting.Value;
            }

            db.SaveChanges();

            return View(db.Settings);
        }
    }
}
