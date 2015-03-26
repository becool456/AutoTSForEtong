using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoTSForETongModel;

namespace AutoTSForEtong.ViewModel
{
    public class PaperIndexViewModel
    {
        public IEnumerable<Exampaper> Papers { get; set; }
        //public int ? SubjectID { get; set; }
    }
}