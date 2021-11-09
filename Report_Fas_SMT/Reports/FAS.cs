using Report_Fas_SMT.Classes.Classes_FAS;
using Report_Fas_SMT.Classes.Classes_FAS.Contract;
using Report_Fas_SMT.Classes.Classes_FAS.Others;
using Report_Fas_SMT.Classes.Classes_FAS.VLV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Reports
{
    public class FAS
    {
        FASEntities fas = new FASEntities();
        SMDCOMPONETSEntities smd = new SMDCOMPONETSEntities();
        public string HTML { get; set; }

        DataFasReport DataFas;

        public FAS ()
        {
            
        }

        CounterStage GetInfoOrder(DataSet data)
        {
            CounterStage counterStage = new CounterStage()
            {
                NameOrder = "sp36_Dp DPA320S_7000шт",
                ListStages = new List<ListStage>(),
                SumCount = 0,
            };

            foreach (DataRow item in data.Tables[0].Rows)
            {
                counterStage.ListStages.Add(new ListStage()
                {
                    NameStage = item[1].ToString(),
                    Result = item[2].ToString(),
                    Count = int.Parse(item[3].ToString())
                });

                counterStage.SumCount += int.Parse(item[3].ToString());
            }
            return counterStage;
        }

        public bool GetDataFAS() {


            DateTime now = DateTime.Parse( DateTime.Now.ToString("yyyy-MM-dd") + " 07:00:00");
            DateTime FiveMonth = DateTime.Parse(DateTime.Now.AddMonths(-5).ToString("yyyy-MM-dd") + " 07:00:00");
            DataFas = new DataFasReport();

            var result = Connect.Loadgrid(@"use fas select _table.НомерЭтапа,_table.Этап, _table.Результат, count(1) 'Кол-во' from (SELECT  
	                                       (select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser = PCBID) Decode      
	                                      ,(select FullLOTCode from Contract_LOT c where LOTID = c.ID) 'Order'
                                          ,(select sc.StepName from Ct_StepScan sc where sc.ID = op.StepID) Этап
	                                      ,(select sc.NumStep from Ct_StepScan sc where sc.ID = op.StepID) НомерЭтапа
	                                      ,(select Result from Ct_TestResult t where op.TestResultID = t.ID ) Результат      
                                          ,Format([StepDate],'dd.MM.yyyy HH:mm:ss') Дата          
                                          ,ROW_NUMBER() over (partition by pcbid order by stepdate desc) num
                                          FROM [FAS].[dbo].[Ct_OperLog] op
                                          where LOTID = '20111') as _table
                                          where num = 1
                                          group by _table.НомерЭтапа,_table.Этап, _table.Результат
                                          order by _table.НомерЭтапа");

            if (result.Tables.Count == 0)
            {
                return false;
            }          

            DataFas.CounterStages.Add(GetInfoOrder(result));





        DataFas.DataContract.ListLog = fas.Ct_OperLog.Where(c => c.StepDate >= now).GroupBy(c=> new { c.StepID,c.TestResultID, c.LOTID }).Select(c => new Log { 
            
                Count = c.Count(),
                Name =  c.Select(b=> fas.Ct_StepScan.Where(x=>x.ID == c.Key.StepID).Select(x=>x.StepName).FirstOrDefault()).FirstOrDefault(),
                NameModel = c.Select(b => fas.Contract_LOT.Where(x => x.ID == c.Key.LOTID).Select(x => fas.FAS_Models.Where(z=>z.ModelID == x.ModelID).FirstOrDefault().ModelName).FirstOrDefault()).FirstOrDefault(),
                Result = c.Select(b => fas.Ct_TestResult.Where(x => x.ID == c.Key.TestResultID).Select(x => x.Result).FirstOrDefault()).FirstOrDefault(),

            }).OrderByDescending(c => new { c.Count,c.Name,c.NameModel}).ToList();

            DataFas.DataContract.ListTop = fas.Ct_OperLog.Where(c => c.StepDate >= now & c.TestResultID == 3 & c.ErrorCodeID != null).GroupBy(c => new { c.LOTID, c.ErrorCodeID }).Select(c => new TopErrorList_ct
            {
                Count = c.Count(),
                ErrorCode = c.Select(b => fas.FAS_ErrorCode.Where(x => x.ErrorCodeID == c.Key.ErrorCodeID).Select(x => x.ErrorCode + "-"+ x.Description).FirstOrDefault()).FirstOrDefault(),
                NameModel = c.Select(b => fas.Contract_LOT.Where(x => x.ID == c.Key.LOTID).Select(x => fas.FAS_Models.Where(z => z.ModelID == x.ModelID).FirstOrDefault().ModelName).FirstOrDefault()).FirstOrDefault(),
            }).OrderByDescending(c=>c.Count).Take(7).ToList();

            DataFas.DataContract.ListFPY = fas.Ct_OperLog.Where(c => c.StepDate >= now & c.LOTID != null).GroupBy(c=> new { c.LOTID }).Select(c => new FPY_Ct
            {
                ModelName = c.Select(b => fas.Contract_LOT.Where(x => x.ID == c.Key.LOTID).Select(x => fas.FAS_Models.Where(z => z.ModelID == x.ModelID).FirstOrDefault().ModelName).FirstOrDefault()).FirstOrDefault(),
                Count = c.Where(b=>b.TestResultID == 2 & b.StepID == 6).Count(),
                CountError = c.Where(b => b.TestResultID == 3 & b.ErrorCodeID != null).Count(),

            }).ToList();
            

            DataFas.Data_VLV.ListFPY = fas.FAS_PackingGS.Where(c => c.PackingDate >= now).GroupBy(c=> new { c.LiterID, c.LOTID}).Select(c => new FPY_VLV
            {
                Count = c.Count(),
                CountError = fas.FAS_Disassembly.Where(b=>b.DisassemblyDate >= now & b.DisAssemblyLineID == fas.FAS_Liter.Where(z=>z.ID == c.Key.LiterID).FirstOrDefault().LineID  & c.Key.LOTID == b.LOTID).Count(),
                LineName = fas.FAS_Liter.Where(z => z.ID == c.Key.LiterID).FirstOrDefault().Description,
                ModelName = c.Select(b => fas.FAS_GS_LOTs.Where(x => x.LOTID == c.Key.LOTID).Select(x => fas.FAS_Models.Where(z => z.ModelID == x.ModelID).FirstOrDefault().ModelName).FirstOrDefault()).FirstOrDefault(),

            }).ToList();

            DataFas.Data_VLV.ListTOP = fas.FAS_Disassembly.Where(c => c.DisassemblyDate >= now).GroupBy(c => new { c.DisAssemblyLineID, c.ErrorCodeID, c.LOTID }).Select(c => new TOP_VLV
            {
                LineName = fas.FAS_Lines.Where(b => b.LineID == c.Key.DisAssemblyLineID).Select(b => b.LineName).FirstOrDefault(),             
                ModelName = fas.FAS_Models.Where(b => b.ModelID == fas.FAS_GS_LOTs.Where(z => z.LOTID == c.Key.LOTID).Select(z => z.ModelID).FirstOrDefault()).Select(b => b.ModelName).FirstOrDefault(),
                ErrorCode = fas.FAS_ErrorCode.Where(b=>b.ErrorCodeID == c.Key.ErrorCodeID).Select(b=>b.ErrorCode + "-" + b.Description).FirstOrDefault(),
                Count = c.Count(),

            }).OrderByDescending(c=> new { c.Count}).Take(7).ToList();

            DataFas.DataOther.ListLots = fas.FAS_PackingGS.Where(c => c.PackingDate >= now).Select(c => fas.FAS_GS_LOTs.Where(b => b.LOTID == c.LOTID).Select(b => b.FULL_LOT_Code).FirstOrDefault()).Distinct().ToList();
            DataFas.DataOther.ListLots.AddRange(fas.Ct_OperLog.Where(c => c.StepDate >= now & c.LOTID != null).Select(c => fas.Contract_LOT.Where(b => b.ID == c.LOTID).Select(b => b.FullLOTCode).FirstOrDefault()).Distinct().ToList());

            //var list = smd.LazerBase.Select(c => new Lazer { IdLazer = c.IDLaser, Decode = c.Content }).Distinct();

            DataFas.DataOther.ListRepair = fas.M_Repair_Table.Where(c => c.RapairDate >= now  ).GroupBy(c => new { c.RepairCode, c.Position }).Select(c => new ReapirFAS
            {

                PositionName = c.Key.Position,
                Count = c.Count(),
                RepairCode = fas.FAS_RepairCode.Where(b => b.NameCode == c.Key.RepairCode).Select(b => b.NameCode + "-" + b.DescriptionCode).FirstOrDefault(),
                //ModelName = fas.FAS_Models.Where(z=>z.ModelID == fas.Ct_OperLog.Where(b => b.PCBID == list.Where(x => x.Decode == c.Select(q => q.Barcode).FirstOrDefault()).Select(x => x.IdLazer).FirstOrDefault()).Select(b => b.LOTID).FirstOrDefault()).FirstOrDefault().ModelName,                


            }).OrderByDescending(c => c.Count).Take(8).ToList();


            var listKGP = fas.KGP_Control.Where(c => c.START_DATE >= now).Select(c=> 
            new { 
                decode = c.Barcode,
                ДатаВхода = c.START_DATE.HasValue,
                ДатаВыхода = c.END_DATE.HasValue,
                Результат = c.Pass,
                КодОтказа = c.DefectCode, 
                Описание = c.Description,
                Тип = "Контрактное",
            });

            var listKGPS = fas.KGP_Control_Sputnik.Where(c => c.START_DATE >= now).Select(c =>
             new {

                 decode = c.ID,
              ДатаВхода = c.START_DATE.HasValue,
              ДатаВыхода = c.END_DATE.HasValue,
              Результат = c.Pass,
              КодОтказа = c.DefectCode,
              Описание = c.Description,
              Тип = "ВЛВ"
              
             }); 

            DataFas.DataOther.ListKGP = listKGP
                //.Where(c => c.ДатаВыхода == true)
                .GroupBy(c=> c.Тип).Select(c => new KGP { 
            
                Count = c.Count(),
                Result = c.Select(b=>b.Результат).FirstOrDefault().ToString(),
                Type = c.Key,

            }).ToList();

            var kgp = listKGPS
                //.Where(c => c.ДатаВыхода == true)
                .GroupBy(c => c.Тип).Select(c => new KGP
                {

                    Count = c.Count(),
                    Result = c.Select(b => b.Результат).FirstOrDefault().ToString(),
                    Type = c.Key,

                }).ToList();

           

            DataFas.DataOther.ListKGP.AddRange(kgp);

            foreach (var item in DataFas.DataOther.ListKGP)
            {
                if (item.Result == "1")
                    item.Result = "OK";
                else
                    item.Result = "NOK";
            }


            DataFas.Data_VLV.ListFPY.ForEach(c => c.FPY = (100 - (c.CountError / (c.Count + c.CountError) * 100)).ToString("##.##"));
            DataFas.DataContract.ListFPY.ForEach(c => c.FPY = (100 - (c.CountError / (c.Count + c.CountError) * 100 )).ToString("##.##") );

            return true;
        }

        string GetLog(Log log)
        {
            return $@"  <tr>
                           <td> {log.Count} </td> 
                           <td> {log.Name}</td> 
                           <td> {log.Result}</td> 
                           <td> {log.NameModel}</td> 
                        </tr>";
        }

      

        string GetTopErrorCt(TopErrorList_ct top)
        {
            return $@"  <tr>
                           <td style = ""padding: 1%""> {top.Count} </td> 
                           <td> {top.NameModel}</td>
                           <td> {top.ErrorCode} </td>     
                        </tr>";
        } 
        string GetFPYCt(FPY_Ct fpy) {

            if (fpy.Count == 0)
            {
                return "";
            }

            string style = GetStyle(fpy.FPY.ToString());
            return $@"  <tr >
                          <td> {fpy.ModelName}</td>
                          <td> {fpy.Count} </td> 
                          <td> {fpy.CountError} </td>    
                          <td {style}> {fpy.FPY}% </td> 
                        </tr>";
        }

        string GetVLVFPY(FPY_VLV fpy)
        {
            string style = GetStyle(fpy.FPY.ToString());
            return $@"  <tr >      
                      <td>{fpy.ModelName}</td>
                      <td>{fpy.LineName}</td>
                      <td> {fpy.Count} </td>
                      <td> {fpy.CountError} </td>
                      <td {style}> {fpy.FPY}% </td>      
                    </tr>";
        }

        string GetTopVLV(TOP_VLV top)
        {
            return $@"   <tr>     
                        <td> {top.Count} </td>
                        <td>{top.ModelName}</td>
                        <td>{top.LineName}</td>
                        <td> {top.ErrorCode} </td>        
                      </tr>";
        }

        string GetOrders(string order)
        {
            return $@"  <tr>     
                                      <td> {order}</td>
                                    </tr>";
        }

        string GetRepairs(ReapirFAS Repair)
        {
            return $@"    <tr>     
                            <td> {Repair.Count} </td>  
                            <td> {Repair.PositionName}</td>
                            <td> {Repair.RepairCode} </td>
                            <td> {Repair.ModelName}</td>
                          </tr>";
        }

        string GetTraccir(ListStage stage)
        {
            return $@"    <tr>     
                            <td> {stage.NameStage} </td>  
                            <td> {stage.Result}</td>
                            <td> {stage.Count} </td>                            
                          </tr>";
        }

        string GetKGP(KGP kgp)
        {
            string style = "";
            if (kgp.Result == "OK")
            {
                style = @"style = ""background-color: lightgreen;"" ";
            }
            else if (kgp.Result == "NOK")
            {
                style = @"style = ""background-color: coral;"" ";
            }
            return $@"     <tr>     
                            <td> {kgp.Count} </td>                              
                            <td {style}> {kgp.Result} </td>
                            <td> {kgp.Type} </td>
                            
                          </tr>";
        }

        string GetStyle(string FPY)
        {
            string style = "";
            if (FPY == "")
            {
                return style;
            }

            if (double.Parse(FPY) >= 98)
            {
                style = @"style = ""background-color: lightgreen;"" ";
            }
            else if (double.Parse(FPY) < 98)
            {
                style = @"style = ""background-color: coral;"" ";
            }
            return style;
        }

        public void RenerHTML()
        {
            string html = "";

            string Traccir = "";
            string log = "";
            string TopErrorContract = "";
            string FPYCt = "";
            string FPYVLV = "";
            string TOPVLV = "";
            string orders = "";
            string Repairs = "";
            string KGP = "";

            if (DataFas.CounterStages != null)
                foreach (var item in DataFas.CounterStages.FirstOrDefault().ListStages)               
                    Traccir += GetTraccir(item);
                

            if (DataFas.DataContract.ListLog != null)           
                foreach (var item in DataFas.DataContract.ListLog)            
                    log += GetLog(item);

            if (DataFas.DataContract.ListTop != null)
                foreach (var item in DataFas.DataContract.ListTop)
                     TopErrorContract += GetTopErrorCt(item);

            if (DataFas.DataContract.ListFPY != null)
                foreach (var item in DataFas.DataContract.ListFPY)
                     FPYCt += GetFPYCt(item);

            if (DataFas.Data_VLV.ListFPY != null)
                foreach (var item in DataFas.Data_VLV.ListFPY)
                    FPYVLV += GetVLVFPY(item);

            if (DataFas.Data_VLV.ListTOP != null)
                foreach (var item in DataFas.Data_VLV.ListTOP)
                 TOPVLV += GetTopVLV(item);

            if (DataFas.DataOther.ListLots != null)
                foreach (var item in DataFas.DataOther.ListLots)
                    orders += GetOrders(item);

            if (DataFas.DataOther.ListRepair != null)
                foreach (var item in DataFas.DataOther.ListRepair)
                    Repairs += GetRepairs(item);

            if (DataFas.DataOther.ListKGP != null)
                foreach (var item in DataFas.DataOther.ListKGP)
                  KGP += GetKGP(item);

            html  = $@"
                                 <style type=""text/css"">
                                    body{{
                                         background-color: lavender;
                                    }}

                                    .Container{{
                                      display:flex;  
                                      align-items:flex-start;
                                    }}

                                    .HeadTable{{
                                      padding: 2px;  
                                      border: 2px solid gray ;
                                      background-color: gold;  
                                      padding: 10px;
                                    }}

                                    td{{
                                      border: 1px solid gray;   
                                      text-align:center;
                                      vertical-align:top;
                                      background-color: azure;
                                      margin: 1%;
                                      padding: 1%;
  
                                    }}

                                    .intd td{{
                                      background-color: white;
                                    }}

                                    tr{{
                                      text-align: center;  
                                    }}

                                    th{{
                                      border: 3px dashed gray;
                                      background-color: gold;
                                    }}

                                    table {{
                                       margin-top: 1%;    
                                       margin-left: 0.6%;
                                       padding: 0.3%;
                                       font-size: 14px; 
                                       text-align: center;
                                       border-left: 1.5px solid black; 
                                       border-top: 1.5px solid black;   
                                    }}

                                    a#txt{{
                                      margin-left: 10%;
                                      font-size: 30px;  
                                      color: gray;
                                    }}

                                    #date{{
                                       font-size: 30px;   
                                       padding: 10px;   
                                       background-color: khaki;
                                       border: 1px solid gray;
                                    }}

                                 </style>
                                  Добрый день!
</br>
</br>
</br>
                                  
                                        <div style = ""font-size: 30px;   
                                           padding: 10px;   
                                           background-color: khaki;
                                           border: 1px solid gray;"">

                                           Дата: {DateTime.UtcNow.AddHours(2).ToString("dd.MM.yyyy")} Час {DateTime.UtcNow.AddHours(2).ToString("HH:mm:ss")}
                                        </div>
                                        <div style = ""margin-left: 10%;
                                          font-size: 30px;  
                                          color: gray;"">
                                          FAS Карта контроля!
                                        </div>
                            
                           <table>                               
                            <tr>                         
                                <td>
                                    
                                     <table>
                                    <th class = ""HeadTable"" colspan=""3""> Трассировка плат Заказ - {DataFas.CounterStages.FirstOrDefault().NameOrder} </th> 
                                    <tr>
                                        <th> Этап </th>
                                        <th> Результат </th>
                                        <th> Кол-во </th>
                                    </tr>
                                    {Traccir}
                                    <tr>
                                        <td colspan =3 style = ""text-align:right; background-color: lightblue"">
                                            Всего = {DataFas.CounterStages.FirstOrDefault().SumCount}
                                        </td>
                                    </tr>

                              </table>


                                </td>
                            <td>
                                  <table class = ""intd""> <!-- FPY - Контрактное -->

                                                 <th class = ""HeadTable"" colspan=""4""> FPY - Контрактное производство</th>  
                                        <tr> 
                                          <th>Модель</th>
                                          <th>Выпуск</th>
                                          <th>Отказов</th>
                                          <th>FPY</th>
                                        </tr>
                                               {FPYCt}
                        

                            </table>
</td>                  
   <td>
                                    <table class = ""intd""> <!-- Топ ошибок -->

                                                 <th class = ""HeadTable"" colspan=""3""> Топ 7 ошибок контрактное производство</th>   
                                        <tr>
                                          <th>Кол-во</th>
                                          <th>Модель</th>
                                          <th>Отказ</th>
                                        </tr>
                                                 {TopErrorContract}

                                     </table>  
                            </td>
 </tr>
                                
  <tr>
<td>
                                  <table class = ""intd""> <!-- FPY ВЛВ -->
                                     <th class = ""HeadTable"" colspan=""5""> FPY ВЛВ</th>  
                                    <tr> 
                                      <th>Модель</th>
                                      <th> Линия</th>
                                      <th> Выпуск</th>
                                      <th> Отказы</th>
                                      <th> FPY</th>
                                    </tr>
    
                                    {FPYVLV}

                                  </table>
  </td>
<td>
                                  <table class = ""intd""> <!-- Отказы ВЛВ -->
                                     <th class = ""HeadTable"" colspan=""4""> Топ 7 Отказов ВЛВ</th>  
<tr>
                                          <th>Кол-во</th>
                                          <th>Модель</th>
                                          <th>Линия</th>
                                            <th>Отказ</th>
</tr>
    

                                    {TOPVLV}
                                  </table>
    </td>
<td>
                                  <table class = ""intd""> <!-- Лоты все -->
                                     <th class = ""HeadTable"" colspan=""1""> Выпускаемые заказы</th>  
                                     {orders}
                                  </table>
    </td>
  </tr>
<tr>
   <td>
                                   <table class = ""intd""> <!-- ЛОГ -->
     
                                               <th class = ""HeadTable"" colspan=""4""> FAS - Лог</th>   
                                                 <tr> 
                                          <th>Кол-во</th>
                                          <th>Этап</th>
                                          <th>Результат</th>
                                          <th>Модель</th>
                                        </tr>
                                               {log}               
                                   </table>  
                             </td>
<td>
                                  <table class = ""intd""><!-- Ремонт общий -->
                                   <th class = ""HeadTable"" colspan=""4""> Ремонт FAS</th>  
                                    {Repairs}
                                  </table>
  </td>
<td>
                                   <table><!-- Выборочный контроль -->
                                   <th class = ""HeadTable"" colspan=""3""> Выборочный контроль</th> 
<tr> 
                                          <th>Кол-во</th>
                                          <th>Результат</th>
                                          <th>Тип продукции</th>
                                         
                                        </tr>
                                    {KGP}
                                  </table>
</td>
</td>
                        </table>";

            HTML = html;
        }
    }
}
