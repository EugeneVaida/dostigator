﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dostigator.Models;
using System.Text.RegularExpressions;
using System.Data.Entity.Migrations;

namespace Dostigator.Controllers
{    
    public class AimsController : Controller
    {
        private UserContext db = new UserContext();
        DateTime thisDay = DateTime.Today;

        // GET: Profile
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                User user = GetUser();

                IEnumerable<Aim> aim = null;
                using (UserContext db = new UserContext())
                {
                    aim = db.Aims.Include(y => y.User).Where(x => x.UserId == user.Id).ToList();
                }

                ViewBag.Aims = aim;
                ViewBag.User = user;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [Authorize(Roles = "User")]
        public ActionResult All()
        {

            if (User.Identity.IsAuthenticated)
            {
                User user = GetUser();

                IEnumerable<Aim> aim = null;
                using (UserContext db = new UserContext())
                {
                    aim = db.Aims.Include(y => y.User).ToList();
                }

                ViewBag.Aims = aim;
                ViewBag.User = user;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }


        // GET: Aims/Details/5
        [Authorize(Roles = "User")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aim aim = db.Aims.Find(id);
            if (aim == null)
            {
                return HttpNotFound();
            }

            User user = GetUser();

            IEnumerable<TimeLine> timeLines;
            timeLines = db.TimeLines.Include(y => y.Aim).Where(y =>  y.AimId == aim.Id );

            ViewBag.Panel = "";
            ViewBag.User = user;
            ViewBag.Lines = timeLines;

            return View(aim);
        }


        [Authorize(Roles = "User")]
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = GetUser();
                
                Aim emp = new Aim();
                ViewBag.User = user;
                ViewBag.UserId = user.Id;
                return View(emp);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PreviewText,Text,FinishDate,Group,UserId")] Aim aim, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                string pic;
                try
                {
                    pic = System.IO.Path.GetFileName(Img.FileName);
                    string path = System.IO.Path.Combine(
                                       Server.MapPath("~/AppFiles/Images"), pic);
                    // file is uploaded
                    Img.SaveAs(path);
                }
                catch
                {
                    pic = "default.png";
                }


                aim.ImagePath = "/AppFiles/Images/" + pic;
                aim.StartDate = thisDay.ToString("d");
                db.Aims.Add(aim);
                db.SaveChanges();
                return RedirectToAction("Index", "Profile");
            }
            User user = GetUser();
            ViewBag.User = user;

            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", aim.UserId);
            return View(aim);
        }

        [Authorize(Roles = "User")]
        public ActionResult Edit(int? id)
        {
            User user = GetUser();
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aim aim = db.Aims.Find(id);
            if (aim == null)
            {
                return HttpNotFound();
            }
            string s = aim.ImagePath;
            Regex regex = new Regex(@"[a-z\-0-9]+\.(jpg|png|gif)");
            MatchCollection matches = regex.Matches(s);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                    aim.ImagePath = match.Value;
            }
            ViewBag.Image = s;
            ViewBag.User = user;
            ViewBag.UserId = aim.UserId;
            ViewBag.StartDate = aim.StartDate;
            return View(aim);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PreviewText,Text,FinishDate,Group,UserId,StartDate,ImagePath")] Aim aim, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                string pic;
                try
                {
                    pic = System.IO.Path.GetFileName(Img.FileName);
                    string path = System.IO.Path.Combine(
                                       Server.MapPath("~/AppFiles/Images"), pic);
                    // file is uploaded
                    Img.SaveAs(path);
                }
                catch
                {
                    pic = aim.ImagePath;
                }

                aim.ImagePath = "/AppFiles/Images/" + pic;
                //db.Set<Aim>().AddOrUpdate(aim);
                db.Entry(aim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", aim.UserId);
            return View(aim);
        }

        [Authorize(Roles = "User")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aim aim = db.Aims.Where(x => x.Id == id).FirstOrDefault();
            if (aim == null)
            {
                return HttpNotFound();
            }
            User user = GetUser();
            ViewBag.User = user;
            return View(aim);
        }

        [Authorize(Roles = "User")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aim aim = db.Aims.Find(id);
            db.Aims.Remove(aim);
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



        //Search Method
        [Authorize(Roles = "User")]
        public ActionResult TagSearch(string id)
        {
            IEnumerable<Aim> aim = null;
            using (UserContext db = new UserContext())
            {
                aim = db.Aims.ToList().Where(z => z.Group == id);
            }

            if (aim == null)
            {
                return HttpNotFound();
            }

            ViewBag.Aims = aim;
            ViewBag.Tag = id;

            User user = GetUser();
            ViewBag.User = user;

            return View( );
        }

        [Authorize(Roles = "User")]
        public ActionResult Tag()
        {
            User user = GetUser();

            List<string> x = null;
            using (UserContext db = new UserContext())
            {
                x = db.Aims.GroupBy(y => y.Group).Select(y => y.Key).ToList();
            }

            ViewBag.Tags = x;            
            ViewBag.User = user;

            return View();
        }

        [Authorize(Roles = "User")]
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
