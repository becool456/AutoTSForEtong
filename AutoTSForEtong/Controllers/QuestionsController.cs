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

namespace AutoTSForEtong.Controllers
{
    public class QuestionsController : Controller
    {
        private SqlDbContext db = new SqlDbContext();

        // GET: Questions
        public ActionResult Index(int ? knowledgestieID  ,string questionType)
        {
            IEnumerable<Question> questions;
            if(knowledgestieID == null)
            {
                knowledgestieID = db.KnowledgeSites.First().KnowledgeSiteID;
            }
            if (string.IsNullOrEmpty(questionType))
            {
                questionType = db.QuestionTypes.First().TypeName;
            }
            questions = db.Questions.Include(q => q.KnowledgeSite)
                .Where( o => o.KnowledgeSiteID == knowledgestieID && o.QuestionType == questionType);
            if (questions.Any())
                ViewBag.FindResult = true;
            return View(questions.ToList());
        }


        // GET: Questions/Create
        public ActionResult Create()
        {
            Question question = new Question();
            ViewBag.KnowledgeSiteID = new SelectList(db.KnowledgeSites, "KnowledgeSiteID", "KnowledgeSiteName");
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Question question,int QuestionTypeID)
        {
            if (ModelState.IsValid)
            {
                var questionType = db.QuestionTypes.Find(QuestionTypeID);
                if (questionType == null)
                    throw new Exception("找不到指定的题目类型");
                question.QuestionType = questionType.TypeName;
                question.AddTime = DateTime.Now;
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KnowledgeSiteID = new SelectList(db.KnowledgeSites, "KnowledgeSiteID", "KnowledgeSiteName", question.KnowledgeSiteID);
            return View(question);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.KnowledgeSiteID = new SelectList(db.KnowledgeSites, "KnowledgeSiteID", "KnowledgeSiteName", question.KnowledgeSiteID);
            return View(question);
        }

        [HttpPost]
        public ActionResult Edit(Question question,int QuestionTypeID)
        {
            if (question != null)
            {
                var questionType = db.QuestionTypes.Find(QuestionTypeID);
                if (questionType == null)
                    throw new Exception("找不到指定的题目类型");
                question.QuestionType = questionType.TypeName;
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KnowledgeSiteID = new SelectList(db.KnowledgeSites, "KnowledgeSiteID", "KnowledgeSiteName", question.KnowledgeSiteID);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5

        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            question.IsDeleted = true;
            db.Entry(question).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RollBack(int ? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Question question = db.Questions.Find(id);
            question.IsDeleted = false;
            db.Entry(question).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetAllQuestionType()
        {
            IEnumerable<QuestionType> types = db.QuestionTypes;
            return Json(types.Select(o => new {TypeID = o.TypeID, TypeName = o.TypeName }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllSubjects()
        {
            IEnumerable<Subject> subjects = db.Subjects;
            return Json(subjects.Select(o => new { SubjectID = o.SubjectID, SubjectName = o.SubjectName}), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetKnowlegeSites(int? subjects)
        {
            IEnumerable<KnowledgeSite> knowledgesites = db.KnowledgeSites.Where(o => o.SubjectID == subjects);
            return Json(knowledgesites.Select(o => new { KnowledgeSiteID = o.KnowledgeSiteID, KnowledgeSiteName = o.KnowledgeSiteName }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostSearchParam(int? KnowledgeSiteID, int? QuestionTypeID)
        {
            string questionType = db.QuestionTypes.Find(QuestionTypeID).TypeName;
            return RedirectToAction("Index", new { knowledgestieID = KnowledgeSiteID, questionType = questionType });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
