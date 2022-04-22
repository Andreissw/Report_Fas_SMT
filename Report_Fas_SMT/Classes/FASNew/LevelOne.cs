using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes.FASNew
{
    public class LevelOne
    {
        public LevelOne()
        {
            FPYDatas = new List<FPYData>();
            TopDiss = new List<TopDis>();
            TopRemontOnline = new List<TopRemont>();
            TopRemots = new List<TopRemont>();
            Logs = new List<Log>();
            OTKs = new List<OTK>();
        }
                
        public string NameOrder { get; set; }
        public string NameLine { get; set; }

        public List<FPYData> FPYDatas { get; set; }

        public List<TopDis> TopDiss { get; set; }


        public List<TopRemont> TopRemontOnline { get; set; }

        public List<TopRemont> TopRemots { get; set; }

        public List<Log> Logs { get; set; }
        public List<OTK> OTKs { get; set; }

    }

    public class FPYData
    {
        public double? Objective { get; set; }
        public float Count { get; set; }
        public float CountDis { get; set; }
        public string FPY { get; set; }
    }

    public class TopDis
    { 
        public string Name { get; set; }

        public int Count { get; set; }
    }

    public class TopRemont
    { 
        public string PositionName { get; set; }
        public string RepairCode { get; set; }
        public string NamdeDis { get; set; }

        public bool Status { get; set; }

        public int Count { get; set; }

        public string FullLotCode { get; set; }

    }

    public class Log
    { 
        public string Stage { get; set; }
        public string Result { get; set; }
        public int Count { get; set; }
    }

    public class OTK
    {      

        public short? Pass { get; set; }

        public string DefectCode { get; set; }

        public int Count { get; set; }
    }

    public class ModelLines
    { 
        public string FullLotCode { get; set; }

        public string Line { get; set; }

        public string Type { get; set; }

        public byte? LineID { get; set; }

        public int? LOTID { get; set; }
    }
   
}
