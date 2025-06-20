using System.Threading.Tasks;
using APIConsumerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using APIConsumerMVC.DTO;


namespace APIConsumerMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient client;
        public EmployeeController(IHttpClientFactory factory)
        {
            client = factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:23398/api/");
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync("Employee/WithDept");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var empList = JsonConvert.DeserializeObject<List<EmpWithDept>>(data);
                return View(empList);
            }

            return View(new List<EmpWithDept>());
        }

        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"Employee/{id}");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var emp = JsonConvert.DeserializeObject<Employee>(data);
                return View("Details", emp);
            }
            return NotFound();
        }
    }
}
