using AbpSolution1.Administration.Employee;
using AbpSolution1.Books;
using AbpSolution1.Dto.Administration.Employee;
using AutoMapper;

namespace AbpSolution1.Web;

public class AbpSolution1WebAutoMapperProfile : Profile
{
    public AbpSolution1WebAutoMapperProfile()
    {
        CreateMap<BookDto, CreateUpdateBookDto>();

        CreateMap<CreateUpdateEmployeeDto, Employees>();
        CreateMap<Employees, CreateUpdateEmployeeDto>();
        CreateMap<CreateOrEditForViewEmployeeDto, CreateUpdateEmployeeDto>();
        CreateMap<CreateUpdateEmployeeDto, CreateOrEditForViewEmployeeDto>();

        //Define your object mappings here, for the Web project
    }
}
