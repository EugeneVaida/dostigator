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
    public class TimeLinesController : Controller
    {
        private UserContext db = new UserContext();
        DateTime thisDay = DateTime.Today;


        // GET: TimeLines
        public ActionResult Index()
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
            }
            ViewBag.User = user;

            var timeLines = db.TimeLines.Include(y => y.Aim).Where(y => y.Aim.User.Id == user.Id);

            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Tag = "true";
                switch (TempData["Message"].ToString())
                {
                    case "Create":
                        ViewBag.Message = "Новый отчет добавлен";
                        break;
                    case "Delete":
                        ViewBag.Message = "Отчет удален";
                        break;
                    case "Edit":
                        ViewBag.Message = "Отчет изменен";
                        break;
                }
            }
            else
            {
                ViewBag.Tag = "faulse";
            }         
           
            return View(timeLines.ToList());
        }

        // GET: TimeLines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeLine timeLine = db.TimeLines.Find(id);
            if (timeLine == null)
            {
                return HttpNotFound();
            }
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
            }
            ViewBag.User = user;
            return View(timeLine);
        }

        // GET: TimeLines/Create/5
        public ActionResult Create(int? id)
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
            }
            ViewBag.User = user;
            ViewBag.AimId = id;
            return View();
        }

        // POST: TimeLines/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Text,AimId")] TimeLine timeLine)
        {
            timeLine.Date = thisDay.ToString("d");
            if (ModelState.IsValid)
            {
                if (db.TimeLines.Count() != 0)
                {
                    var x = db.TimeLines.ToList().Last();
                    if (x.Position == "r")
                    {
                        timeLine.Position = "l";
                    }
                    else
                    {
                        timeLine.Position = "r";
                    }
                }
                else
                {
                    timeLine.Position = "r";
                }                
                db.TimeLines.Add(timeLine);
                db.SaveChanges();

                TempData["Message"] = "Create";

                return RedirectToAction("Index");
            }
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
            }
            ViewBag.User = user;

            SelectList aims = new SelectList(db.Aims, "AimId", "Id");
            ViewBag.AimId = aims;

            
            
            return View(timeLine);
        }

        // GET: TimeLines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeLine timeLine = db.TimeLines.Find(id);
            if (timeLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.AimId = new SelectList(db.Aims, "Id", "Name", timeLine.AimId);
            return View(timeLine);
        }

        // POST: TimeLines/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Text,PreviewText,Date,AimId")] TimeLine timeLine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeLine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AimId = new SelectList(db.Aims, "Id", "Name", timeLine.AimId);
            return View(timeLine);
        }

        // GET: TimeLines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeLine timeLine = db.TimeLines.Find(id);
            if (timeLine == null)
            {
                return HttpNotFound();
            }
            return View(timeLine);
        }

        // POST: TimeLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimeLine timeLine = db.TimeLines.Find(id);
            db.TimeLines.Remove(timeLine);
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
    }
}
