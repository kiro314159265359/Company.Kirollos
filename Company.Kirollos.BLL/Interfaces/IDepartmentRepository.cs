using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Kirollos.DAL.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;

namespace Company.Kirollos.BLL.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        //public IEnumerable<Department> GetAll(); // In DAL 

        //public Department? Get(int id); // may not return a department

        //public int Add(Department model);

        //public int Update(Department model);

        //public int Delete(Department model);
    }
}
