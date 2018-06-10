using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dostigator.Models;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Text;

namespace Dostigator.Controllers
{
    public class ProfileController : Controller
    {
        private UserContext db = new UserContext();

        // GET: Profile
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
                }

                IEnumerable<Aim> aim = null;
                IEnumerable<TimeLine> lines = null;
                using (UserContext db = new UserContext())
                {
                    aim = db.Aims.Include(y => y.User).Where(z => z.UserId == user.Id).ToList();
                    lines = db.TimeLines.Include(y => y.Aim).Where(y => y.Aim.User.Id == user.Id).ToList();
                } 
                
                ViewBag.Aims = aim.Reverse().Take(3);
                ViewBag.User = user;
                ViewBag.Line = lines.Reverse().Take(6);
                return View();
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
            
        }

        [Authorize(Roles = "User")]
        public ActionResult Room()
        {

            if (User.Identity.IsAuthenticated)
            {
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
                }

                IEnumerable<Aim> aim = null;
                IEnumerable<TimeLine> lines = null;
                using (UserContext db = new UserContext())
                {
                    aim = db.Aims.Include(y => y.User).Where(z => z.UserId == user.Id).ToList();
                    lines = db.TimeLines.Include(y => y.Aim).Where(y => y.Aim.User.Id == user.Id).ToList();
                }

                ViewBag.Aims = aim.Reverse().Take(3);
                ViewBag.User = user;
                ViewBag.Line = lines.Reverse().Take(6);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        /*
        public ActionResult Aims()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();

                }

                IEnumerable<Aim> aim = null;
                using (UserContext db = new UserContext())
                {
                    aim = db.Aims.Include(y => y.User).ToList().Where(z => z.UserId == user.Id);
                }

                ViewBag.User = user;
                ViewBag.Aims = aim;
                return View(user);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        // GET: Aims/Details/5
        public ActionResult AimDetails(int? id)
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
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
            }

            ViewBag.User = user;
            return View(aim);
        }


        public ActionResult CreateAim()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
                }
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAim([Bind(Include = "Id,Name,PreviewText,Text,FinishDate,Group,UserId")] Aim aim, HttpPostedFileBase Img)
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
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
            }
            ViewBag.User = user;
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", aim.UserId);
            return View(aim);
        }

        
        public ActionResult EditAim(int? id)
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
            }
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aim aim = db.Aims.Find(id);
            if (aim == null)
            {
                return HttpNotFound();
            }
            ViewBag.User = user;
            ViewBag.UserId = aim.UserId;
            return View(aim);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAim([Bind(Include = "Id,Name,PreviewText,Text,FinishDate,Group,UserId")] Aim aim, HttpPostedFileBase Img)
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
                db.Entry(aim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", aim.UserId);
            return View(aim);
        }

        
        public ActionResult DeleteAim(int? id)
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
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
            }
            ViewBag.User = user;
            return View(aim);
        }

        
        [HttpPost, ActionName("DeleteAim")]
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
        }*/

    }
}