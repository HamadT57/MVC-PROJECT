using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using MovieTR.Data;
using MovieTR.Models;
using MovieTR.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Helpers;
using Xceed.Wpf.Toolkit;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing;

namespace MovieTR.Controllers
{
    public class MovieTController : Controller
    {
        
        private MovieTDBcontext _Movieconnecter;
        private readonly IWebHostEnvironment webHostEnvironment;


        public MovieTController(MovieTDBcontext dbContext, IWebHostEnvironment hostEnvironment)
        {
            

            _Movieconnecter = dbContext;
            webHostEnvironment = hostEnvironment;

        }
            
    
    public IActionResult SignUpView()
        {
           
            return View();
        }
        public IActionResult SignUp(Users inputData)
        {
            
            var usercount = _Movieconnecter.User.Where(obj => obj.Email == inputData.Email);
            if (usercount.Count() == 0)
            {
                inputData.Role = 0;
                
                _Movieconnecter.User.Add(inputData);
                _Movieconnecter.SaveChanges();
                ViewData["Msg"] = "You have succesfully signed up";
                return View("SignUp");
            }
            else
            {
                ViewData["Msg"] = "User Already Exsit";
            }
            return View("SignUp");
        }
        public IActionResult SignInView()
        {
            
            return View();
        }
        public IActionResult SignIn(SignIn inputData)

        {

            var user = _Movieconnecter.User.Where(obj => obj.Email == inputData.Email && obj.Password == inputData.Password).FirstOrDefault();
            if (user == null)
            {
                ViewData["Msg"] = "User Doesn't Exsit";
                return View("SignInView");


            }
            else
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sfdsdsasdasdadfdsfdsfdsfisdjflkdsfldsfs6556456"));
                var Cridantial = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>();
                claims.Add(new Claim("Id", user.id.ToString()));
                claims.Add(new Claim("MyRole", user.Role.ToString()));
                claims.Add(new Claim("Name", user.Name.ToString()));

                var tokenOptions = new JwtSecurityToken(
                    signingCredentials: Cridantial,
                    claims: claims,
                    expires: DateTime.Now.AddDays(1)
                    );

                var Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                var CookieOption = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(1),
                    Secure = true,
                    HttpOnly = true
                };

                HttpContext.Response.Cookies.Append("Token", Token, CookieOption);
                var MyRole = user.Role;
                
                
                if (MyRole == 1) 
                {
                    return RedirectToAction("MovieView","Movie") ;}
                else
                {
                    return RedirectToAction("MovieView","Movie");
                }
            }
        }

        public IActionResult AdminPanel()
        {
           return View();
         }

        public IActionResult Signout()
        {

            HttpContext.Response.Cookies.Append("Token", "");
            return Redirect("/MovieT/SignInVIew");

        }
        public IActionResult ChangePasswordView()
        {

            return View("ChangePassword1");
        }
        public IActionResult ChangePassword(string OldPassword, ChangePassword input)

        {
            string Uid = User.Claims.Where(c => c.Type == "Id").FirstOrDefault()?.Value;
            string cs = @"Data Source=DESKTOP-A0NIFMH\SQLEXPRESS; Initial Catalog=MovieT0T;Integrated Security=True;TrustServerCertificate=True;";
            string qs = $@"UPDATE [User] SET Password = '{input.NewPassword}', ConfirmPassword= '{input.ConfirmPassword}' WHERE Id = {Uid};";
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(qs, connection);
                command.ExecuteNonQuery();
                ViewData["Msg"] = "Password Updated";

                return View("ChangePassword1");

            }
        }
        public IActionResult BagView()
        {

            return View();
        }

        public IActionResult ForgotPasswordView()
        {

            return View();
        }
        public IActionResult ForgotPassword(ForgotPasswordView input)
        {
            bool Verify = _Movieconnecter.User.Any(c => c.Email == input.Email);
            var PassRem =_Movieconnecter.User.Where(c => c.Email == input.Email).Select(c => c.Password).FirstOrDefault();
            
            if (Verify)
            {
                string Subject = "Password Reminder";
                String Body = $"Dear {input.Email} Your Password For Movie Store is {PassRem}\n Kindly Visit Site to Login";
                MailMessage mc = new MailMessage("hamadtariq567@outlook.com", input.Email);
                mc.Subject = Subject;
                mc.Body = Body;
                mc.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
                smtp.Timeout = 1000000;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                NetworkCredential nc = new NetworkCredential("hamadtariq567@outlook.com", "syawankmk57");
                smtp.Credentials = nc;
                smtp.Send(mc);
                ViewBag.Message = "Password Reminder has been sent to your Email";
                return View("ForgotPassword");
            }
            else
            {

                ViewBag.Message = "Email Does not exist";

                return View("ForgotPassword");
            }
        }

        public async Task<IActionResult> MovieView()
        {   
            return View(await _Movieconnecter.Movie.ToListAsync());
        }
        public async Task<IActionResult> TDetails(int? id)
        {
            if (id == null || _Movieconnecter.Movie == null)
            {
                return NotFound();
            }
            DataTable dt;
            string cs = @"Data Source=DESKTOP-A0NIFMH\SQLEXPRESS; Initial Catalog=MovieT0T;Integrated Security=True;TrustServerCertificate=True;";
            string qs = $@"SELECT Movie.Title, Theater.TheaterName,Theater.TheaterPrice FROM Movie INNER JOIN Theater ON Movie.TheaterId = Theater.Id where Movie.Id ={ id}";
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(qs, connection);
                command.CommandType = CommandType.Text;
                dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    List<MTvm> ListViewItem = new List<MTvm>();
                    DataRow dr = dt.Rows[i];
                    //ListViewItem itm = ListViewItem(dr.ToString());
                    //listView1.Items.Add(itm);
                    string[] allColumns = dr.ItemArray.Select(obj => obj.ToString()).ToArray();
                    //ListViewItem.Add(allColumns);
                }
                
                //foreach (MTvm dr in dt.AsEnumerable().ToList())
                //{
                    
                //    listt.Add(dr);
                
                return View();
            }
            

            
        }

        public async Task<IActionResult> TheaterView()
        {
            return View(await _Movieconnecter.Theater.ToListAsync());
        }

    }


}

