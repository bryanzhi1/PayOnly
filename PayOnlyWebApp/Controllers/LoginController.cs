using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PayOnlyWebApp.DAL;
using PayOnlyWebApp.Models;

namespace PayOnlyWebApp.Controllers
{
    public class LoginController : Controller
    {
        private LoginDAL loginContext = new LoginDAL();

        [HttpPost]
        public ActionResult MerchantLogin(IFormCollection formData)
        {
            string loginID = formData["txtUserName"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();
            int validCredentials = loginContext.merchantChecker(loginID, password);

            if (validCredentials == 0)
            {
                TempData["Message"] = "Invalid credentials.";
                return RedirectToAction("Index", "Home");
            }

            else
            {
                HttpContext.Session.SetString("MerchantID", Convert.ToString(validCredentials));
                return RedirectToAction("Index", "Merchant");
            }
        }

        [HttpPost]
        public ActionResult AdminLogin(IFormCollection formData)
        {
            string loginID = formData["txtUserName"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();
            bool validCredentials = loginContext.adminChecker(loginID, password);

            if (validCredentials == true)
            {
                HttpContext.Session.SetString("AdminUser", loginID);
                return RedirectToAction("Index", "Admin");
            }

            else
            {
                TempData["Message"] = "Invalid credentials.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}