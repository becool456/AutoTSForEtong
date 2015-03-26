using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTSForETongModel
{
    public class MemberSubjectMap:Entity
    {
        [DisplayName("映射ID")]
        [Key]
        public int MapID { get; set; }
        [DisplayName("用户ID")]
        public int MemberID { get; set; }
        [DisplayName("科目ID")]
        public int SubjectID { get; set; }
    }
}
