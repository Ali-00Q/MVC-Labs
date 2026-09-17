using System;
using System.Collections.Generic;
using System.Text;

namespace CourseLab.DAL.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal? Salary { get; set; }
        public string? ImagePath { get; set; }
    }
}
