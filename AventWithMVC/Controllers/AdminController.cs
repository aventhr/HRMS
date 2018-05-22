using AventWithMVC.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AventWithMVC.Controllers
{
    public class AdminController : Controller
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString;
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult IndexMethod()
        {
            if (Session["Roleid"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }
        }

        //Testing purpose View
        public ActionResult DashboardView()
        {
            return View();
        }

        public ActionResult LeavesApprovel()
        {
            ViewBag.succssmsg = " The leave status Updated";
            int i;
            Leave Objleaveinfo = new Leave();
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            try
            {
                DataSet ds = new DataSet();
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_getleavesandleaveaction";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = conn;
                dynamic mymodel = new ExpandoObject();
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    List<Leave> leavelist = new List<Leave>();
                    sda.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                        {
                            Leave objleave = new Leave();
                            objleave.lid = Convert.ToInt32(ds.Tables[0].Rows[x]["Leaveid"]);
                            objleave.empname = Convert.ToString(ds.Tables[0].Rows[x]["empname"]);
                            objleave.Leavestartdate = Convert.ToString(ds.Tables[0].Rows[x]["Startdate"]);
                            //objleave.Leavestartdate = DateTime.ParseExact(ds.Tables[0].Rows[x]["Startdate"],"MM/dd/yyyy", null);
                            objleave.Leaveendtdate = Convert.ToString(ds.Tables[0].Rows[x]["Enddate"]);
                            objleave.leavetype = Convert.ToString(ds.Tables[0].Rows[x]["Leavetype"]);
                            objleave.Reason = Convert.ToString(ds.Tables[0].Rows[x]["Reason"]);
                            leavelist.Add(objleave);
                        }
                    }
                    else
                    {

                    }
                    if (ds.Tables[1].Rows.Count != 0)
                    {
                        List<Leavehistory> leavehistorylist = new List<Leavehistory>();
                        for (int x = 0; x < ds.Tables[1].Rows.Count; x++)
                        {
                            Leavehistory objleavehistory = new Leavehistory();
                            objleavehistory.empname = Convert.ToString(ds.Tables[1].Rows[x]["empname"]);
                            objleavehistory.Leaveid = Convert.ToInt32(ds.Tables[1].Rows[x]["Leaveid"]);
                            objleavehistory.Startdate = (Convert.ToDateTime(ds.Tables[1].Rows[x]["Startdate"]));
                            objleavehistory.Enddate = Convert.ToDateTime(ds.Tables[1].Rows[x]["Enddate"]);
                            objleavehistory.Leavetype = Convert.ToString(ds.Tables[1].Rows[x]["Leavetype"]);
                            objleavehistory.Actions = Convert.ToString(ds.Tables[1].Rows[x]["Actions"]);
                            if (ds.Tables[1].Rows[x]["Updatedstatusdate"] != null &&
                                !string.IsNullOrEmpty(Convert.ToString(ds.Tables[1].Rows[x]["Updatedstatusdate"])))
                            {
                                //var v= ds.Tables[1].Rows[x]["Updatedstatusdate"]
                                objleavehistory.Updatedstatusdate = Convert.ToDateTime(ds.Tables[1].Rows[x]["Updatedstatusdate"]);
                            }
                            //objleavehistory.Updatedstatusdate = Convert.ToDateTime("01/01/2001");
                            leavehistorylist.Add(objleavehistory);
                            Objleaveinfo.Leavehistorydetails = leavehistorylist;
                        }
                    }
                    else
                    {
                        Objleaveinfo.Leavehistorydetails = null;
                    }

                    if (ds.Tables[2].Rows.Count != 0)
                    {
                        List<Leave> leavelist2 = new List<Leave>();
                        for (int y = 0; y < ds.Tables[2].Rows.Count; y++)
                        {
                            Leave objleave2 = new Leave();
                            objleave2.ddlleaveactionid = Convert.ToInt32(ds.Tables[2].Rows[y]["Actionid"]);
                            objleave2.ddlleaveactionstatus = Convert.ToString(ds.Tables[2].Rows[y]["Actions"]);
                            leavelist2.Add(objleave2);

                            ViewBag.leaveactions = leavelist2;
                        }
                    }
                    Objleaveinfo.leaveinfolist = leavelist;
                }

                ViewData["MyProduct"] = ds.Tables[1];
                ViewBag.Leavehistory = ds.Tables[1];
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {

            }
            return View(Objleaveinfo);
        }

        [HttpPost]
        public ActionResult LeavesApprovel(int dropdown, int Leaveid)
        {
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_updleavestatus";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@leavestatusid", dropdown);
                cmd.Parameters.AddWithValue("@leaveid", Leaveid);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Dashboard", "Admin");
        }

        [HttpPost]
        public JsonResult Edit(int? dropdown, int? Leaveid)
        {
            Leave person = new Leave();
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_updleavestatus";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@v_leavestatusid", dropdown);
                cmd.Parameters.AddWithValue("@v_leaveid", Leaveid);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = conn;
                int i = cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            //return RedirectToAction("LeavesApprovel", "Admin");
            return Json(new { person }, JsonRequestBehavior.AllowGet);
        }
    }
}