using AbpSolution1.Administration.Customer;
using AbpSolution1.Dto.Administration.Customer;
using AutoMapper;

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
    }
}
