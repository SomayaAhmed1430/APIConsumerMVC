using APIConsumerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace APIConsumerMVC.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly HttpClient client;
        public DepartmentController(IHttpClientFactory factory)
        {
            client = factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:23398/api/");
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync("Department");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var depts = JsonConvert.DeserializeObject<List<Department>>(data);
                return View(depts);
            }
            return View(new List<Department>());
        }

        public IActionResult New()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SaveNew(Department deptFromReq)
        {
            var data = JsonConvert.SerializeObject(deptFromReq);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("Department", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("New", deptFromReq);
        }


        public async Task<IActionResult> Edit(int id)
        {
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
            HttpResponseMessage response = await client.DeleteAsync($"Department/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return Content("Failed to delete");
        }


    }
}
