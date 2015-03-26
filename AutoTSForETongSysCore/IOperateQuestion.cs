using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTSForETongSysCore.Model;
using AutoTSForETongModel;

namespace AutoTSForETongSysCore
{
    public interface IOperateQuestion
    {
        /// <summary>
        /// 获取指定科目的试题
        /// </summary>
        /// <returns></returns>
        ICollection<Question> AcquireBySubject(int subjectID);


        IEnumerable<QuestionType> GetQuestionTypes();

        QuestionType GetQuestionType(int? typeID);
    }
}
