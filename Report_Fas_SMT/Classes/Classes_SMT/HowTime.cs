using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT.Classes
{
    public class HowTime
    {       
        readonly List<string> ListTimesDay = new List<string>() { "10:00", "12:00", "14:00", "16:00", "18:00", "20:00" };
        readonly List<string> ListTimesNight = new List<string>() { "22:00", "00:00", "02:00", "04:00", "06:00", "08:00" };
        public List<string> TimeList;

        string TypeDay;
        public ModeNightDay mode;        
        bool IsShift;

        public HowTime(bool isSHIFT)
        {
           
            mode = new ModeNightDay();
            TimeList = new List<string>();

            IsShift = isSHIFT;   
            TypeDayGet();
        }

        void TypeDayGet()
        {            
            var NOW = DateTime.UtcNow.AddHours(2).Hour;

            if (NOW >= 8 & NOW <= 20)
            {
                TimeList = ListTimesDay;
                mode.Date1 = DateTime.UtcNow.AddHours(2).ToString("yyyy-MM-dd");
                mode.Date2 = DateTime.UtcNow.AddHours(2).ToString("yyyy-MM-dd");
                if (IsShift)
                {
                    mode.StartTime = "08:00:00";
                    mode.EndTime = "20:00:00";
                }
                else
                {
                    mode.StartTime = "08:00:00";
                    mode.EndTime = "10:00:00";
                }
               
                TypeDay = "Day";
            }
            else
            {
                if (NOW >= 20 & NOW <= 23)
                {
                    if (IsShift)
                    {
                        mode.Date1 = DateTime.UtcNow.AddHours(2).AddDays(0).ToString("yyyy-MM-dd");
                        mode.Date2 = DateTime.UtcNow.AddHours(2).AddDays(1).ToString("yyyy-MM-dd");
                        mode.StartTime = "20:00:00";
                        mode.EndTime = "08:00:00";
                    }
                    else 
                    {
                        mode.Date1 = DateTime.UtcNow.AddHours(2).AddDays(0).ToString("yyyy-MM-dd");
                        mode.Date2 = DateTime.UtcNow.AddHours(2).AddDays(0).ToString("yyyy-MM-dd");
                        mode.StartTime = "20:00:00";
                        mode.EndTime = "22:00:00";
                    }
                                
                }
                else
                {
                    if (IsShift)
                    {
                        mode.Date1 = DateTime.UtcNow.AddHours(2).AddDays(-1).ToString("yyyy-MM-dd");
                        mode.Date2 = DateTime.UtcNow.AddHours(2).AddDays(0).ToString("yyyy-MM-dd");
                        mode.StartTime = "20:00:00";
                        mode.EndTime = "08:00:00";
                    }
                    else
                    {
                        mode.Date1 = DateTime.UtcNow.AddHours(2).AddDays(-1).ToString("yyyy-MM-dd");
                        mode.Date2 = DateTime.UtcNow.AddHours(2).AddDays(-1).ToString("yyyy-MM-dd");
                        mode.StartTime = "20:00:00";
                        mode.EndTime = "22:00:00";
                    }
                }
                
                TypeDay = "Night";
                TimeList = ListTimesNight;
            }
        }

        public void HowTimes(int itter, string line)
        {
            mode.StartTime = DateTime.Parse(mode.StartTime).AddHours(2).ToString("HH:mm:ss");
            mode.EndTime = DateTime.Parse(mode.EndTime).AddHours(2).ToString("HH:mm:ss");

            if (TypeDay == "Day")  
                return;

            if (line == "OrbotecLine"){

                if (itter == 0)
                {
                    mode.Date2 = DateTime.Parse(mode.Date2).AddDays(1).ToString("yyyy-MM-dd");
                    return;
                }

                if (itter == 1)
                {
                    mode.Date1 = DateTime.Parse(mode.Date1).AddDays(1).ToString("yyyy-MM-dd");
                    return;
                }

                return;
            }
            else {

                if (itter == 1)
                {
                    mode.Date2 = DateTime.Parse(mode.Date2).AddDays(1).ToString("yyyy-MM-dd");
                    return;
                }

                if (itter == 2)
                {
                    mode.Date1 = DateTime.Parse(mode.Date1).AddDays(1).ToString("yyyy-MM-dd");
                    return;
                }
            }
                   
           
        }
    }
}
