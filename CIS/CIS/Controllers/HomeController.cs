using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Data.SqlClient;
using CIS.Models;
using CIS.App_Code;



namespace CIS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.LatLongArray =getCrimeReports() ;
            return View(getCrimeReports());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Reports()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult CrimeReport()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public List<ReportsModel> getCrimeReports()
        {
            

            var list = new List<ReportsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Crime_ID,longitude, latitude, incident_details, crime_rating,[Time]  FROM Crime_Reports";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            list.Add(new ReportsModel
                            {
                                CrimeId = int.Parse(sdr["Crime_ID"].ToString()),
                                longitude = sdr["longitude"].ToString(),
                                latitude = sdr["latitude"].ToString(),
                                incident_details = sdr["incident_details"].ToString(),
                                crime_rating = int.Parse(sdr["crime_rating"].ToString())

                            });
                        }
                        
                    }
                }
            }
            return list;

        }

    }


}