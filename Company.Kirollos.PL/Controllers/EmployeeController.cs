using AutoMapper;
using Company.Kirollos.BLL;
using Company.Kirollos.BLL.Interfaces;
using Company.Kirollos.BLL.Repositories;
using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace Company.Kirollos.PL.Controllers
{
    public class EmployeeController : Controller
    {
        #region GenericRepo
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository; 
        #endregion
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeController(
        #region GenericRepo
              //  IEmployeeRepository employeeRepository
              //, IDepartmentRepository departmentRepository 
        #endregion
              IUnitOfWork unitOfWork
             ,IMapper mapper)
        {
            #region GenericRepo
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository; 
            #endregion
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;

            if (!string.IsNullOrEmpty(SearchInput))
            {
                employees = _unitOfWork.EmployeeRepository.GetByName(SearchInput)!;
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            #region Viewbag

            //ViewData["Message"] = "Hello from ViewData";
            //ViewBag.Message = "Hello from ViewBag"; 
            #endregion
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (model is null) return BadRequest();
            if (ModelState.IsValid)
            {
                #region Manual mapping 
                //var employee = new Employee()
                //{
                //    Name = model.Name,
                //    Age = model.Age,
                //    Email = model.Email,
                //    Adress = model.Adress,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    HiringDate = model.HiringDate,
                //    CreateAt = model.CreateAt,
                //    DepartmentId = model.DepartmentId
                //};

                #endregion

                var employee = _mapper.Map<Employee>(model);

                _unitOfWork.EmployeeRepository.Add(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created !!";
                    return Redirect(nameof(Index));
                }
            }
            return View(model);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var Employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (Employee is null) return NotFound(new { StatusCode = 404, Message = "Can't Find Employee!" });

            return View(viewName, Employee);
        }

        public IActionResult Update(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");

            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;

            var Employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (Employee is null) return NotFound(new { StatusCode = 404, Message = "Can't Find Employee!" });

            #region Manual mapping
            //var employee = new CreateEmployeeDto()
            //{
            //    Name = Employee.Name,
            //    Age = Employee.Age,
            //    Email = Employee.Email,
            //    Adress = Employee.Adress,
            //    Phone = Employee.Phone,
            //    Salary = Employee.Salary,
            //    IsActive = Employee.IsActive,
            //    IsDeleted = Employee.IsDeleted,
            //    HiringDate = Employee.HiringDate,
            //    CreateAt = Employee.CreateAt,
            //    DepartmentId = Employee.DepartmentId
            //}; 
            #endregion

            var employee = _mapper.Map<CreateEmployeeDto>(Employee);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Update(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee employee)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (id == employee.Id)
            {
                _unitOfWork.EmployeeRepository.Delete(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employee);
        }
    }
}
