using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoTSForEtong.ViewModel;
using AutoTSForETongUserCore;
using AutoTSForETongUserCore.Model;
using Ninject;

namespace AutoTSForEtong.Controllers
{
    public class UserController : Controller
    {
        [Inject]
        private IUserManager _userTools { get; set; }
        //
        [HttpGet]
        public ActionResult Login(LoginViewModel login)
        {
            if(login == null)
                login = new LoginViewModel();
            return View(login);
        }

        [HttpPost]
        public ActionResult LoginPost(LoginViewModel login)
        {
            if(string.IsNullOrEmpty(login.LoginName)||string.IsNullOrEmpty(login.LoginPassWord))
            {
                TempData["Error"] = "用户名或者密码不能为空！";
                return RedirectToAction("Login",login);
            }
            LoginResult result = _userTools.loginCheck(login.LoginName, login.LoginPassWord);
            if(result.LoginSuccess)
            {
                FormsAuthentication.SetAuthCookie(login.LoginName, false);
                var identity = _userTools.AcquireIdentity(login.LoginName);
                Session["Identity"] = identity.Identity;
                Session["UserID"] = identity.UserID;
                return Redirect(identity.ReturnUrl);
            }
            else
            {
                TempData["Error"] = "用户名或密码错误！";
                return RedirectToAction("Login", login);
            }
        }

        //LogOut
        public ActionResult LogOut()
        {
            if(Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session.RemoveAll();
            }
            return RedirectToAction("Login");
        }

        public ActionResult LayoutChanger()
        {
            if (Session["Identity"]!=null)
            {
                ViewBag.UserName = User.Identity.Name;
            }
            return PartialView("LayoutChanger");
        }

        public ActionResult LowMenuSection()
        {
            if(Session["Identity"] == null)
                return PartialView("_NullPage");
            string identity = (String)Session["Identity"];
            switch(identity)
            {
                case "学生" :
                    return PartialView("_StuMenu");
                case "教师":
                    return PartialView("_TeachMenu");
                case "教研员":
                    return PartialView("_EduerMenu");
                case "超级管理员":
                    return PartialView("_ManagerMenu");
                default:
                    return PartialView("_NullPage");
            }
        }
	}
}