using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Report_Fas_SMT.Classes;
using Report_Fas_SMT.Reports;
using System.Threading;

namespace Report_Fas_SMT
{
    public partial class Report : Form
    {

        Message mes;
        readonly List<string> SMTTimes = new List<string>() {"00:00:00", "02:00:00", "04:00:00", "06:00:00", "07:58:00", 
            "10:00:00", "12:00:00", "14:00:00","16:00:00","18:00:00","20:00:00","22:00:00"};

        readonly List<string> FasTimes = new List<string>() {"00:00:00", "02:00:00", "04:00:00", "06:00:00", "07:58:00",
            "10:00:00", "12:00:00", "14:00:00","16:00:00","18:00:00","20:00:00","22:00:00"};

        readonly List<string> Listbot = new List<string>() { "bot","BOT","bOT","Bot", "bOt","boT","BOt","ВОТ","ВOT" };
        readonly List<string> Listtop = new List<string>() { "top", "TOP", "tOP", "Top", "tOp", "toP", "TOp", "ТОР", "тор" };

        public Report()
        {
            InitializeComponent();
            timer1.Enabled = false;
            LBStatus.Text = "Отчёт выключен!";
            LBStatus.BackColor = Color.White;

        }

        bool offOn = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (!offOn)
            {
                timer1.Enabled = true;
                LBStatus.Text = "Отчёт запущен!";
                LBStatus.BackColor = Color.LightGreen;
            }
            else
            {
                timer1.Enabled = false;
                LBStatus.Text = "Отчёт выключен!";
                LBStatus.BackColor = Color.White;
            }

            offOn = !offOn;
            
        }

        private void SendEmailSMT(string time)
        {
            SMT smt = new SMT(Grid);
            if (!smt.GetDataSMT())
            {
                return;
            }
            
            smt.RenderHTML();
            mes = new Message();
            mes.Content = smt.HTML;
            Thread.Sleep(1000);
            mes.RunEmail(time);
        }

        private void SendEmailFAS(string time)
        {
            FASNew FAS = new FASNew();
            FAS fas = new FAS();
            if (!FAS.GetReport())
            {
                return;
            }

            FAS.RenderHTML();
            mes = new Message();
            mes.Content = FAS.HTML;
            Thread.Sleep(1000);
            mes.RunEmailFAS(time);

            //FAS FAS = new FAS();
         
            //if (!FAS.GetDataFAS())
            //{
            //    return;
            //}

            //FAS.RenerHTML();
            //mes = new Message();
            //mes.Content = FAS.HTML;
            //Thread.Sleep(1000);
            //mes.RunEmailFAS(time);
        }



        //double GetCell(string model)
        //{
        //    FASEntities fas = new FASEntities();
        //    if (Listbot.Select(c=> model.Contains(c)).FirstOrDefault())
        //    {
        //        var listbots = fas.FAS_Objective.Where(c => c.Prod == "BOT").Select(c => new { cell = c.Cell, mask = c.Mask });
        //        foreach (var item in listbots) {
        //            if (item.mask == "")                   
        //                continue;

        //            foreach (var i in item.mask.Split(';'))                  
        //                if (model.Contains(i))                       
        //                    return (double)item.cell;
        //        }

        //    }            
        //    else
        //    {
        //        var listbots = fas.FAS_Objective.Where(c => c.Prod == "TOP").Select(c => new { cell = c.Cell, mask = c.Mask });
        //        foreach (var item in listbots)
        //        {
        //            if (item.mask == "")
        //                continue;

        //            foreach (var i in item.mask.Split(';'))
        //                if (model.Contains(i))
        //                    return (double)item.cell;
        //        }
        //    }

        //    return 0;
        //}

        //List<TopDefects> GetTopDefects(string model, string line, ModeNightDay mode)
        //{
        //    List<TopDefects> Top = new List<TopDefects>();
        //    if (line == "OrbotecLine")
        //    {

        //    }
        //    else
        //    {
        //        var Mode = ChangeFormatDate(mode);
        //        Database.LoadGridOmron(Grid, $@"SELECT SIX.USR_INSP_RESULT_NAME,COUNT(SIX.USR_INSP_RESULT_NAME) c
        //             FROM PRISM.INSP_RESULT_SUMMARY_INFO one   
        //             LEFT JOIN COMP_RESULT_INFO two ON one.INSP_ID = two.INSP_ID AND one.SYS_MACHINE_NAME = two.SYS_MACHINE_NAME
        //             LEFT JOIN COMP_INFO three ON one.PG_ITEM_ID = three.PG_ITEM_ID AND two.COMP_ID = three.COMP_ID  
        //             LEFT JOIN USR_INSP_RESULT_NAME six ON two.VC_LAST_RESULT_CODE = six.USR_INSP_RESULT_CODE
        //             LEFT JOIN CIR_INFO four ON three.CIR_ID = four.CIR_ID AND  three.PG_ITEM_ID = four.PG_ITEM_ID
        //             Left JOIN PG_INFO mod ON one.PG_ITEM_ID = mod.PG_ITEM_ID
        //             left JOIN CONNECTION_MACHINE_INFO L ON one.SYS_MACHINE_NAME = l.SYS_MACHINE_NAME
        //             WHERE INSP_END_DATE BETWEEN TO_DATE(CONCAT('{Mode.Date1}','{Mode.StartTime}'),'DD.MM.YY HH24:MI:SS') AND TO_DATE(CONCAT('{Mode.Date2}','{Mode.EndTime}'),'DD.MM.YY HH24:MI:SS')              

        //             AND two.VC_LAST_RESULT_CODE <> 0
        //             AND six.LANG_ID = 3 
        //             AND MOD.PG_NAME = '{model}' AND L.USR_MACHINE_NAME ='{line}'                 
        //             GROUP BY SIX.USR_INSP_RESULT_NAME   ORDER BY c DESC ");

        //        Top.AddRange(AddListTopDefects());
        //    }


        //    return Top;
        //}

        //List<TOPError> GetTopError(string model, string line, ModeNightDay mode)
        //{
        //    List<TOPError> Top = new List<TOPError>();
        //    if (line == "OrbotecLine")
        //    {
        //        Database.loadgridOrbotec(Grid,$@"Use Orbo00_0000 SELECT  TOP(3) OrboCdbRecipe.dbo.TblComponent.componentName, COUNT(*) as Qty FROM Orbo00_0000.dbo.TblComponent P
        //     LEFT JOIN OrboCdbRecipe.dbo.TblComponent  ON ProductID=OrboCdbRecipe.dbo.TblComponent.recipeId   Left join [dbo].[TblPanel] as pn on p.PanelID = pn.PanelID
        //      WHERE CompID=OrboCdbRecipe.dbo.TblComponent.componentTan and P.Classification<>0 and p.TestDateTime  between ('{mode.Date1} {mode.StartTime}') and ('{mode.Date2} {mode.EndTime}')
        //        and ProductName = '{model}' GROUP BY OrboCdbRecipe.dbo.TblComponent.componentName ORDER BY Qty Desc");

        //        Top.AddRange(AddListError());
        //    }
        //    else
        //    {
        //        var Mode = ChangeFormatDate(mode);
        //        Database.LoadGridOmron(Grid,$@"SELECT DISTINCT(four.CIR_NAME),   COUNT(four.CIR_NAME) FROM PRISM.INSP_RESULT_SUMMARY_INFO one  LEFT JOIN COMP_RESULT_INFO two ON one.INSP_ID = two.INSP_ID
        //      LEFT JOIN COMP_INFO three ON one.PG_ITEM_ID = three.PG_ITEM_ID AND two.COMP_ID = three.COMP_ID  
        //        LEFT JOIN USR_INSP_RESULT_NAME six ON one.INSP_RESULT_CODE = six.USR_INSP_RESULT_CODE 
        //        LEFT JOIN CIR_INFO four ON three.CIR_ID = four.CIR_ID AND  three.PG_ITEM_ID = four.PG_ITEM_ID
        //        Left JOIN PG_INFO mod ON one.PG_ITEM_ID = mod.PG_ITEM_ID
        //        INNER JOIN CONNECTION_MACHINE_INFO L ON one.SYS_MACHINE_NAME = l.SYS_MACHINE_NAME
        //      WHERE INSP_END_DATE BETWEEN TO_DATE(CONCAT('{Mode.Date1}','{Mode.StartTime}'),'DD.MM.YY HH24:MI:SS') AND TO_DATE(CONCAT('{Mode.Date2}','{Mode.EndTime}'),'DD.MM.YY HH24:MI:SS') 
        //        AND one.INSP_RESULT_CODE<>0 AND one.VC_LAST_RESULT_CODE<>0  AND two.INSP_RESULT_CODE<>0 AND two.VC_LAST_RESULT_CODE<>0 AND six.LANG_ID = 3 
        //        AND MOD.PG_NAME = '{model}' AND L.USR_MACHINE_NAME ='{line}'   GROUP BY four.CIR_NAME   ORDER BY COUNT(four.CIR_NAME) DESC");

        //        Top.AddRange(AddListError());
        //    }


        //    return Top;
        //}

        //List<DataReportOtkazFPY> GetListDatas(string model, string line)
        //{
        //    List<DataReportOtkazFPY> listdatas = new List<DataReportOtkazFPY>();

        //    HowTime how = new HowTime(false);

        //    for (int i = 0; i < 6; i++)
        //    {
        //        DataReportOtkazFPY data = new DataReportOtkazFPY()  
        //        {
        //            Vypusk = GetShiftCvypusk(model,line,how.mode),
        //            Otkaz = GetShiftOtkaz(model, line, how.mode),     
        //        };

        //        how.HowTimes(i,line);
        //        if (data.Vypusk == 0 & data.Otkaz == 0)                
        //            data.FPY = "0";                
        //        else                
        //            data.FPY = (100 - (data.Otkaz / data.Vypusk * 100)).ToString("##.##");

        //        listdatas.Add(data);
        //    }
        //    return listdatas;
        //}

        //List<TopDefects> AddListTopDefects()
        //{
        //    List<TopDefects> list = new List<TopDefects>();
        //    for (int i = 0; i < Grid.RowCount; i++)
        //    {
        //        if (i == 3)
        //            break;

        //        TopDefects data = new TopDefects() { NameDefects = Grid[0, i].Value.ToString(), Count = int.Parse(Grid[1, i].Value.ToString()) };
        //        list.Add(data);
        //    }
        //    return list;
        //}

        //List<TOPError> AddListError()
        //{
        //    List<TOPError> list = new List<TOPError>();
        //    for (int i = 0; i < Grid.RowCount; i++)
        //    {
        //        if (i == 3)
        //            break;

        //        TOPError data = new TOPError() { NamePosition = Grid[0,i].Value.ToString() ,Count = int.Parse(Grid[1,i].Value.ToString())};
        //        list.Add(data);
        //    }
        //    return list;
        //}

        //ModeNightDay ChangeFormatDate(ModeNightDay mode)
        //{
        //    ModeNightDay Mode = new ModeNightDay() { 
        //        Date1 = DateTime.Parse(mode.Date1).ToString("dd.MM.yyyy"),
        //        Date2 = DateTime.Parse(mode.Date2).ToString("dd.MM.yyyy"),
        //        StartTime = DateTime.Parse(mode.StartTime).AddHours(-2).ToString("HH:mm:ss"),
        //        EndTime = DateTime.Parse(mode.EndTime).AddHours(-2).ToString("HH:mm:ss"),
        //    };
        //    return Mode;
        //}

        void GetGridSMT()
        {
            //Database.LoadGridOra(Grid,$@"");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var Now = DateTime.UtcNow.AddHours(2).ToString("HH:mm:ss");            
            if (SMT.Checked)
            {            
                foreach (var item in SMTTimes)
                {
                    if (Now == item)                
                        SendEmailSMT(item);                

                    var nowdate = DateTime.Parse(Now);
                    var itemdate = DateTime.Parse(item);
                    if (nowdate < itemdate)
                    {
                        var result = itemdate - nowdate;
                        LBStatus.BackColor = Color.LightGreen;
                        LBStatus.Text = $"Отчёт запущен!\nДо следующей отправки SMT Карты \nосталось {result}";
                        break;
                    }
                }
            }
            else
            {
                LBStatus.BackColor = Color.White;
                LBStatus.Text = $"Карта SMT отключена";
            }

            if (FAS.Checked)
            {
                foreach (var item in FasTimes)
                {
                    if (Now == item)
                        SendEmailFAS(item);

                    var nowdate = DateTime.Parse(Now);
                    var itemdate = DateTime.Parse(item);
                    if (nowdate < itemdate)
                    {
                        var result = itemdate - nowdate;
                        LBStatusFAS.BackColor = Color.LightGreen;
                        LBStatusFAS.Text = $"Отчёт запущен!\nДо следующей отправки FAS Карты \nосталось {result}";
                        return;
                    }

                }
            }
            else
            {
                LBStatusFAS.BackColor = Color.White;
                LBStatusFAS.Text = $"Карта FAS отключена";
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {  
            SendEmailSMT(DateTime.UtcNow.AddHours(2).ToString("HH:mm:ss"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //FASNew fASNew = new FASNew();
            //fASNew.GetReport();
            //fASNew.RenderHTML();
            SendEmailFAS(DateTime.UtcNow.AddHours(2).ToString("HH:mm:ss"));
        }

        private void Report_Load(object sender, EventArgs e)
        {
            if (true)
            {

                return;
            }


            /*  Код после условия
             
             foreach (var item in FasTimes)
                {
                    if (Now == item)
                        SendEmailFAS(item);

                    var nowdate = DateTime.Parse(Now);
                    var itemdate = DateTime.Parse(item);
                    if (nowdate < itemdate)
                    {
                        var result = itemdate - nowdate;
                        LBStatusFAS.BackColor = Color.LightGreen;
                        LBStatusFAS.Text = $"Отчёт запущен!\nДо следующей отправки FAS Карты \nосталось {result}";
                        return;
                    }

                }
             
             
             
             
             
             */
        }



    }
}
