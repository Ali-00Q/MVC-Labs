using CourseLab.BLL.ModelVM.EmployeeVM;
using CourseLab.BLL.Services.Abstraction;
using CourseLab.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CourseLab.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService; 
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var employee = employeeService.GetById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        public IActionResult GetAll()
        {
            var result = employeeService.GetAll();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult SaveData(CreateEmployeeVM model)
        {
            if (ModelState.IsValid)
            {
                var result = employeeService.Create(model);
                if (result)
                {
                    return RedirectToAction("GetAll");
                }
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var employee = employeeService.GetAll().FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            var model = new EditEmployeeVM
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                ImagePath = employee.ImagePath,
            };
            return View(model);
        }

        public IActionResult SaveEditData(EditEmployeeVM model)
        {
            if (ModelState.IsValid)
            {
                var result = employeeService.Update(model);
                if (result)
                {
                    return RedirectToAction("GetAll");
                }
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var result = employeeService.Delete(id);
            if (result)
            {
                return RedirectToAction("GetAll");
            }
            return NotFound();
        }



        public IActionResult SetSession(int id)
        {
            var emp = employeeService.GetById(id);
            if(emp == null)
            {
                return NotFound();
            }

            var json = JsonSerializer.Serialize(emp);
            HttpContext.Session.SetString("employee-details", json);
            return RedirectToAction("GetSession");
        }

        public IActionResult GetSession()
        {
            var json = HttpContext.Session.GetString("employee-details");
            if (string.IsNullOrEmpty(json))
            {
                return View(null);
            }

            var emp = JsonSerializer.Deserialize<Employee>(json);
            return View(emp);
        }
    }
}
