using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTSForETongSysCore.Model;
using AutoTSForETongModel;
using AutoTSForETongImplOfData;
using Ninject;


namespace AutoTSForETongSysCore.ImplOfSysCore
{
    public class OperateQuestion : IOperateQuestion
    {

        #region 属性
        [Inject]
        private IQuestionDB _questionDB { get; set; }
        [Inject]
        private IKnowledgeSiteDB _knowledgeSiteDB { get; set; }

        [Inject]
        private IQuestionTypeDB _questionTypeDB { get ;set;}
        #endregion

        #region 接口实现
        /// <summary>
        /// 获取指定科目的试题
        /// </summary>
        /// <returns></returns>
        public ICollection<Question> AcquireBySubject(int subjectID)
        {
            var knowledgesites = _knowledgeSiteDB.Entities.Where(o => o.SubjectID == subjectID);
            if (knowledgesites == null)
                throw new Exception("该科目编号无对应的知识点！");
            var result = _questionDB.Entities.Where(o => knowledgesites.Any(u => u.KnowledgeSiteID == o.KnowledgeSiteID)).ToList();
            return result;
        }
        public IEnumerable<QuestionType> GetQuestionTypes()
        {
            return _questionTypeDB.Entities;
        }

        public QuestionType GetQuestionType(int? typeID)
        {
            return _questionTypeDB.GetByKey(typeID);
        }
        #endregion
    }
}
