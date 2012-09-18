using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAT.Models;

namespace COAT.Controllers
{ 
    public class UserController : Controller
    {
        private COATEntities db = new COATEntities();

        //
        // GET: /User/

        public ViewResult Index()
        {
            var users = db.Users.Include("BusinessRole").Include("SystemRole");
            return View(users.ToList());
        }

        //
        // GET: /User/Details/5

        public ViewResult Details(int id)
        {
            User user = db.Users.Single(u => u.Id == id);
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            ViewBag.BusinessRoleId = new SelectList(db.BusinessRoles, "Id", "Name");
            ViewBag.SystemRoleId = new SelectList(db.SystemRoles, "Id", "Name");
            return View();
        } 

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.AddObject(user);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.BusinessRoleId = new SelectList(db.BusinessRoles, "Id", "Name", user.BusinessRoleId);
            ViewBag.SystemRoleId = new SelectList(db.SystemRoles, "Id", "Name", user.SystemRoleId);
            return View(user);
        }
        
        //
        // GET: /User/Edit/5
 
        public ActionResult Edit(int id)
        {
            User user = db.Users.Single(u => u.Id == id);
            ViewBag.BusinessRoleId = new SelectList(db.BusinessRoles, "Id", "Name", user.BusinessRoleId);
            ViewBag.SystemRoleId = new SelectList(db.SystemRoles, "Id", "Name", user.SystemRoleId);
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Attach(user);
                db.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessRoleId = new SelectList(db.BusinessRoles, "Id", "Name", user.BusinessRoleId);
            ViewBag.SystemRoleId = new SelectList(db.SystemRoles, "Id", "Name", user.SystemRoleId);
            return View(user);
        }

        //
        // GET: /User/Delete/5
 
        public ActionResult Delete(int id)
        {
            User user = db.Users.Single(u => u.Id == id);
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            User user = db.Users.Single(u => u.Id == id);
            db.Users.DeleteObject(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}