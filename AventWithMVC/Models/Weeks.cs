using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AventWithMVC.Models
{
    public class Weeks
    {
        public int FirstWeek { get; set; }
        public int SecondWeek { get; set; }
        public int ThirdWeek { get; set; }
        public int FourthWeek { get; set; }
        public string month { get; set; }
        public int year { get; set; }
        public List<string> dates { get; set; }
        public List<int> WeekWiseHours { get; set; }
        public string WeekDates { get; set; }
        public string Startdate{ get; set; }
        public string Enddate { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Please enter numbers only")]
        public double Hours { get; set; }
        public double Avg { get; set; }
        public int Totalhours { get; set; }
        public int Numofleaves { get; set; }
    }
}