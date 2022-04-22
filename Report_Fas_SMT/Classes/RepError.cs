using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes
{
    public class RepError
    {
        public string Barcode { get; set; }

        public int PCBID { get; set; }
        public string ErrorCode { get; set; }

        public string FulLotCode { get; set; }

        public int LOTID { get; set; }

    }
}
