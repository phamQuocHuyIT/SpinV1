using AbpSolution1.Dto.Config.Spin.HistorySpin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpSolution1.Interface
{
    public interface IAbpSoluation1CommonAppSevice : IApplicationService
    {
        Task CreateHistorySpin(CreateUpdateHistorySpinDto input);
    }
}
