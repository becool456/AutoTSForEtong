using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTSForETongModel;
using AutoTSForETongUserCore.Model;

namespace AutoTSForETongUserCore
{
    public interface IUserManager
    {
        void AddNewMember(Member member);
        bool IsContained(string LoginName);
        int Update(Member member);

        Member GetByID(int ? memberID);

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPassword"></param>
        /// <returns>返回登录验证信息</returns>

        LoginResult loginCheck(string loginName, string loginPassword);

        /// <summary>
        /// 根据用户名获取权限
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns>权限类</returns>
        IdentityResult AcquireIdentity(string LoginName);

        /// <summary>
        /// 根据用户ID获取对应的科目
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        IEnumerable<Subject> GetOwingSubjects(int UserID);

        /// <summary>
        /// 添加新成员（指明对应的科目ID）
        /// </summary>
        /// <param name="member"></param>
        /// <param name="subjects"></param>
        void AddNewMember(Member member, IEnumerable<int> subjects);

        IEnumerable<Subject> GetAllSubjects();


        void DeleteMember(int ? userID);

        /// <summary>
        /// 获取未被删除的所有成员
        /// </summary>
        /// <returns></returns>
        IEnumerable<Member> GetExistMembers();

    }
}
