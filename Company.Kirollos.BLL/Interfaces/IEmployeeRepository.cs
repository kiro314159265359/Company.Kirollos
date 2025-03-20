using Company.Kirollos.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Kirollos.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        //public IEnumerable<Employee> GetAll(); // In DAL 

        //public Employee? Get(int id); // may not return a department

        //public int Add(Employee model);

        //public int Update(Employee model);

        //public int Delete(Employee model);

        public Task<List<Employee>> GetByNameAsync(string name); 
    }
}
