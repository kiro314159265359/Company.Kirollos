﻿using AutoMapper;
using Company.Kirollos.BLL;
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
        #region .
        //private readonly IDepartmentRepository _departmentRepository; 
        #endregion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #region How Injection works
        // ASk clr to create object from DepartmentRepository 
        // Dependency injection
        // here DepartmentRepository isn't a controller but we want to create object 
        // so we add Scooped func  
        #endregion
        public DepartmentController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var department = _unitOfWork.DepartmentRepository.GetAll();

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
                #region Manual mapping
                //var department = new Department()
                //{
                //    Code = model.Code,
                //    Name = model.Name,
                //    CreateAt = model.CreateAt
                //}; 
                #endregion

                var department = _mapper.Map<Department>(model);

                _unitOfWork.DepartmentRepository.Add(department);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department is null) return NotFound(new { StatusCode = 404, message = $"Department with Id:{id} Not Found!" });

            return View(viewName, department);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department is null) return NotFound(new { StatusCode = 404, message = $"Department with Id:{id} Not Found!" });

            #region Manual mapping
            //var departmentDto = new CreateDepartmentDto()
            //{
            //    Code = department.Code,
            //    Name = department.Name,
            //    CreateAt = department.CreateAt
            //};
            #endregion
            var departmentDto = _mapper.Map<CreateDepartmentDto>(department);

            return View(departmentDto);
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
                var departmentDto = _mapper.Map<Department>(model);

                _unitOfWork.DepartmentRepository.Update(departmentDto);
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
            #region Keep Code Clean
            //    if (id is null) return BadRequest("Invalid Id");
            //    var department = _departmentRepository.Get(id.Value);
            //    if (department is null) return NotFound(new { StatusCode = 404, message = $"Department with Id:{id} Not Found!" }); 
            #endregion
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department model)
        {
            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);

                if (id != department.Id) return BadRequest();
                _unitOfWork.DepartmentRepository.Delete(department);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }          
            return View(model);
        }

    }
}
