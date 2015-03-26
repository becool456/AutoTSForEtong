using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoTSForETongImplOfData;
using AutoTSForETongModel;
using AutoTSForETongSysCore.Model;
using AutoTSForETongSysCore;
using AutoTSForETongUserCore;
using AutoTSForEtong.ViewModel;
using Ninject;

namespace AutoTSForEtong.Controllers
{
    public class ExampapersController : Controller
    {
        #region 属性
        //private SqlDbContext db = new SqlDbContext();

        [Inject]
        private IGeneratePaper _examTools { get; set; }

        [Inject]
        private IOperateQuestion _questionTools {get ;set;}
        [Inject]
        private IUserManager _userTools { get; set; }
        [Inject]
        private IKnowledgeSites _siteTools { get; set; }

        #endregion


        // GET: Exampapers
        public ActionResult Index(int ? subjectID,string keyWords)
        {
            IEnumerable<Subject> subjects = _userTools.GetOwingSubjects((int)Session["UserID"]);  
            ViewBag.SubjectID = new SelectList(subjects, "SubjectID", "SubjectName");
            if(subjectID == null)
            {
                if(Session["SubjectID"] == null)
                {
                    var firstSubject = subjects.First();
                    subjectID = firstSubject.SubjectID;
                }
                else
                {
                    subjectID = (int)Session["SubjectID"];
                }
            }
            Session["SubjectID"] = subjectID;
            var exampapers = _examTools.GetPaperBySubject(subjectID);
            if(!string.IsNullOrEmpty(keyWords))
            {
                exampapers = exampapers.Where(o => o.PaperTitle.Contains(keyWords));
            }
            PaperIndexViewModel viewModel = new PaperIndexViewModel
            {
                Papers = exampapers.ToList()
            };
            ViewBag.CurSubject = subjects.First(o => o.SubjectID == subjectID).SubjectName;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult GetPaperDemand()
        {
            IEnumerable<Subject> subjects = _userTools.GetOwingSubjects((int)Session["UserID"]);
            ViewBag.Subjects = new SelectList(subjects, "SubjectID", "SubjectName");
            var firstSubject = subjects.First();
            ViewBag.KnowledgeSites = _siteTools.GetSitesBySubject(firstSubject.SubjectID);
            Session["SubjectID"] = firstSubject.SubjectID;
            #region 初始化需求
            PaperDemand demand = new PaperDemand();
            //{
            //    PaperTitle = "马克思主义原理单元测试",
            //    Easyage = 20,
            //    Middleage = 60,
            //    Hardage = 20,
            //    InputionNum = 20,
            //    SelectionNum = 30,
            //    AnswerNum = 4,
            //    InputionScore = 2,
            //    SelectionScore = 2,
            //    AnswerScore = 10,
            //    ExamTime = 120,
            //    SubjectID = 7,
            //    MemberID = 1,
            //    UpdateTime = DateTime.Now
            //};
            #endregion
            return View(demand);
        }

        [HttpPost]
        public ActionResult GetPaperDemand(PaperDemand demand, int[] knowledgeSites, int Subjects)
        {
            if (knowledgeSites == null)
            {
                ModelState.AddModelError("警告", "请选择知识点");
                return RedirectToAction("GetPaperDemand");
            }
            demand.SubjectID = Subjects;
            demand.KnowledgeSiteIDs = knowledgeSites.ToList();
            var paper = _examTools.AutoGeneratePaper(demand);
            Session["NewPaper"] = paper;
            return RedirectToAction("PreviewPaper", paper);
        }

        public ActionResult PreviewPaper()
        {
            var paper = (Exampaper)Session["NewPaper"];
            return View(paper);
        }

        public ActionResult SavePaper()
        {
            var paper = (Exampaper)Session["NewPaper"];
            paper.UpdateTime = DateTime.Now;
            paper.MemberID = (int)Session["UserID"];
            paper.SubjectID = (int)Session["SubjectID"];
            try
            {
                bool isSucceed = _examTools.SaveToDB(paper);
                if(isSucceed)
                {
                    return View("SaveSuccess");
                }
                else
                {
                    return View("SaveFailed");
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.ToString());     
            }
        }

        public ActionResult RemoveQuestion(int questionID,bool isNewPaper = false)
        {
            var paper = (Exampaper)Session["NewPaper"];
            paper = _examTools.RemoveFrom(paper, questionID);
            Session["NewPaper"] = paper;
            if (isNewPaper)
                return RedirectToAction("PreviewPaper");
            else
                return RedirectToAction("PaperDetail");
        }

        [HttpGet]
        public ActionResult PreViewQuestion(int TypeID = 0 , int KnowledgeSiteID = 0)
        {
            var paper = (Exampaper)Session["NewPaper"];
            int subjectID = (int)Session["SubjectID"];
            var selectedTypes = _questionTools.GetQuestionTypes();
            var selectedKnowledgeSites = _siteTools.GetSitesBySubject(subjectID).ToList();
            if(TypeID == 0 && KnowledgeSiteID == 0)
            {
                TypeID = selectedTypes.First().TypeID;
                KnowledgeSiteID = selectedKnowledgeSites.First().KnowledgeSiteID;
            }
            var QuestionType =_questionTools.GetQuestionType(TypeID);
            var totalQuestions = _questionTools.AcquireBySubject(subjectID).Except(paper.Questions)
                .Where( o => o.KnowledgeSiteID == KnowledgeSiteID&&o.QuestionType == QuestionType.TypeName);
            if (QuestionType == null)
                throw new Exception("找不到指定题型");
            ViewBag.TypeID = new SelectList(selectedTypes, "TypeID", "TypeName");
            ViewBag.KnowledgeSiteID = new SelectList(selectedKnowledgeSites, "KnowledgeSiteID", "KnowledgeSiteName");
            IEnumerable<Question> result = totalQuestions
                .Where(o => o.QuestionType == QuestionType.TypeName && o.KnowledgeSiteID == KnowledgeSiteID&&o.IsDeleted == false);
            return View(result);
        }

        [HttpPost]
        public ActionResult PostSearchParam(int TypeID, int KnowledgeSiteID)
        {          
            return RedirectToAction("PreViewQuestion", new { TypeID = TypeID, KnowledgeSiteID = KnowledgeSiteID });
        }

        public ActionResult AddQuestion(int questionID,bool isNewPaper = false)
        {
            var paper = (Exampaper)Session["NewPaper"];
            paper = _examTools.AddInto(paper, questionID);
            Session["NewPaper"] = paper;
            if (isNewPaper)
                return RedirectToAction("PreviewPaper");
            else
                return RedirectToAction("PaperDetail");
        }
        
        // GET: Exampapers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var paper = _examTools.GetPaperByID(id);
            Session["NewPaper"] = paper;
            if (paper == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("PaperDetail");
        }

        //查看并编辑试卷
        public ActionResult PaperDetail()
        {
            var paper = (Exampaper)Session["NewPaper"];
            return View(paper);
        }

        public ActionResult SaveAsPaper()
        {
            var paper = (Exampaper)Session["NewPaper"];

            #region 新建一个拷贝试卷对象
            ICollection<PaperExtraInfo> paperInfos = new List<PaperExtraInfo>();
            foreach(var item in paper.PaperExtraInfo)
            {
                var newInfo = new PaperExtraInfo{
                    QuestionType = item.QuestionType,
                    QuestionTypeGrade = item.QuestionTypeGrade,
                    QuestionTypeTitle = item.QuestionTypeTitle,
                    UpdateTime = DateTime.Now
                };
                paperInfos.Add(newInfo);
            }

            Exampaper newPaper = new Exampaper
            {
                ExamTime = paper.ExamTime,
                MemberID = paper.MemberID,
                SubjectID = paper.SubjectID,
                PaperTitle = paper.PaperTitle,
                UpdateTime = DateTime.Now,
                Questions = paper.Questions,
                PaperExtraInfo = paperInfos
            };
            #endregion

            Session["NewPaper"] = newPaper;
            return RedirectToAction("SavePaper");
        }

        public ActionResult SaveToLocal()
        {
            var paper = (Exampaper)Session["NewPaper"];
            paper.Member = _userTools.GetByID(paper.MemberID);
            _examTools.SaveLocalPaper(paper);
            return View("SaveSuccess");
        }

        // GET: Exampapers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exampaper exampaper = _examTools.GetByID(id);
            if (exampaper == null)
            {
                return HttpNotFound();
            }
            return View(exampaper);
        }

        public ActionResult DeleteConfirmed(int id)
        {
            Exampaper exampaper = _examTools.GetByID(id);
            exampaper.IsDeleted = true;
            _examTools.UpdatePaper(exampaper);
            return RedirectToAction("Index");
        }

        public ActionResult RollBack(int ?id)
        {
            Exampaper exampaper = _examTools.GetByID(id);
            exampaper.IsDeleted = false;
            _examTools.UpdatePaper(exampaper);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PostIndexParam(int ? SubjectID,string KeyWords)
        {
            return RedirectToAction("Index", new { subjectID = SubjectID,keyWords = KeyWords });
        }

        public JsonResult GetKnowledgeSites(int  subjectID)
        {
            var knowledgesites = _siteTools.GetSitesBySubject(subjectID);
            IList<KnowledgeSiteKeyValue> result = new List<KnowledgeSiteKeyValue>();
            foreach(var item in knowledgesites)
            {
                result.Add(new KnowledgeSiteKeyValue
                {
                    key = item.KnowledgeSiteName,
                    value = item.KnowledgeSiteID
                });
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
    public class KnowledgeSiteKeyValue
    {
        public string key { get; set; }

        public int value { get; set; }
    }
}
