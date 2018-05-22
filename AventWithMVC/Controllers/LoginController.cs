using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using AventWithMVC.Models;
using System.Data;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web.Security;

namespace AventWithMVC.Controllers
{
    public class LoginController : Controller
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString;


        [HttpGet]
        public ActionResult IndexMethod()
        {
            if (Session["Roleid"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Home()
        {
            Login Objlogin = new Login();
            Objlogin.Username = Convert.ToString(Session["Empcode"]);
            Objlogin.password = Convert.ToString(Session["Password"]);
            TotalLeaveDetails obj = new TotalLeaveDetails();
            //DataSet ds = (DataSet)Session["Employees"];
            DataSet ds = getuserandleavedetails(Objlogin);
            DataSet leavesds = GetMonthlyLeaves();
            Session["Roleid"] = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpDepartmenttype"]);
            Session["Username"] = (ds.Tables[0].Rows[0]["Empname"]).ToString();
            List<TotalLeaveDetails> leavelist = new List<TotalLeaveDetails>();
            List<MonthlyLeaves> monthlylist = new List<MonthlyLeaves>();
            if (leavesds.Tables[0].Rows.Count >= 0)
            {
                for (int x = 0; x <= leavesds.Tables[0].Rows.Count - 1 && x <= leavesds.Tables[0].Rows.Count; x++)
                {
                    MonthlyLeaves objmonthwiseleave = new MonthlyLeaves();
                    if (!string.IsNullOrEmpty(Convert.ToString(leavesds.Tables[0].Rows[x]["numberofleaves"])))
                    {
                        objmonthwiseleave.NumberOfLeaves = Convert.ToInt32(leavesds.Tables[0].Rows[x]["numberofleaves"]);
                        objmonthwiseleave.Month = Convert.ToString(leavesds.Tables[0].Rows[x]["month"]);
                    }
                    monthlylist.Add(objmonthwiseleave);
                    ViewBag.Monthwiseleave = monthlylist;
                }
            }
                if (ds.Tables[1].Rows.Count >= 0 && ds.Tables[2].Rows.Count >= 0)
            {

                //ObjLeaves.TotalCasualLeaves = Convert.ToString(ds.Tables[2].Rows[0]["TotalCasualLeaves"]);
                //ObjLeaves.TotalBereavementLeaves = Convert.ToString(ds.Tables[2].Rows[0]["TotalBereavementLeaves"]);
                //ObjLeaves.TodatlPaternityLeave = Convert.ToString(ds.Tables[2].Rows[0]["TodatlPaternityLeave"]);
                //ObjLeaves.TotalLOP = Convert.ToString(ds.Tables[2].Rows[0]["TotalLOP"]);
                for (int x = 0; x <= ds.Tables[1].Rows.Count-1 && x<= ds.Tables[1].Rows.Count; x++)
                {
                    TotalLeaveDetails ObjLeaves = new TotalLeaveDetails();
                    ObjLeaves.TotalCasualLeaves = Convert.ToString(ds.Tables[2].Rows[0]["TotalCasualLeaves"]);
                    ObjLeaves.TotalBereavementLeaves = Convert.ToString(ds.Tables[2].Rows[0]["TotalBereavementLeaves"]);
                    ObjLeaves.TodatlPaternityLeave = Convert.ToString(ds.Tables[2].Rows[0]["TodatlPaternityLeave"]);
                    ObjLeaves.TotalLOP = Convert.ToString(ds.Tables[2].Rows[0]["TotalLOP"]);
                    if (ds.Tables[1]!=null&& ds.Tables[1].Rows.Count > 0
                        && !string.IsNullOrEmpty(ds.Tables[1].Columns["Openingbalance"].ToString())
                        || !string.IsNullOrEmpty(ds.Tables[1].Rows[x]["Openingbalance"].ToString()))
                    { 
                        ObjLeaves.Openingbalance = Convert.ToString(ds.Tables[1].Rows[x]["Openingbalance"]);
                    }
                    else
                    {
                        ObjLeaves.Openingbalance = "N/A";
                    }
                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[1].Columns["grantedleaves"].ToString()))
                    {
                        ObjLeaves.grantedleaves = Convert.ToString(ds.Tables[1].Rows[x]["grantedleaves"]);
                    }
                    else
                    {
                        ObjLeaves.grantedleaves = "N/A";
                    }
                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[1].Columns["appliedleaves"].ToString()))
                    {
                        ObjLeaves.appliedleaves = Convert.ToString(ds.Tables[1].Rows[x]["appliedleaves"]);
                    }
                    else
                    {
                        ObjLeaves.appliedleaves = "N/A";
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[1].Columns["leavetype"].ToString()))
                    {
                        ObjLeaves.leavetype = Convert.ToString(ds.Tables[1].Rows[x]["leavetype"]);
                    }
                    else
                    {
                        ObjLeaves.leavetype = "N/A";
                    }
                    //ObjLeaves.leavetype = Convert.ToString(ds.Tables[1].Rows[x]["leavetype"]);
                    ObjLeaves.MonthlyWiseLeaves = monthlylist;
                    leavelist.Add(ObjLeaves);
                }

            }
            List<Events> Objeventlist = new List<Events>();
            for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
            {
                Events Objevent = new Events();
                if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[3].Rows[i]["startdate"])))
                {
                    Objevent.startdate = Convert.ToString(ds.Tables[3].Rows[i]["startdate"]);
                    Objevent.startdate = Convert.ToDateTime(Objevent.startdate).ToShortDateString();
                }
                if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[3].Rows[i]["enddate"])))
                {
                    Objevent.enddate = Convert.ToString(ds.Tables[3].Rows[i]["enddate"]);
                    Objevent.enddate = Convert.ToDateTime(Objevent.enddate).ToShortDateString();
                }
                if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[3].Rows[i]["organizedby"])))
                {
                    Objevent.organizedby = Convert.ToString(ds.Tables[3].Rows[i]["organizedby"]);
                }
                if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[3].Rows[i]["eventname"])))
                {
                    Objevent.eventname = Convert.ToString(ds.Tables[3].Rows[i]["eventname"]);
                }
              
                Objeventlist.Add(Objevent);
            }
            ViewBag.Events = Objeventlist;
            if (Session["Username"] == null)
            {
                Session["Username"] = "New Employee";
            }
            return View(leavelist);
        }


        //Call on clicking Logout button
        public ActionResult LogOff()
        {
           
            return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// To get Monthly wise leaves from leaves table for display in Dashboard page graph.
        /// </summary>
        /// <returns></returns>
        public DataSet GetMonthlyLeaves()
        {
            DataSet ds = new DataSet();
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
            conn.Open();
            string query = "sp_getmonthlyleaves";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@empid", Session["Empid"]);
            cmd.Connection = conn;
            List<MonthlyLeaves> objMonthlyLeaveslst = new List<MonthlyLeaves>();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[0].Columns["numberofleaves"].ToString()))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    MonthlyLeaves objMonthlyLeaves = new MonthlyLeaves();
                    objMonthlyLeaves.EmpId = Convert.ToInt32(ds.Tables[0].Rows[i]["EmpId"]);
                    objMonthlyLeaves.Month = Convert.ToString(ds.Tables[0].Rows[i]["month"]);
                    objMonthlyLeaves.NumberOfLeaves = ds.Tables[0].Rows[i]["numberofleaves"].ToString() == string.Empty ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["numberofleaves"].ToString());
                    objMonthlyLeaveslst.Add(objMonthlyLeaves);
                }
            }
            ViewBag.monthlstleavs = objMonthlyLeaveslst;
            conn.Close();
            return ds;

        }


        [HttpPost]
        public ActionResult Index(Login objlogin)
        {
            Session["Empcode"] = objlogin.Username;
            Session["Password"] = objlogin.password;
            //Login Objlogin = new Login();
            //DataSet ds = getuserandleavedetails(objlogin);
            DataSet ds = getuserandleavedetails(objlogin);
            Session["Employees"] = ds;
            if (ds.Tables[0].Rows.Count != 0&&!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Empid"])))
            {
                bool passwordflag = Convert.ToBoolean(ds.Tables[0].Rows[0]["Updatedpasswordflag"]);
                Session["RemaingLeaves"] = Convert.ToString(ds.Tables[0].Rows[0]["Availableleaves"]);
                Session["LOP"] = Convert.ToString(ds.Tables[0].Rows[0]["LOP"]);
                Session["BereavementLeave"] = Convert.ToString(ds.Tables[0].Rows[0]["BereavementLeave"]);
                Session["PaternityorMaternityLeave"] = Convert.ToString(ds.Tables[0].Rows[0]["Paternityleave"]);
                Session["Empid"] = Convert.ToString(ds.Tables[0].Rows[0]["Empid"]);
                Session["Emp_Code"] = Convert.ToString(ds.Tables[0].Rows[0]["Empcode"]);
                Session["Email"] = Convert.ToString(ds.Tables[0].Rows[0]["Email"]);

                if (passwordflag)
                {
                    return RedirectToAction("Home", "Login");
                }
                else
                {
                    
                    
                    return RedirectToAction("Updatepassword", "Login");
                }
            }
            else
            {
                ViewBag.result = "Username or Password incorrect.";
                return View();
            }
           
        }
        public DataSet getuserandleavedetails(Login objlogin)
        {
            int i;
            DataSet ds = new DataSet();
            string psw = Encrypt(objlogin.password);
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_getlogin";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@p_usename", objlogin.Username);
                cmd.Parameters.AddWithValue("@P_pasword", psw);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = conn;
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(ds);
                    //if (ds.Tables[0].Rows.Count != 0)
                    //{
                    //    bool passwordflag = Convert.ToBoolean(ds.Tables[0].Rows[0]["Updatedpasswordflag"]);
                    //    Session["Empid"] = (ds.Tables[0].Rows[0]["Empid"]).ToString();
                    //    if (passwordflag)
                    //    {

                    //    }
                    //    else
                    //    {

                    //    }
                    //}
                    //else
                    //{
                    //    Session["Username"] = "New Employee";
                    //    Session["id"] = "Id";
                    //    Session["RemaingLeaves"] = 0;
                    //}
                }
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {

            }
            return ds;
        }
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }   
        public ActionResult Updatepassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Updatepassword(Login objlogin)
        {
            Session["Password"] = objlogin.password;
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            try
            {
                objlogin.password = Encrypt(objlogin.password);
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_updloginpassword";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_password", objlogin.password);
                cmd.Parameters.AddWithValue("@p_Empid", Session["Empid"]);
                cmd.Connection = conn;
                int i = cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                return RedirectToAction("ApplyLeave", "Leave");
            }
            return RedirectToAction("Home", "Login");
        }

        public ActionResult Leave()
        {
            return View();
        }

        //Testing Purpouse
        public ActionResult LeaveView()
        {
            return View();
        }

        public ActionResult ApplyLeave()
        {
            return RedirectToAction("ApplyLeaveView", "Leave");
        }

    }
}