using CourseLab.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CourseLab.DAL.Repository.Abstraction
{
    public interface IEmployeeRepo
    {
        void Create(Employee emp);
        void Update(Employee newEmp);
        void Delete(Employee emp);
        List<Employee> GetAll(Expression<Func<Employee, bool>> filter);
        Employee GetById(int id);
    }
}
