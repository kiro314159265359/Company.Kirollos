using Company.Kirollos.BLL.Interfaces;
using Company.Kirollos.BLL.Repositories;
using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Dtos;
using Humanizer;
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server side validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                var count = _departmentRepository.Add(department);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var result = _departmentRepository.Get(id);

            return View(result);
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            var result = _departmentRepository.Get(id);
            return View(result);
        }

        [HttpPost]
        public IActionResult Update(Department model)
        {
            if (model is not null)
            {
                var count = _departmentRepository.Update(model);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = _departmentRepository.Get(id);

            var department = new Department()
            {
                Id = id,
                Code = result.Code,
                Name = result.Name,
                CreateAt = result.CreateAt
            };
            return View(department);
        }

        [HttpPost]
        public IActionResult Delete(Department model)
        {
            if (model is not null)
            {
                var count = _departmentRepository.Delete(model);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

    }
}
