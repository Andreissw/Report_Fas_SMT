using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes.FASNew
{
    public class Repair
    {
        public string Barcode { get; set; }
        public int PCBID { get; set; }
        public string RepairCode { get; set; }
        public string DefectCode { get; set; }
        public string ErrorCode { get; set; }

        public int? LOTID { get; set; }

        public string PositionName { get; set; }
        public bool Status { get; set; }
        public string FullLotCode { get; set; }

        //Barcode = c.Ba
        //ID = 1,
        //RepairCode = f
        //  DefectCode = f
        //  ErrorDis = "",
        //  Status = c.isU
        //  Model = "",
    }
}
