using Report_Fas_SMT.Classes.Classes_FAS.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes.Classes_FAS
{
     public class Other
     {
        public List<string> ListLots { get; set; }
        public List<ReapirFAS> ListRepair { get; set; } 
        public List<KGP> ListKGP { get; set; }
        
     }

    public class RepairTable
    { 
        public string PositionName { get; set; }

        public string Barcode { get; set; }

        public string RepairName { get; set; }

        public string DefectCode { get; set; }

        public string FullLotCode { get; set; }
   

    }
}
