using Report_Fas_SMT.Classes.Classes_FAS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes.Classes_FAS.Others
{
     public class CounterStage
     {
        public string NameOrder { get; set; }

        public int SumCount { get; set; }
        public List<ListStage> ListStages { get; set; }

     }
}
