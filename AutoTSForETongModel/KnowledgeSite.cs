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
    /// 知识点
    /// </summary>
    public class KnowledgeSite : Entity
    {
        [Key]
        public int KnowledgeSiteID { get; set; }
        [DisplayName("知识点名称")]
        public string KnowledgeSiteName { get; set; }
        [DisplayName("是否被删除")]
        public bool IsDeleted { get; set; }
        [DisplayName("科目ID")]
        public int SubjectID { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
