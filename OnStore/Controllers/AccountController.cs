using OnStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnStore.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext db = new AppDbContext();

        //Đăng ký
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterView model)
        {
            if (ModelState.IsValid)
            {
                var existing = db.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existing != null)
                {
                    ViewBag.Error = "Email này đã được sử dụng.";
                    return View();
                }

                var user = new Users
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "User"
                };

                db.Users.Add(user);
                db.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(model);
        }
        //Đăng nhập
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginView model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null && model.Password == user.Password)
                {
                    Session["UserName"] = user.UserName;
                    Session["Email"] = user.Email;
                    Session["Role"] = user.Role;
                    return RedirectToAction("Index", "Home", new { area = "" });
                }

                ViewBag.Error = "Email hoặc mật khẩu không đúng.";
            }
            return View(model);
        }

        // LOGOUT 
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}