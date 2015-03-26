using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTSForETongModel;

namespace AutoTSForETongSysCore
{
    public interface IKnowledgeSites
    {
        IEnumerable<KnowledgeSite> GetSitesBySubject(int? subjectID);
    }
}
