using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTSForETongModel
{
    public class PaperQuestionMap:Entity
    {
        [Key]
        public int MapID { get; set; }
        [DisplayName("试卷ID")]
        public int PaperID { get; set; }
        [DisplayName("试题ID")]
        public int QuestionID { get; set; }
    }
}
