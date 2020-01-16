using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PayOnlyWebApp.DAL;
using Microsoft.AspNetCore.Mvc;
using PayOnlyWebApp.Models;

namespace PayOnlyWebApp.Controllers
{
    public class AdminController : Controller
    {
        private AdminDAL adminContext = new AdminDAL();

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminUser") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            else {
                return View();
            }
            
        }

        public IActionResult LogOut()
        {
             HttpContext.Session.Clear();
             return RedirectToAction("Index", "Home");
        }

        public IActionResult ViewRequests()
        {
            if (HttpContext.Session.GetString("AdminUser") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                List<CashOutRequest> RequestList = adminContext.GetRequests();
                return View(RequestList);
            }
        }

        public IActionResult ModifyRequest(int CashOutID, bool Approved)
        {
            if (HttpContext.Session.GetString("AdminUser") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                if (Approved == true) { 
                    bool CashOut = adminContext.ModifyCashOut(CashOutID, "Approved"); 
                }

                else { 
                    bool CashOut = adminContext.ModifyCashOut(CashOutID, "Rejected"); 
                }

                return RedirectToAction("ViewRequests", "Admin");
            }
        }
    }
}