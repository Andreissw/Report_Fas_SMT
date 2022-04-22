using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes
{
    public class KGPData
    {
        public string FULLLotCode { get; set; }

        public int? LOTID { get; set; }

        public int PCBID { get; set; }

        public long SerialNumber { get; set; }
        public string Barcode { get; set; }

        public short? Pass { get; set; }

        public int? DefectCode { get; set; }

        public int LineID { get; set; }

        public string LineName { get; set; }
    }
}
