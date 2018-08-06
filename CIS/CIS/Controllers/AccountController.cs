using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Data.SqlClient;
using CIS.App_Code;
using CIS.Models;

namespace CIS.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
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
        public ActionResult Register()
        {
            var record = new UserModel();
            record.UserTypes = GetUserTypes();
            return View(record);
        }

        public bool IsExisting(string email)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Email FROM Users
                    WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    return cmd.ExecuteScalar() == null ? false : true;
                }
            }
        }

        [HttpPost]
        public ActionResult Register(UserModel record)
        {
            if (IsExisting(record.Email))
            {
                ViewBag.Message =
                    "<div class='alert alert-danger col-lg-6'>Email already existing.</div>";
                return View(record);
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Helper.GetCon()))
                {
                    con.Open();
                    string query = @"INSERT INTO Users 
                    (usertype, email, password, UserName, Phone) VALUES
                    (@TypeID, @Email, @Password, @UserName, @Phone)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@TypeID", record.TypeId);
                        cmd.Parameters.AddWithValue("@Email", record.Email);
                        cmd.Parameters.AddWithValue("@Password", Helper.Hash(record.Password));
                        cmd.Parameters.AddWithValue("@UserName", record.UserName);
                        cmd.Parameters.AddWithValue("@Phone", record.Phone);
                        cmd.ExecuteNonQuery();


                        Helper.SendEmail(record.Email, "Account Activation ??", "Thank you for choosing us");
                        return RedirectToAction("Login");

                    }
                }
            }

        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ReportsModel record)
        {
 
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT UserId, UserType, username FROM Users
                    WHERE Email=@Email AND Password=@Password";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", record.Login.Email);
                    cmd.Parameters.AddWithValue("@Password", Helper.Hash(record.Login.Password));

                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows) // valid credentials
                        {
                            while (data.Read())
                            {
                                Session["userid"] = data["UserID"].ToString();
                                Session["typeid"] = data["UserType"].ToString();

                                Session["username"] = data["username"].ToString();
                            }
                            return RedirectToAction("Index","Home");
                        }
                        else // invalid credentials
                        {
                            ViewBag.Message =
                                "<div class='alert alert-danger col-lg-6'>Invalid email or password</div>";
                            return View();
                        }
                    }
                }
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClearSession()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}