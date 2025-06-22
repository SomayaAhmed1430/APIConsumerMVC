using APIConsumerMVC.Models;

namespace APIConsumerMVC.ViewModels
{
    public class EmployeeFormViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public int DepartmentId { get; set; }

        public List<Department> Departments { get; set; }
    }
}
