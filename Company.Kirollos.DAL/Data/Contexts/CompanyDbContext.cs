using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Company.Kirollos.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Kirollos.DAL.Data.Contexts
{
    public class CompanyDbContext : DbContext
    {
        // base() doesn't do anything and it chains on DbContextOptions
        // It does not call OnConfiguring(), because OnConfiguring()
        // is only called if options are not provided in the constructor.
        // When you pass DbContextOptions<T> to the base constructor (base(options)),
        // EF Core assumes configuration is already handled and skips OnConfiguring().

        // to call OnConfiguring we need to make object from CompanyDbContext
        // and to make object from CompanyDbContext we need DbContextOptions
        // and DbContextOptions needs a connection string so we must send the 
        // connection string with the DbContextOptions<CompanyDbContext> options
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = DESKTOP-PEC2QL2\\SQLEXPRESS; Database = MVCProject; Trusted_Connection = True; TrustServerCertificate = True");
        //}

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
