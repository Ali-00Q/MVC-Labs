using CourseLab.DAL.Database;
using CourseLab.DAL.Entities;
using CourseLab.DAL.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CourseLab.DAL.Repository.Implementation
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly CourseLabDbContext db;

        public EmployeeRepo(CourseLabDbContext db)
        {
            this.db = db;
        }
        public void Create(Employee emp)
        {
            try
            {
                var result = db.Employees.Add(emp);
                db.SaveChanges();
                if(result.Entity.Id > 0)
                {
                    Console.WriteLine("Employee Created Successfully");
                }
                else
                {
                    throw new Exception("Employee Creation Failed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Employee> GetAll(Expression<Func<Employee, bool>> filter)
        {
            var result = db.Employees.Where(filter).ToList();
            return result;
        }

        public Employee GetById(int id)
        {
            var result = db.Employees.Where(e => e.Id == id).FirstOrDefault();
            if(result == null)
            {
                throw new Exception("Employee Not Found");
            }
            return result;
        }

        public void Update(Employee newEmp)
        {
            db.Employees.Update(newEmp);
            db.SaveChanges();

        }

        public void Delete(Employee emp)
        {
            db.Employees.Remove(emp);
            db.SaveChanges();
        }
    }
}
