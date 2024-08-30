using ManageLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManageLibrary.Controllers
{
    public class AccountController : Controller
    {
        private ManagerLibraryContext context = new ManagerLibraryContext();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            if (userName == null || password == null) {
                ViewBag.message = "userName, password empty";
                return View();
            }
            var user = context.Users.FirstOrDefault(x => x.UserName == userName && x.Password == password);
            var admin = context.Admins.FirstOrDefault(x => x.AdminName == userName && x.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("Index", "Home");
            }
            else if (admin != null)
            {
                HttpContext.Session.SetInt32("AdminId", admin.Id);
                return RedirectToAction("Dashboard", "Admin");
            }
            else 
            {
                ViewBag.message = "userName, password wrong";
                return View();
            }
        }
        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("UserId");  
            HttpContext.Session.Remove("AdminId");

            return RedirectToAction("Login");
        }


    }
}
