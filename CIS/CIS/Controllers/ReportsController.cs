
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
            var record = new ReportsModel();
            record.CrimeTypes = getCrimeTypes();
            

            return View(record);
        }

        [HttpPost]
        public ActionResult Index(ReportsModel record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO crime_reports(Time,longitude, latitude, incident_details,users,crimetype) 
                                VALUES (@Date,  @longitude, @latitude, @incident_details,@users, @CrimeType)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@longitude", record.longitude);
                    cmd.Parameters.AddWithValue("@latitude", record.latitude);
                    cmd.Parameters.AddWithValue("@incident_details", record.incident_details);

                    cmd.Parameters.AddWithValue("@CrimeType", record.CrimeTypeId);

                    cmd.Parameters.AddWithValue("@users", 1);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
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

        public ActionResult Analytics(int? ID)
        {

            ViewBag.TheftCount = getTheftCount();
            ViewBag.BrawlCount = getBrawlCount();
            ViewBag.DrinkingCount = getDrinkingCount();
            ViewBag.SexualHarassmentCount = getSexualHarassmentCount();



            ViewBag.MonthsCount = getCountPerMonth();

            HomeController viewCrimes = new HomeController();

            if (ID == 1)
            {
                return View(viewCrimes.getCrimeReportsTheft());
            }
            else if (ID == 2)
            {
                return View(viewCrimes.getCrimeReportsSexualHarassment());
            }
            else if (ID == 3)
            {
                return View(viewCrimes.getCrimeReportsBrawl());
            }
            else if (ID == 4)
            {
                return View(viewCrimes.getCrimeReportsDrinkingInPublic());
            }
            else
            {
                return View(viewCrimes.getCrimeReports());
            }
        }


        public int getTheftCount()
        {

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"Select (Select count(CrimeType) ctr from Crime_Reports where CrimeType='1') 
                                 + (Select count(CrimeType) ctr from Crime_Reports where CrimeType='2') AS ctr";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    return int.Parse(cmd.ExecuteScalar().ToString());


                }
            }
        }

        public int getSexualHarassmentCount()
        {

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"Select count(CrimeType) ctr from Crime_Reports where CrimeType='3'";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    return int.Parse(cmd.ExecuteScalar().ToString());


                }
            }
        }

        public int getBrawlCount()
        {

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"Select count(CrimeType) ctr from Crime_Reports where CrimeType='6'";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    return int.Parse(cmd.ExecuteScalar().ToString());


                }
            }
        }

        public int getDrinkingCount()
        {

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"Select count(CrimeType) ctr from Crime_Reports where CrimeType='7'";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    return int.Parse(cmd.ExecuteScalar().ToString());


                }
            }
        }

        public int[] getCountPerMonth()
        {
            int[] months = new int[12];

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Month([time]) month from Crime_Reports";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {


                            switch (int.Parse(data["month"].ToString()))
                            {
                                case 1:
                                    months[0]++;
                                    break;
                                case 2:
                                    months[1]++;
                                    break;
                                case 3:
                                    months[2]++;
                                    break;
                                case 4:
                                    months[3]++;
                                    break;
                                case 5:
                                    months[4]++;
                                    break;
                                case 6:
                                    months[5]++;
                                    break;
                                case 7:
                                    months[6]++;
                                    break;
                                case 8:
                                    months[7]++;
                                    break;
                                case 9:
                                    months[8]++;
                                    break;
                                case 10:
                                    months[9]++;
                                    break;
                                case 11:
                                    months[10]++;
                                    break;
                                case 12:
                                    months[11]++;
                                    break;

                            }

                        }
                    }
                    return months;
                }
            }
        }




        public List<ReportsModel> getCrimeReportsByID(int CrimeID)
        {


            var list = new List<ReportsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Crime_ID,longitude,Image, latitude, incident_details, ct.CrimeName, votes, users FROM Crime_Reports
                                INNER JOIN CrimeTypes ct on ct.TypeId = CrimeType
                                WHERE Crime_ID = @CrimeID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CrimeID", CrimeID);
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
                                CrimeName = sdr["CrimeName"].ToString(),
                                votes = int.Parse(sdr["votes"].ToString()),
                                Image = sdr["Image"].ToString(),
                                user_id = 7

                            });
                        }

                    }
                }
            }
            return list;

        }


        public List<SuspectsModel> getSuspectsByID(int CrimeID)
        {


            var list = new List<SuspectsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Suspect_ID, Crime_Id, s.[Name], s.[Image], Face_Shape, ht.Name as Hair_Style, Prominent_Facial_Feature, Body_built,
                                Shirt_color, Tattoo_Location, w.WeaponName  as Type_of_weapon, Other_Description FROM Suspects s
                                INNER JOIN HairType ht on ht.TypeId = s.Hair_Style
                                INNER JOIN Weapons w on w.WeaponId = s.Type_of_weapon
                                WHERE Crime_Id= @CrimeID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CrimeID", CrimeID);
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            list.Add(new SuspectsModel
                            {
                                suspect_id = int.Parse(sdr["Suspect_ID"].ToString()),
                                crime_id = int.Parse(sdr["Crime_Id"].ToString()),
                                Name = sdr["Name"].ToString(),
                                Face_Shape = sdr["Face_Shape"].ToString(),
                                Hair_Style = sdr["Hair_Style"].ToString(),
                                Prominent_Facial_Feature = sdr["Prominent_Facial_Feature"].ToString(),
                                Body_Built = sdr["Body_built"].ToString(),
                                Shirt_Color = sdr["Shirt_color"].ToString(),
                                Tattoo_Location = sdr["Tattoo_Location"].ToString(),
                                Other_Description = sdr["Other_Description"].ToString(),
                                Weapon = sdr["Type_of_weapon"].ToString(),
                                Image = sdr["Image"].ToString()


                            });
                        }

                    }
                }
            }
            return list;

        }


        public List<SuspectsModel> getSuspectsBySuspectID(int Suspect_Id)
        {


            var list = new List<SuspectsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Suspect_ID, Crime_Id, [Name], Face_Shape, Hair_Style, Prominent_Facial_Feature, Body_built,
                                Shirt_color, Tattoo_Location, Type_of_weapon, Other_Description FROM Suspects 
                                WHERE Suspect_Id= @Suspect_Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Suspect_Id", Suspect_Id);
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            list.Add(new SuspectsModel
                            {
                                suspect_id = int.Parse(sdr["Suspect_ID"].ToString()),
                                crime_id = int.Parse(sdr["Crime_Id"].ToString()),
                                Name = sdr["Name"].ToString(),
                                Face_Shape = sdr["Face_Shape"].ToString(),
                                Hair_Style = sdr["Hair_Style"].ToString(),
                                Prominent_Facial_Feature = sdr["Prominent_Facial_Feature"].ToString(),
                                Body_Built = sdr["Body_built"].ToString(),
                                Shirt_Color = sdr["Shirt_color"].ToString(),
                                Tattoo_Location = sdr["Tattoo_Location"].ToString(),
                                Other_Description = sdr["Other_Description"].ToString(),
                                Type_of_Weapon = int.Parse(sdr["Type_of_weapon"].ToString())

                            });
                        }

                    }
                }
            }
            return list;

        }


        public ActionResult Details(int id)
        {

            var record = new ReportsModel();
            record.SuspectsList = getSuspectsByID(id);
            ViewBag.HasReported = VerifyVote(id, int.Parse(Session["userid"].ToString()));

            record.ReportsList = getCrimeReportsByID(id);
            return View(record);
        }


        public ActionResult Edit(int id)
        {
            var record = new SuspectsModel();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Suspect_ID, Crime_Id, [Name], Face_Shape, Hair_Style, Prominent_Facial_Feature, Body_built,
                                Shirt_color, Tattoo_Location, Type_of_weapon, Other_Description FROM Suspects 
                                WHERE Suspect_Id= @Suspect_Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Suspect_Id", id);
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {

                            record.suspect_id = int.Parse(sdr["Suspect_ID"].ToString());
                            record.crime_id = int.Parse(sdr["Crime_Id"].ToString());
                            record.Name = sdr["Name"].ToString();
                            record.Face_Shape = sdr["Face_Shape"].ToString();
                            record.Hair_Style = sdr["Hair_Style"].ToString();
                            record.Prominent_Facial_Feature = sdr["Prominent_Facial_Feature"].ToString();
                            record.Body_Built = sdr["Body_built"].ToString();
                            record.Shirt_Color = sdr["Shirt_color"].ToString();
                            record.Tattoo_Location = sdr["Tattoo_Location"].ToString();
                            record.Other_Description = sdr["Other_Description"].ToString();
                            record.Type_of_Weapon = int.Parse(sdr["Type_of_weapon"].ToString());


                        }

                        SuspectsController home = new SuspectsController();
                        record.HairTypes = home.GetHairType();
                        record.suspect_id = id;
                        return View(record);
                    }
                }
            }

        }



        [HttpPost]
        public ActionResult Edit(SuspectsModel record)
        {

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"UPDATE Suspects SET   
                                Name=@Name, Face_Shape=@Face_Shape, 
                                Hair_Style=@HairStyle, Prominent_Facial_Feature=@Prominent, 
                                Body_built=@BodyBuilt, Shirt_color=@ShirtColor, Tattoo_Location=@Tattoo, 
                                Type_of_weapon=@TypeOfWeapon, Other_Description=@OtherDescription FROM Suspects 
                                WHERE Suspect_Id= @Suspect_Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {


                    cmd.Parameters.AddWithValue("@Name", record.Name);
                    cmd.Parameters.AddWithValue("@Face_Shape", record.Face_Shape);
                    cmd.Parameters.AddWithValue("@HairStyle", record.Hair_Style);
                    cmd.Parameters.AddWithValue("@Prominent", record.Prominent_Facial_Feature);
                    cmd.Parameters.AddWithValue("@BodyBuilt", "Endomorph");
                    cmd.Parameters.AddWithValue("@ShirtColor", record.Shirt_Color);
                    cmd.Parameters.AddWithValue("@Tattoo", record.Tattoo_Location);
                    cmd.Parameters.AddWithValue("@OtherDescription", record.Other_Description);
                    cmd.Parameters.AddWithValue("@TypeOfWeapon", record.Type_of_Weapon);

                    cmd.Parameters.AddWithValue("@Suspect_Id", record.suspect_id);

                    cmd.ExecuteNonQuery();






                }
                return RedirectToAction("Details","Reports", new { id = record.crime_id });
            }

        }

        public ActionResult AddVote(int id, int userid)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"UPDATE crime_reports SET Votes= Votes+1
                                 WHERE CRIME_ID = @CrimeID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CrimeID", id);
                    
                    cmd.ExecuteNonQuery();
                }
            }

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO USERHASVOTE VALUES(@userid, @crimeid)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@crimeid", id);

                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Details", "Reports", new { id =id});

        }


        public int VerifyVote(int id, int userid)
        {
          
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT count(*) From UserHasVote WHERE userid=@userid and crimeid=@crimeid;";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@crimeid", id);

                    return int.Parse(cmd.ExecuteScalar().ToString());
                }
            }
           

        }

        public ActionResult Delete(int id)
        {
            int crimeid;

            #region 
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"Select crime_id FROM Suspects WHERE Suspect_Id=@SuspectID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {


                    cmd.Parameters.AddWithValue("@SuspectID", id);


                    crimeid = int.Parse(cmd.ExecuteScalar().ToString());






                }



            }

            #endregion
            #region


            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"DELETE FROM Suspects WHERE Suspect_Id=@SuspectID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {


                    cmd.Parameters.AddWithValue("@SuspectID", id);


                    cmd.ExecuteNonQuery();






                }



                return RedirectToAction("Details", "Reports", new { id = crimeid });
            }

            #endregion
        }





    }
}