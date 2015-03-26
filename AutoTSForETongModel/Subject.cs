using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AutoTSForETongModel
{
    /// <summary>
    /// 科目
    /// </summary>
    public class Subject : Entity
    {
        [Key]
        public int SubjectID { get; set; }
        [DisplayName("科目名称")]
        public string SubjectName { get; set; }
    }
}
