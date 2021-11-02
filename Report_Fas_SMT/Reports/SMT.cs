using Report_Fas_SMT.Classes.Classes_SMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Report_Fas_SMT.Classes
{

    public  class SMT
    {
         List<string> ListTimes;
         List<DataLineSMT> DataSMT;
         DataGridView Grid;

         readonly List<string> Listbot = new List<string>() { "bot", "BOT", "bOT", "Bot", "bOt", "boT", "BOt", "ВОТ", "ВOT" };

        public string HTML { get; set; }       

        public SMT(DataGridView grid)
        {
            Grid = grid;          
        }

        public bool GetDataSMT() {

            DataSMT = new List<DataLineSMT>();

            HowTime How = new HowTime(true);
            ListTimes = How.TimeList;

            DataSMT = GetCountModel(How.mode);
            

            if (DataSMT.Count == 0)
                return false;

            foreach (var item in DataSMT)
            {
                item.ListDataReport = GetListDatas(item.NameModel, item.NameLine);

                foreach (var b in item.ListDataReport)
                {
                    item.ShiftVypusk += b.Vypusk;
                    item.ShiftOtkaz += b.Otkaz;
                }

                if (item.ShiftVypusk == 0 & item.ShiftOtkaz == 0)
                    item.ShiftFPY = "0";
                else
                    item.ShiftFPY = (100 - (item.ShiftOtkaz / item.ShiftVypusk * 100)).ToString("##.##");

                item.Cell = GetCell(item.NameModel);
                item.ListTop = GetTopError(item.NameModel, item.NameLine, How.mode);
                item.ListTopDefects = GetTopDefects(item.NameModel, item.NameLine, How.mode);
                item.ListTopGroups = GetTopGRoups(item.NameModel,item.NameLine,How.mode);
                item.TopListComp = GetTopComp(item.NameModel, item.NameLine, How.mode);
            }
            return true;
        }

        List<TOPComp> GetTopComp(string model, string line, ModeNightDay mode)
        {
            List<TOPComp> Top = new List<TOPComp>();
            if (line == "OrbotecLine")
            {

            }
            else
            {
                var Mode = ChangeFormatDate(mode);
                Database.LoadGridOmron(Grid, $@"SELECT CNI.COMP_NUMBER_NAME,COUNT(CNI.COMP_NUMBER_NAME) c
                     FROM PRISM.INSP_RESULT_SUMMARY_INFO one   
                     LEFT JOIN COMP_RESULT_INFO two ON one.INSP_ID = two.INSP_ID AND one.SYS_MACHINE_NAME = two.SYS_MACHINE_NAME
                     LEFT JOIN COMP_INFO three ON one.PG_ITEM_ID = three.PG_ITEM_ID AND two.COMP_ID = three.COMP_ID  
                     LEFT JOIN USR_INSP_RESULT_NAME six ON two.VC_LAST_RESULT_CODE = six.USR_INSP_RESULT_CODE
                     LEFT JOIN CIR_INFO four ON three.CIR_ID = four.CIR_ID AND  three.PG_ITEM_ID = four.PG_ITEM_ID
                     Left JOIN PG_INFO mod ON one.PG_ITEM_ID = mod.PG_ITEM_ID
                     left JOIN CONNECTION_MACHINE_INFO L ON one.SYS_MACHINE_NAME = l.SYS_MACHINE_NAME                    
                     LEFT JOIN COMP_NUMBER_INFO cni ON three.COMP_NUMBER_ID = cni.COMP_NUMBER_ID 
                     AND three.COMP_NUMBER_REV = cni.COMP_NUMBER_REV
                     AND three.COMP_TYPE_CODE = cni.COMP_TYPE_CODE
                     LEFT JOIN COMP_NUMBER_GROUP_INFO cngi ON cni.COMP_NUMBER_GROUP_ID = cngi.COMP_NUMBER_GROUP_ID and cni.COMP_NUMBER_GROUP_REV
                    = cngi.COMP_NUMBER_GROUP_REV AND cni.COMP_TYPE_CODE = cngi.COMP_TYPE_CODE

                     WHERE INSP_END_DATE BETWEEN TO_DATE(CONCAT('{Mode.Date1}','{Mode.StartTime}'),'DD.MM.YY HH24:MI:SS') AND TO_DATE(CONCAT('{Mode.Date2}','{Mode.EndTime}'),'DD.MM.YY HH24:MI:SS')              

                     AND two.VC_LAST_RESULT_CODE <> 0
                     AND six.LANG_ID = 3 
                     AND MOD.PG_NAME = '{model}' AND L.USR_MACHINE_NAME ='{line}'                 
                     GROUP BY CNI.COMP_NUMBER_NAME   ORDER BY c DESC ");

                Top.AddRange(AddListTopComp());
            }


            return Top;
        }

        List<TopGroups> GetTopGRoups(string model, string line, ModeNightDay mode)
        {
            List<TopGroups> Top = new List<TopGroups>();
            if (line == "OrbotecLine")
            {

            }
            else
            {
                var Mode = ChangeFormatDate(mode);
                Database.LoadGridOmron(Grid, $@"SELECT cngi.COMP_NUMBER_GROUP_NAME,COUNT(cngi.COMP_NUMBER_GROUP_NAME) c
                     FROM PRISM.INSP_RESULT_SUMMARY_INFO one   
                     LEFT JOIN COMP_RESULT_INFO two ON one.INSP_ID = two.INSP_ID AND one.SYS_MACHINE_NAME = two.SYS_MACHINE_NAME
                     LEFT JOIN COMP_INFO three ON one.PG_ITEM_ID = three.PG_ITEM_ID AND two.COMP_ID = three.COMP_ID  
                     LEFT JOIN USR_INSP_RESULT_NAME six ON two.VC_LAST_RESULT_CODE = six.USR_INSP_RESULT_CODE
                     LEFT JOIN CIR_INFO four ON three.CIR_ID = four.CIR_ID AND  three.PG_ITEM_ID = four.PG_ITEM_ID
                     Left JOIN PG_INFO mod ON one.PG_ITEM_ID = mod.PG_ITEM_ID
                     left JOIN CONNECTION_MACHINE_INFO L ON one.SYS_MACHINE_NAME = l.SYS_MACHINE_NAME                    
                     LEFT JOIN COMP_NUMBER_INFO cni ON three.COMP_NUMBER_ID = cni.COMP_NUMBER_ID 
                     AND three.COMP_NUMBER_REV = cni.COMP_NUMBER_REV
                     AND three.COMP_TYPE_CODE = cni.COMP_TYPE_CODE
                     LEFT JOIN COMP_NUMBER_GROUP_INFO cngi ON cni.COMP_NUMBER_GROUP_ID = cngi.COMP_NUMBER_GROUP_ID and cni.COMP_NUMBER_GROUP_REV
                    = cngi.COMP_NUMBER_GROUP_REV AND cni.COMP_TYPE_CODE = cngi.COMP_TYPE_CODE

                     WHERE INSP_END_DATE BETWEEN TO_DATE(CONCAT('{Mode.Date1}','{Mode.StartTime}'),'DD.MM.YY HH24:MI:SS') AND TO_DATE(CONCAT('{Mode.Date2}','{Mode.EndTime}'),'DD.MM.YY HH24:MI:SS')              

                     AND two.VC_LAST_RESULT_CODE <> 0
                     AND six.LANG_ID = 3 
                     AND MOD.PG_NAME = '{model}' AND L.USR_MACHINE_NAME ='{line}'                 
                     GROUP BY cngi.COMP_NUMBER_GROUP_NAME   ORDER BY c DESC ");

                Top.AddRange(AddListTopGRoups());
            }


            return Top;
        }

        List<TopDefects> GetTopDefects(string model, string line, ModeNightDay mode)
        {
            List<TopDefects> Top = new List<TopDefects>();
            if (line == "OrbotecLine")
            {

            }
            else
            {
                var Mode = ChangeFormatDate(mode);
                Database.LoadGridOmron(Grid, $@"SELECT SIX.USR_INSP_RESULT_NAME,COUNT(SIX.USR_INSP_RESULT_NAME) c
                     FROM PRISM.INSP_RESULT_SUMMARY_INFO one   
                     LEFT JOIN COMP_RESULT_INFO two ON one.INSP_ID = two.INSP_ID AND one.SYS_MACHINE_NAME = two.SYS_MACHINE_NAME
                     LEFT JOIN COMP_INFO three ON one.PG_ITEM_ID = three.PG_ITEM_ID AND two.COMP_ID = three.COMP_ID  
                     LEFT JOIN USR_INSP_RESULT_NAME six ON two.VC_LAST_RESULT_CODE = six.USR_INSP_RESULT_CODE
                     LEFT JOIN CIR_INFO four ON three.CIR_ID = four.CIR_ID AND  three.PG_ITEM_ID = four.PG_ITEM_ID
                     Left JOIN PG_INFO mod ON one.PG_ITEM_ID = mod.PG_ITEM_ID
                     left JOIN CONNECTION_MACHINE_INFO L ON one.SYS_MACHINE_NAME = l.SYS_MACHINE_NAME
                     WHERE INSP_END_DATE BETWEEN TO_DATE(CONCAT('{Mode.Date1}','{Mode.StartTime}'),'DD.MM.YY HH24:MI:SS') AND TO_DATE(CONCAT('{Mode.Date2}','{Mode.EndTime}'),'DD.MM.YY HH24:MI:SS')              

                     AND two.VC_LAST_RESULT_CODE <> 0
                     AND six.LANG_ID = 3 
                     AND MOD.PG_NAME = '{model}' AND L.USR_MACHINE_NAME ='{line}'                 
                     GROUP BY SIX.USR_INSP_RESULT_NAME   ORDER BY c DESC ");

                Top.AddRange(AddListTopDefects());
            }


            return Top;
        }
        ModeNightDay ChangeFormatDate(ModeNightDay mode)
        {
            ModeNightDay Mode = new ModeNightDay()
            {
                Date1 = DateTime.Parse(mode.Date1).ToString("dd.MM.yyyy"),
                Date2 = DateTime.Parse(mode.Date2).ToString("dd.MM.yyyy"),
                StartTime = DateTime.Parse(mode.StartTime).AddHours(-2).ToString("HH:mm:ss"),
                EndTime = DateTime.Parse(mode.EndTime).AddHours(-2).ToString("HH:mm:ss"),
            };
            return Mode;
        }

        List<TOPComp> AddListTopComp()
        {
            List<TOPComp> list = new List<TOPComp>();
            for (int i = 0; i < Grid.RowCount; i++)
            {
                if (i == 3)
                    break;

                TOPComp data = new TOPComp() { NameComp = Grid[0, i].Value.ToString(), Count = int.Parse(Grid[1, i].Value.ToString()) };
                list.Add(data);
            }
            return list;
        }

        List<TopGroups> AddListTopGRoups()
        {
            List<TopGroups> list = new List<TopGroups>();
            for (int i = 0; i < Grid.RowCount; i++)
            {
                if (i == 3)
                    break;

                TopGroups data = new TopGroups() { NameGroup = Grid[0, i].Value.ToString(), Count = int.Parse(Grid[1, i].Value.ToString()) };
                list.Add(data);
            }
            return list;
        }

        List<TopDefects> AddListTopDefects()
        {
            List<TopDefects> list = new List<TopDefects>();
            for (int i = 0; i < Grid.RowCount; i++)
            {
                if (i == 3)
                    break;

                TopDefects data = new TopDefects() { NameDefects = Grid[0, i].Value.ToString(), Count = int.Parse(Grid[1, i].Value.ToString()) };
                list.Add(data);
            }
            return list;
        }

        List<TOPError> GetTopError(string model, string line, ModeNightDay mode)
        {
            List<TOPError> Top = new List<TOPError>();
            if (line == "OrbotecLine")
            {
                Database.loadgridOrbotec(Grid, $@"Use Orbo00_0000 SELECT  TOP(3) OrboCdbRecipe.dbo.TblComponent.componentName, COUNT(*) as Qty FROM Orbo00_0000.dbo.TblComponent P
             LEFT JOIN OrboCdbRecipe.dbo.TblComponent  ON ProductID=OrboCdbRecipe.dbo.TblComponent.recipeId   Left join [dbo].[TblPanel] as pn on p.PanelID = pn.PanelID
              WHERE CompID=OrboCdbRecipe.dbo.TblComponent.componentTan and P.Classification<>0 and p.TestDateTime  between ('{mode.Date1} {mode.StartTime}') and ('{mode.Date2} {mode.EndTime}')
                and ProductName = '{model}' GROUP BY OrboCdbRecipe.dbo.TblComponent.componentName ORDER BY Qty Desc");

                Top.AddRange(AddListError());
            }
            else
            {
                var Mode = ChangeFormatDate(mode);
                Database.LoadGridOmron(Grid, $@"SELECT DISTINCT(four.CIR_NAME),   COUNT(four.CIR_NAME) FROM PRISM.INSP_RESULT_SUMMARY_INFO one  LEFT JOIN COMP_RESULT_INFO two ON one.INSP_ID = two.INSP_ID
              LEFT JOIN COMP_INFO three ON one.PG_ITEM_ID = three.PG_ITEM_ID AND two.COMP_ID = three.COMP_ID  
                LEFT JOIN USR_INSP_RESULT_NAME six ON one.INSP_RESULT_CODE = six.USR_INSP_RESULT_CODE 
                LEFT JOIN CIR_INFO four ON three.CIR_ID = four.CIR_ID AND  three.PG_ITEM_ID = four.PG_ITEM_ID
                Left JOIN PG_INFO mod ON one.PG_ITEM_ID = mod.PG_ITEM_ID
                INNER JOIN CONNECTION_MACHINE_INFO L ON one.SYS_MACHINE_NAME = l.SYS_MACHINE_NAME
              WHERE INSP_END_DATE BETWEEN TO_DATE(CONCAT('{Mode.Date1}','{Mode.StartTime}'),'DD.MM.YY HH24:MI:SS') AND TO_DATE(CONCAT('{Mode.Date2}','{Mode.EndTime}'),'DD.MM.YY HH24:MI:SS') 
                AND one.INSP_RESULT_CODE<>0 AND one.VC_LAST_RESULT_CODE<>0  AND two.INSP_RESULT_CODE<>0 AND two.VC_LAST_RESULT_CODE<>0 AND six.LANG_ID = 3 
                AND MOD.PG_NAME = '{model}' AND L.USR_MACHINE_NAME ='{line}'   GROUP BY four.CIR_NAME   ORDER BY COUNT(four.CIR_NAME) DESC");

                Top.AddRange(AddListError());
            }


            return Top;
        }

        List<TOPError> AddListError()
        {
            List<TOPError> list = new List<TOPError>();
            for (int i = 0; i < Grid.RowCount; i++)
            {
                if (i == 3)
                    break;

                TOPError data = new TOPError() { NamePosition = Grid[0, i].Value.ToString(), Count = int.Parse(Grid[1, i].Value.ToString()) };
                list.Add(data);
            }
            return list;
        }

        double GetCell(string model)
        {
            FASEntities fas = new FASEntities();
            if (Listbot.Select(c => model.Contains(c)).FirstOrDefault())
            {
                var listbots = fas.FAS_Objective.Where(c => c.Prod == "BOT").Select(c => new { cell = c.Cell, mask = c.Mask });
                foreach (var item in listbots)
                {
                    if (item.mask == "")
                        continue;

                    foreach (var i in item.mask.Split(';'))
                        if (model.Contains(i))
                            return (double)item.cell;
                }

            }
            else
            {
                var listbots = fas.FAS_Objective.Where(c => c.Prod == "TOP").Select(c => new { cell = c.Cell, mask = c.Mask });
                foreach (var item in listbots)
                {
                    if (item.mask == "")
                        continue;

                    foreach (var i in item.mask.Split(';'))
                        if (model.Contains(i))
                            return (double)item.cell;
                }
            }

            return 0;
        }

        List<DataReportOtkazFPY> GetListDatas(string model, string line)
        {
            List<DataReportOtkazFPY> listdatas = new List<DataReportOtkazFPY>();

            HowTime how = new HowTime(false);

            for (int i = 0; i < 6; i++)
            {
                DataReportOtkazFPY data = new DataReportOtkazFPY()
                {
                    Vypusk = GetShiftCvypusk(model, line, how.mode),
                    Otkaz = GetShiftOtkaz(model, line, how.mode),
                };

                how.HowTimes(i, line);
                if (data.Vypusk == 0 & data.Otkaz == 0)
                    data.FPY = "0";
                else
                    data.FPY = (100 - (data.Otkaz / data.Vypusk * 100)).ToString("##.##");

                listdatas.Add(data);
            }
            return listdatas;
        }

        float GetShiftOtkaz(string model, string line, ModeNightDay mode)
        {
            float count = 0;
            if (line == "OrbotecLine")
            {
                var Result = Database.SelectStringIntOrbotec($@"SELECT count (ReferenceBarCode) as Выпуск FROM [Orbo00_0000].[dbo].[TblPanel]  
                    where TestDateTime between  ('{mode.Date1}  {mode.StartTime}') and ('{mode.Date2}  {mode.EndTime}') 
                  and ProductName = '{model}'  and FailedBoards = 1  and ClassifiedDefects != 0");
                count = float.Parse(Result.ToString());
            }
            else
            {
                var Mode = ChangeFormatDate(mode);
                var Result = Database.SelectStringOmronInteger($@"SELECT count(1)  FROM PRISM.INSP_RESULT_SUMMARY_INFO 
                    INNER JOIN SEG_RESULT_INFO sri ON INSP_RESULT_SUMMARY_INFO.INSP_ID = sri.INSP_ID AND INSP_RESULT_SUMMARY_INFO.SYS_MACHINE_NAME = sri.SYS_MACHINE_NAME
                    INNER JOIN PG_INFO mod ON INSP_RESULT_SUMMARY_INFO.PG_ITEM_ID = mod.PG_ITEM_ID INNER JOIN CONNECTION_MACHINE_INFO L ON INSP_RESULT_SUMMARY_INFO.SYS_MACHINE_NAME = l.SYS_MACHINE_NAME
                   WHERE INSP_END_DATE   BETWEEN TO_DATE(CONCAT('{Mode.Date1}',' {Mode.StartTime}'),'DD.MM.YY HH24:MI:SS') AND TO_DATE(CONCAT('{Mode.Date2}',' {Mode.EndTime}'),'DD.MM.YY HH24:MI:SS') 
                    and MOD.pg_name = '{model}' and L.USR_MACHINE_NAME = '{line}'  AND SRI.INSP_RESULT_CODE <> 0 AND SRI.VC_LAST_RESULT_CODE <> 0 ");
                count = float.Parse(Result.ToString());

            }
            return count;

        }


        float GetShiftCvypusk(string model, string line, ModeNightDay mode)
        {
            float count = 0;
            if (line == "OrbotecLine")
            {
                var Result = Database.SelectStringIntOrbotec($@"use [Orbo00_0000] select  count(1)
                    FROM [Orbo00_0000].[dbo].[TblPanel]
                     where TestDateTime between  ('{mode.Date1} {mode.StartTime}') and ('{mode.Date2} {mode.EndTime}') and ProductName = '{model}'");
                count = float.Parse(Result.ToString());
            }
            else
            {
                var Mode = ChangeFormatDate(mode);
                var Result = Database.SelectStringOmronInteger($@"SELECT count(1)  FROM PRISM.INSP_RESULT_SUMMARY_INFO 
                    INNER JOIN SEG_RESULT_INFO sri ON INSP_RESULT_SUMMARY_INFO.INSP_ID = sri.INSP_ID AND INSP_RESULT_SUMMARY_INFO.SYS_MACHINE_NAME = sri.SYS_MACHINE_NAME
                    INNER JOIN PG_INFO mod ON INSP_RESULT_SUMMARY_INFO.PG_ITEM_ID = mod.PG_ITEM_ID INNER JOIN CONNECTION_MACHINE_INFO L ON INSP_RESULT_SUMMARY_INFO.SYS_MACHINE_NAME = l.SYS_MACHINE_NAME
                   WHERE INSP_END_DATE   BETWEEN TO_DATE(CONCAT('{Mode.Date1}',' {Mode.StartTime}'),'DD.MM.YY HH24:MI:SS') AND TO_DATE(CONCAT('{Mode.Date2}',' {Mode.EndTime}'),'DD.MM.YY HH24:MI:SS') and MOD.pg_name = '{model}' and L.USR_MACHINE_NAME = '{line}'");
                count = float.Parse(Result.ToString());
            }
            return count;

        }


        List<DataLineSMT> AddListForGrid()
        {
            List<DataLineSMT> list = new List<DataLineSMT>();
            for (int i = 0; i < Grid.RowCount; i++)
            {
                DataLineSMT data = new DataLineSMT() { NameModel = Grid[0, i].Value.ToString(), NameLine = Grid[1, i].Value.ToString() };
                list.Add(data);
            }
            return list;
        }

        List<DataLineSMT> GetCountModel(ModeNightDay mode)
        {
            Grid.DataSource = null;
            List<DataLineSMT> ModelList = new List<DataLineSMT>();
            Database.loadgridOrbotec(Grid, $@"use [Orbo00_0000] select  distinct(ProductName), 'OrbotecLine'
                    FROM [Orbo00_0000].[dbo].[TblPanel]
                     where TestDateTime between  ('{mode.Date1} {mode.StartTime}') and ('{mode.Date2} {mode.EndTime}')");

            ModelList.AddRange(AddListForGrid());
            var Mode = ChangeFormatDate(mode);

            Database.LoadGridOmron(Grid, $@"SELECT distinct MOD.pg_name, L.USR_MACHINE_NAME  FROM PRISM.INSP_RESULT_SUMMARY_INFO 
                    INNER JOIN SEG_RESULT_INFO sri ON INSP_RESULT_SUMMARY_INFO.INSP_ID = sri.INSP_ID AND INSP_RESULT_SUMMARY_INFO.SYS_MACHINE_NAME = sri.SYS_MACHINE_NAME
                    INNER JOIN PG_INFO mod ON INSP_RESULT_SUMMARY_INFO.PG_ITEM_ID = mod.PG_ITEM_ID INNER JOIN CONNECTION_MACHINE_INFO L ON INSP_RESULT_SUMMARY_INFO.SYS_MACHINE_NAME = l.SYS_MACHINE_NAME
                   WHERE INSP_END_DATE   BETWEEN TO_DATE(CONCAT('{Mode.Date1}',' {Mode.StartTime}'),'DD.MM.YY HH24:MI:SS') AND TO_DATE(CONCAT('{Mode.Date2}',' {Mode.EndTime}'),'DD.MM.YY HH24:MI:SS')");

            ModelList.AddRange(AddListForGrid());
            return ModelList;
            //return int.Parse(oraCount.ToString());
        }

        string GetStyle(string FPY, double cell)
        {
            string style = "";

            if (FPY == "")
            {
                return style;
            }

            if (double.Parse(FPY) == 0)
            {
                style = @"style = ""background-color: white;""";
            }
            else if (double.Parse(FPY) >= cell)
            {
                style = @"style = ""background-color: lightgreen;""";
            }
            else if (double.Parse(FPY) < cell)
            {
                style = @"style = ""background-color: coral;""";
            }
            return style;
        }

        string GetVipyskData(float vypusk, float otkaz, string FPY, double cell)
        {
            var style = GetStyle(FPY,cell);

            return $@"
               <td> <!-- Данные столбец 1  -->
                   <table>
                      <tr><td class = ""Vyp"">{vypusk}</td></tr>
                      <tr><td class = ""Otkaz"">{otkaz}</td></tr>
                      <tr><td {style}>{FPY}%</td></tr>
                   </table>
               </td>";
        }

        string GetTop(TOPError Top)
        {           
            return $@"<tr><td>{Top.NamePosition}</td> <td>{Top.Count}</td> </tr>";
        }

        string GetDefectTops(TopDefects Top)
        {         
            return $@"<tr><td>{Top.NameDefects}</td> <td>{Top.Count}</td></tr>";
        }

        string GetGroups(TopGroups Top)
        {
            return $@"<tr><td>{Top.NameGroup}</td> <td>{Top.Count}</td></tr>";
        }

        string GetComp(TOPComp Top)
        {
            return $@"<tr><td>{Top.NameComp}</td> <td>{Top.Count}</td></tr>";
        }

        string GetRow(string Line,string Model, float ShiftVypusk, float shiftOtkaz, string shiftFPY ,double cell, List<TOPError> TOP, List<TopDefects> TopDefectsList, List<TopGroups> TopListGroups ,List<TOPComp> Topcomps, string GetVipyskData)
        {
            var style = GetStyle(shiftFPY, cell);
            string TOPs = "";
            string TopDefects = "";
            string TopGroups = "";
            string TopComps = "";
            foreach (var item in TOP)        
                TOPs += GetTop(item);
          

            foreach (var item in TopDefectsList)          
                TopDefects += GetDefectTops(item);

            foreach (var item in Topcomps)         
                TopComps += GetComp(item);
            
            

            foreach (var item in TopListGroups)          
                TopGroups += GetGroups(item);
          

            return $@"<tr> <!-- Строка данных первая -->
               <td class = ""modelfont"">{Line}</td> 
               <td class = ""modelfont"">{Model}</td>
               <td> <!-- Инфо  -->
                   <table>
                      <tr><td class = ""Vyp"">Выпуск</td></tr>
                      <tr><td class = ""Otkaz"">Отказов</td></tr>
                      <tr><td>FPY</td></tr>
                   </table>
               </td>
                   {GetVipyskData}
                   <td>
                   <table> <!-- Данные За смену столбец 7  -->
                      <tr><td class = ""Vyp"">{ShiftVypusk}</td></tr>
                      <tr><td class = ""Otkaz"">{shiftOtkaz}</td></tr>
                      <tr><td {style}>{shiftFPY}%</td></tr>
                   </table>
                 </td>       
                 <td class = ""cell""> <!-- ЦЕЛЬ -->
                    {cell}%                         
                 </td>  
                   <td> <!-- Топ отказов -->
                    <table> 
                      <tr>
                          <th>Дефект</th>
                          <th>Кол-во</th>                  
                      </tr>     
                      {TOPs}
                   </table>
                 </td>  
                <td> <!-- Топ Дефектов -->
                     <table> 
                      <tr>
                          <th>Дефект</th>
                          <th>Кол-во</th>                  
                      </tr>     
                      {TopDefects}
                   </table>
                 </td>    
                <td> <!-- Топ Components -->
                     <table> 
                      <tr>
                          <th>Comp Number</th>
                          <th>Кол-во</th>                  
                      </tr>     
                      {TopComps}
                   </table>
                 </td>    
                <td> <!-- Топ Корпусов -->
                     <table> 
                      <tr>
                          <th>Корпус</th>
                          <th>Кол-во</th>                  
                      </tr>     
                      {TopGroups}
                   </table>
                 </td> 
        </tr>";
        }
        public void RenderHTML() 
        {
            string time = "";
            string Row = "";

            foreach (var item in ListTimes)           
                time += $@"<th>{item}</th>";

            foreach (var item in DataSMT)
            {
                string DataReportOtkazFPY = "";
                foreach (var i in item.ListDataReport)               
                    DataReportOtkazFPY += GetVipyskData(i.Vypusk,i.Otkaz,i.FPY,item.Cell);
               
                Row += GetRow(item.NameLine,item.NameModel,item.ShiftVypusk,item.ShiftOtkaz,item.ShiftFPY,item.Cell,item.ListTop, item.ListTopDefects, item.ListTopGroups, item.TopListComp,DataReportOtkazFPY);
            }       

            var html = $@"
                      <style type=""text/css"">
                        body{{
                             background-color: antiquewhite;
                        }}

                        .cell{{
                            background-color: lightskyblue;
                        }}
                        
                        .modelfont{{
                            font-size: 15px; 
                        }}   

                        th{{
                          padding: 2px;  
                          border: 1px solid gray;
                          background-color: White;  
                          padding: 10px;
                        }}
                        
                        .Vyp{{  
                          background-color:greenyellow;
                        }}
                        
                        .Otkaz {{
                          
                          background-color:white;
                        }}
                        
                        td{{
                          border: 1px solid gray;   
                          text-align:center;
                          background-color: azure;
                          
                        }}
                        
                        tr{{
                          text-align:center;
                          
                        }}
                        
                        table {{  
                           margin-top: 1%;  
                           font-size: 18px; 
                           text-align:center;
                        }}                        
                          
                 </style>
Добрый день!
</br>
</br>
</br>
</br>
 <div style = ""display=""flex""> 
  <div style = "" font-size: 30px;   
                           padding: 10px;   
                           background-color: khaki;       
                          border: 1px solid gray; "" > Дата: { DateTime.UtcNow.AddHours(2).ToString("dd.MM.yyyy")}
                          Час { DateTime.UtcNow.AddHours(2).ToString("HH:mm:ss")}  <div style = ""margin-left:20%;
                          font-size: 30px;  
                          color: gray;""> SMT карта карта контроля </div> </div>
  
</div>
               
<div>
   <table>
       <tr> <!-- Заголовки таблицы -->
           <th>Линия</th>
           <th>Модель</th>
           <th></th>
           {time}
           <th>За смену</th>
           <th class = ""cell"">Цель</th>
           <th>Топ Отказов</th>
           <th>Топ Дефектов</th>
           <th>Топ Comp Number</th>
           <th>Топ корпуса</th>
       </tr>
        {Row}
   </table>            
</div>  ";
            HTML = html;
    }


       
     }    
}
