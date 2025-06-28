using APIConsumerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using APIConsumerMVC.DTO;
using System.Net.Http.Headers;

namespace APIConsumerMVC.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartmentController(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _factory = factory;
            _httpContextAccessor = httpContextAccessor;
        }


        private HttpClient CreateClientWithToken()
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:23398/api/");

            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }




        public async Task<IActionResult> Index()
        {
            var client = CreateClientWithToken();
            HttpResponseMessage response = await client.GetAsync("Department/EmpCount");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var depts = JsonConvert.DeserializeObject<List<DeptWithEmp>>(data);
                return View(depts);
            }
            return View(new List<DeptWithEmp>());
        }

        public IActionResult New()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> SaveNew(Department deptFromReq)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWToken")))
            {
                return RedirectToAction("Login", "Account");
            }

            var client = CreateClientWithToken();

            var data = JsonConvert.SerializeObject(deptFromReq);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("Department", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("New", deptFromReq);
        }


        public async Task<IActionResult> Details(string name)
        {
            var client = CreateClientWithToken();
            HttpResponseMessage response = await client.GetAsync($"Department/WithEmployees");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                Console.WriteLine(data);  

                var allDepts = JsonConvert.DeserializeObject<List<DeptWithEmps>>(data);
                var dept = allDepts.FirstOrDefault(d => d.DeptName == name);
                return View(dept);
            }

            return NotFound();
        }




        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            HttpResponseMessage response = await client.GetAsync($"Department/{id}");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var dept = JsonConvert.DeserializeObject<Department>(data);
                return View(dept);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> SaveEdit(Department deptFromForm)
        {
            var client = CreateClientWithToken();
            var data = JsonConvert.SerializeObject(deptFromForm);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"Department/{deptFromForm.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Edit", deptFromForm);
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWToken")))
            {
                return RedirectToAction("Login", "Account");
            }

            var client = CreateClientWithToken();

            HttpResponseMessage response = await client.DeleteAsync($"Department/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return Content("Failed to delete");
        }


    }
}
