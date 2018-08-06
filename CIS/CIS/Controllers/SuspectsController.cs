using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Data.SqlClient;
using CIS.App_Code;
using CIS.Models;
using System.Drawing;

namespace CIS.Controllers
{
    public class SuspectsController : Controller
    {
        // GET: Suspects
        public ActionResult Index()
        {
            return View();
        }


        public List<HairTypeViewModel> GetHairType()
        {
            var list = new List<HairTypeViewModel>(); // instantiation
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT TypeID, Name
                    FROM HairType ORDER BY Name";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            list.Add(new HairTypeViewModel
                            {
                                HairTypeId = int.Parse(sdr["TypeID"].ToString()),
                                Name = sdr["Name"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }


        public List<WeaponTypesModel> GetWeaponType()
        {
            var list = new List<WeaponTypesModel>(); // instantiation
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT WeaponId, WeaponName
                    FROM Weapons ORDER BY WeaponId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            list.Add(new WeaponTypesModel
                            {
                                WeaponId = int.Parse(sdr["WeaponId"].ToString()),
                                WeaponName = sdr["WeaponName"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public ActionResult Suspects(int ID)
        {

            var record = new SuspectsModel();
            record.crime_id = ID;
            record.HairTypes = GetHairType();
            record.WeaponTypes = GetWeaponType();
            return View(record);
        }

        [HttpPost]
        public ActionResult Suspects(SuspectsModel record, HttpPostedFileBase image)
        {
           
           
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO SUSPECTS VALUES(@Crime_id,@Name,@Face_Shape,
                               @Hair_Style,@Prominent_Facial_Feature,@Body_Built,
                               @Shirt_Color,@Tattoo_Location,@Is_Armed,
                               @Type_of_Weapon,@Other_Description,@Image)";

                using (SqlCommand com = new SqlCommand(query, con))
                {
                    com.Parameters.AddWithValue("@Crime_id", record.crime_id);
                    com.Parameters.AddWithValue("@Name", record.Name == null ? DBNull.Value.ToString() : record.Name);
                    com.Parameters.AddWithValue("@Face_Shape", record.Face_Shape == null ? DBNull.Value.ToString(): record.Face_Shape);
                    com.Parameters.AddWithValue("@Hair_Style", record.Hair_Style == null ? DBNull.Value.ToString() : record.Hair_Style);
                    com.Parameters.AddWithValue("@Prominent_Facial_Feature", record.Prominent_Facial_Feature == null ? DBNull.Value.ToString() : record.Prominent_Facial_Feature);
                    com.Parameters.AddWithValue("@Body_Built", record.Body_Built == null ? DBNull.Value.ToString() : record.Body_Built);
                    com.Parameters.AddWithValue("@Image", image == null ? "None" :
                       DateTime.Now.ToString("yyyyMMddHHmmss-") + image.FileName);
                    if (image != null)
                    {
                        image.SaveAs(Server.MapPath("~/Images/SuspectImages/" +
                    DateTime.Now.ToString("yyyyMMddHHmmss-") + image.FileName));
                    }
                    com.Parameters.AddWithValue("@Shirt_Color", record.Shirt_Color == null ? DBNull.Value.ToString() : record.Shirt_Color);
                    com.Parameters.AddWithValue("@Tattoo_Location", record.Tattoo_Location == null ? DBNull.Value.ToString() : record.Tattoo_Location);
                    com.Parameters.AddWithValue("@Is_Armed", 0);
                    com.Parameters.AddWithValue("@Type_of_Weapon", record.Type_of_Weapon == null ? 0 : record.Type_of_Weapon);
                    com.Parameters.AddWithValue("@Other_Description", record.Other_Description == null ? DBNull.Value.ToString() : record.Other_Description);


                    com.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index","Home");
        }
    }
}