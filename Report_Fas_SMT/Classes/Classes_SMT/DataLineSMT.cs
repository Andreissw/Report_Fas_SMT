using Report_Fas_SMT.Classes.Classes_SMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes
{
    public class DataLineSMT
    {
        public string NameLine { get; set; }

        public string NameModel { get; set; }

        public float ShiftVypusk { get; set; }

        public float ShiftOtkaz { get; set; }

        public string ShiftFPY { get; set; }

        public double Cell { get; set; }

        public List<DataReportOtkazFPY> ListDataReport { get; set; }

        public List<TOPError> ListTop { get; set; }

        public List<TopDefects> ListTopDefects { get; set; }

        public List<TopGroups> ListTopGroups { get; set; }

        public List<TOPComp> TopListComp { get; set; }
       
    }
}
