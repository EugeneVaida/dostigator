using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dostigator.Models;

namespace Dostigator.Controllers
{
    public class TutorialsController : Controller
    {
        private UserContext db = new UserContext();
        
        // GET: Tutorials
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            User user = GetUser();
            ViewBag.User = user;

            return View(db.Tutorials.ToList());
        }

        // GET: Tutorials/Details/5        
        public ActionResult Details(int? id)
        {
            User user = GetUser();
            ViewBag.User = user;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutorial tutorial = db.Tutorials.Find(id);
            if (tutorial == null)
            {
                return HttpNotFound();
            }
            return View(tutorial);
        }

        // GET: Tutorials/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            User user = GetUser();
            ViewBag.User = user;

            return View();
        }

        // POST: Tutorials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "Id,Name,Text,PreviewText,Date")] Tutorial tutorial)
        {
            User user = GetUser();
            ViewBag.User = user;

            if (ModelState.IsValid)
            {
                DateTime thisDay = DateTime.Today;
                tutorial.Date = thisDay.ToString("d");

                db.Tutorials.Add(tutorial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tutorial);
        }

        // GET: Tutorials/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            User user = GetUser();
            ViewBag.User = user;
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutorial tutorial = db.Tutorials.Find(id);
            if (tutorial == null)
            {
                return HttpNotFound();
            }

            ViewBag.Date = tutorial.Date;
            return View(tutorial);
        }

        // POST: Tutorials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,Text,PreviewText,Date")] Tutorial tutorial)
        {
            User user = GetUser();
            ViewBag.User = user;

            if (ModelState.IsValid)
            {
                db.Entry(tutorial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tutorial);
        }

        // GET: Tutorials/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            User user = GetUser();
            ViewBag.User = user;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutorial tutorial = db.Tutorials.Find(id);
            if (tutorial == null)
            {
                return HttpNotFound();
            }
            return View(tutorial);
        }

        // POST: Tutorials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = GetUser();
            ViewBag.User = user;

            Tutorial tutorial = db.Tutorials.Find(id);
            db.Tutorials.Remove(tutorial);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public User GetUser()
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
            }
            return user;
        }
    }
}
