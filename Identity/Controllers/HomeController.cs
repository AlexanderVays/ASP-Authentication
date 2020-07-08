using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                // user sign in
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false); // the first bool isPersistant, second bool is Lockout on failure

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            // login functionality provided by the identity packages
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = "",
                //PasswordHash = password  //posible to crate a password here, for example "rAmbama!"
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded) 
            {
                // sign in
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false); // the first bool isPersistant, second bool is Lockout on failure

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            // register functionality provided by the identity packages
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SendEmail()
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("wow.alpha.00@gmail.com");
                mail.To.Add("plotnik.alexander@gmail.com");
                mail.Subject = "PIN Code Authentication";
                mail.Body = "<h1>PIN: 987654</h1>";
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("wow.alpha.00@gmail.com", "########");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
