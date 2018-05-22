using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AventWithMVC.Models
{
    public class Timesheet
    {
        public string time { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
    }
}