using AventWithMVC.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AventWithMVC.Controllers
{
    public class WeeksBackUpController : Controller
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;

        ArrayList Startdate = new ArrayList();
        ArrayList Enddate = new ArrayList();
        ArrayList Hours = new ArrayList();
        // GET: Weeks
        List<Weeks> ObjweeksList = new List<Weeks>();
        public ActionResult Index()
        {

            List<string> weeks = new List<string>();
            DataSet ds = new DataSet();

            List<string> weekdays = GetDatesOnWeekWise(DateTime.Now.Year, DateTime.Now.Month).ToList();
            for (int i = 0; i < weekdays.Count; i++)
            {
                Weeks ObjWeeks = new Weeks();
                ObjWeeks.WeekDates = weekdays[i];
                ObjweeksList.Add(ObjWeeks);
            }
            ds = GetWeeklyTimes();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x <= ds.Tables[0].Rows.Count - 1 && x <= ds.Tables[0].Rows.Count; x++)
                {
                    Weeks NewObjWeeks = new Weeks();
                }
            }
            ViewBag.Results = ObjweeksList;
            return View();
        }

        [HttpPost]
        public ActionResult Index(List<string> WeekWiseHoursandDates)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StartDate", typeof(string));
            dt.Columns.Add("EndDate", typeof(string));
            dt.Columns.Add("Hours", typeof(int));

            for (int l = 0; l < WeekWiseHoursandDates.Count; l++)
            {
                string[] SplitingDatesandHours = WeekWiseHoursandDates[l].Split('&');
                string[] SplitingWeekdates = SplitingDatesandHours[0].Split('/');
                Startdate.Add(SplitingWeekdates[0]);
                Enddate.Add(SplitingWeekdates[1]);
                Hours.Add(SplitingDatesandHours[1]);
            }
            for (int p = 0; p < Startdate.Count; p++)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Hours[p])))
                {
                    dt.Rows.Add(Startdate[p], Enddate[p], Hours[p]);
                }

            }
            int u = InsertWeeklyTimes(dt);
            return View();
        }

        public IList<string> GetDatesOnWeekWise(int year, int month)
        {
            //DataSet ds = null;
            List<string> li = new List<string>();
            DateTime start = new DateTime(year, month, 1);
            DateTime end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            String StartDate = "";
            String EndDate = "";
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Monday || date == start)
                {
                    StartDate = date.ToShortDateString().ToString();
                }
                if (date.DayOfWeek == DayOfWeek.Friday || date.AddDays(1).Month != month && (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday))
                {
                    EndDate = date.ToShortDateString().ToString();
                    li.Add(StartDate + "--" + EndDate);
                    //yield return StartDate + " -- " + EndDate;
                }
            }
            return li;

        }

        public DataSet GetWeeklyTimes()
        {
            string EmpCode = Convert.ToString(Session["Emp_Code"]);
            DataSet ds = new DataSet();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_getDatesandTimesOnWeekWise";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Emp_Code", EmpCode);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = conn;
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(ds);

                }
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public int InsertWeeklyTimes(DataTable dt)
        {
            string EmpCode = Convert.ToString(Session["Emp_Code"]);
            int i = 0;
            conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
            conn.Open();
            string query = "sp_insWeekWiseDatesandHours";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            for (int t = 0; t < dt.Rows.Count; t++)
            {
                string StartDate = Convert.ToString(dt.Rows[t]["StartDate"]);
                string EndDate = Convert.ToString(dt.Rows[t]["EndDate"]);
                int Hours = Convert.ToInt32(dt.Rows[t]["Hours"]);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@EndDate", EndDate);
                cmd.Parameters.AddWithValue("@Hours", Hours);
                cmd.Parameters.AddWithValue("@EmpCode", EmpCode);
                i = cmd.ExecuteNonQuery();
            }
            conn.Close();
            return i;
        }
    }
}