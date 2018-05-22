using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AventWithMVC.Models;
using System.Configuration;

namespace AventWithMVC.Controllers
{
    public class TimesheetController : Controller
    {
        ArrayList dates = new ArrayList();
        ArrayList Tasks = new ArrayList();
        ArrayList Workinghours = new ArrayList();
        ArrayList finaldata = new ArrayList();
        string[] hr;

        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;

        public ActionResult Timesheet()
        {
            DataSet ds = new DataSet();
            conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
            string query = "sp_gettasks";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
            {
                sda.Fill(ds);
            }
            List<Dropdown> Objlistfortasktype = new List<Dropdown>();
            for (int y = 0; y < ds.Tables[0].Rows.Count; y++)
            {
                Dropdown Objddl = new Dropdown();
                Objddl.ddlvalue = Convert.ToInt32(ds.Tables[0].Rows[y]["Taskid"]);
                Objddl.ddltext = Convert.ToString(ds.Tables[0].Rows[y]["Taskname"]);
                Objlistfortasktype.Add(Objddl);
                ViewBag.Tasks = Objlistfortasktype;
            }

            return View();
        }

        [HttpPost]
        public JsonResult Timesheet(List<string> hours)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Dates", typeof(string));
            dt.Columns.Add("Tasks", typeof(string));
            dt.Columns.Add("Hours", typeof(string));
            string[] ddlcount = hours[0].Split(',');
            for (int i = 1; i < hours.Count; i++)
            {
                string[] coltotal = hours[i].Split('!');
                string[] date = coltotal[0].Split('&');
                dates.Add(date[0]);
                hr = date[1].Split(',');
                for (int y = 0; y < hr.Count(); y++)
                {
                    Workinghours.Add(hr[y]);
                }
            }
            for (int j = 0; j < ddlcount.Count(); j++)
            {
                Tasks.Add(ddlcount[j]);
            }
            for (int x = 0; x < Tasks.Count; x++)
            {
                int p = x;
                DataRow[] sheetrow;
                for (int k = 0; k < dates.Count; k++)
                {
                    dt.Rows.Add(dates[k], Tasks[x]);
                    sheetrow = dt.Select("Dates='" + dates[k] + "' and Tasks='" + Tasks[x] + "'");
                    sheetrow[0]["Hours"] = Workinghours[p];
                    p = p + Tasks.Count;
                }
            }
            int o = dt.Rows.Count;

            for (int t = 0; t < dt.Rows.Count; t++)
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[t]["Dates"]))
                    && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[t]["Tasks"]))
                    && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[t]["Hours"])))
                {
                    string date = Convert.ToString(dt.Rows[t]["Dates"]);
                    int task = Convert.ToInt32(dt.Rows[t]["Tasks"]);
                    int hour = Convert.ToInt32(dt.Rows[t]["Hours"]);
                    conn.Open();
                    string query = "sp_instimesheet";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_task", task);
                    cmd.Parameters.AddWithValue("@p_date", date);
                    try
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[t]["Hours"])))
                        {
                            cmd.Parameters.AddWithValue("@p_hour", hour);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@p_hour", 0);
                        }
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        int u = cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {

                    }

                }


            }
            return Json("window.alert('HELLO');");
            //return JavaScript("window.alert('HELLO');");
            //return RedirectToAction("Timesheet");
        }
    }
}