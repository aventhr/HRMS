using AventWithMVC.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AventWithMVC.Controllers
{
    public class WeeksController : Controller
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;

        ArrayList Startdate = new ArrayList();
        ArrayList Enddate = new ArrayList();
        ArrayList Hours = new ArrayList();
        ArrayList ArrNumberofleave = new ArrayList();
        // GET: Weeks
        List<Weeks> ObjweeksList = new List<Weeks>();

        //string EmpCode = Convert.ToString(Session["Emp_Code"]);
        //string EmpCode = "avent_005";
        int Currentmonth = DateTime.Now.Month;
        public ActionResult TimeSheetDashBoard()
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
            return View();
        }

        public ActionResult Index()
        {
            //List<string> weeks = new List<string>();
            DataSet dsWeeklySubmitted = new DataSet();
            dsWeeklySubmitted = GetWeeklyTimesIfExisted();
            if (!string.IsNullOrEmpty(Convert.ToString(dsWeeklySubmitted.Tables[2].Rows[0]["till_date_avg"])))
            {
                ViewBag.tilldateaverage = Convert.ToInt32(dsWeeklySubmitted.Tables[2].Rows[0]["till_date_avg"]);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsWeeklySubmitted.Tables[2].Rows[0]["Empcode"])))
            {
                ViewBag.Empcode = Convert.ToString(dsWeeklySubmitted.Tables[2].Rows[0]["Empcode"]);
            }
            if (dsWeeklySubmitted.Tables[0] != null && dsWeeklySubmitted.Tables[0].Rows.Count > 0)
            {
                ViewBag.Totalhours = Convert.ToDouble(dsWeeklySubmitted.Tables[1].Rows[0]["Totalhours"]);
                for (int x = 0; x <= dsWeeklySubmitted.Tables[0].Rows.Count - 1 && x <= dsWeeklySubmitted.Tables[0].Rows.Count; x++)
                {
                    Weeks ObjExistingDates = new Weeks();
                    ObjExistingDates.Startdate = Convert.ToString(dsWeeklySubmitted.Tables[0].Rows[x]["StartDate"]);
                    ObjExistingDates.Enddate = Convert.ToString(dsWeeklySubmitted.Tables[0].Rows[x]["EndDate"]);
                    ObjExistingDates.Hours = Convert.ToDouble(dsWeeklySubmitted.Tables[0].Rows[x]["Hours"]);
                    ObjExistingDates.Avg = Convert.ToDouble(dsWeeklySubmitted.Tables[0].Rows[x]["daily_avg"]);
                    ObjExistingDates.Numofleaves = Convert.ToInt32(dsWeeklySubmitted.Tables[0].Rows[x]["number_of_leaves"]);
                    ObjweeksList.Add(ObjExistingDates);
                }
            }
            else
            {
                List<string> weekdays = GetDatesOnWeekWise(DateTime.Now.Year, DateTime.Now.Month).ToList();
                for (int i = 0; i < weekdays.Count; i++)
                {
                    Weeks ObjWeeks = new Weeks();
                    ObjWeeks.WeekDates = weekdays[i];
                    ObjweeksList.Add(ObjWeeks);
                }
            }
            ViewBag.Results = ObjweeksList;
            return View(ObjweeksList);
        }

        [HttpPost]
        public JsonResult Index(List<string> WeekWiseHoursandDates)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StartDate", typeof(string));
            dt.Columns.Add("EndDate", typeof(string));
            //dt.Columns.Add("Hours", typeof(int));
            dt.Columns.Add("Hours", typeof(double));
            dt.Columns.Add("Numberofleave", typeof(int));

            for (int l = 0; l < WeekWiseHoursandDates.Count; l++)
            {
                string[] SplitingDatesandHours = WeekWiseHoursandDates[l].Split('&');
                string[] SplitingWeekdates = SplitingDatesandHours[0].Split('/');
                string[] Numbeofleave = SplitingDatesandHours[1].Split('#');
                Startdate.Add(SplitingWeekdates[0]);
                Enddate.Add(SplitingWeekdates[1]);
                Hours.Add(Numbeofleave[0]);
                ArrNumberofleave.Add(Numbeofleave[1]);
            }
            for (int p = 0; p < Startdate.Count; p++)
            {
                //if (!string.IsNullOrEmpty(Convert.ToString(Hours[p])))
                //if (Convert.ToDouble(Hours[p])>0 || !string.IsNullOrEmpty(Convert.ToString(Hours[p]))&& Convert.ToString(Hours[p])=="")
                if (Convert.ToString(Hours[p]) == "")
                {
                    Hours[p] = 0;
                    if (Convert.ToString(ArrNumberofleave[p]) == "")
                    {
                        ArrNumberofleave[p] = 0;
                        dt.Rows.Add(Startdate[p], Enddate[p], Hours[p], ArrNumberofleave[p]);
                    }
                    else
                    {
                        dt.Rows.Add(Startdate[p], Enddate[p], Hours[p], ArrNumberofleave[p]);
                    }
                }
                else
                {
                    if (Convert.ToString(ArrNumberofleave[p]) == "")
                    {
                        ArrNumberofleave[p] = 0;
                        dt.Rows.Add(Startdate[p], Enddate[p], Hours[p], ArrNumberofleave[p]);
                    }
                    else
                    {
                        dt.Rows.Add(Startdate[p], Enddate[p], Hours[p], ArrNumberofleave[p]);
                    }
                }
            }
            int u = InsertWeeklyTimes(dt);
            if (u > 0)
            {
                //string message = "Your details have been saved successfully.";
                //string script = "window.onload = function(){ alert('";
                //script += message;
                //script += "')};";
                //ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
                ViewData["Message"] = "Success";
            }
            //return Json("window.alert('HELLO');");
            //return RedirectToAction("Index", "Admin");
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        //Testing Purpouse
        public ActionResult TimeSheetView()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.weeks = GetWeeklyTimesIfExisted();
            //List<string> weeks = new List<string>();
            DataSet dsWeeklySubmitted = new DataSet();
            dsWeeklySubmitted = GetWeeklyTimesIfExisted();
            if (!string.IsNullOrEmpty(Convert.ToString(dsWeeklySubmitted.Tables[2].Rows[0]["till_date_avg"])))
            {
                ViewBag.tilldateaverage = Convert.ToInt32(dsWeeklySubmitted.Tables[2].Rows[0]["till_date_avg"]);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsWeeklySubmitted.Tables[2].Rows[0]["Empcode"])))
            {
                ViewBag.Empcode = Convert.ToString(dsWeeklySubmitted.Tables[2].Rows[0]["Empcode"]);
            }
            if (dsWeeklySubmitted.Tables[0] != null && dsWeeklySubmitted.Tables[0].Rows.Count > 0)
            {
                ViewBag.Totalhours = Convert.ToDouble(dsWeeklySubmitted.Tables[1].Rows[0]["Totalhours"]);
                for (int x = 0; x <= dsWeeklySubmitted.Tables[0].Rows.Count - 1 && x <= dsWeeklySubmitted.Tables[0].Rows.Count; x++)
                {
                    Weeks ObjExistingDates = new Weeks();
                    ObjExistingDates.Startdate = Convert.ToString(dsWeeklySubmitted.Tables[0].Rows[x]["StartDate"]);
                    ObjExistingDates.Enddate = Convert.ToString(dsWeeklySubmitted.Tables[0].Rows[x]["EndDate"]);
                    ObjExistingDates.Hours = Convert.ToDouble(dsWeeklySubmitted.Tables[0].Rows[x]["Hours"]);
                    ObjExistingDates.Avg = Convert.ToDouble(dsWeeklySubmitted.Tables[0].Rows[x]["daily_avg"]);
                    ObjExistingDates.Numofleaves = Convert.ToInt32(dsWeeklySubmitted.Tables[0].Rows[x]["number_of_leaves"]);
                    ObjweeksList.Add(ObjExistingDates);
                }
            }
            else
            {
                List<string> weekdays = GetDatesOnWeekWise(DateTime.Now.Year, DateTime.Now.Month).ToList();
                for (int i = 0; i < weekdays.Count; i++)
                {
                    Weeks ObjWeeks = new Weeks();
                    ObjWeeks.WeekDates = weekdays[i];
                    ObjweeksList.Add(ObjWeeks);
                }
            }
            ViewBag.Results = ObjweeksList;
            return View(ObjweeksList);
        }

        [HttpPost]
        public JsonResult TimeSheetView(List<string> WeekWiseHoursandDates)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StartDate", typeof(string));
            dt.Columns.Add("EndDate", typeof(string));
            //dt.Columns.Add("Hours", typeof(int));
            dt.Columns.Add("Hours", typeof(double));
            dt.Columns.Add("Numberofleave", typeof(int));

            for (int l = 0; l < WeekWiseHoursandDates.Count; l++)
            {
                string[] SplitingDatesandHours = WeekWiseHoursandDates[l].Split('&');
                string[] SplitingWeekdates = SplitingDatesandHours[0].Split('/');
                string[] Numbeofleave = SplitingDatesandHours[1].Split('#');
                Startdate.Add(SplitingWeekdates[0]);
                Enddate.Add(SplitingWeekdates[1]);
                Hours.Add(Numbeofleave[0]);
                ArrNumberofleave.Add(Numbeofleave[1]);
            }
            for (int p = 0; p < Startdate.Count; p++)
            {
                //if (!string.IsNullOrEmpty(Convert.ToString(Hours[p])))
                //if (Convert.ToDouble(Hours[p])>0 || !string.IsNullOrEmpty(Convert.ToString(Hours[p]))&& Convert.ToString(Hours[p])=="")
                if (Convert.ToString(Hours[p]) == "")
                {
                    Hours[p] = 0;
                    if (Convert.ToString(ArrNumberofleave[p]) == "")
                    {
                        ArrNumberofleave[p] = 0;
                        dt.Rows.Add(Startdate[p], Enddate[p], Hours[p], ArrNumberofleave[p]);
                    }
                    else
                    {
                        dt.Rows.Add(Startdate[p], Enddate[p], Hours[p], ArrNumberofleave[p]);
                    }
                }
                else
                {
                    if (Convert.ToString(ArrNumberofleave[p]) == "")
                    {
                        ArrNumberofleave[p] = 0;
                        dt.Rows.Add(Startdate[p], Enddate[p], Hours[p], ArrNumberofleave[p]);
                    }
                    else
                    {
                        dt.Rows.Add(Startdate[p], Enddate[p], Hours[p], ArrNumberofleave[p]);
                    }
                }
            }
            int u = InsertWeeklyTimes(dt);
            if (u > 0)
            {
                //string message = "Your details have been saved successfully.";
                //string script = "window.onload = function(){ alert('";
                //script += message;
                //script += "')};";
                //ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
                ViewData["Message"] = "Success";
            }
            //return Json("window.alert('HELLO');");
            //return RedirectToAction("Index", "Admin");
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public IList<string> GetDatesOnWeekWise(int year, int month)
        {
            //DataSet ds = null;
            List<string> li = new List<string>();
            DateTime start = new DateTime(year, month, 1);
            DateTime end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            String StartDate = "";
            String EndDate = "";
            DateTime enddt; DateTime stdt = new DateTime(year, month, 1); ;
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Monday || date == start)
                {
                    StartDate = date.ToShortDateString().ToString();
                    stdt = Convert.ToDateTime(date.ToShortDateString());
                }
                if (date.DayOfWeek == DayOfWeek.Friday || date.AddDays(1).Month != month && (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday))
                {
                    EndDate = date.ToShortDateString().ToString();
                    enddt = Convert.ToDateTime(date.ToShortDateString());
                    //li.Add(StartDate + "--" + EndDate+"&"+ ((enddt-stdt).TotalDays + 1));
                    li.Add(StartDate + " to " + EndDate);
                    //yield return StartDate + " -- " + EndDate;
                }
            }
            return li;

        }


        public DataSet GetWeeklyTimesIfExisted()
        {
            string EmpCode = Convert.ToString(Session["Emp_Code"]);
            DataSet ds = new DataSet();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_getDatesandTimesOnWeekWise";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@p_Emp_Code", EmpCode);
                cmd.Parameters.AddWithValue("@P_MonthNum", Currentmonth);
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
            //string EmpCode = "Avent_943";
            //int Currentmonth = DateTime.Now.Month;
            int i = 0;
            conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
            DataSet ds = new DataSet();
            ds = GetWeeklyTimesIfExisted();
            try
            {
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    conn.Open();
                    string query = "";
                    string StartDate = Convert.ToString(dt.Rows[t]["StartDate"]);
                    string EndDate = Convert.ToString(dt.Rows[t]["EndDate"]);
                    DateTime StartDate1 = DateTime.Now;
                    DateTime EndDate1 = DateTime.Now;
                    StartDate1 = Convert.ToDateTime(dt.Rows[t]["StartDate"]);
                    EndDate1 = Convert.ToDateTime(dt.Rows[t]["EndDate"]);
                    int days = Convert.ToInt32((EndDate1 - StartDate1).TotalDays + 1);
                    //int Hours = Convert.ToInt32(dt.Rows[t]["Hours"]);
                    double Hours = Convert.ToDouble(dt.Rows[t]["Hours"]);
                    int numberofleaves = Convert.ToInt32(dt.Rows[t]["Numberofleave"]);
                    decimal avg = (decimal)Hours / days;
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        query = "sp_updWeekWiseDatesandHours";
                    }
                    else
                    {
                        query = "sp_insWeekWiseDatesandHours";
                    }
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_StartDate", StartDate1.ToShortDateString());
                    cmd.Parameters.AddWithValue("@p_EndDate", EndDate1.ToShortDateString());
                    cmd.Parameters.AddWithValue("@p_Days", days);
                    cmd.Parameters.AddWithValue("@p_Numberofleaves", numberofleaves);
                    //cmd.Parameters.AddWithValue("@p_avg", avg);
                    cmd.Parameters.Add("@p_avg", MySqlDbType.Decimal).Value = avg;
                    //cmd.Parameters.AddWithValue("@p_Hours", Hours);
                    cmd.Parameters.Add("@p_Hours", MySqlDbType.Decimal).Value = Hours;
                    cmd.Parameters.AddWithValue("@p_EmpCode", EmpCode);
                    cmd.Parameters.AddWithValue("@p_MonthNum", Currentmonth);
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
                //Calculateremainingleave(EmpCode);
            }
            catch (Exception ex)
            {

            }


            return i;
        }


        //public int Calculateremainingleave(string Empcode)
        //{
        //    conn.Open();
        //    string query = "";
        //    query = "sp_calculateremaingleaves";
        //    MySqlCommand cmdforleaves = new MySqlCommand(query, conn);
        //    cmdforleaves.Connection = conn;
        //    cmdforleaves.CommandType = CommandType.StoredProcedure;
        //    cmdforleaves.Parameters.AddWithValue("@P_Emp_code", EmpCode);
        //    int i=cmdforleaves.ExecuteNonQuery();
        //    conn.Close();
        //    return i;
        //}
    }
}