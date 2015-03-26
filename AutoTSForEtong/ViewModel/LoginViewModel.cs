using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace AutoTSForEtong.ViewModel
{
    public class LoginViewModel
    {
         [DisplayName("登录名")]
        public string LoginName { get; set; }
         [DisplayName("密码")]
        public string LoginPassWord { get; set; }
    }
}