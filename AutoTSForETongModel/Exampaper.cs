using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace AutoTSForETongModel
{
    public class Exampaper:Entity
    {
        [Key]
        public int PaperID { get; set; }

        [DisplayName("试卷标题")]
        public string PaperTitle { get; set; }

        [DisplayName("出卷人ID")]
        public int MemberID { get; set; }

        public Member Member { get; set; }

        [DisplayName("考试时间")]
        public int ExamTime { get; set; }

        [DisplayName("更新时间")]
        public DateTime UpdateTime { get; set; }

        [DisplayName("科目ID")]
        public int SubjectID { get; set; }

        [DisplayName("是否删除")]
        public bool IsDeleted { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<PaperExtraInfo> PaperExtraInfo { get; set; }
    }
}
