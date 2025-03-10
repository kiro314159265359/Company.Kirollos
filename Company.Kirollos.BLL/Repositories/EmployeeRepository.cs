using Company.Kirollos.BLL.Interfaces;
using Company.Kirollos.DAL.Data.Contexts;
using Company.Kirollos.DAL.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Kirollos.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        //private readonly CompanyDbContext _context;
        //public EmployeeRepository(CompanyDbContext context)
        //{
        //    _context = context;
        //}
        //public int Add(Employee model)
        //{
        //    _context.Employees.Add(model);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Employee model)
        //{
        //     _context.Employees.ToList();
        //    return _context.SaveChanges();
        //}

        //public Employee? Get(int id)
        //{
        //    return _context.Employees.Find(id);
        //}

        //public IEnumerable<Employee> GetAll()
        //{

        //    return _context.Employees.ToList();
        //}

        //public int Update(Employee model)
        //{
        //    _context.Employees.Update(model);
        //    return _context.SaveChanges();
        //}
        public EmployeeRepository(CompanyDbContext context) : base(context)
        {
        }
    }
}
