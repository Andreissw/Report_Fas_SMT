using Report_Fas_SMT.Classes.Classes_FAS.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes.Classes_FAS
{
    public  class DataFasReport
    {       
        public List<CounterStage> CounterStages { get; set; }
        public Other DataOther { get; set; }
        public DataContract DataContract { get; set; }       
        public Data_VLV Data_VLV { get; set; }

        public DataFasReport()
        {
            CounterStages = new List<CounterStage>();
            DataOther = new Other();
            DataContract = new DataContract();
            Data_VLV = new Data_VLV();
        }
    }
}
