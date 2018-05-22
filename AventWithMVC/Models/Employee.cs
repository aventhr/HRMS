using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AventWithMVC.Models
{
    public class Employee
    {
        //public int Empid { get; set; }
        public string Empname { get; set; }
        public string Empfathername { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }
        public string EmpPAN { get; set; }
        public int Empaadhar { get; set; }
        public string Permanentaddrs { get; set; }
        public string Presentaddrs { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EmpDOJ { get; set; }
        public int Empjobtype { get; set; }
        public int EmpProbationtype { get; set; }
        public string Empjobrole { get; set; }
        public string Empqualification { get; set; }
        public string Email { get; set; }
        public int Empcontactnumber { get; set; }
        public bool EmpGender { get; set; }
        public string EmpDesignation { get; set; }
        public string EmpCode { get; set; }
        public string EmpRegistrationPassword { get; set; }
        public int EmpDepartmenttype { get; set; }
        public string Encryptionpassword { get; set; }
    }
}