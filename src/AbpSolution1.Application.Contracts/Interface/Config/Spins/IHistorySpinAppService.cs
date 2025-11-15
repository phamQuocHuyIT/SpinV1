using AbpSolution1.Dto.Administration.Department;
using AbpSolution1.Dto.Config.Spin.HistorySpin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpSolution1.Interface.Config.Spins
{
    public interface IHistorySpinAppService: IApplicationService
    {
        Task<PagedResultDto<GetHistorySpinForViewDto>> GetAll(GetAllDepartmentInput input);
    }
}
