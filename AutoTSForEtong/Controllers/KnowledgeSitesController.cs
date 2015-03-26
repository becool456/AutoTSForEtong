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
    public class KnowledgeSitesController : Controller
    {
        private SqlDbContext db = new SqlDbContext();

        // GET: KnowledgeSites
        public ActionResult Index()
        {
            var knowledgeSites = db.KnowledgeSites.Include(k => k.Subject);
            return View(knowledgeSites.ToList());
        }

        // GET: KnowledgeSites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KnowledgeSite knowledgeSite = db.KnowledgeSites.Find(id);
            if (knowledgeSite == null)
            {
                return HttpNotFound();
            }
            return View(knowledgeSite);
        }

        // GET: KnowledgeSites/Create
        public ActionResult Create()
        {
            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectName");
            return View();
        }

        // POST: KnowledgeSites/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KnowledgeSiteID,KnowledgeSiteName,SubjectID")] KnowledgeSite knowledgeSite)
        {
            if (ModelState.IsValid)
            {
                db.KnowledgeSites.Add(knowledgeSite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectName", knowledgeSite.SubjectID);
            return View(knowledgeSite);
        }

        // GET: KnowledgeSites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KnowledgeSite knowledgeSite = db.KnowledgeSites.Find(id);
            if (knowledgeSite == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectName", knowledgeSite.SubjectID);
            return View(knowledgeSite);
        }

        // POST: KnowledgeSites/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KnowledgeSiteID,KnowledgeSiteName,SubjectID")] KnowledgeSite knowledgeSite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(knowledgeSite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectName", knowledgeSite.SubjectID);
            return View(knowledgeSite);
        }

        // GET: KnowledgeSites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KnowledgeSite knowledgeSite = db.KnowledgeSites.Find(id);
            if (knowledgeSite == null)
            {
                return HttpNotFound();
            }
            return View(knowledgeSite);
        }

        // POST: KnowledgeSites/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            KnowledgeSite knowledgeSite = db.KnowledgeSites.Find(id);
            knowledgeSite.IsDeleted = true;
            db.Entry(knowledgeSite).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RollBack(int id)
        {
            KnowledgeSite knowledgeSite = db.KnowledgeSites.Find(id);
            knowledgeSite.IsDeleted = false;
            db.Entry(knowledgeSite).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
