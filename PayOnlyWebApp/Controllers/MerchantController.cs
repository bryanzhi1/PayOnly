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
    public class MerchantController : Controller
    {
        private MerchantDAL merchantContext = new MerchantDAL();

        public IActionResult Index()
        {
            if ((HttpContext.Session.GetString("MerchantID") == null))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                int MerchantID = Convert.ToInt32(HttpContext.Session.GetString("MerchantID"));
                string MerchantName = merchantContext.GetName(MerchantID);
                double balance = merchantContext.GetBalance(MerchantID);
                ViewData["balance"] = balance;
                ViewData["merchantName"] = MerchantName;

                return View();
            }
        }

        public IActionResult EditDetails()
        {
            if ((HttpContext.Session.GetString("MerchantID") == null))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                Merchant merchantDetail = merchantContext.GetDetails(Convert.ToInt32(HttpContext.Session.GetString("MerchantID")));
                ViewBag.merchantDetail = merchantDetail;
                return View(merchantDetail);
            }
        }

        [HttpPost]
        public ActionResult EditDetails(IFormCollection formData)
        {
            string merchantName = formData["txtMerchantName"].ToString();
            string inChargeName = formData["txtInChargeName"].ToString();
            string phoneNumber = formData["txtPhoneNumber"].ToString();
            string address = formData["txtAddress"].ToString();

            bool status = merchantContext.UpdateDetails(Convert.ToInt32(HttpContext.Session.GetString("MerchantID")), merchantName, inChargeName, phoneNumber, address);

            return RedirectToAction("Index", "Merchant");
        }

        public IActionResult ChangePassword()
        {
            if ((HttpContext.Session.GetString("MerchantID") == null))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(IFormCollection formData)
        {
            string oldpwd = formData["txtOldPwd"].ToString();
            string newpwd = formData["txtnewPwd"].ToString();
            string cfmpwd = formData["txtCfmPwd"].ToString();
            string MerchantID = HttpContext.Session.GetString("MerchantID");

            bool correctOldPwd = merchantContext.passwordChecker(MerchantID, oldpwd);

            if (correctOldPwd == true)
            {
                if (newpwd == cfmpwd)
                {
                    bool status = merchantContext.changePassword(MerchantID, newpwd);
                    return RedirectToAction("Index", "Merchant");
                }

                else
                {
                    TempData["Message"] = "New passwords don't match";
                    return RedirectToAction("ChangePassword", "Merchant");
                }
            }

            else
            {
                TempData["Message"] = "Current password incorrect.";
                return RedirectToAction("ChangePassword", "Merchant");
            }
        }

        public IActionResult TransactionHistory()
        {
            if ((HttpContext.Session.GetString("MerchantID") == null))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                int MerchantID = Convert.ToInt32(HttpContext.Session.GetString("MerchantID"));

                List<Transaction> TransactionList = merchantContext.GetTransactions(MerchantID);
                return View(TransactionList);
            }
        }

        public IActionResult CashOut()
        {
            if ((HttpContext.Session.GetString("MerchantID") == null))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CashOut(IFormCollection formData)
        {
            if ((HttpContext.Session.GetString("MerchantID") == null))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                string bankName = formData["txtBankName"].ToString();
                string accountName = formData["txtAccName"].ToString();
                string accountNumber = formData["txtAccNumber"].ToString();
                double amount = Convert.ToDouble(formData["txtAmt"]);
                int merchantID = Convert.ToInt32(HttpContext.Session.GetString("MerchantID"));

                bool status = merchantContext.CashRequest(merchantID, bankName, accountName, accountNumber, amount);

                return RedirectToAction("Index", "Merchant");
            }
        }

        public IActionResult LogOut()
        {
            if ((HttpContext.Session.GetString("MerchantID") == null))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
        }
    }
}