using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTSForETongModel;
using AutoTSForETongImplOfData;
using Ninject;

namespace AutoTSForETongSysCore.ImplOfSysCore
{
    public class KnowledgeSites:IKnowledgeSites
    {
        [Inject]
        private IKnowledgeSiteDB _sitesDB { get; set; }

        public IEnumerable<KnowledgeSite> GetSitesBySubject(int? subjectID)
        {
            return _sitesDB.Entities.Where(o => o.SubjectID == subjectID&&o.IsDeleted == false);
        }

    }
}
