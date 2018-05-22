using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AventWithMVC.Models
{
    public class Myclass
    {
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM-dd-yy}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Choose leave start date")]
        public string date { get; set; }
    }
}