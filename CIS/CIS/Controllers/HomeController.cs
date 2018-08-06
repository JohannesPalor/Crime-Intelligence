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
            ViewBag.LatLongArray = getCrimeReports();
            
            var record = new ReportsModel();
            record.CrimeTypes = getCrimeTypes();
            return View(record);
        }

        [HttpPost]
        public ActionResult Index(ReportsModel record, HttpPostedFileBase image)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO crime_reports(Time,longitude, latitude, incident_details,users,Image,crimetype,Votes) 
                                VALUES (@Date,  @longitude, @latitude, @incident_details,@users,@Image, @CrimeType,@Votes)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Date", record.time);
                    cmd.Parameters.AddWithValue("@longitude", record.longitude);
                    cmd.Parameters.AddWithValue("@latitude", record.latitude);
                    cmd.Parameters.AddWithValue("@incident_details", record.incident_details);
                    cmd.Parameters.AddWithValue("@Image", image == null ? "None" :
                       DateTime.Now.ToString("yyyyMMddHHmmss-") + image.FileName);

                    if (image != null)
                    {
                        image.SaveAs(Server.MapPath("~/Images/CrimeImages/" +
                    DateTime.Now.ToString("yyyyMMddHHmmss-") + image.FileName));
                    }



                    cmd.Parameters.AddWithValue("@CrimeType", record.CrimeTypeId);

                    cmd.Parameters.AddWithValue("@Votes", 1);

                    cmd.Parameters.AddWithValue("@users", int.Parse(Session["userid"].ToString()));
                    cmd.ExecuteNonQuery();
                }
            }


            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT MAX(Crime_ID) as CrimeId  from Crime_Reports";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            record.CrimeId = int.Parse(data["CrimeId"].ToString());

                        }
                    }
                    return RedirectToAction("Suspects", "Suspects", new { id = record.CrimeId });
                    //return Convert.ToDouble((decimal)cmd.ExecuteScalar());

                }
            }


        }




        public List<CrimeTypesModel> getCrimeTypes()
        {


            var list = new List<CrimeTypesModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT TypeId, CrimeName FROM CrimeTypes";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            list.Add(new CrimeTypesModel
                            {
                                TypeId = int.Parse(sdr["TypeId"].ToString()),
                                CrimeName = sdr["CrimeName"].ToString(),


                            });
                        }

                    }
                }
            }
            return list;

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
                string query = @"SELECT Crime_ID,longitude, latitude, incident_details,ct.TypeId, ct.CrimeName, votes FROM Crime_Reports
                                INNER JOIN CrimeTypes ct on ct.TypeId = CrimeType";
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
                                CrimeTypeId = int.Parse(sdr["TypeId"].ToString()),
                                CrimeName = sdr["CrimeName"].ToString(),
                                votes = int.Parse(sdr["votes"].ToString()),
                                SuspectCount = suspectCount(int.Parse(sdr["Crime_ID"].ToString()))

                            });
                        }

                    }
                }
            }
            return list;

        }

        public List<ReportsModel> getCrimeReportsTheft()
        {


            var list = new List<ReportsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Crime_ID,longitude, latitude, incident_details, ct.CrimeName FROM Crime_Reports
                                INNER JOIN CrimeTypes ct on ct.TypeId = CrimeType
                                WHERE ct.CrimeName = 'Theft' or ct.CrimeName='Robbery'";
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
                                CrimeName = sdr["CrimeName"].ToString()

                            });
                        }

                    }
                }
            }
            return list;

        }

        public List<ReportsModel> getCrimeReportsSexualHarassment()
        {


            var list = new List<ReportsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Crime_ID,longitude, latitude, incident_details, ct.CrimeName FROM Crime_Reports
                                INNER JOIN CrimeTypes ct on ct.TypeId = CrimeType
                                WHERE CrimeType = 3";
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
                                CrimeName = sdr["CrimeName"].ToString()

                            });
                        }

                    }
                }
            }
            return list;

        }
        public List<ReportsModel> getCrimeReportsBrawl()
        {


            var list = new List<ReportsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Crime_ID,longitude, latitude, incident_details, ct.CrimeName FROM Crime_Reports
                                INNER JOIN CrimeTypes ct on ct.TypeId = CrimeType
                                WHERE CrimeType = 6";
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
                                CrimeName = sdr["CrimeName"].ToString()

                            });
                        }

                    }
                }
            }
            return list;

        }
        public List<ReportsModel> getCrimeReportsDrinkingInPublic()
        {


            var list = new List<ReportsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Crime_ID,longitude, latitude, incident_details, ct.CrimeName FROM Crime_Reports
                                INNER JOIN CrimeTypes ct on ct.TypeId = CrimeType
                                WHERE CrimeType = 7";
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
                                CrimeName = sdr["CrimeName"].ToString()

                            });
                        }

                    }
                }
            }
            return list;

        }

        public int suspectCount(int id)
        {

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Count(*) from Suspects where Crime_Id = @id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    return int.Parse(cmd.ExecuteScalar().ToString());


                }
            }
        }
        public int VotesCount(int id)
        {

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Count(votes) from Crime_Reports where Crime_Id = @id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    return int.Parse(cmd.ExecuteScalar().ToString());


                }
            }
        }
    }
}