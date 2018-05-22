using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AventWithMVC.Controllers
{
    public class LeadsDashboardController : Controller
    {
        // GET: LeadsDashboard
        public ActionResult AvgHoursDashboard()
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
    }
}