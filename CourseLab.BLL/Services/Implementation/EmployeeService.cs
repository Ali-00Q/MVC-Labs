using CourseLab.BLL.Helper;
using CourseLab.BLL.ModelVM.EmployeeVM;
using CourseLab.BLL.Services.Abstraction;
using CourseLab.DAL.Entities;
using CourseLab.DAL.Repository.Abstraction;
using CourseLab.DAL.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseLab.BLL.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo employeeRepo;
        public EmployeeService(IEmployeeRepo employeeRepo)
        {
            this.employeeRepo = employeeRepo;
        }

        public Employee GetById(int id)
        {
            return employeeRepo.GetById(id);
        }
        public bool Create(CreateEmployeeVM employee)
        {
            if (string.IsNullOrEmpty(employee.Name))
            {
                throw new Exception("Employee name is required");
            }
            else if (employee.Age <= 0)
            {
                throw new Exception("Employee age must be greater than zero");
            }

            string Image = Upload.UploadFile("Files", employee.Image);

            Employee newEmployee = new Employee{Name = employee.Name, Age = employee.Age, ImagePath = Image};

            employeeRepo.Create(newEmployee);
            return true;
        }

        public List<GetAllEmployeeVM> GetAll()
        {
            return employeeRepo.GetAll(e => true).Select(e => new GetAllEmployeeVM
            {
                Id = e.Id,
                Name = e.Name,
                Age = e.Age,
                ImagePath = e.ImagePath
            }).ToList();
        }

        public bool Update(EditEmployeeVM newEmployee)
        {
            Employee oldEmployee = employeeRepo.GetById(newEmployee.Id);
            if (oldEmployee == null)
            {
                throw new Exception("Employee not found");
            }
            
            oldEmployee.Name = newEmployee.Name;
            oldEmployee.Age = newEmployee.Age;

            if (newEmployee.Image != null && newEmployee.Image.Length > 0)
            {
                if (!string.IsNullOrEmpty(oldEmployee.ImagePath))
                {
                    Upload.RemoveFile("Files", oldEmployee.ImagePath);
                }
                oldEmployee.ImagePath = Upload.UploadFile("Files", newEmployee.Image);
            }
            employeeRepo.Update(oldEmployee);
            return true;
        }

        public bool Delete(int id)
        {
            Employee employee = employeeRepo.GetById(id);
            if (employee == null)
            {
                throw new Exception("Employee not found");
            }
            employeeRepo.Delete(employee);
            return true;
        }
    }
}
