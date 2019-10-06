using ServiceStation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ServiceStation.Controllers
{
    public class AccountController : Controller
    {
        MyModel db = new MyModel();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user == null)
                {
                    int guestId = db.Roles.Where(g => g.RoleName == "User").FirstOrDefault().Id;
                    db.Users.Add(new User
                    {
                        Email = model.Email,
                        PasswordHash = RegisterModel.CreateHash(model.PasswordHash),
                        UserName = model.UserName,
                        PhoneNumber = model.PhoneNumber,
                        RoleId = guestId
                    });
                    db.SaveChanges();

                    string checkPass = RegisterModel.CreateHash(model.PasswordHash);
                    user = db.Users.Where(u => u.Email == model.Email && u.PasswordHash == checkPass).FirstOrDefault();

                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Email, true);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ошибка! Пользователь с таким email уже существует!");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string checkPass = RegisterModel.CreateHash(model.PasswordHash);
                User user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == checkPass);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка! Пользователь с таким email и паролем не найден!");
                }
            }

            return View(model);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}