using AbpSolution1.Dto.Administration;
using AbpSolution1.Dto.Administration.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace AbpSolution1.Interface.Administration.Customer
{
   public interface ICustomerAppService
    {
        Task<PagedResultDto<GetCustomerForViewDto>> GetAll(GetAllCustomerInput input);
        Task<CreateUpdateCustomerDto> GetForCustomerId(GetAllCustomerInput input);
        Task CreateOrEdit(CreateUpdateCustomerDto input);

        Task Delete(int id);
    }
}
