using IRunes.Data;
using IRunes.Models;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer;
using SIS.WebServer.Atributes;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IRunes.App.Controllers
{
    public class UsersController : Controller
    {
        public IHttpResponse Login(IHttpRequest httpRequest)
        {
            return this.View();
        }

        [HttpPost(ActionName = "Login")]
        public IHttpResponse LoginConfirm(IHttpRequest httpRequest)
        {
            using (var context = new RunesDbContext())
            {
                string username = ((ISet<string>)httpRequest.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)httpRequest.FormData["password"]).FirstOrDefault();
                //string confirmPassword = ((ISet<string>)httpRequest.FormData["confirmPassword"]).FirstOrDefault();
                // string email = ((ISet<string>)httpRequest.FormData["email"]).FirstOrDefault();

                User userFromDb = context.Users.FirstOrDefault(userA => (userA.Username == username || userA.Email == username) && userA.Password == this.HashPassword(password));

                if (userFromDb == null)
                {
                    return this.Redirect("/Users/Login");
                }

                this.SignIn(httpRequest, userFromDb.Id, userFromDb.Username, userFromDb.Email);

            }
            return this.Redirect("/");

        }

        public IHttpResponse Register(IHttpRequest httpRequest)
        {
            return this.View();
        }

        [HttpPost(ActionName = "Register")]
        public IHttpResponse RegisterConfirm(IHttpRequest httpRequest)
        {
            using (var context = new RunesDbContext())
            {
                string username = ((ISet<string>)httpRequest.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)httpRequest.FormData["password"]).FirstOrDefault();
                string confirmPassword = ((ISet<string>)httpRequest.FormData["confirmPassword"]).FirstOrDefault();
                string email = ((ISet<string>)httpRequest.FormData["email"]).FirstOrDefault();

                if (password != confirmPassword)
                {
                    return this.Redirect("/Users/Register");

                }

                User user = new User
                {
                    Username = username,
                    Password = this.HashPassword(password),
                    Email = email
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            return this.Redirect("/Users/Login");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }

        }

        public IHttpResponse Logout(IHttpRequest httpRequest)
        {
            this.SignOut(httpRequest);

            return this.Redirect("/");
        }


    }
}
