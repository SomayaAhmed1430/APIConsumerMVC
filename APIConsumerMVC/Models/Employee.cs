﻿namespace APIConsumerMVC.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
