using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AventWithMVC.Models
{
    public class Dropdown
    {
        public int ddlvalue { get; set; }
        public string ddltext { get; set; }
        public List<Dropdown> ddllist { get; set; }
    }
}