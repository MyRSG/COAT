using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Web.Mvc;
using COAT.Models;

namespace COAT.Controllers
{
    public class UserController : Controller
    {
        private readonly COATEntities _db = new COATEntities();

        //
        // GET: /User/

        public ViewResult Index()
        {
            ObjectQuery<User> users = _db.Users.Include("BusinessRole").Include("SystemRole");
            return View(users.ToList());
        }

        //
        // GET: /User/Details/5

        public ViewResult Details(int id)
        {
            User user = _db.Users.Single(u => u.Id == id);
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            ViewBag.BusinessRoleId = new SelectList(_db.BusinessRoles, "Id", "Name");
            ViewBag.SystemRoleId = new SelectList(_db.SystemRoles, "Id", "Name");
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.AddObject(user);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessRoleId = new SelectList(_db.BusinessRoles, "Id", "Name", user.BusinessRoleId);
            ViewBag.SystemRoleId = new SelectList(_db.SystemRoles, "Id", "Name", user.SystemRoleId);
            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            User user = _db.Users.Single(u => u.Id == id);
            ViewBag.BusinessRoleId = new SelectList(_db.BusinessRoles, "Id", "Name", user.BusinessRoleId);
            ViewBag.SystemRoleId = new SelectList(_db.SystemRoles, "Id", "Name", user.SystemRoleId);
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Attach(user);
                _db.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessRoleId = new SelectList(_db.BusinessRoles, "Id", "Name", user.BusinessRoleId);
            ViewBag.SystemRoleId = new SelectList(_db.SystemRoles, "Id", "Name", user.SystemRoleId);
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id)
        {
            User user = _db.Users.Single(u => u.Id == id);
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = _db.Users.Single(u => u.Id == id);
            _db.Users.DeleteObject(user);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}