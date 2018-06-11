using Dostigator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Dostigator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //string result = "Вы не авторизованы";

            /*if (User.Identity.IsAuthenticated)
            {
                result = "Добро пожаловать," + User.Identity.Name;
            }*/
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Admin()
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.Where(x => x.Email.Contains(User.Identity.Name)).FirstOrDefault();
            }

            ViewBag.Message = "Your application description page.";
            ViewBag.User = user;

            return View();
        }
        
    }
}