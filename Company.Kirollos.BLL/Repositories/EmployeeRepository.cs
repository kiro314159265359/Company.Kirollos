using Company.Kirollos.BLL.Interfaces;
using Company.Kirollos.DAL.Data.Contexts;
using Company.Kirollos.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Kirollos.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        #region Notes
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
        #endregion

        private readonly CompanyDbContext _context;
        public EmployeeRepository(CompanyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetByNameAsync(string name)
        {
            return await _context.Employees.Include(E => E.Department)
                           .Where(E => E.Name.ToLower().Contains(name.ToLower()))
                           .ToListAsync();
        }
    }
}
