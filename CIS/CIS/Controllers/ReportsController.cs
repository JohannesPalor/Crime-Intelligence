
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIS.Models;
using CIS.App_Code;
using System.Data.Sql;
using System.Data.SqlClient;


namespace CIS.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(ReportsModel record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO crime_reports(Time,longitude, latitude, incident_details,users) VALUES (@Date,  @longitude, @latitude, @incident_details,@users)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@longitude", record.longitude);
                    cmd.Parameters.AddWithValue("@latitude", record.latitude);
                    cmd.Parameters.AddWithValue("@incident_details", record.incident_details);

                    cmd.Parameters.AddWithValue("@users", 1);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }
    }
}