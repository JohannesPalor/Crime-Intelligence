using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.Sql;
using CIS.App_Code;
using CIS.Models;

namespace CIS.Controllers
{
    public class UserController : Controller
    {

        public List<UserTypesModel> GetUserTypes()
        {
            var list = new List<UserTypesModel>(); // instantiation
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT TypeID, Description 
                    FROM UserTypes ORDER BY Description";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            list.Add(new UserTypesModel
                            {
                                TypeID = int.Parse(sdr["TypeID"].ToString()),
                                Description = sdr["Description"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public ActionResult Create()
        {
            var record = new UserModel();
            record.UserTypes = GetUserTypes();

            return View(record);
        }

        [HttpPost]
        public ActionResult Create(UserModel record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO Users VALUES
                    (@Email, @Password, @UserName,
                    @Phone, @UserType)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", record.Email);
                    cmd.Parameters.AddWithValue("@Password", Helper.Hash(record.Password));
                    cmd.Parameters.AddWithValue("@UserName", record.UserName);
                    cmd.Parameters.AddWithValue("@Phone", record.Phone);
                    cmd.Parameters.AddWithValue("@UserType", record.UserType);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }

        // GET: User
        public ActionResult Index()
        {
            var list = new List<UserModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT u.userID,
                    t.TypeId, u.Email, u.Username,
                    u.Phone
                    FROM Users u
                    INNER JOIN Usertypes t ON u.UserType = t.TypeID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            list.Add(new UserModel
                            {
                                UserID = int.Parse(sdr["UserID"].ToString()),
                                UserType = sdr["TypeId"].ToString(),
                                Email = sdr["Email"].ToString(),
                                UserName = sdr["UserName"].ToString(),
                                Phone = sdr["Phone"].ToString()
                            });
                        }
                        return View(list);
                    }
                }
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var record = new UserModel();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT UserID, Email, UserName,                   
                    Phone
                    FROM Users
                    WHERE UserID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", id);
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows) // record is existing
                        {
                            record.UserTypes = GetUserTypes();

                            while (sdr.Read())
                            {
                                record.UserID = int.Parse(sdr["TypeID"].ToString());
                                record.Email = sdr["Email"].ToString();
                                record.UserName = sdr["UserName"].ToString();
                                record.Phone = sdr["Phone"].ToString();
                            }
                            return View(record);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
        }


    }
}