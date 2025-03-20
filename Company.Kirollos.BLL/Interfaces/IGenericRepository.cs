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
        public Task<IEnumerable<T>> GetAllAsync(); // In DAL 

        public Task<T?> GetAsync(int id); // may not return a department

        public Task AddAsync(T model);

        public void Update(T model);

        public void Delete(T model);
    }
}
