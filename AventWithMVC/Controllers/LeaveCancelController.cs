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
    public class LeaveCancelController : Controller
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

        // GET: LeaveCancel
        public ActionResult Cancelleave()
        {
            int i;
            Leave Objleaveinfo = new Leave();
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            try
            {
                int empid = Convert.ToInt32(Session["Empid"]);
                DataSet ds = new DataSet();
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_getleavedataonid";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@empid", empid);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = conn;
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    List<Leave> leavelist = new List<Leave>();
                    sda.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                        {
                            Leave objleave = new Leave();
                            objleave.empname = Convert.ToString(ds.Tables[0].Rows[x]["Empname"]);
                            objleave.lid = Convert.ToInt32(ds.Tables[0].Rows[x]["Leaveid"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[x]["numberofleaves"])))
                            {
                                objleave.numberofleaves = Convert.ToInt32(ds.Tables[0].Rows[x]["numberofleaves"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[x]["reason"])))
                            {
                                objleave.Reason = Convert.ToString(ds.Tables[0].Rows[x]["reason"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[x]["Startdate"])))
                            {
                                objleave.Leavestartdate = Convert.ToString(ds.Tables[0].Rows[x]["Startdate"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[x]["enddate"])))
                            {
                                objleave.Leaveendtdate = Convert.ToString(ds.Tables[0].Rows[x]["enddate"]);
                            }
                            objleave.leavetype = Convert.ToString(ds.Tables[0].Rows[x]["leavetype"]);
                            leavelist.Add(objleave);
                        }
                    }
                    Objleaveinfo.leaveinfolist = leavelist;
                }
            }
            catch (Exception ex)
            {

            }
            return View(Objleaveinfo);
        }

        [HttpPost]
        public JsonResult Cancelleave(int Leaveid)
        {
            Leave person = new Leave();
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_delleave";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@v_leaveid", Leaveid);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = conn;
                int i = cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return Json(new { person }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelleaveView()
        {
            int i;
            Leave Objleaveinfo = new Leave();
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            try
            {
                int empid = Convert.ToInt32(Session["Empid"]);
                DataSet ds = new DataSet();
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_getleavedataonid";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@empid", empid);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = conn;
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    List<Leave> leavelist = new List<Leave>();
                    sda.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                        {
                            Leave objleave = new Leave();
                            objleave.empname = Convert.ToString(ds.Tables[0].Rows[x]["Empname"]);
                            objleave.lid = Convert.ToInt32(ds.Tables[0].Rows[x]["Leaveid"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[x]["numberofleaves"])))
                            {
                                objleave.numberofleaves = Convert.ToInt32(ds.Tables[0].Rows[x]["numberofleaves"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[x]["reason"])))
                            {
                                objleave.Reason = Convert.ToString(ds.Tables[0].Rows[x]["reason"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[x]["Startdate"])))
                            {
                                objleave.Leavestartdate = Convert.ToString(ds.Tables[0].Rows[x]["Startdate"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[x]["enddate"])))
                            {
                                objleave.Leaveendtdate = Convert.ToString(ds.Tables[0].Rows[x]["enddate"]);
                            }
                            objleave.leavetype = Convert.ToString(ds.Tables[0].Rows[x]["leavetype"]);
                            leavelist.Add(objleave);
                        }
                    }
                    Objleaveinfo.leaveinfolist = leavelist;
                }
            }
            catch (Exception ex)
            {

            }
            return View(Objleaveinfo);
        }

        [HttpPost]
        public JsonResult CancelleaveView(int Leaveid)
        {
            Leave person = new Leave();
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_delleave";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@v_leaveid", Leaveid);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = conn;
                int i = cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return Json(new { person }, JsonRequestBehavior.AllowGet);
        }

    }
}