using CourseLab.BLL.ModelVM.EmployeeVM;
using CourseLab.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseLab.BLL.Services.Abstraction
{
    public interface IEmployeeService
    {
        Employee GetById(int id);
        bool Create(CreateEmployeeVM employee);
        List<GetAllEmployeeVM> GetAll();
        bool Update(EditEmployeeVM employee);
        bool Delete(int id);

    }
}
