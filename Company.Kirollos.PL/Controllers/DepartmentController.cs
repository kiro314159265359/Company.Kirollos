using Company.Kirollos.BLL.Interfaces;
using Company.Kirollos.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Company.Kirollos.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        // ASk clr to create object from DepartmentRepository 
        // Dependency injection
        // here DepartmentRepository isn't a controller but we want to create object 
        // so we add Scooped func 
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var department = _departmentRepository.GetAll();

            return View(department);
        }
    }
}
