using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTSForETongModel
{
    /// <summary>
    /// 试题
    /// </summary>
    public class Question: Entity
    {
        [Key]
        public int QuestionID { get; set; }
        [DisplayName("试题类型")]
        public string QuestionType { get; set; }
        [DisplayName("题干内容")]
        public string Content { get; set; }
        [DisplayName("参考答案")]
        public string Answer { get; set; }
        [DisplayName("难度系数")]
        public int Difficulty { get; set; }
        [DisplayName("添加时间")]
        public DateTime AddTime { get; set; }
        [DisplayName("知识点ID")]
        public int KnowledgeSiteID { get; set; }
        public KnowledgeSite KnowledgeSite { get; set; }
        [DisplayName("是否被删除")]
        public bool IsDeleted { get; set; }
    }
}
