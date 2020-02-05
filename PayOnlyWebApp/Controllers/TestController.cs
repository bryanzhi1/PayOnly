using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayOnlyWebApp.Models;
using PayOnlyWebApp.DAL;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace PayOnlyWebApp.Controllers
{
    public class TestController : Controller
    {
        static HttpClient client = new HttpClient();
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> testGetUserByIdAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri("localhost:55555/api/GetUserById/1"); //Gets user 1, replace localhost:5555 with actual ip and port
                    var response = await client.GetAsync(uri);
                    string textResult = await response.Content.ReadAsStringAsync();
                    bool success = response.IsSuccessStatusCode;
                    TempData["Message"] = "Test Passed.";
                    return RedirectToAction("Index", "Home");
                }
            }

            catch
            {
                TempData["Message"] = "Test Failed.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> testGetUserByEmail()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri("localhost:55555/api/GetUserByEmail/test@example.com"); //Gets user 1, replace localhost:5555 with actual ip and port
                    var response = await client.GetAsync(uri);
                    string textResult = await response.Content.ReadAsStringAsync();
                    bool success = response.IsSuccessStatusCode;
                    TempData["Message"] = "Test Passed.";
                    return RedirectToAction("Index", "Home");
                }
            }

            catch
            {
                TempData["Message"] = "Test Failed.";
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<IActionResult> testGetMerchant()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri("localhost:55555/api/GetMerchant/1"); //Gets merchant 1, replace localhost:5555 with actual ip and port
                    var response = await client.GetAsync(uri);
                    string textResult = await response.Content.ReadAsStringAsync();
                    bool success = response.IsSuccessStatusCode;
                    TempData["Message"] = "Test Passed.";
                    return RedirectToAction("Index", "Home");
                }
            }

            catch
            {
                TempData["Message"] = "Test Failed.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> TestPostTransaction()
        {
            client.BaseAddress = new Uri("http://localhost:64195/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                TransactionPost transaction = new TransactionPost
                {
                    TransactionAmount = 1.00,
                    UserID = 1,
                    MerchantID = 1
                };

                string json = JsonConvert.SerializeObject(transaction);
                var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                var result = client.PostAsync(client.BaseAddress, content).Result;

                TempData["Message"] = "Test Passed.";
                return RedirectToAction("Index", "Home");
            }

            catch
            {
                TempData["Message"] = "Test Failed.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}