using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutoTSForETongModel;

namespace AutoTSForETongSysCore.Model
{
    public class PreviewPaper
    {
        [DisplayName("试卷标题")]
        public string PaperTitle { get; set; }
        [DisplayName("出卷人ID")]
        public int MemberID { get; set; }
        [DisplayName("更新时间")]
        public DateTime UpdateTime { get; set; }
        [DisplayName("科目ID")]
        public int SubjectID { get; set; }
        [DisplayName("考试时间")]
        public int ExamTime { get; set; }
        [DisplayName("难题比例")]
        public int Hardage { get; set; }
        [DisplayName("中等题比例")]
        public int Middleage { get; set; }
        [DisplayName("简单题比例")]
        public int Easyage { get; set; }
        [DisplayName("选择题题量")]
        public int SelectionNum { get; set; }
        [DisplayName("填空题题量")]
        public int InputionNum { get; set; }
        [DisplayName("解答题题量")]
        public int AnswerNum { get; set; }
        [DisplayName("选择题分值")]
        public int SelectionScore { get; set; }
        [DisplayName("填空题分值")]
        public int InputionScore { get; set; }
        [DisplayName("解答题分值")]
        public int AnswerScore { get; set; }
        [DisplayName("包含知识点")]
        public ICollection<int> KnowledgeSiteIDs { get; set; }
    }
}
