namespace APIConsumerMVC.DTO
{
    public class DeptWithEmps
    {
        public string DeptName { get; set; }
        public string Description { get; set; }

        public List<string> EmpNames { get; set; }
    }
}
