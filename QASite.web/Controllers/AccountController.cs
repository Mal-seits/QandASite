using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using QASite.data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace QASite.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repository = new UserRepository(connectionString);
            var user = repository.AuthorizeUser(email, password);
            if(user == null)
            {
                return Redirect("/account/signup");
            }
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
             new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/home/index");

        }

       
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(string email, string password, string name)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repository = new UserRepository(connectionString);
            repository.AddUser(email, password, name);
            return Redirect("/account/login");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }


}
