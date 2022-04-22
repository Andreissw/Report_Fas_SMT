using Report_Fas_SMT.Classes;
using Report_Fas_SMT.Classes.Classes_FAS.Contract;
using Report_Fas_SMT.Classes.Classes_FAS.Others;
using Report_Fas_SMT.Classes.FASNew;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Reports
{
    public class FASNew
    {
        FASEntities1 fas = new FASEntities1();
        SMDCOMPONETSEntities smd = new SMDCOMPONETSEntities();
        public string HTML { get; set; }

        HowTime HowTime { get; set; }

        List<LevelOne> levelOnes;

        List<ModelLines> ModelLiness;

        List<CounterStage> ListStages;

        List<TopRemont> listAllRemonts;

        public bool GetReport()
        {   levelOnes = new List<LevelOne>();
            HowTime = new HowTime();
            ListStages = new List<CounterStage>();
            listAllRemonts = new List<TopRemont>();
            //HowTime.mode.DateSt = HowTime.mode.DateSt.AddDays(-4).AddHours(5); 18.11.2021 20:00:00
            //HowTime.mode.Dateend = HowTime.mode.Dateend.AddDays(-4).AddHours(5); 19.11.2021 08:00:00
            //HowTime.mode.DateSt = DateTime.Parse( "18.11.2021 20:00:00");
            //HowTime.mode.Dateend = DateTime.Parse( "19.11.2021 08:00:00");

            var ListModels = GetModelsLines();

            if (ListModels.Count == 0)          
                return false;
           

            foreach (var item in ListModels)
                levelOnes.Add(GetLevelOne(item));


            var TracModels =  fas.Ct_OperLog.Where(c => c.StepID == 6 & c.LOTID != null).GroupBy(c => c.LOTID).Where(c => c.Count() > 10).Select(c => new 
            {
                Name = fas.Contract_LOT.Where(b => b.ID == c.Key & b.IsActive == true).Select(b => b.FullLOTCode).FirstOrDefault(),
                ID = fas.Contract_LOT.Where(b => b.ID == c.Key & b.IsActive == true).Select(b => b.ID).FirstOrDefault(),

            }).OrderByDescending(c => c.ID).Where(c => c.ID != 0).Take(15).ToList();
       

            foreach (var item in TracModels)
                ListStages.Add(GetTracc(item.ID, item.Name));


            AllRemont allRemont = new AllRemont();
            listAllRemonts = allRemont.GetDataRemonts(allRemont.GetList(HowTime)).OrderByDescending(c=>c.FullLotCode).ThenByDescending(c=>c.Count).ToList();


            return true;
        }

        CounterStage GetTracc(int LOTID, string FULLLOTCODE)
        {
            CounterStage counterStages = new CounterStage()
            {
                NameOrder = FULLLOTCODE,
                ListStages = new List<ListStage>(),
            };

            var result = Connect.Loadgrid($@"use fas select _table.НомерЭтапа,_table.Этап, _table.Результат, count(1) 'Кол-во' from (SELECT  
	                                       (select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser = PCBID) Decode      
	                                      ,(select FullLOTCode from Contract_LOT c where LOTID = c.ID) 'Order'
                                          ,(select sc.StepName from Ct_StepScan sc where sc.ID = op.StepID) Этап
	                                      ,(select sc.NumStep from Ct_StepScan sc where sc.ID = op.StepID) НомерЭтапа
	                                      ,(select Result from Ct_TestResult t where op.TestResultID = t.ID ) Результат      
                                          ,Format([StepDate],'dd.MM.yyyy HH:mm:ss') Дата          
                                          ,ROW_NUMBER() over (partition by pcbid order by stepdate desc) num
                                          FROM [FAS].[dbo].[Ct_OperLog] op
                                          where LOTID = '{LOTID}') as _table
                                          where num = 1
                                          group by _table.НомерЭтапа,_table.Этап, _table.Результат
                                          order by _table.НомерЭтапа");


            if (result.Tables.Count == 0)
            {
                return null;
            }

            foreach (DataRow item in result.Tables[0].Rows)
            {
                counterStages.ListStages.Add(new ListStage()
                {
                    NameStage = item[1].ToString(),
                    Result = item[2].ToString(),
                    Count = int.Parse(item[3].ToString())
                });

                counterStages.SumCount += int.Parse(item[3].ToString());
            }

            return counterStages;
        }

        List<ModelLines> GetModelsLines()
        {
            ModelLiness = new List<ModelLines>();

            ModelLiness.AddRange(fas.Ct_OperLog
                .Where(c => c.StepDate >= HowTime.mode.DateSt & c.StepDate <= HowTime.mode.Dateend & c.LOTID != null & c.StepID == 6)
                .Select(c => new { LOTID = c.LOTID, LineID = c.LineID }).Distinct()
                .Select(c => new ModelLines()
                {
                    Type = "Контрактное",
                    FullLotCode = fas.Contract_LOT.Where(b => b.ID == c.LOTID).FirstOrDefault().FullLOTCode,
                    Line = fas.FAS_Lines.Where(b => b.LineID == c.LineID).FirstOrDefault().LineName,
                    LineID = c.LineID,
                    LOTID = c.LOTID,

                })
                //.Where(c => c.Line.Contains("FAS"))
                .ToList());

           ModelLiness.AddRange(fas.FAS_PackingGS.Where(c => c.PackingDate >= HowTime.mode.DateSt & c.PackingDate <= HowTime.mode.Dateend )
                .Select(c => new
                {
                    LineID = fas.FAS_Liter.Where(b=>b.ID == c.LiterID).FirstOrDefault().LineID,
                    LOTID = fas.FAS_SerialNumbers.Where(x => x.SerialNumber == c.SerialNumber).FirstOrDefault().LOTID
                })
                .Distinct()
                .Select(c => new ModelLines
                {
                    Type = "ВЛВ",
                    Line = fas.FAS_Lines.Where(x=>x.LineID == c.LineID).FirstOrDefault().LineName,
                    FullLotCode = fas.FAS_GS_LOTs.Where(x=>x.LOTID == c.LOTID).FirstOrDefault().FULL_LOT_Code,
                    LineID = (byte)c.LineID,
                    LOTID = c.LOTID,
                }));

            return ModelLiness;
        }

        LevelOne GetLevelOne(ModelLines modelLines)
        {            
            List<GetData> Data = new List<GetData>() { new GS(modelLines), new Contract(modelLines) };
            var ResultData = Data.Where(c => c.Type == modelLines.Type).FirstOrDefault();
            ResultData.HowTime = HowTime;

            var lvl = new LevelOne()
            {                
                NameOrder = modelLines.FullLotCode,
                NameLine = modelLines.Line,
                FPYDatas = ResultData.GetFPYData(),             
                TopDiss = ResultData.GetTopDiss(),
                Logs = ResultData.GetLog(),
                TopRemontOnline = ResultData.GetRemontOnline(),
                TopRemots = ResultData.GetTopRemots(),                
                OTKs = ResultData.GetOTK(),
            };

            return lvl;
        }

        string TOPOneDis(List<TopDis> datas)
        {
           

            string line = "";
            foreach (var item in datas)
            {
                line += $@" <tr>
                                  <td>{item.Name}</td>
                                  <td style=""width:200px"">{item.Count}</td>                              
                                </tr>";
            }

            string Head = $@"  
                            <td>
                              <table>
                                <tr>
                                  <th colspan=""2""> TOP Отказов </th>
                                </tr>
                                <tr>
                                  <th style=""width:200px""> Имя отказа </th>
                                  <th> Кол-во </th>
                                </tr>                            
                                  {line}                             

                              </table>
                            </td>";

            return Head;
        }

        string GetOneFPY(List<FPYData> datas)
        {
            
            
            string line = $@"";
            
            foreach (var item in datas)
            {
                string style = GetStyle(item.FPY.ToString(),item.Objective);
                line += $@" <tr>
                                  <td>{item.Count}</td>
                                  <td>{item.CountDis}</td>
                                  <td {style}>{item.FPY}%</td>
                                  <td>{item.Objective}</td>
                                </tr>";
            }

            string Head = $@"<td>
                              <table>
                                <tr>
                                  <th colspan=""4""> FPY статистика </th>
                                </tr>
                                <tr>
                                  <th> Выпуск </th>
                                  <th> Отказов </th>
                                  <th> FPY </th>
                                    <th> Цель </th>
                                </tr>

                                {line}

                              </table>
                            </td>";

            return Head;
        
        }

        string GetOneRepOnline(List<TopRemont> datas)
        {

            string line = "";
            foreach (var item in datas)
            {
                line += $@" <tr>
                                  <td>{item.PositionName}</td>
                                  <td style=""white-space: normal;"">{item.RepairCode}</td>
                                  <td>{item.NamdeDis}</td>
                                    <td>{item.Status}</td>
                                    <td>{item.Count}</td>
                                </tr>";
            }


            string Head = $@"
                            <td>
                                <div style=""height: 300px; overflow-y:auto"">
                                <table>
                                <tr>
                                  <th colspan=""5""> Топ 7 по ремонту за текущий день в режиме ONLINE  </th>
                                </tr>
                                <tr>
                                  <th> Позиция </th>
                                  <th> КодРемонта </th>
                                  <th> Имя отказа </th>
                                  <th> Статус ремонта </th>
                                  <th> Кол-во</th>

                                </tr>
                                 {line}

                              </table>
                              </div>
                            </td>";
            return Head;
        }

        string GetOneRep(List<TopRemont> datas)
        {

            string style = "";
            string line = "";
            foreach (var item in datas)
            {
                string status = "";
                if (item.Status)
                {
                    status = "ОК";
                    style = @"style = ""background-color: lightgreen;"" ";
                }
                else { 
                    status = "NOK";
                    style = @"style = ""background-color: coral;"" ";
                }

                line += $@"  <tr>
                                  <td>{item.PositionName}</td>
                                  <td>{item.RepairCode}</td>
                                  <td>{item.NamdeDis}</td>
                                    <td {style}>{status}</td>
                                    <td>{item.Count}</td>
                                </tr>                          
";

            }


            string Head = $@"
                            <td>
                               
                                <table>
                                <tr>
                                  <th colspan=""5""> Топ 7 по ремонту в режиме OFFLINE  </th>
                                </tr>
                                <tr>
                                  <th> Позиция </th>
                                  <th> КодРемонта </th>
                                  <th> Имя отказа </th>
                                  <th> Статус ремонта </th>
                                  <th> Кол-во</th>

                                </tr>
                            <div style=""white-space: nowrap ;"" >
                                 {line}
                                </div>
                              </table>
                             
                            </td>";
            return Head;              
        }

        string GetOneLog(List<Log> datas)
        {
        
            string line = "";
            foreach (var item in datas)
            {
                line += $@" <tr>
                                  <td>{item.Stage}</td>
                                  <td>{item.Result}</td>
                                  <td>{item.Count}</td>
                                   
                                </tr>";
            }


            string Head = $@"
                            <td>
                              <table>
                                <tr>
                                  <th colspan=""3""> Лог </th>
                                </tr>
                                <tr>
                                  <th> Этап </th>
                                  <th> Результат </th>
                                  <th> Кол-во </th>         

                                </tr>
                                  {line}

                              </table>
                            </td>";
            return Head;
        }

        string GetOneOTK(List<OTK> datas)
        {
            //if (double.Parse(FPY) >= 98)
            //{
                
            //}
            //else if (double.Parse(FPY) < 98)
            //{
            //    style = @"style = ""background-color: coral;"" ";
            //}
            string line = "";
            foreach (var item in datas)
            {
                string style = "";
                string pass = "";
                if (item.Pass == 1) {     
                    pass = "ОК";
                    style = @"style = ""background-color: lightgreen;"" ";
                }
                else { 
                    pass = "NOK";
                    style = @"style = ""background-color: coral;"" ";
                }

                line += $@" <tr>     
                                    <td>{item.DefectCode}</td>
                                  
                                  <td {style}>{pass}</td>
                                  <td>{item.Count}</td>                                   
                                </tr>";
            }

            string Head = $@" 
                            <td>
                              <table>
                                <tr>
                                  <th colspan=""3""> Выборочный контроль </th>
                                </tr>
                                <tr>   
                                    <th> Дефект </th>
                                  <th> Результат </th>
                                  <th> Кол-во </th>   
                                </tr>
                                {line}

                              </table>
                            </td>
                          ";
            return Head;
        }


        string GetLevelOne()
        {
            string line = "";
            foreach (var item in levelOnes)
            {
                //{ GetOneRepOnline(item.TopRemontOnline)}
                line += $@" <tr> 
                            <td>
                            <div style=""font-size:large; border: 1px dashed black; padding: 9px; background-color: gold;
                                white-space: normal; width: 75px;"">
                               { item.NameOrder+"-"+item.NameLine}
                            </div>
                            </td>
                             
                             {GetOneFPY(item.FPYDatas)} 
                             {TOPOneDis(item.TopDiss)}
                             
                             {GetOneLog(item.Logs)}  
                             {GetOneRep(item.TopRemots)}    
                             {GetOneOTK(item.OTKs)}

                          </tr>";
            }
            return line;
        }
        string GetLevelTwo()
        {
            string line = "";
            string line2 = "";

            foreach (var item in listAllRemonts)
            {
                string style = "";
                string pass = "";
                if (item.Status)
                {
                    pass = "ОК";
                    style = @"style = ""background-color: lightgreen;"" ";
                }
                else
                {
                    pass = "NOK";
                    style = @"style = ""background-color: coral;"" ";
                }

                line += $@"  
                                <tr>
                                  <td>{item.FullLotCode}</td>
                                  <td>{item.PositionName}</td>
                                  <td>{item.RepairCode}</td>      
                                    <td>{item.NamdeDis}</td> 
                                    <td>{item.Status}</td> 
                                    <td>{item.Count}</td> 
                                </tr>";
            }

            line2 += $@"<td>
                               <table style=""font-size: 12px"">
                                <tr>
                                  <th colspan=""6""> ТОП 7 ремонта за текущий день </th>
                                </tr>
                                <tr>
                                  <th> Заказ </th>
                                  <th> Позиция </th>
                                  <th> Код ремонта </th>     
                                 <th> Код отказа </th> 
                                 <th> Статус ремонта </th> 
                                 <th> Кол-во </th> 
                                {line}
                              </table>
      
                            </td>";
            return line2;
        }


        string GetLevelThree()
        {
            string line = "";
            string line2 = "";

            for (int i = 0; i < 3; i++)
            {           
                foreach (var item in ListStages.Take(5))
                {
                    string line3 = $@"";

                    foreach (var item2 in item.ListStages)
                    {
                        line3 += $@"  
                                    <tr>
                                      <td>{item2.NameStage}</td>
                                      <td>{item2.Result}</td>
                                      <td>{item2.Count}</td>       
                                    </tr>";
                    }

                    line2 += $@"<td>
                                   <table>
                                    <tr>
                                      <th colspan=""3""> {item.NameOrder} </th>
                                    </tr>
                                    <tr>
                                      <th> Этап </th>
                                      <th> Результат </th>
                                      <th> Буфер шт. </th>        

                                    </tr>

                                    {line3}
                                    <tr>
                                            <td colspan =3 style = ""text-align:right; background-color: lightblue"">
                                                Всего = {item.SumCount}
                                            </td>
                                        </tr>

                                  </table>
      
                                </td>";
                }

                line += $@"
                          <tr style=""margin-top:5%"">
                          
                            {line2}                            
    
                          </tr>";

                ListStages.RemoveRange(0, 5);
                line2 = "";

            }


            return line;
        }

        string GetStyle(string FPY, double? Objective)
        {
            string style = "";
            if (FPY == "")
            {
                return style;
            }

            if (double.Parse(FPY) >= Objective)
            {
                style = @"style = ""background-color: lightgreen;"" ";
            }
            else if (double.Parse(FPY) < Objective)
            {
                style = @"style = ""background-color: coral;"" ";
            }
            return style;
        }

        public void RenderHTML()
        {
            HTML = $@"
                
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

                                   td {{
                                      
                                      
                                      border: 1px solid gray;   
                                      text-align:center;      
                                      background-color: azure;
                                      margin: 1%;
                                      padding: 1%;
                                    }} 
                                        td table td {{
                                            background - color:white;
                                        }}

                                    .intd td{{
                                      background-color: white;
                                    }}

                                    tr{{
                                          text-align: center;  
                                        }}

                                    td table  th{{
                                      border: 2px dotted gray;
                                      background-color: gold;
                                    }}

                                     table {{
                                         margin - top: 1%;    
                                         margin-left: 0.6%;
                                         padding: 0.3%;
                                         font-size: 14px; 
                                         text-align: center;
                                         border: 0.7 px dashed gray;            
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

                        <div style=""font-size: 14px;"">
                        <table>

                            <tr>
                            <th colspan=""5"" style=""padding:3%; font-size:40px; border-top: 2px solid gray"">
                                  Статистика по заказам в реальном времени
                            </th>
                            </tr>

                          {GetLevelOne()}

                             <tr>
                            <th colspan=""5"" style=""padding:3%; font-size:40px; border-top: 2px solid gray"" >
                                  Выполненный ремонт по всем текущим заказам за текущий день
                            </th>
                            </tr>

                            {GetLevelTwo()}
                          <tr>
                            <th colspan=""5"" style=""padding:3%; font-size:40px; border-top: 2px solid gray"" >
                                  Буфер между этапами Последних 15 заказов
                            </th>
                            </tr>
                            
                          {GetLevelThree()}

                        </table>
                    </div>";
        }

    }
}



  