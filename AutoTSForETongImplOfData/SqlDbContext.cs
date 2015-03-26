using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AutoTSForETongModel;
using AutoTSForETongDataFramework;

namespace AutoTSForETongImplOfData
{
    public class SqlDbContext :DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public DbSet<KnowledgeSite> KnowledgeSites { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Exampaper> Exampapers { get; set; }

        public DbSet<PaperExtraInfo> PaperExtraInformation { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<PaperQuestionMap> PQMaps { get; set; }

        public DbSet<MemberSubjectMap> MSMaps { get; set; }

        public DbSet<QuestionType> QuestionTypes { get; set; }
    }
}
