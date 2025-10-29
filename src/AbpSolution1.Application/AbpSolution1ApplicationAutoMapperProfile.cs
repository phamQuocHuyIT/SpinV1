using AutoMapper;
using AbpSolution1.Books;
using AbpSolution1.Dto.Administration;
using AbpSolution1.Administration.Departmant;
using AbpSolution1.Dto.Administration.Department;

namespace AbpSolution1;

public class AbpSolution1ApplicationAutoMapperProfile : Profile
{
    public AbpSolution1ApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        CreateMap<CreateUpdateDepartmentDto, Departments>();
        CreateMap<Departments, CreateUpdateDepartmentDto>();
        CreateMap<CreateOrEditForViewDepartment, CreateUpdateDepartmentDto>();
        CreateMap<CreateUpdateDepartmentDto, CreateOrEditForViewDepartment>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
