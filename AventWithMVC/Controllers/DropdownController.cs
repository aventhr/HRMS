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
    public class DropdownController : Controller
    {
        public ActionResult Index()
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

        public DataSet Alldropdownbind()
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;
            DataSet ds = new DataSet();
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                conn.Open();
                string query = "sp_getalldropdowns";
                MySqlCommand cmd = new MySqlCommand(query, conn);
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
    }
}