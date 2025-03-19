using Company.Kirollos.BLL.Interfaces;
using Company.Kirollos.BLL.Repositories;
using Company.Kirollos.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Kirollos.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _context;

        // it was a Reserch to try the lazy loading so i googled it 
        private Lazy<IDepartmentRepository> _departmentRepository { get; }
        private Lazy<IEmployeeRepository> _employeeRepository { get; }
        public IDepartmentRepository DepartmentRepository => _departmentRepository.Value;
        public IEmployeeRepository EmployeeRepository => _employeeRepository.Value;
        public UnitOfWork(CompanyDbContext context)
        {
            _context = context;
            _departmentRepository = new Lazy<IDepartmentRepository>(() => new DepartmentRepository(_context));
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_context));
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }


        #region Without the LazyLoading 
        //public class UnitOfWork : IUnitOfWork
        //{
        //    private readonly CompanyDbContext _context;
        //    public IDepartmentRepository DepartmentRepository { get; }

        //    public IEmployeeRepository EmployeeRepository { get; }

        //    public UnitOfWork(CompanyDbContext context)
        //    {
        //        _context = context;
        //        DepartmentRepository = new DepartmentRepository(_context);
        //        EmployeeRepository = new EmployeeRepository(_context);
        //    }
        //} 
        #endregion
    }
}
