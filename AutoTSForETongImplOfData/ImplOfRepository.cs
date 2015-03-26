using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoTSForETongDataFramework;
using AutoTSForETongModel;

namespace AutoTSForETongImplOfData
{
 
        /// <summary>
        /// 试题仓储
        /// </summary>
        public interface IQuestionDB : IRepository<Question> { }

        public class QuestionDB : RepositoryBase<Question>, IQuestionDB { }
        /// <summary>
        /// 科目仓储
        /// </summary>
        public interface ISubjectDB : IRepository<Subject> { }

        public class SubjectDB : RepositoryBase<Subject>, ISubjectDB { }
        /// <summary>
        /// 知识点仓储
        /// </summary>
        public interface IKnowledgeSiteDB : IRepository<KnowledgeSite> { }

        public class KnowledgeSiteDB : RepositoryBase<KnowledgeSite>, IKnowledgeSiteDB { }

        public interface IExampapaperDB : IRepository<Exampaper> { }

        public class ExampaperDB : RepositoryBase<Exampaper>, IExampapaperDB { }

        public interface IMemberDB : IRepository<Member> { }

        public class MemberDB : RepositoryBase<Member>, IMemberDB { }

        public interface IPaperExtraInfoDB : IRepository<PaperExtraInfo> { }

        public class PaperExtraInfoDB : RepositoryBase<PaperExtraInfo>, IPaperExtraInfoDB { }

        public interface IPQMapDB : IRepository<PaperQuestionMap> { }

        public class PQMapDB :RepositoryBase<PaperQuestionMap> , IPQMapDB { }

        public interface IMSMapDB : IRepository<MemberSubjectMap> { }

        public class MSMapDB : RepositoryBase<MemberSubjectMap>, IMSMapDB { }

        public interface IQuestionTypeDB : IRepository<QuestionType> { }

        public class QuestionTypeDB : RepositoryBase<QuestionType>, IQuestionTypeDB { }
}
