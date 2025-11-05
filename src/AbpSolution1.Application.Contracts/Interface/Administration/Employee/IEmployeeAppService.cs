using AbpSolution1.Dto.Administration.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpSolution1.Interface.Administration.Employee
{
    public interface IEmployeeAppService: IApplicationService
    {
        Task<PagedResultDto<GetEmployeeForViewDto>> GetAll(GetAllEmployeetInput input);
        Task<CreateUpdateEmployeeDto> GetForEmployeeId(GetAllEmployeetInput input);
        Task CreateOrEdit(CreateUpdateEmployeeDto input);
        Task Delete(int id);
    }
}
