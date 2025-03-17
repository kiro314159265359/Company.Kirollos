using AutoMapper;
using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Dtos;

namespace Company.Kirollos.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<Employee, CreateEmployeeDto>();
        }
    }
}
