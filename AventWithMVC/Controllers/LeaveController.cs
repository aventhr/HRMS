using AventWithMVC.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AventWithMVC.Controllers
{
    public class LeaveController : Controller
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;


        [HttpGet]
        public ActionResult IndexMethod()
        {
            if (Session["Roleid"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult ApplyLeave()
        {
            DataSet ds = new DataSet();
            conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
            conn.Open();
            string query = "sp_getleavetype";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = conn;
            Leave objmainleave = new Leave();
            List<Leave> leavelist = new List<Leave>();
            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
            {

                sda.Fill(ds);
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    Leave objleave = new Leave();

                    objleave.leavetype = Convert.ToString(ds.Tables[0].Rows[k]["leavetype"]);
                    objleave.ddlleaveid = Convert.ToInt32(ds.Tables[0].Rows[k]["lid"]);
                    leavelist.Add(objleave);
                }
                objmainleave.leaveinfolist = leavelist;
                //ViewBag.ItemsSelect = new SelectList((leavelist), "leaveid", "leavetype", 1);
            }
            conn.Close();

            List<SelectListItem> sessionlist = new List<SelectListItem>();
            sessionlist.Add(new SelectListItem
            {
                Text = "Session1",
                Value = "1"
            });
            sessionlist.Add(new SelectListItem
            {
                Text = "Session2",
                Value = "2"
            });
            ViewBag.sessionlist = sessionlist;

            return View(objmainleave);
        }
        
        [HttpPost]
        public ActionResult ApplyLeave(Leave Objleave)
        {
            int i;
            try
            {
                if (ModelState.IsValid)
                {
                    //DateTime startdate;
                    DataSet ds = new DataSet();
                   
                    DateTime startdate = DateTime.ParseExact(Objleave.Leavestartdate, "MM/dd/yyyy", null);
                    DateTime enddate = DateTime.ParseExact(Objleave.Leaveendtdate, "MM/dd/yyyy", null);
                    //Objleave.numberofleaves = Convert.ToInt32((Convert.ToInt32(Objleave.Leaveendtdate)) - (Convert.ToInt32(Objleave.Leavestartdate)));
                    TimeSpan tm = enddate - startdate;
                    Objleave.numberofleaves = Convert.ToInt32(tm.TotalDays);
                    if (Objleave.numberofleaves == 0)
                    {
                        Objleave.numberofleaves = 1;
                    }
                    conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                    conn.Open();
                    string query = "sp_insonlyleave";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Startdate", DateTime.ParseExact(Objleave.Leavestartdate, "MM/dd/yyyy", null));
                    cmd.Parameters.AddWithValue("@Enddate", DateTime.ParseExact(Objleave.Leaveendtdate, "MM/dd/yyyy", null));
                    cmd.Parameters.AddWithValue("@Reportingto", Objleave.Reportingto);
                    cmd.Parameters.AddWithValue("@Startsession", Objleave.startsession);
                    cmd.Parameters.AddWithValue("@Endsession", Objleave.endtsession);
                    cmd.Parameters.AddWithValue("@Leavetype", Objleave.ddlleaveid);
                    //cmd.Parameters.AddWithValue("@Availableleaves", Objleave.Availableleaves);
                    cmd.Parameters.AddWithValue("@Contactnumber", Objleave.Contactnumber);
                    cmd.Parameters.AddWithValue("@v_Empid", Session["Empid"]);
                    cmd.Parameters.AddWithValue("@Reason", Objleave.Reason);
                    cmd.Parameters.AddWithValue("@v_numberofleaves", Objleave.numberofleaves);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return RedirectToAction("Leave", "Login");
                }
                ApplyLeave();
                return View();

            }
            catch (Exception ex)
            {
                return RedirectToAction("ApplyLeave", "Leave");
            }
        }
   


        //Testing purpouse view
        public ActionResult ApplyLeaveView()
        {
            DataSet ds = new DataSet();
            conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
            conn.Open();
            string query = "sp_getleavetype";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = conn;
            Leave objmainleave = new Leave();
            List<Leave> leavelist = new List<Leave>();
            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
            {

                sda.Fill(ds);
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    Leave objleave = new Leave();

                    objleave.leavetype = Convert.ToString(ds.Tables[0].Rows[k]["leavetype"]);
                    objleave.ddlleaveid = Convert.ToInt32(ds.Tables[0].Rows[k]["lid"]);
                    leavelist.Add(objleave);
                }
                objmainleave.leaveinfolist = leavelist;
                //ViewBag.ItemsSelect = new SelectList((leavelist), "leaveid", "leavetype", 1);
            }
            conn.Close();

            List<SelectListItem> sessionlist = new List<SelectListItem>();
            sessionlist.Add(new SelectListItem
            {
                Text = "Session1",
                Value = "1"
            });
            sessionlist.Add(new SelectListItem
            {
                Text = "Session2",
                Value = "2"
            });
            ViewBag.sessionlist = sessionlist;

            return View(objmainleave);
        }

        // Testing purpouse
        [HttpPost]
        public ActionResult ApplyLeaveView(Leave Objleave)
        {
            int i;
            try
            {
                if (ModelState.IsValid)
                {
                    //DateTime startdate;
                    DataSet ds = new DataSet();

                    DateTime startdate = DateTime.ParseExact(Objleave.Leavestartdate, "MM/dd/yyyy", null);
                    DateTime enddate = DateTime.ParseExact(Objleave.Leaveendtdate, "MM/dd/yyyy", null);
                    //Objleave.numberofleaves = Convert.ToInt32((Convert.ToInt32(Objleave.Leaveendtdate)) - (Convert.ToInt32(Objleave.Leavestartdate)));
                    TimeSpan tm = enddate - startdate;
                    Objleave.numberofleaves = Convert.ToInt32(tm.TotalDays);
                    if (Objleave.numberofleaves == 0)
                    {
                        Objleave.numberofleaves = 1;
                    }
                    conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                    conn.Open();
                    string query = "sp_insleave";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Startdate", DateTime.ParseExact(Objleave.Leavestartdate, "MM/dd/yyyy", null));
                    cmd.Parameters.AddWithValue("@Enddate", DateTime.ParseExact(Objleave.Leaveendtdate, "MM/dd/yyyy", null));
                    cmd.Parameters.AddWithValue("@Reportingto", Objleave.Reportingto);
                    cmd.Parameters.AddWithValue("@Startsession", Objleave.startsession);
                    cmd.Parameters.AddWithValue("@Endsession", Objleave.endtsession);
                    cmd.Parameters.AddWithValue("@Leavetype", Objleave.ddlleaveid);
                    //cmd.Parameters.AddWithValue("@Availableleaves", Objleave.Availableleaves);
                    cmd.Parameters.AddWithValue("@Contactnumber", Objleave.Contactnumber);
                    cmd.Parameters.AddWithValue("@v_Empid", Session["Empid"]);
                    cmd.Parameters.AddWithValue("@Reason", Objleave.Reason);
                    cmd.Parameters.AddWithValue("@v_numberofleaves", Objleave.numberofleaves);
                    cmd.Parameters.AddWithValue("@v_month", DateTime.ParseExact(Objleave.Leavestartdate, "MM/dd/yyyy", null).Month);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return RedirectToAction("Home", "Login");
                }
                ApplyLeaveView();
                return View();

            }
            catch (Exception ex)
            {
                return RedirectToAction("ApplyLeaveView", "Leave");
            }
        }

        [HttpPost]
        public ActionResult Myaction(Myclass Objleave)
        {
            DateTime fromDateAsDateTime = DateTime.ParseExact(Objleave.date, "MM/dd/yyyy", null);
            conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
            conn.Open();
            string query = "sp_insdateonly";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@mydate", fromDateAsDateTime);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
            conn.Close();
            return View();
        }

        /// <summary>
        /// To get Monthly wise leaves from leaves table for display in Dashboard page graph.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMonthlyLeaves()
        {
            DataSet ds = new DataSet();
            conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
            conn.Open();
            string query = "sp_getmonthlyleaves";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = conn;
            List<MonthlyLeaves> objMonthlyLeaveslst = new List<MonthlyLeaves>();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                MonthlyLeaves objMonthlyLeaves = new MonthlyLeaves();
                objMonthlyLeaves.EmpId = Convert.ToInt32(ds.Tables[0].Rows[i]["EmpId"]);
                objMonthlyLeaves.Month = Convert.ToString(ds.Tables[0].Rows[i]["month"]);
                objMonthlyLeaves.NumberOfLeaves = Convert.ToInt32(ds.Tables[0].Rows[i]["numberoflaves"]);
                objMonthlyLeaveslst.Add(objMonthlyLeaves);
            }

            return View(objMonthlyLeaveslst);
        }
          


    }
}