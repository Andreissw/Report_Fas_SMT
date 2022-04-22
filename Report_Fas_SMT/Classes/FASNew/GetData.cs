using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes.FASNew
{

   

    public abstract class GetData 
    {
        public FASEntities1 fas = new FASEntities1();
        public SMDCOMPONETSEntities smd = new SMDCOMPONETSEntities();

        public HowTime HowTime { get; set; }

        public string Type { get; set; }

        public readonly ModelLines ModelLiness;
        public GetData(ModelLines modelLines)
        {
            ModelLiness = modelLines;
        }

        public abstract List<FPYData> GetFPYData();
        public abstract List<TopDis> GetTopDiss();

        public abstract List<TopRemont> GetRemontOnline();
        public abstract List<TopRemont> GetTopRemots();
        public abstract List<Log> GetLog();
        public abstract List<OTK> GetOTK();

        //public List<Repair> GetList()
        //{ 
        //    return fas.M_Repair_Table.Where(c => c.RapairDate >= HowTime.mode.DateSt & c.RapairDate <= HowTime.mode.Dateend & c.Position != null & c.LOTID == ModelLiness.LOTID).
        //            Select(c => new Repair()
        //            {
        //                Barcode = c.Barcode,
        //                RepairCode = fas.FAS_RepairCode.Where(b => b.NameCode == c.RepairCode).Select(b => b.NameCode + "-" + b.DescriptionCode).FirstOrDefault(),
        //                DefectCode = fas.FAS_DefectCode.Where(b => b.NameCode == c.DefectCode).Select(b => b.NameCode + "-" + b.DescriptionCode).FirstOrDefault(),
        //                PositionName = c.Position,
        //                Status = (bool)c.isUnitOK,
        //                LOTID = c.LOTID,

        //            }).ToList();
        //}

        //public List<TopRemont> GetDataRemonts(List<Repair> repairs)
        //{
        //    List<TopRemont> TopRemotss = new List<TopRemont>();

        //    var ListBarcode = repairs.Select(c => c.Barcode).Distinct().ToList();          

        //    var BarLaze = smd.LazerBase.Where(c => ListBarcode.Contains(c.Content)).Select(c => new  RepError {

        //        Barcode = c.Content,
        //        PCBID = c.IDLaser,
        //        //LOTID = repairs.Where(b=>b.Barcode == c.Content).Select(b=>(int)b.LOTID).FirstOrDefault(),

        //    }).ToList();

        //    Parallel.ForEach(BarLaze, x => {   x.LOTID = repairs.Where(c => c.Barcode == x.Barcode).Select(b => (int)b.LOTID).FirstOrDefault();        });

        //    var ListPCBID = BarLaze.Select(c => c.PCBID).ToList();

        //    var LOTListVLV = fas.FAS_GS_LOTs.Select(c =>   c.LOTID ).ToList();
        //    var COntractListVLV = fas.Contract_LOT.Select(c => c.ID ).ToList();
        //    //var BarTonListLOT = new List<int>() { 5154 , 5156 };

        


        //    List<RepError> Datas;
       
        //     Datas = BarLaze.Where(c=>LOTListVLV.Contains((short)c.LOTID)).Select(c => new RepError {

        //        Barcode = c.Barcode,
        //        PCBID = c.PCBID,
        //        ErrorCode = fas.FAS_Disassembly
        //        .Where(x => ListPCBID.Contains((int)x.PCBID) & x.PCBID == c.PCBID )
        //        .OrderByDescending(b => b.DisassemblyDate)
        //        .Select(x => fas.FAS_ErrorCode.Where(v => v.ErrorCodeID == x.ErrorCodeID).Select(a => a.ErrorCode + " - " + a.Description).FirstOrDefault()).FirstOrDefault(),
        //        FulLotCode = fas.FAS_GS_LOTs.Where(b=>b.LOTID == c.LOTID).Select(b=>b.FULL_LOT_Code).FirstOrDefault(),
        //         LOTID = (int)c.LOTID,

        //     }).ToList();
           
        //     Datas.AddRange( BarLaze.Where(c=> COntractListVLV.Contains((short)c.LOTID)).Select(c => new RepError
        //    {
        //        Barcode = c.Barcode,
        //        PCBID = c.PCBID,
        //        ErrorCode = fas.Ct_OperLog.Where(x => ListPCBID.Contains((int)x.PCBID) & x.PCBID == c.PCBID & x.TestResultID == 3 & x.ErrorCodeID != null)
        //        .OrderByDescending(b => b.StepDate).Select(x => fas.FAS_ErrorCode.Where(v => v.ErrorCodeID == x.ErrorCodeID).Select(a => a.ErrorCode + " - " + a.Description).FirstOrDefault()).FirstOrDefault(),
        //        FulLotCode = fas.Contract_LOT.Where(b => b.ID == c.LOTID).Select(b => b.FullLOTCode).FirstOrDefault(),
        //        LOTID = (int)c.LOTID,
        //     }).ToList());



        //    Parallel.ForEach<Repair>(repairs, x =>
        //    {
        //        x.PCBID = Datas.Where(b => b.Barcode == x.Barcode).FirstOrDefault().PCBID;
        //        x.ErrorCode = Datas.Where(b => b.PCBID == x.PCBID).FirstOrDefault().ErrorCode;
        //        x.FullLotCode = Datas.Where(b => b.LOTID == x.LOTID).FirstOrDefault().FulLotCode;
        //        if (x.FullLotCode == "")
        //        {
        //            if (x.Barcode.Contains("BTA"))
        //                x.FullLotCode = "BarTon BTA";
        //            if (x.Barcode.Contains("BTH"))
        //                x.FullLotCode = "BarTon BTH";
        //        }
        //    });

        //    TopRemotss = repairs.GroupBy(c => new { c.RepairCode,  c.ErrorCode, c.Status, c.PositionName, c.FullLotCode }).Select(c => new TopRemont()
        //    {
        //        Count = c.Count(),
        //        NamdeDis = c.Key.ErrorCode,
        //        PositionName = c.Key.PositionName,
        //        RepairCode = c.Key.RepairCode,
        //        Status = c.Key.Status,
        //        FullLotCode = c.Key.FullLotCode,

        //    }).OrderByDescending(c => c.Count).Where(c => c.Count >= 2).Take(7).ToList();

        //    return TopRemotss;
        //}


    }

    public class GS : GetData
    {
        public GS(ModelLines modelLines) : base(modelLines)
        {
            Type = "ВЛВ";   
        }

        List<int> ListDis;

        public override List<FPYData> GetFPYData()
        {
            List<FPYData> fPYDatas = new List<FPYData>();

            

            fPYDatas = fas.FAS_PackingGS.Where(c => fas.FAS_Liter.Where(b => b.ID == c.LiterID).FirstOrDefault().LineID == (short)ModelLiness.LineID
                & c.LOTID == ModelLiness.LOTID & c.PackingDate >= HowTime.mode.DateSt & c.PackingDate <= HowTime.mode.Dateend
            ) 
            .GroupBy(c=> new { c.LOTID, c.LiterID })
            .Select(x => new FPYData()
            {                
                Count = x.Count(),
                CountDis = fas.FAS_Disassembly.Where(c=>c.LOTID == x.Key.LOTID & c.DisassemblyDate >= HowTime.mode.DateSt & c.DisassemblyDate <= HowTime.mode.Dateend &
                    fas.FAS_Liter.Where(b=>b.ID == x.Key.LiterID).FirstOrDefault().LineID == c.DisAssemblyLineID).Count(),
                Objective = fas.FAS_Objective.Where(c=>c.LOTID == ModelLiness.LOTID & c.Manuf == "Цех Сборки").Select(c=>c.Objective).FirstOrDefault(),
                
            }).ToList();
            
            fPYDatas.ForEach(c => c.FPY = (100 - (c.CountDis / (c.Count + c.CountDis) * 100)).ToString("##.##"));

            return fPYDatas;
        }

        public override List<TopDis> GetTopDiss()
        {
            List<TopDis> TopDiss = new List<TopDis>();

            var dis = fas.FAS_Disassembly.Where(c => c.DisAssemblyLineID == (byte)ModelLiness.LineID
                & c.LOTID == ModelLiness.LOTID & c.DisassemblyDate >= HowTime.mode.DateSt & c.DisassemblyDate <= HowTime.mode.Dateend
            );

            ListDis = dis.Select(c => c.PCBID).ToList();

            TopDiss = dis.GroupBy(c => new { c.LOTID, c.ErrorCodeID })
            .Select(c => new TopDis()
            {
                Count = c.Count(),
                Name = fas.FAS_ErrorCode.Where(b=>b.ErrorCodeID == c.Key.ErrorCodeID).Select(b=>b.ErrorCode + " " + b.Description).FirstOrDefault(),

            }).OrderByDescending(c=>c.Count).ToList();

           

            return TopDiss;
        }

        public override List<TopRemont> GetRemontOnline()
        {
            List<TopRemont> TopRemotss = new List<TopRemont>();
            if (ListDis == null)
            {
                return TopRemotss;
            }

            string fff = "";

            ListDis.ForEach(c => fff += Environment.NewLine + c.ToString());

            var listbar= ListDis.Select(c =>                           
               smd.LazerBase.Where(b=>b.IDLaser == c).Select(b=>b.Content).FirstOrDefault()
            ).ToList();

            TopRemotss = fas.M_Repair_Table.Where(b => listbar.Contains(b.Barcode) & b.RapairDate >= HowTime.mode.DateSt & b.RapairDate <= HowTime.mode.Dateend)
                .GroupBy(c => new
            {
                c.RepairCode,
                c.DefectCode,
                c.ErrorCode,
                c.isUnitOK,
                c.Position,
            }).Select(c => new TopRemont()
            {
                Count = c.Count(),
                NamdeDis = c.Key.ErrorCode,
                PositionName = c.Key.Position,
                RepairCode = fas.FAS_RepairCode.Where(b => b.NameCode == c.Key.RepairCode).Select(b => b.NameCode + "-" + b.DescriptionCode).FirstOrDefault(),
                Status = (bool)c.Key.isUnitOK,

            }).OrderByDescending(c=>c.Count).ToList();
            

            return TopRemotss;

        }

        public override List<TopRemont> GetTopRemots()
        {
            List<TopRemont> TopRemotss = new List<TopRemont>();
            AllRemont allRemont = new AllRemont();
            TopRemotss = allRemont.GetDataRemonts(allRemont.GetList(HowTime,ModelLiness));

            return TopRemotss;
        }

        public override List<Log> GetLog()
        {
            List<Log> Logs = new List<Log>();

            var list = fas.Ct_OperLog.Where(c => c.StepDate >= HowTime.mode.DateSt & c.StepDate <= HowTime.mode.Dateend & c.LineID == ModelLiness.LineID)
                .Select(c => new
                {

                    PCBID = c.PCBID,
                    Result = fas.Ct_TestResult.Where(b => b.ID == c.TestResultID).FirstOrDefault().Result,
                    Step = fas.Ct_StepScan.Where(b => b.ID == c.StepID).FirstOrDefault().StepName,
                    LINEID = c.LineID,

                }).ToList();

            var listPCBID = list.Select(c => (int)c.PCBID).ToList();

            var LL = fas.FAS_OperationLog.Where(c => listPCBID.Contains(c.PCBID)).Select(b => new { 
            
                PCBID =  b.PCBID,
                LOTID = fas.FAS_SerialNumbers.Where(z=>z.SerialNumber == b.SerialNumber).Select(z=> z.LOTID).FirstOrDefault(),
                FullLotCode = fas.FAS_SerialNumbers.Where(z => z.SerialNumber == b.SerialNumber).Select(z => fas.FAS_GS_LOTs.Where(a=>a.LOTID == z.LOTID).FirstOrDefault().FULL_LOT_Code).FirstOrDefault(),

            }).Distinct().Where(c=>c.LOTID == ModelLiness.LOTID).ToList();

            listPCBID = LL.Select(c => c.PCBID).ToList();

            Logs = list.Where(c => listPCBID.Contains((int)c.PCBID)).GroupBy(c => new { c.LINEID, c.Result, c.Step }).Select(c => new Log()
            {
                Count =c.Count(),
                Result = c.Key.Result,
                Stage = c.Key.Step,

            }).ToList();

            return Logs;
        }

        public override List<OTK> GetOTK()
        {
            List<OTK> OTKs = new List<OTK>();

            var datas = fas.KGP_Control_Sputnik.Where(c => c.START_DATE >= HowTime.mode.DateSt & c.START_DATE <= HowTime.mode.Dateend)
                .Select(c => new KGPData()
                {
                    SerialNumber = c.ID,
                    Pass = c.Pass,
                    DefectCode = c.DefectCode,
                    FULLLotCode = fas.FAS_SerialNumbers.Where(b => b.SerialNumber == c.ID).Select(b => fas.FAS_GS_LOTs.Where(x => x.LOTID == b.LOTID).FirstOrDefault().FULL_LOT_Code).FirstOrDefault()
                    ,LOTID = fas.FAS_SerialNumbers.Where(b => b.SerialNumber == c.ID).Select(b=>b.LOTID).FirstOrDefault(),
                    LineID = fas.FAS_Start.Where(b=>b.SerialNumber == c.ID).Select(b=>b.LineID).FirstOrDefault(),
                    LineName = fas.FAS_Start.Where(b => b.SerialNumber == c.ID).Select(b => fas.FAS_Lines.Where(x=>x.LineID == b.LineID).FirstOrDefault().LineName).FirstOrDefault(),
                }).ToList();
         
            OTKs = datas.Where(c=>c.LOTID == ModelLiness.LOTID & ModelLiness.Line.Substring(ModelLiness.Line.Length - 1) == 
             c.LineName.Substring(c.LineName.Length - 1)
            ).GroupBy(c => new { fullcode = c.FULLLotCode, pass = c.Pass, defectcode = c.DefectCode })
                .Select(c => new OTK()
                {
                    Count = c.Count(),
                    DefectCode = fas.FAS_ErrorCode.Where(b=>b.ErrorCodeID == c.Key.defectcode).Select(b=>b.Category + " " + b.Code).FirstOrDefault(),
                    Pass = c.Key.pass,                    
                }).ToList();

            return OTKs;
        }
    }

    public class Contract : GetData
    {
        public Contract( ModelLines modelLines) : base(modelLines)
        {
            Type = "Контрактное";
        }

        public override List<FPYData> GetFPYData()
        {
            List<FPYData> fPYDatas = new List<FPYData>();

            fPYDatas = fas.Ct_OperLog.Where(c => c.StepDate >= HowTime.mode.DateSt & c.StepDate <= HowTime.mode.Dateend 
            & c.LOTID == ModelLiness.LOTID & c.ErrorCodeID != 580 &
            fas.FAS_Lines.Where(b=>b.LineID == c.LineID).Select(b=>b.LineName.Substring(b.LineName.Length- 1 )).FirstOrDefault() == ModelLiness.Line.Substring(ModelLiness.Line.Length - 1)
            
            ).GroupBy(c => new { c.LOTID, }).Select(c => new FPYData
            {               
                Count = c.Where(b => b.TestResultID == 2 & b.StepID == 6).Count(),
                CountDis = c.Where(b => b.TestResultID == 3 & b.ErrorCodeID != null & b.ErrorCodeID != 580).Count(),
                Objective = fas.FAS_Objective.Where(b=>b.LOTID == ModelLiness.LOTID & b.Manuf == "Цех Сборки").Select(b=>b.Objective).FirstOrDefault(),

            }).ToList();

            fPYDatas.ForEach(c => c.FPY = (100 - (c.CountDis / (c.Count + c.CountDis) * 100)).ToString("##.##"));

            return fPYDatas;
        }

        public override List<TopDis> GetTopDiss()
        {
            List<TopDis> TopDiss = new List<TopDis>();

            TopDiss = fas.Ct_OperLog.Where(c => c.StepDate >= HowTime.mode.DateSt & c.StepDate <= HowTime.mode.Dateend 
            & c.TestResultID == 3 & c.ErrorCodeID != null & c.LOTID == ModelLiness.LOTID & c.ErrorCodeID != 580 &
            fas.FAS_Lines.Where(b => b.LineID == c.LineID).Select(b => b.LineName.Substring(b.LineName.Length - 1)).FirstOrDefault() == ModelLiness.Line.Substring(ModelLiness.Line.Length - 1)).GroupBy(c => new { c.LOTID, c.ErrorCodeID }).Select(c => new TopDis
            {
                Count = c.Count(),
                Name = c.Select(b => fas.FAS_ErrorCode.Where(x => x.ErrorCodeID == c.Key.ErrorCodeID).Select(x => x.ErrorCode + "-" + x.Description).FirstOrDefault()).FirstOrDefault(),
              
            }).OrderByDescending(c => c.Count).Take(7).ToList();


            return TopDiss;
        }
        public override List<TopRemont> GetRemontOnline()
        {
            List<TopRemont> TopRemotss = new List<TopRemont>();

            return TopRemotss;
        }

        public override List<TopRemont> GetTopRemots()
        {
            List<TopRemont> TopRemotss = new List<TopRemont>();
            AllRemont allRemont = new AllRemont();
            TopRemotss = allRemont.GetDataRemonts(allRemont.GetList(HowTime, ModelLiness));            

            return TopRemotss;
        }

        public override List<Log> GetLog()
        {
            List<Log> Logs = new List<Log>();

            Logs = fas.Ct_OperLog.Where(c => c.StepDate >= HowTime.mode.DateSt &  c.StepDate <= HowTime.mode.Dateend & c.LOTID == ModelLiness.LOTID &
            fas.FAS_Lines.Where(b => b.LineID == c.LineID).Select(b => b.LineName.Substring(b.LineName.Length - 1)).FirstOrDefault() == ModelLiness.Line.Substring(ModelLiness.Line.Length - 1)

            ).GroupBy(c => new { c.StepID, c.TestResultID, c.LOTID }).Select(c => new Log
            {

                Count = c.Count(),
                Stage = c.Select(b => fas.Ct_StepScan.Where(x => x.ID == c.Key.StepID).Select(x => x.StepName).FirstOrDefault()).FirstOrDefault(),               
                Result = c.Select(b => fas.Ct_TestResult.Where(x => x.ID == c.Key.TestResultID).Select(x => x.Result).FirstOrDefault()).FirstOrDefault(),

            }).OrderByDescending(c => new { c.Count, c.Stage }).ToList();

            return Logs;
        }

        public override List<OTK> GetOTK()
        {
            List<OTK> OTKs = new List<OTK>();

            var datas = fas.KGP_Control.Where(c => c.START_DATE >= HowTime.mode.DateSt & c.START_DATE <= HowTime.mode.Dateend)
               .Select(c => new KGPData()
               {
                   Barcode = c.Barcode,
                   Pass = c.Pass,
                   DefectCode = c.DefectCode,       
                   LOTID = fas.Ct_FASSN_reg.Where(b=>b.SN == c.Barcode).Select(b=>b.LOTID).FirstOrDefault(),                   
               
               }).ToList();

            var listbar = datas.Select(c => c.Barcode).ToList();

            var d = smd.LazerBase.Where(c => listbar.Contains(c.Content)).Select(b => new { 
            
                Barcode = b.Content,
                PCBID = b.IDLaser,
            
            }).ToList();

            var listpcbid = d.Select(c => c.PCBID).ToList();

            var dat = fas.Ct_OperLog.Where(c => listpcbid.Contains((int)c.PCBID) & c.LOTID != null).Select(c => new { 
            
                PCBID = c.PCBID,
                LOTID =c.LOTID,         
                FULLLOTCODE = fas.Contract_LOT.Where(b=>b.ID == c.LOTID).FirstOrDefault().FullLOTCode,

            
            }).ToList();

            Parallel.ForEach(datas.Where(c =>c.LOTID == 0), x =>
            {                
                x.PCBID = d.Where(b => b.Barcode == x.Barcode).Select(b => b.PCBID).FirstOrDefault();
                x.LOTID = dat.Where(b => b.PCBID == x.PCBID).Select(b => b.LOTID).FirstOrDefault();
                x.FULLLotCode = dat.Where(b => b.PCBID == x.PCBID).Select(b => b.FULLLOTCODE).FirstOrDefault();
               
            });


            OTKs = datas.Where(c => c.LOTID == ModelLiness.LOTID 
            ).GroupBy(c => new { fullcode = c.FULLLotCode, pass = c.Pass, defectcode = c.DefectCode })
                .Select(c => new OTK()
                {
                    Count = c.Count(),
                    DefectCode = fas.FAS_ErrorCode.Where(b => b.ErrorCodeID == c.Key.defectcode).Select(b => b.Category + " " + b.Code).FirstOrDefault(),
                    Pass = c.Key.pass,
                }).ToList();

            return OTKs;
        }
    }
}
