using AbpSolution1.Dto.Administration;
using AbpSolution1.Dto.Administration.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpSolution1.Interface.Administration.Department
{
    public interface IDepartmentAppService: IApplicationService
    {
        Task<PagedResultDto<GetDepartmentForViewDto>> GetAll(GetAllDepartmentInput input);
        Task<CreateUpdateDepartmentDto> GetForDepartmentId(GetAllDepartmentInput input);
        Task CreateOrEdit(CreateUpdateDepartmentDto input);

        Task Delete(int id);
    }
}
