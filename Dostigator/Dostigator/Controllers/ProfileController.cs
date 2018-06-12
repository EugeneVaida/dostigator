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

        //[Authorize(Roles = "User")]
        public ActionResult Materials()
        {

            if (User.Identity.IsAuthenticated)
            {
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
                }

                IEnumerable<Tutorial> tutorial = null;
                using (UserContext db = new UserContext())
                {
                    tutorial = db.Tutorials.ToList();                 
                }

                ViewBag.Tutorials = tutorial;
                ViewBag.User = user;                
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }        

    }
}