using AbpSolution1.Administration.Customer;
using AbpSolution1.Dto.Administration.Customer;
using AutoMapper;
using AbpSolution1.Books;
using AbpSolution1.Dto.Administration;
using AbpSolution1.Administration.Departmant;
using AbpSolution1.Dto.Administration.Department;
using AbpSolution1.Dto.Administration.Employee;
using AbpSolution1.Administration.Employee;

namespace AbpSolution1;

public class AbpSolution1ApplicationAutoMapperProfile : Profile
{
    public AbpSolution1ApplicationAutoMapperProfile()
    {
        CreateMap<Customers, CustomerDto>();
        CreateMap<Customers, CreateUpdateCustomerDto>();
        
        CreateMap<CreateUpdateCustomerDto, Customers>()
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.CreatorId, opt => opt.Ignore())
            .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
            .ForMember(x => x.LastModifierId, opt => opt.Ignore())
            .ForMember(x => x.IsDeleted, opt => opt.Ignore())
            .ForMember(x => x.DeleterId, opt => opt.Ignore())
            .ForMember(x => x.DeletionTime, opt => opt.Ignore());

        CreateMap<CreateOrEditForViewCustomer, CreateUpdateCustomerDto>();
        CreateMap<CreateUpdateCustomerDto, CreateOrEditForViewCustomer>();
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();

        CreateMap<CreateUpdateDepartmentDto, Departments>();
        CreateMap<Departments, CreateUpdateDepartmentDto>();
        CreateMap<CreateOrEditForViewDepartment, CreateUpdateDepartmentDto>();
        CreateMap<CreateUpdateDepartmentDto, CreateOrEditForViewDepartment>();

        CreateMap<CreateUpdateEmployeeDto, Employees>();
        CreateMap<Employees, CreateUpdateEmployeeDto>();
        CreateMap<CreateOrEditForViewEmployeeDto, CreateUpdateEmployeeDto>();
        CreateMap<CreateUpdateEmployeeDto, CreateOrEditForViewEmployeeDto>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
