using AventWithMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AventWithMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult Services()
        {
            return View();
        }
        public ActionResult Partners()
        {
            return View();
        }
        public ActionResult Careers()
        {
            return View();
        }
        public ActionResult Gallery()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult Staffing()
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

        //Testing purpouse
        public ActionResult TestView()
        {
            TotalLeaveDetails obj = new TotalLeaveDetails();
            DataSet ds = (DataSet)Session["Employees"];
            Session["Roleid"] = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpDepartmenttype"]);
            Session["Username"] = (ds.Tables[0].Rows[0]["Empname"]).ToString();
            List<TotalLeaveDetails> leavelist = new List<TotalLeaveDetails>();

            if (ds.Tables[1].Rows.Count >= 0 && ds.Tables[2].Rows.Count >= 0)
            {

                //ObjLeaves.TotalCasualLeaves = Convert.ToString(ds.Tables[2].Rows[0]["TotalCasualLeaves"]);
                //ObjLeaves.TotalBereavementLeaves = Convert.ToString(ds.Tables[2].Rows[0]["TotalBereavementLeaves"]);
                //ObjLeaves.TodatlPaternityLeave = Convert.ToString(ds.Tables[2].Rows[0]["TodatlPaternityLeave"]);
                //ObjLeaves.TotalLOP = Convert.ToString(ds.Tables[2].Rows[0]["TotalLOP"]);
                for (int x = 0; x <= ds.Tables[1].Rows.Count - 1 && x <= ds.Tables[1].Rows.Count; x++)
                {
                    TotalLeaveDetails ObjLeaves = new TotalLeaveDetails();
                    ObjLeaves.TotalCasualLeaves = Convert.ToString(ds.Tables[2].Rows[0]["TotalCasualLeaves"]);
                    ObjLeaves.TotalBereavementLeaves = Convert.ToString(ds.Tables[2].Rows[0]["TotalBereavementLeaves"]);
                    ObjLeaves.TodatlPaternityLeave = Convert.ToString(ds.Tables[2].Rows[0]["TodatlPaternityLeave"]);
                    ObjLeaves.TotalLOP = Convert.ToString(ds.Tables[2].Rows[0]["TotalLOP"]);
                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0
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
                    leavelist.Add(ObjLeaves);
                }

            }
            if (Session["Username"] == null)
            {
                Session["Username"] = "New Employee";
            }
            return View(leavelist);
        }

       
    }
}