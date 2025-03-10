using Company.Kirollos.BLL.Interfaces;
using Company.Kirollos.BLL.Repositories;
using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Dtos;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

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
        [ValidateAntiForgeryToken]
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
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = _departmentRepository.Get(id.Value);
            if (department is null) return NotFound(new { StatusCode = 404, message = $"Department with Id:{id} Not Found!" });

            return View(viewName,department);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id");
            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(new { StatusCode = 404, message = $"Department with Id:{id} Not Found!" });

            return Details(id,"Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        #region UpdateWays
        //public IActionResult Update([FromRoute] int id, CreateDepartmentDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department()
        //        {
        //            Id = id,
        //            Code = model.Code,
        //            Name = model.Name,
        //            CreateAt = model.CreateAt
        //        };
        //        var count = _departmentRepository.Update(department);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(model);
        //} 
        #endregion
        public IActionResult Update([FromRoute] int id, Department model)
        {
            if (ModelState.IsValid)
            {
                if (id == model.Id)
                {
                    var count = _departmentRepository.Update(model);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //    if (id is null) return BadRequest("Invalid Id");
            //    var department = _departmentRepository.Get(id.Value);
            //    if (department is null) return NotFound(new { StatusCode = 404, message = $"Department with Id:{id} Not Found!" });
            return Details(id,"Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute]int id , Department model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest();               
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
