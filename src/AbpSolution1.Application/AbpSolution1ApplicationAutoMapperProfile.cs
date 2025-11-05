using AbpSolution1.Administration.Customer;
using AbpSolution1.Dto.Administration.Customer;
using AutoMapper;
using AbpSolution1.Books;
using AbpSolution1.Dto.Administration;
using AbpSolution1.Administration.Departmant;
using AbpSolution1.Dto.Administration.Department;
using AbpSolution1.Dto.Administration.Employee;
using AbpSolution1.Administration.Employee;

namespace AbpSolution1
{
    public class AbpSolution1ApplicationAutoMapperProfile : Profile
    {
        public AbpSolution1ApplicationAutoMapperProfile()
        {
            // -----------------------------
            // ✅ Customer Mapping
            // -----------------------------
            CreateMap<Customers, CustomerDto>();
            CreateMap<Customers, CreateUpdateCustomerDto>();

            CreateMap<CreateUpdateCustomerDto, Customers>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.IsDeleted, opt => opt.Ignore())
                .ForMember(x => x.DeleterId, opt => opt.Ignore())
                .ForMember(x => x.DeletionTime, opt => opt.Ignore())
                .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
                .ForMember(x => x.LastModifierId, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.CreatorId, opt => opt.Ignore())
                .ForMember(x => x.ExtraProperties, opt => opt.Ignore())
                .ForMember(x => x.ConcurrencyStamp, opt => opt.Ignore());

            CreateMap<CreateOrEditForViewCustomer, CreateUpdateCustomerDto>();
            CreateMap<CreateUpdateCustomerDto, CreateOrEditForViewCustomer>();

            // -----------------------------
            // ✅ Book Mapping
            // -----------------------------
            CreateMap<Book, BookDto>();

            CreateMap<CreateUpdateBookDto, Book>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
                .ForMember(x => x.LastModifierId, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.CreatorId, opt => opt.Ignore())
                .ForMember(x => x.ExtraProperties, opt => opt.Ignore())
                .ForMember(x => x.ConcurrencyStamp, opt => opt.Ignore());

            // -----------------------------
            // ✅ Department Mapping
            // -----------------------------
            CreateMap<CreateUpdateDepartmentDto, Departments>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Employees, opt => opt.Ignore())
                .ForMember(x => x.IsDeleted, opt => opt.Ignore())
                .ForMember(x => x.DeleterId, opt => opt.Ignore())
                .ForMember(x => x.DeletionTime, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.CreatorId, opt => opt.Ignore())
                .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
                .ForMember(x => x.LastModifierId, opt => opt.Ignore());


            CreateMap<Departments, CreateUpdateDepartmentDto>();
            CreateMap<CreateOrEditForViewDepartment, CreateUpdateDepartmentDto>()
                .ForMember(x => x.TenantId, opt => opt.Ignore());
            CreateMap<CreateUpdateDepartmentDto, CreateOrEditForViewDepartment>();

            // -----------------------------
            // ✅ Employee Mapping
            // -----------------------------
            CreateMap<CreateUpdateEmployeeDto, Employees>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.IsDeleted, opt => opt.Ignore())
                .ForMember(x => x.DeleterId, opt => opt.Ignore())
                .ForMember(x => x.DeletionTime, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.CreatorId, opt => opt.Ignore())
                .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
                .ForMember(x => x.LastModifierId, opt => opt.Ignore());

            CreateMap<Employees, CreateUpdateEmployeeDto>();
            CreateMap<CreateOrEditForViewEmployeeDto, CreateUpdateEmployeeDto>();
            CreateMap<CreateUpdateEmployeeDto, CreateOrEditForViewEmployeeDto>();
        }
    }
}
