using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTSForETongModel;
using AutoTSForETongSysCore.Model;
using AutoTSForETongImplOfData;
using System.IO;
using Ninject;

namespace AutoTSForETongSysCore.ImplOfSysCore
{
    public class GeneratePaper:IGeneratePaper
    {

        #region 成员变量
        private const int _difTypeNum = 3;
        private const int _quesTypeNum = 3;
        [Inject]
        private IQuestionDB _questionDB { get; set; }
        [Inject]
        private IExampapaperDB _paperDB { get; set; }
        [Inject]
        private IMemberDB _memberDB { get; set; }
        [Inject]
        private ISubjectDB _subjectDB { get; set; }
        [Inject]
        private IPaperExtraInfoDB _paperExtraInfoDB { get; set; }
        [Inject]
        private IPQMapDB _pQMapDB { get; set; }

        [Inject]
        private IKnowledgeSiteDB _knowledgeSitesDB { get; set; }

        private SqlDbContext db = new SqlDbContext();
        #endregion 

        #region 公有方法(接口实现)

        /// <summary>
        /// 将试卷保存到本地
        /// </summary>
        /// <param name="paper"></param>
        public void SaveLocalPaper(Exampaper paper)
        {
            if (paper == null)
                throw new Exception("试卷对象为空！");
            string path = @"D:/data/" + paper.PaperTitle + ".txt";
            if(File.Exists(path))
            {
                File.Delete(path);
            }
            FileStream fs = new FileStream(path, FileMode.Create);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("                          "  + paper.PaperTitle + "(考试时间:" + paper.ExamTime + "分钟)");
                sw.WriteLine("                              出题人：" + paper.Member.MemberName);
                int i = 1 ;
                foreach(var item in paper.PaperExtraInfo)
                {
                    int j = 1;
                    sw.WriteLine("第" + i.ToString() + "部分:" + item.QuestionTypeTitle + "(每条："
                        + item.QuestionTypeGrade + "分）");
                    foreach(var question in paper.Questions.Where( o => o.QuestionType == item.QuestionType))
                    {
                        sw.WriteLine("  " + j.ToString() + "." + question.Content.Replace("<br/>&nbsp;&nbsp;&nbsp;&nbsp;","").Replace("\n",""));
                        sw.WriteLine();
                        if(item.QuestionType == "选择题")
                        {
                            sw.WriteLine("  答案：(   )");
                            sw.WriteLine();
                        }
                        else if (item.QuestionType == "填空题")
                        {
                            sw.WriteLine("  答案：__________");
                            sw.WriteLine();
                        }
                        else
                        {
                            for (int t = 0; t < 5; t++)
                                sw.WriteLine();
                        }
                        j++;
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// 获取指定科目ID的试卷
        /// </summary>
        /// <param name="SubjectID"></param>
        /// <returns></returns>
        public IEnumerable<Exampaper> GetPaperBySubject(int? SubjectID)
        {
            //var knowledgesites = _knowledgeSitesDB.Entities.Where( o => o.SubjectID == SubjectID);
            var papers = _paperDB.Entities.Where(o => o.SubjectID == SubjectID);
            foreach(var item in papers)
            {
                var member = _memberDB.GetByKey(item.MemberID);
                item.Member = member;
            }
            return papers;
        }

        public IEnumerable<Exampaper> GetPaperBySubject(IEnumerable<int> SujectIDs)
        {
            IEnumerable<Exampaper> papers = new List<Exampaper>();
            foreach(var item in SujectIDs)
            {
                var paper = GetPaperBySubject(item);
                papers = papers.Union(paper);
            }
            return papers;
        }

        /// <summary>
        /// 删除试卷（假删）
        /// </summary>
        /// <param name="paperID">试卷ID</param>
        public void DeletePaper(int? paperID)
        {
            var paper = _paperDB.GetByKey(paperID);
            if (paper == null)
                throw new Exception("找不到指定试卷ID！");
            paper.IsDeleted = true;
            _paperDB.Update(paper);
        }

        /// <summary>
        /// 根据需求自动生成试卷
        /// </summary>
        /// <param name="demand">试卷需求</param>
        /// <returns>生成的试卷</returns>
        public Exampaper AutoGeneratePaper(PaperDemand demand)
        {
            int[,] arrayQuesDist;//题型难度具体分布数组
            string[] type;          //题型分布字段数组
            Exampaper paper = new Exampaper()
            {
                PaperTitle = demand.PaperTitle,
                SubjectID = demand.SubjectID,
                Subject = _subjectDB.GetByKey(demand.SubjectID),
                ExamTime = demand.ExamTime,
                MemberID = demand.MemberID,
                Member = _memberDB.GetByKey(demand.MemberID),
                UpdateTime = demand.UpdateTime
            };
            ICollection<PaperExtraInfo> paperInfos ;
            InitialDistrubteArray(out arrayQuesDist,out type,out paperInfos, demand);
            paper.PaperExtraInfo = paperInfos;
            paper.Questions = RandomMatch(arrayQuesDist, type, demand);
            return paper;
        }

        /// <summary>
        /// 保存试卷到数据库中
        /// </summary>
        /// <param name="paper">试卷实例</param>
        /// <returns>是否保存成功</returns>
        public bool SaveToDB(Exampaper paper)
        {
            try
            {
                var paperInfos = paper.PaperExtraInfo;
                paper.PaperExtraInfo = null;
                var questionMaps = paper.Questions;
                paper.Questions = null;
                _paperDB.Insert(paper);
                int paperID = paper.PaperID;
                //保存试卷
                foreach (var item in paperInfos)
                {
                    item.PaperID = paperID;
                }
                //保存试卷附加信息
                _paperExtraInfoDB.Insert(paperInfos);
                //保存试卷试题映射
                foreach(var item in questionMaps)
                {
                    PaperQuestionMap newItem = new PaperQuestionMap
                    {
                        PaperID = paperID,
                        QuestionID = item.QuestionID
                    };
                    _pQMapDB.Insert(newItem, false);
                }
                _pQMapDB.SaveChanges();
                var newPaperInfos = _paperExtraInfoDB.Entities.Where(o => o.PaperID == paperID);
                foreach (var item in newPaperInfos)
                {
                    paper.PaperExtraInfo.Add(item);
                }
                _paperExtraInfoDB.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        /// <summary>
        /// 从试卷中删除指定ID的试题
        /// </summary>
        /// <param name="paper">原来试卷</param>
        /// <param name="questionID">试题ID</param>
        /// <returns>修改后的试卷</returns>
        public Exampaper RemoveFrom(Exampaper paper, int questionID)
        {
            var question = _questionDB.GetByKey(questionID);
            if (question == null)
                throw new Exception("未能找到指定ID的试题！");
            paper.Questions.Remove(question);
            return paper;
        }

        /// <summary>
        /// 将指定题目添加到试卷中
        /// </summary>
        /// <param name="paper">原试卷</param>
        /// <param name="questionID">试题ID</param>
        /// <returns>修改后的试卷</returns>
        public Exampaper AddInto(Exampaper paper, int questionID)
        {
            var question = _questionDB.GetByKey(questionID);
            if(question == null || question.IsDeleted == true)
                throw new Exception("未能找到指定ID的试题！");
            paper.Questions.Add(question);
            return paper;
        }

        /// <summary>
        /// 获取指定ID的试卷
        /// </summary>
        /// <param name="paperID">试卷ID</param>
        /// <returns>试卷</returns>
        public Exampaper GetPaperByID(int ? paperID)
        {
            Exampaper paper = _paperDB.GetByKey(paperID);
            if (paper == null)
                throw new Exception("找不到指定试卷ID");
            ICollection<Question> questions = (from question in _questionDB.Entities
                                            join map in _pQMapDB.Entities
                                            on question.QuestionID equals map.QuestionID
                                            where map.PaperID == paper.PaperID 
                                            select question).ToList();
            ICollection<PaperExtraInfo> paperInfo = _paperExtraInfoDB.Entities.Where(o => o.PaperID == paperID).ToList();
            paper.Questions = questions;
            paper.PaperExtraInfo = paperInfo;
            return paper;
        }

        /// <summary>
        /// 更新paper内容
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        public int UpdatePaper(Exampaper paper)
        {
            return _paperDB.Update(paper);
        }

        public Exampaper GetByID(int? paperID)
        {
            return _paperDB.GetByKey(paperID);
        }
        #endregion


        #region 私有方法（辅助接口实现）
        /// <summary>
        /// 相关数组数据初始化
        /// </summary>
        /// <param name="arrayQuesDist">题型难度具体分布数组</param>
        /// <param name="type">题型分布字段数组</param>
        /// <param name="demand">试卷需求</param>
        private void InitialDistrubteArray(out int[,] arrayQuesDist, out string[] type,out ICollection<PaperExtraInfo> paperInfos, PaperDemand demand)
        {        
            #region This code make me sick~ demand class is a bad guy!
            arrayQuesDist = new int[_difTypeNum, _quesTypeNum];
            arrayQuesDist[0, 0] = demand.InputionNum * demand.Easyage / 100;
            arrayQuesDist[0, 1] = demand.InputionNum * demand.Middleage / 100;
            arrayQuesDist[0, 2] = demand.InputionNum - arrayQuesDist[0, 1] - arrayQuesDist[0, 0];
            arrayQuesDist[1, 0] = demand.SelectionNum * demand.Easyage / 100;
            arrayQuesDist[1, 1] = demand.SelectionNum * demand.Middleage / 100;
            arrayQuesDist[1, 2] = demand.SelectionNum - arrayQuesDist[1, 1] - arrayQuesDist[1, 0];
            arrayQuesDist[2, 0] = demand.AnswerNum * demand.Easyage / 100;
            arrayQuesDist[2, 1] = demand.AnswerNum * demand.Middleage / 100;
            arrayQuesDist[2, 2] = demand.AnswerNum - arrayQuesDist[2, 1] - arrayQuesDist[2, 0];
            
            type = new string[_quesTypeNum];
            type[0] = "填空题"; type[1] = "选择题"; type[2] = "解答题";

            paperInfos = new List<PaperExtraInfo>();
            PaperExtraInfo info ;
            if(demand.InputionNum != 0 )
            {
                info = new PaperExtraInfo{
                    QuestionType = "填空题",
                    QuestionTypeGrade = demand.InputionScore,
                    QuestionTypeTitle = "填空题",
                    UpdateTime = DateTime.Now
                };
                paperInfos.Add(info);
            }
            if(demand.SelectionNum != 0 )
            {
                info = new PaperExtraInfo
                {
                    QuestionType = "选择题",
                    QuestionTypeGrade = demand.SelectionScore,
                    QuestionTypeTitle = "选择题（每条题目有四个选项，仅有一个选项为正确答案）",
                    UpdateTime = DateTime.Now
                };
                paperInfos.Add(info);
            }
            if (demand.AnswerNum != 0)
            {
                info = new PaperExtraInfo
                {
                    QuestionType = "解答题",
                    QuestionTypeGrade = demand.AnswerScore,
                    QuestionTypeTitle = "解答题",
                    UpdateTime = DateTime.Now
                };
                paperInfos.Add(info);
            }
            #endregion
        }

        /// <summary>
        /// 随机化算法匹配试题
        /// </summary>
        /// <param name="arrayQuesDist">题型难度具体分布数组</param>
        /// <param name="type">题型分布字段数组</param>
        /// <param name="demand">试卷需求</param>
        /// <returns>生成试题集合</returns>
        private ICollection<Question> RandomMatch(int[,] arrayQuesDist ,string[] type ,PaperDemand demand )
        {
            Random rd = new Random(); //随机数生成器
            ICollection<Question> newPaper = new List<Question>();//试卷集合
            var totalQuestions = _questionDB.Entities.Where(o => demand.KnowledgeSiteIDs.Any(u => u == o.KnowledgeSiteID)).ToList();
            for (int i = 0; i < _quesTypeNum; i++)
            {
                ICollection<int> unAddedSiteIDs = demand.KnowledgeSiteIDs; //该题型中未添加的知识点
                for (int j = 0; j < _difTypeNum; j++)
                {
                    int unAddedNum = arrayQuesDist[i, j];
                    string curType = type[i];
                    var firstAlterQs = totalQuestions.Where
                        (o => o.QuestionType == curType && o.Difficulty == j + 1).ToList();
                    while (unAddedNum > 0)
                    {
                        Question item;
                        IList<Question> secondAlterQs ;
                        if (unAddedSiteIDs.Count() != 0&&unAddedSiteIDs != null)
                            secondAlterQs = firstAlterQs.Where(o => unAddedSiteIDs.Any(u => u == o.KnowledgeSiteID)).ToList();
                        else
                            secondAlterQs = firstAlterQs;
                        int num = rd.Next(secondAlterQs.Count() - 1);
                        item = secondAlterQs.OrderBy(o => o.QuestionID).Skip(num).First();
                        newPaper.Add(item);
                        unAddedNum--;
                        if (!newPaper.Any(o => o.QuestionID == item.QuestionID))
                        {   
                            unAddedSiteIDs.Remove(item.KnowledgeSiteID);                            
                        }
                    }
                }
            }
            return newPaper;
        }

        

        #endregion
    }

    
}
