using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTSForETongSysCore.Model;
using AutoTSForETongModel;

namespace AutoTSForETongSysCore
{
    public interface IGeneratePaper
    {
        /// <summary>
        /// 根据需求自动生成试卷
        /// </summary>
        /// <param name="demand">试卷需求</param>
        /// <returns>生成的试卷</returns>
        Exampaper AutoGeneratePaper(PaperDemand demand);

        /// <summary>
        /// 保存试卷到数据库中
        /// </summary>
        /// <param name="paper">试卷实例</param>
        /// <returns>是否保存成功</returns>
        bool SaveToDB(Exampaper paper);

        /// <summary>
        /// 从试卷中删除指定ID的试题
        /// </summary>
        /// <param name="paper">原试卷</param>
        /// <param name="questionID">试题ID</param>
        /// <returns>修改后的试卷</returns>
        Exampaper RemoveFrom(Exampaper paper, int questionID);

        /// <summary>
        /// 将指定题目添加到试卷中
        /// </summary>
        /// <param name="paper">原试卷</param>
        /// <param name="questionID">试题ID</param>
        /// <returns>修改后的试卷</returns>
        Exampaper AddInto(Exampaper paper, int questionID);

        /// <summary>
        /// 获取指定ID的试卷
        /// </summary>
        /// <param name="paperID">试卷ID</param>
        /// <returns>试卷</returns>
        Exampaper GetPaperByID(int ? paperID);

        /// <summary>
        /// 删除试卷（假删）
        /// </summary>
        /// <param name="paperID">试卷ID</param>
        void DeletePaper(int? paperID);

        /// <summary>
        /// 获取指定科目ID的试卷
        /// </summary>
        /// <param name="SubjectID"></param>
        /// <returns></returns>
        IEnumerable<Exampaper> GetPaperBySubject(int? SubjectID);

        IEnumerable<Exampaper> GetPaperBySubject(IEnumerable<int> SujectIDs);

        /// <summary>
        /// 将试卷保存到本地
        /// </summary>
        /// <param name="paper"></param>
        void SaveLocalPaper(Exampaper paper);


        /// <summary>
        /// 更新paper内容
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        int UpdatePaper(Exampaper paper);


        Exampaper GetByID(int? paperID);
    }
}
