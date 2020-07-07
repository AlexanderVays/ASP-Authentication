using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TwoStepVerification.Controllers
{
    public class HomeController : Controller
    {
        //IActionResult is an interface that defines a contract that represents the result of an action method.
        public IActionResult Index() 
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate() 
        {
            var licenceClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Alexander Vays"),
                new Claim("DrivingLicence", "BC"),
            };

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Alex"),
                new Claim(ClaimTypes.Email, "alex@gmail.com"),
                new Claim("Claim says", "User1")
            };

            var userIdentity = new ClaimsIdentity(claims, "User identity");
            var licenceIdentity = new ClaimsIdentity(licenceClaims, "Company");

            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity, licenceIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}
