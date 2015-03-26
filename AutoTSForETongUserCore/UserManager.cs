using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTSForETongUserCore.Model;
using AutoTSForETongImplOfData;
using AutoTSForETongModel;
using Ninject;
using AutoTSForETongDataFramework;



namespace AutoTSForETongUserCore
{
    public class UserManager : IUserManager
    {
        #region 属性
        [Inject]
        public IMemberDB _memberDB { get; set; }
        [Inject]
        public IMSMapDB _memberSubjectDB { get; set; }

        [Inject]
        public ISubjectDB _subjectDB { get; set; }

        #endregion


        #region 接口实现


        public bool IsContained(string LoginName)
        {
            return _memberDB.Entities.Any(o => o.MemberName == LoginName);
        }

        public int Update(Member member)
        {
            return _memberDB.Update(member);
        }
        public Member GetByID(int ? memberID)
        {
            var member = _memberDB.GetByKey(memberID);
            if (member == null)
                throw new Exception("找不到指定用户！");
            return member;
        }

        /// <summary>
        /// 获取未被删除的所有成员
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Member> GetExistMembers()
        {
            return _memberDB.Entities.Where(o => o.IsDeleted == false);
        }

        public void DeleteMember(int ? userID)
        {
            var member = _memberDB.GetByKey(userID);
            member.IsDeleted = true;
            _memberDB.Update(member);
        }

        public IEnumerable<Subject> GetAllSubjects()
        {
            return _subjectDB.Entities;
        }

        /// <summary>
        /// 添加新成员（指明对应的科目ID）
        /// </summary>
        /// <param name="member"></param>
        /// <param name="subjects"></param>
        public void AddNewMember(Member member, IEnumerable<int> subjects)
        {
            try
            {
                _memberDB.Insert(member);
                foreach(var item in subjects)
                {
                    var newMap = new MemberSubjectMap
                    {
                        MemberID = member.MemberID,
                        SubjectID = item
                    };
                    _memberSubjectDB.Insert(newMap, false);
                }
                _memberSubjectDB.SaveChanges();
            }
            catch(Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public void AddNewMember(Member member)
        {
            _memberDB.Insert(member);
        }

        /// <summary>
        /// 根据用户ID获取对应的科目
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public IEnumerable<Subject> GetOwingSubjects(int UserID)
        {
            var subjectIDs = _memberSubjectDB.Entities.Where(o => o.MemberID == UserID);
            var subjects = _subjectDB.Entities.Where(o => subjectIDs.Any(s => s.SubjectID == o.SubjectID));
            return subjects;
        }


        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPassword"></param>
        /// <returns>返回登录验证信息</returns>
        public LoginResult loginCheck(string loginName, string loginPassword)
        {
            Member curMember = _memberDB.Entities.SingleOrDefault(u =>
               u.MemberName == loginName && u.Password == loginPassword);
            if (curMember == null)
            {
                return new LoginResult
                {
                    LoginSuccess = false,
                    LoginErrorInfo = "用户名或密码错误！"
                };
            }
            return new LoginResult
            {
                LoginSuccess = true,
                LoginErrorInfo = "登陆成功"
            };
        }

        /// <summary>
        /// 根据用户名获取权限
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns>权限类</returns>
        public IdentityResult AcquireIdentity(string LoginName)
        {
            var member = _memberDB.Entities.SingleOrDefault(o => o.MemberName == LoginName);
            if (member == null)
                return new IdentityResult
                {
                    UserID = 0,
                    Identity = string.Empty,
                    ReturnUrl = "~/User/Login"
                };
            if(member.Identity == "学生")
            {
                return new IdentityResult
                {
                    UserID = member.MemberID,
                    Identity = "学生",
                    ReturnUrl = "~/Examination/Index"
                };
            }
            else if (member.Identity == "教师")
            {
                return new IdentityResult
                {
                    UserID = member.MemberID,
                    Identity = "教师",
                    ReturnUrl = "~/Exampapers/Index"
                };
            }
            else if (member.Identity == "教研员")
            {
                return new IdentityResult
                {
                    UserID = member.MemberID,
                    Identity = "教研员",
                    ReturnUrl = "~/Questions/Index"
                };
            }
            else if (member.Identity == "超级管理员")
            {
                return new IdentityResult
                {
                    UserID = member.MemberID,
                    Identity = "超级管理员",
                    ReturnUrl = "~/Members/Index"
                };
            }
            else
            {
                throw new Exception("未知的角色！");
            }
        }


        #endregion 
    }
}
