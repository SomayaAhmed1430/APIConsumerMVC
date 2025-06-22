using System.Threading.Tasks;
using APIConsumerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using APIConsumerMVC.DTO;
using APIConsumerMVC.ViewModels;
using System.Text;


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


        public async Task<IActionResult> New() 
        {
            var model = new EmployeeFormViewModel();

            HttpResponseMessage response = await client.GetAsync($"Department");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                model.Departments = JsonConvert.DeserializeObject<List<Department>>(data);
            }
            else
            {
                model.Departments = new List<Department>();
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SaveNew(EmployeeFormViewModel vm)
        {
            Employee emp = new Employee
            {
                Name = vm.Name,
                Address = vm.Address,
                DepartmentId = vm.DepartmentId
            };

            var data = JsonConvert.SerializeObject(emp);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("Employee", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // If error, reload department list again
            HttpResponseMessage deptResponse = await client.GetAsync("Department");
            if (deptResponse.IsSuccessStatusCode)
            {
                string deptData = await deptResponse.Content.ReadAsStringAsync();
                vm.Departments = JsonConvert.DeserializeObject<List<Department>>(deptData);
            }

            return View("New", vm);
        }



        public async Task<IActionResult> Edit(int id)
        {
            var empResponse = await client.GetAsync($"Employee/{id}");
            var deptResponse = await client.GetAsync("Department");

            if (!empResponse.IsSuccessStatusCode || !deptResponse.IsSuccessStatusCode)
                return NotFound();

            var empData = await empResponse.Content.ReadAsStringAsync();
            var deptData = await deptResponse.Content.ReadAsStringAsync();

            var emp = JsonConvert.DeserializeObject<Employee>(empData);
            var departments = JsonConvert.DeserializeObject<List<Department>>(deptData);

            var vm = new EmployeeFormViewModel
            {
                Id = emp.Id,
                Name = emp.Name,
                Address = emp.Address,
                DepartmentId = emp.DepartmentId,
                Departments = departments
            };

            return View("Edit", vm);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeFormViewModel vm)
        {
            var updatedEmp = new Employee
            {
                Id = vm.Id,
                Name = vm.Name,
                Address = vm.Address,
                DepartmentId = vm.DepartmentId
            };

            var jsonData = JsonConvert.SerializeObject(updatedEmp);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"Employee/{vm.Id}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View("Edit", vm); 
        }




        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"Employee/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return NotFound(); 
        }


    }
}
