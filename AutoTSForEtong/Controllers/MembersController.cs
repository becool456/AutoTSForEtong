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
using AutoTSForETongUserCore;
using Ninject;

namespace AutoTSForEtong.Controllers
{
    public class MembersController : Controller
    {
        //private SqlDbContext db = new SqlDbContext();

        [Inject]
        private IUserManager _userTools { get; set; }

        private IEnumerable<IdentityKeyValue> identities
        {
            get
            {
                var result = new List<IdentityKeyValue>();
                result.Add(new IdentityKeyValue { IdentityKey = "教师", IdentityValue = "教师" });
                result.Add(new IdentityKeyValue { IdentityKey = "教研员", IdentityValue = "教研员" });
                result.Add(new IdentityKeyValue { IdentityKey = "超级管理员", IdentityValue = "超级管理员" });
                return result;
            }
        }

        // GET: Members
        public ActionResult Index()
        {
            return View(_userTools.GetExistMembers());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = _userTools.GetByID(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            Member member = new Member
            {
                Identity = "",
                MemberName = "",
                Password = ""
            };
            ViewBag.Subjects = _userTools.GetAllSubjects();
            ViewBag.Identity = new SelectList(identities, "IdentityValue", "IdentityKey");
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberID,MemberName,Password,Identity")] Member member,int [] subjects)
        {
            if (ModelState.IsValid&&!_userTools.IsContained(member.MemberName))
            {
                if(subjects == null)
                {
                    _userTools.AddNewMember(member);
                }
                else
                {
                    IEnumerable<int> subjectIDs = subjects.ToList();
                    _userTools.AddNewMember(member, subjectIDs);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Error = "该用户名已存在！";
            ViewBag.Subjects = _userTools.GetAllSubjects();
            ViewBag.Identity = new SelectList(identities, "IdentityValue", "IdentityKey");
            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = _userTools.GetByID(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.Identity = new SelectList(identities, "IdentityValue", "IdentityKey");
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberID,MemberName,Password,Identity")] Member member)
        {
            if (ModelState.IsValid)
            {
                _userTools.Update(member);
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = _userTools.GetByID(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        public ActionResult DeleteConfirmed(int id)
        {
            _userTools.DeleteMember(id);
            return RedirectToAction("Index");
        }
    }
    public class IdentityKeyValue
    {
        public string IdentityValue { get; set; }

        public string IdentityKey { get; set; }
    }
}
