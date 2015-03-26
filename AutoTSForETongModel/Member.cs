using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTSForETongModel
{
    public enum Identity { 学生 ,老师,教务员,超级管理员};
    public class Member:Entity
    {
        public int MemberID { get; set; }

        [DisplayName("姓名")]
        public string MemberName { get; set; }

        [DisplayName("密码")]
        public string Password { get; set; }

        [DisplayName("身份")]
        public string Identity { get; set; }

        public bool IsDeleted { get; set; }
    }
}
