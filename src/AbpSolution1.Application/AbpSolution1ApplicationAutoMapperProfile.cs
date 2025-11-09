using AbpSolution1.Administration.Customer;
using AbpSolution1.Administration.Departmant;
using AbpSolution1.Administration.Employee;
using AbpSolution1.Books;
using AbpSolution1.Config.Product;
using AbpSolution1.Config.Spins;
using AbpSolution1.Dto.Administration;
using AbpSolution1.Dto.Administration.Customer;
using AbpSolution1.Dto.Administration.Department;
using AbpSolution1.Dto.Administration.Employee;
using AbpSolution1.Dto.Config.Product;
using AbpSolution1.Dto.Config.Spin;
using AutoMapper;

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

        CreateMap<CreateUpdateEmployeeDto, Employees>();
        CreateMap<Employees, CreateUpdateEmployeeDto>();
        CreateMap<CreateOrEditForViewEmployeeDto, CreateUpdateEmployeeDto>();
        CreateMap<CreateUpdateEmployeeDto, CreateOrEditForViewEmployeeDto>();

        CreateMap<CreateUpdateCustomerDto, Customers>().ReverseMap();
        CreateMap<CreateOrEditForViewCustomerDto, CreateUpdateCustomerDto>().ReverseMap();

        CreateMap<CreateUpdateProductDto, Products>().ReverseMap();
        CreateMap<CreateOrEditForViewProductDto, CreateUpdateProductDto>().ReverseMap();

        CreateMap<CreateUpdateSpinDto, Spin>().ReverseMap();
        CreateMap<CreateOrEditForViewSpinDto, CreateUpdateSpinDto>().ReverseMap();
        CreateMap<SpinDto, CreateUpdateSpinDto>().ReverseMap();
        CreateMap<SpinProductDto, SpinProduct>().ReverseMap();
        CreateMap<SpinCustomerDto, SpinCustomer>().ReverseMap();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
