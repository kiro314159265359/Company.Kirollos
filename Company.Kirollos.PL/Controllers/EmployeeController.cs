using AutoMapper;
using Company.Kirollos.BLL;
using Company.Kirollos.BLL.Interfaces;
using Company.Kirollos.BLL.Repositories;
using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Dtos;
using Company.Kirollos.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Numerics;
using System.Threading.Tasks;

namespace Company.Kirollos.PL.Controllers
{
    [Authorize]
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
             , IMapper mapper)
        {
            #region GenericRepo
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository; 
            #endregion
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;

            if (!string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput)!;
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            #region Viewbag

            //ViewData["Message"] = "Hello from ViewData";
            //ViewBag.Message = "Hello from ViewBag"; 
            #endregion
           return View(employees);
        }

        public async Task<IActionResult> Search(string? SearchInput)
        {
            IEnumerable<Employee> employees;

            if (!string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput)!;
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            return PartialView("EmployeePartialView/EmployeesTablePartialView", employees);
        }

        [HttpGet]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (model is null) return BadRequest();
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

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

                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created !!";
                    return Redirect(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var Employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (Employee is null) return NotFound(new { StatusCode = 404, Message = "Can't Find Employee!" });

            return View(viewName, Employee);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");

            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;

            var Employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
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
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }

                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;

                _unitOfWork.EmployeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id, Employee employee)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (id == employee.Id)
            {
                _unitOfWork.EmployeeRepository.Delete(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    if (employee.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(employee.ImageName, "images");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employee);
        }
    }
}
