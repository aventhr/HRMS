using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AventWithMVC.Models
{
    public class TotalLeaveDetails
    {
        public string Openingbalance { get; set; }
        public string appliedleaves { get; set; }
        public string grantedleaves { get; set; }
        public string leavetype { get; set; }

        public string TotalCasualLeaves { get; set; }
        public string TotalBereavementLeaves { get; set; }
        public string TodatlPaternityLeave { get; set; }
        public string TotalLOP { get; set; }
        public ICollection<MonthlyLeaves> MonthlyWiseLeaves { get; set; }
    }

    public class MonthlyLeaves
    {
        public string Month { get; set; }
        public int NumberOfLeaves { get; set; }
        public int EmpId { get; set; }
    }
}