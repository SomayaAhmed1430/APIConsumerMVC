using Microsoft.AspNetCore.Mvc;
using APIConsumerMVC.ViewModels;
using APIConsumerMVC.Models;
using Newtonsoft.Json;
using System.Text;

namespace APIConsumerMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient client;

        public AccountController(IHttpClientFactory factory)
        {
            client = factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:23398/api/");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var data = JsonConvert.SerializeObject(model);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("Account/Register", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Registered successfully!";
                return RedirectToAction("Login");
            }

            var error = await response.Content.ReadAsStringAsync();
            ViewBag.Error = error;
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var data = JsonConvert.SerializeObject(model);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("Account/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                dynamic tokenObj = JsonConvert.DeserializeObject(result);
                string token = tokenObj.token;

                HttpContext.Session.SetString("JWToken", token);

                return RedirectToAction("Index", "Home");
            }

            var error = await response.Content.ReadAsStringAsync();
            ViewBag.Error = error;
            return View(model);
        }
    }
}
