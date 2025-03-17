using AutoMapper;
using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Dtos;

namespace Company.Kirollos.PL.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDto, Department>().ReverseMap();
        }
    }
}
