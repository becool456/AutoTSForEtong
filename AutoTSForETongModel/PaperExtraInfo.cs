using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTSForETongModel
{
    public class PaperExtraInfo:Entity
    {
        [Key]
        public int ExtraID { get; set; }
        [DisplayName("所在试卷ID")]
        public int PaperID { get; set; }
        [DisplayName("题型")]
        public string QuestionType { get; set; }
        [DisplayName("题型主标题")]
        public string QuestionTypeTitle { get; set; }
        [DisplayName("题型分值")]
        public int QuestionTypeGrade { get; set; }
        [DisplayName("添加时间")]
        public DateTime UpdateTime { get; set; }
    }
}
