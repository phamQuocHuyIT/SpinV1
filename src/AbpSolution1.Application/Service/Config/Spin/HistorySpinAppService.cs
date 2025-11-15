using AbpSolution1.Administration.Customer;
using AbpSolution1.Administration.Employee;
using AbpSolution1.Config.Product;
using AbpSolution1.Config.Spins;
using AbpSolution1.Dto.Administration.Department;
using AbpSolution1.Dto.Administration.Employee;
using AbpSolution1.Dto.Config.Spin.HistorySpin;
using AbpSolution1.Interface.Config.Product;
using AbpSolution1.Interface.Config.Spins;
using AbpSolution1.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
namespace AbpSolution1.Service.Config.Spin
{
    [Authorize(AbpSolution1Permissions.HistorySpins.Default)]
    public class HistorySpinAppService : AbpSolution1AppService, IHistorySpinAppService
    {
        private readonly Volo.Abp.Domain.Repositories.IRepository<HistorySpin, int> _historySpinRepository;
        private readonly Volo.Abp.Domain.Repositories.IRepository<Customers, int> _customterRepository;
        private readonly Volo.Abp.Domain.Repositories.IRepository<Products, int> _productRepository;
        private readonly Volo.Abp.Domain.Repositories.IRepository<Employees, int> _employeeRepository;
        private readonly Volo.Abp.Domain.Repositories.IRepository<AbpSolution1.Config.Spins.Spin, int> _spinRepository;
        public HistorySpinAppService(Volo.Abp.Domain.Repositories.IRepository<HistorySpin, int> historySpinRepository,
            Volo.Abp.Domain.Repositories.IRepository<Customers, int> customterRepository, Volo.Abp.Domain.Repositories.IRepository<Products, int> productRepository,
            Volo.Abp.Domain.Repositories.IRepository<Employees, int> employeeRepository, Volo.Abp.Domain.Repositories.IRepository<AbpSolution1.Config.Spins.Spin, int> spinRepository) 
        {
                _historySpinRepository = historySpinRepository;
                _customterRepository = customterRepository;
                _productRepository = productRepository;
                _employeeRepository = employeeRepository;
                _spinRepository = spinRepository;
        }

        private async Task<IQueryable<HistorySpinDto>> HistorySpinQuery(GetAllDepartmentInput input)
        {
            var historySpins = await _historySpinRepository.GetQueryableAsync();
            var historySpinsList = historySpins.ToList();
            var spin = await _spinRepository.GetQueryableAsync();
            var product = await _productRepository.GetQueryableAsync();
            var customers  = await _customterRepository.GetQueryableAsync();
            var employees = await _employeeRepository.GetQueryableAsync();
            var query =
                        from his in historySpins
                        where !his.IsDeleted && his.TenantId == CurrentTenant.Id
                        join s in spin on his.SpinId equals s.Id
                        join p in product on his.ProductId equals p.Id
                        join c in customers on his.CustomerId equals c.Id into cusJoin
                        from c in cusJoin.DefaultIfEmpty() // LEFT JOIN
                        join emp in employees on his.EmployeeId equals emp.Id into empJoin
                        from emp in empJoin.DefaultIfEmpty()
                        select new HistorySpinDto
                        {
                            Id = his.Id,
                            SpinId = his.SpinId,
                            SpinName = s.Name,
                            ProductId = his.ProductId,
                            ProductName = p.Name,
                            RewardDate = his.RewardDate,
                            CustomerId = his.CustomerId,  // giữ nguyên
                            CustomerName = c != null ? c.FullName : "Khách vãng lai",

                            EmployeeId = his.EmployeeId,
                            EmployeeName = emp != null ? emp.FullName : "Admin",
                        };


            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(e => e.EmployeeName.Contains(input.Filter) || e.CustomerName.Contains(input.Filter) || e.ProductName.Contains(input.Filter) || e.SpinName.Contains(input.Filter));
            }

            if (input.Id.HasValue)
            {
                query = query.Where(e => e.Id == input.Id.Value);
            }

            return query;


        }

        public async Task<PagedResultDto<GetHistorySpinForViewDto>> GetAll(GetAllDepartmentInput input)
        {
            try
            {
                var query = await HistorySpinQuery(input);
                var totalCount = await query.CountAsync();

                //Loại bỏ "department." trong chuỗi sorting nếu có
                var sorting = input.Sorting?.Replace("historySpin.", "").Replace("name", "spinName") ?? "Id desc";

                var pagedQuery = query
                    .OrderBy(x => x.Id)
                    .PageBy(input);

                var list = await pagedQuery
                    .Select(d => new GetHistorySpinForViewDto { HistorySpin = d })
                    .ToListAsync();

                return new PagedResultDto<GetHistorySpinForViewDto>(totalCount, list);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Erorr", ex.Message);
            }
            
        }
    }
}
