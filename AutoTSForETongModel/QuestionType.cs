using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTSForETongModel
{
    public class QuestionType:Entity
    {
        [Key]
        public int TypeID { get; set; }
        [DisplayName("题型名称")]
        public string TypeName { get; set; }
    }
}
