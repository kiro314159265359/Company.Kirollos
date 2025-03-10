using Company.Kirollos.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Kirollos.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // i want the T to be either employee or department only so i have to add constraint 
        // but we cant add two types it only can be one type  where T : Employee , Department
        public IEnumerable<T> GetAll(); // In DAL 

        public T? Get(int id); // may not return a department

        public int Add(T model);

        public int Update(T model);

        public int Delete(T model);
    }
}
