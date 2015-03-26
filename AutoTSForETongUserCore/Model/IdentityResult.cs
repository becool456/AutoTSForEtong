using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTSForETongUserCore.Model
{
    public class IdentityResult
    {
        [DisplayName("用户ID")]
        public int UserID { get; set; }
        [DisplayName("身份名")]
        public string Identity { get; set; }

        [DisplayName("返回路径")]
        public string ReturnUrl { get; set; }
    }
}
