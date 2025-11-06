using AbpSolution1.Dto.Administration;
using AbpSolution1.Dto.Administration.Customer;
using AbpSolution1.Dto.Administration.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpSolution1.Interface.Administration.Customer
{
    public interface ICustomerAppService: IApplicationService
    {
        Task<PagedResultDto<GetCustomerForViewDto>> GetAll(GetAllCustomertInput input);
        Task<CreateUpdateCustomerDto> GetForCustomerId(GetAllCustomertInput input);
        Task CreateOrEdit(CreateUpdateCustomerDto input);

        Task Delete(int id);
        Task<ListResultDto<CustomerLookupDto>> GetLookupAsync();
    }
}
