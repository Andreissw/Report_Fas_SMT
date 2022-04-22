using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes.FASNew
{
    public class AllRemont
    {
        public FASEntities1 fas = new FASEntities1();
        public SMDCOMPONETSEntities smd = new SMDCOMPONETSEntities();
        public List<Repair> GetList(HowTime HowTime, ModelLines modelLiness)
        {
            return fas.M_Repair_Table.Where(c => c.RapairDate >= HowTime.mode.DateSt & c.RapairDate <= HowTime.mode.Dateend & c.Position != null & c.LOTID == modelLiness.LOTID).
                    Select(c => new Repair()
                    {
                        Barcode = c.Barcode,
                        RepairCode = fas.FAS_RepairCode.Where(b => b.NameCode == c.RepairCode).Select(b => b.NameCode + "-" + b.DescriptionCode).FirstOrDefault(),
                        DefectCode = fas.FAS_DefectCode.Where(b => b.NameCode == c.DefectCode).Select(b => b.NameCode + "-" + b.DescriptionCode).FirstOrDefault(),
                        PositionName = c.Position,
                        Status = (bool)c.isUnitOK,
                        LOTID = c.LOTID,

                    }).ToList();
        }

        public List<Repair> GetList(HowTime HowTime)
        {
            return fas.M_Repair_Table.Where(c => c.RapairDate >= HowTime.mode.DateSt & c.RapairDate <= HowTime.mode.Dateend & c.Position != null).
                    Select(c => new Repair()
                    {
                        Barcode = c.Barcode,
                        RepairCode = fas.FAS_RepairCode.Where(b => b.NameCode == c.RepairCode).Select(b => b.NameCode + "-" + b.DescriptionCode).FirstOrDefault(),
                        DefectCode = fas.FAS_DefectCode.Where(b => b.NameCode == c.DefectCode).Select(b => b.NameCode + "-" + b.DescriptionCode).FirstOrDefault(),
                        PositionName = c.Position,
                        Status = (bool)c.isUnitOK,
                        LOTID = c.LOTID,

                    }).ToList();
        }


        public List<TopRemont> GetDataRemonts(List<Repair> repairs)
        {
            List<TopRemont> TopRemotss = new List<TopRemont>();

            var ListBarcode = repairs.Select(c => c.Barcode).Distinct().ToList();

            var BarLaze = smd.LazerBase.Where(c => ListBarcode.Contains(c.Content)).Select(c => new RepError
            {

                Barcode = c.Content,
                PCBID = c.IDLaser,
                //LOTID = repairs.Where(b=>b.Barcode == c.Content).Select(b=>(int)b.LOTID).FirstOrDefault(),

            }).ToList();

            Parallel.ForEach(BarLaze, x => { x.LOTID = repairs.Where(c => c.Barcode == x.Barcode).Select(b => (int)b.LOTID).FirstOrDefault(); });

            var ListPCBID = BarLaze.Select(c => c.PCBID).ToList();

            var LOTListVLV = fas.FAS_GS_LOTs.Select(c => c.LOTID).ToList();
            var COntractListVLV = fas.Contract_LOT.Select(c => c.ID).ToList();
            //var BarTonListLOT = new List<int>() { 5154 , 5156 };




            List<RepError> Datas;

            Datas = BarLaze.Where(c => LOTListVLV.Contains((short)c.LOTID)).Select(c => new RepError
            {

                Barcode = c.Barcode,
                PCBID = c.PCBID,
                ErrorCode = fas.FAS_Disassembly
                 .Where(x => ListPCBID.Contains((int)x.PCBID) & x.PCBID == c.PCBID)
                 .OrderByDescending(b => b.DisassemblyDate)
                 .Select(x => fas.FAS_ErrorCode.Where(v => v.ErrorCodeID == x.ErrorCodeID).Select(a => a.ErrorCode + " - " + a.Description).FirstOrDefault()).FirstOrDefault(),
                FulLotCode = fas.FAS_GS_LOTs.Where(b => b.LOTID == c.LOTID).Select(b => b.FULL_LOT_Code).FirstOrDefault(),
                LOTID = (int)c.LOTID,

            }).ToList();

            Datas.AddRange(BarLaze.Where(c => COntractListVLV.Contains((short)c.LOTID)).Select(c => new RepError
            {
                Barcode = c.Barcode,
                PCBID = c.PCBID,
                ErrorCode = fas.Ct_OperLog.Where(x => ListPCBID.Contains((int)x.PCBID) & x.PCBID == c.PCBID & x.TestResultID == 3 & x.ErrorCodeID != null)
               .OrderByDescending(b => b.StepDate).Select(x => fas.FAS_ErrorCode.Where(v => v.ErrorCodeID == x.ErrorCodeID).Select(a => a.ErrorCode + " - " + a.Description).FirstOrDefault()).FirstOrDefault(),
                FulLotCode = fas.Contract_LOT.Where(b => b.ID == c.LOTID).Select(b => b.FullLOTCode).FirstOrDefault(),
                LOTID = (int)c.LOTID,
            }).ToList());



            Parallel.ForEach<Repair>(repairs, x =>
            {
                x.PCBID = Datas.Where(b => b.Barcode == x.Barcode).Select(c=>c.PCBID).FirstOrDefault();
                x.ErrorCode = Datas.Where(b => b.PCBID == x.PCBID).Select(c=>c.ErrorCode).FirstOrDefault();
                x.FullLotCode = Datas.Where(b => b.LOTID == x.LOTID).Select(c => c.FulLotCode).FirstOrDefault();
                if (x.FullLotCode == "")
                {
                    if (x.Barcode.Contains("BTA"))
                        x.FullLotCode = "BarTon BTA";
                    if (x.Barcode.Contains("BTH"))
                        x.FullLotCode = "BarTon BTH";
                }
            });

            TopRemotss = repairs.GroupBy(c => new { c.RepairCode, c.ErrorCode, c.Status, c.PositionName, c.FullLotCode }).Select(c => new TopRemont()
            {
                Count = c.Count(),
                NamdeDis = c.Key.ErrorCode,
                PositionName = c.Key.PositionName,
                RepairCode = c.Key.RepairCode,
                Status = c.Key.Status,
                FullLotCode = c.Key.FullLotCode,

            }).OrderByDescending(c => c.Count).Where(c => c.Count >= 2).Take(12).ToList();

            return TopRemotss;
        }
    }
}
