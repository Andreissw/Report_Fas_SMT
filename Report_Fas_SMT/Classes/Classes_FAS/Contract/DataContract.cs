using Report_Fas_SMT.Classes.Classes_FAS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes.Classes_FAS
{
    public class DataContract
    {
        public List<LogContract> ListLog { get; set; }

        public List<FPY_Ct> ListFPY { get; set; }

        public List<TopErrorList_ct> ListTop { get; set; }
    }
}
