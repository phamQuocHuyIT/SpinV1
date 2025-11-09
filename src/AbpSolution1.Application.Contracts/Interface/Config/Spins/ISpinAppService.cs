using AbpSolution1.Dto.Config.Spin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpSolution1.Interface.Config.Spins
{
    public interface ISpinAppService: IApplicationService
    {
        Task<PagedResultDto<GetSpinForViewDto>> GetAll(GetAllSpinInput input);
        Task<CreateUpdateSpinDto> GetForSpinId(GetAllSpinInput input);
        Task CreateOrEdit(CreateUpdateSpinDto input);

        Task<PagedResultDto<GetSpinForViewDto>> GetSpinByEmployee(GetAllSpinInput input);
        Task Delete(int id);
    }
}
