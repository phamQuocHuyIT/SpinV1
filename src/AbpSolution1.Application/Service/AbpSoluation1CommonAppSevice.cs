using AbpSolution1.Administration.Employee;
using AbpSolution1.Config.Spins;
using AbpSolution1.Dto.Config.Spin.HistorySpin;
using AbpSolution1.Interface;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
namespace AbpSolution1.Service
{
    public class AbpSoluation1CommonAppSevice : AbpSolution1AppService, IAbpSoluation1CommonAppSevice
    {
        private readonly Volo.Abp.Domain.Repositories.IRepository<HistorySpin, int> _historySpinRepository;
        public readonly IRepository<Employees, int> _employeeRepository;

        public AbpSoluation1CommonAppSevice(Volo.Abp.Domain.Repositories.IRepository<HistorySpin, int> historySpinRepository, IRepository<Employees, int> employeeRepository)
        {
            _historySpinRepository = historySpinRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task CreateHistorySpin(CreateUpdateHistorySpinDto input)
        {
            var checkEmployee = await _employeeRepository.FirstOrDefaultAsync(e => e.UserId == CurrentUser.Id && e.TenantId == CurrentTenant.Id);
            if(checkEmployee != null)
            {
                input.EmployeeId = checkEmployee.Id;
            }
            input.RewardDate = DateTime.Now;
            input.TenantId = CurrentTenant.Id;

            var entity = ObjectMapper.Map<CreateUpdateHistorySpinDto, HistorySpin>(input);
            await _historySpinRepository.InsertAsync(entity, autoSave: true);
        }


    }
}
