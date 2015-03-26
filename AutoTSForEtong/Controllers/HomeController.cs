using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using AutoTSForETongModel;
using AutoTSForETongUserCore;
using AutoTSForETongUserCore.Model;

namespace AutoTSForEtong.Controllers
{
    public class HomeController : Controller
    {
        [Inject]
        private IUserManager _userManager { get; set; }

        public ActionResult Index()
        {
            IdentityResult jumper;
            if(Session["UserID"] != null)
            {
                var user = _userManager.GetByID((int)Session["UserID"]);
                jumper = _userManager.AcquireIdentity(user.MemberName);
            }
            else
            {
                jumper = _userManager.AcquireIdentity(string.Empty);
            }
            return Redirect(jumper.ReturnUrl);
        }
    }
}
