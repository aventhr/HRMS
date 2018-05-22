using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AventWithMVC.Models
{
    public class Leave
    {
        public int lid { get; set; }

        public string empname { get; set; }
        
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM-dd-yy}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Choose leave start date")]
        public string Leavestartdate { get; set; }

        //[Required(ErrorMessage = "Choose leave end date")]
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM-dd-yy}", ApplyFormatInEditMode = true)]
        public string Leaveendtdate { get; set; }

        [Required(ErrorMessage = "Choose start session")]
        public int startsession { get; set; }

        [Required(ErrorMessage = "Choose end session")]
        public int endtsession { get; set; }

        [Required(ErrorMessage = "Please write your reporting to")]
        public string Reportingto { get; set; }

        public int Availableleaves { get; set; }


        public string Contactnumber { get; set; }


        public string Reason { get; set; }
        public List<Leave> leaveinfolist { get; set; }

        [Required(ErrorMessage = "Choose leave type")]
        public int ddlleaveid { get; set; }


        public string leavetype { get; set; }
        public int ddlleaveactionid { get; set; }
        public string ddlleaveactionstatus { get; set; }
        public int numberofleaves { get; set; }
        public ICollection<Leavehistory> Leavehistorydetails { get; set; }



        public DateTime date { get; set; }
    }
    public class Leavehistory
    {
        public string empname { get; set; }
        public int Leaveid { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public string Leavetype { get; set; }
        public List<Leavehistory> leavehistorylist { get; set; }
        public string Actions { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Updatedstatusdate { get; set; }
    }

    
}