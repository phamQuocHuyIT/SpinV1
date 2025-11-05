using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using AbpSolution1.Administration.Customer;
using AbpSolution1.Dto.Administration.Customer;
using AbpSolution1.Interface.Administration.Customer;
using AbpSolution1.Localization;
using AbpSolution1.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace AbpSolution1.Service.Administration.Customer
{
    public class CustomerAppService : AbpSolution1AppService, ICustomerAppService
    {
        private readonly IRepository<Customers, int> _customerRepository;

        public CustomerAppService(IRepository<Customers, int> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        private async Task<IQueryable<CustomerDto>> CustomerQuery(GetAllCustomerInput input)
        {
            var query = await _customerRepository.GetQueryableAsync();

            var filteredQuery = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    d => d.Code.Contains(input.Filter)
                      || d.FullName.Contains(input.Filter)
                      || (d.Note != null && d.Note.Contains(input.Filter))
                )
                .WhereIf(input.Id.HasValue, x => x.Id == input.Id)
                .Select(d => new CustomerDto
                {
                    Id = d.Id,
                    Code = d.Code,
                    FullName = d.FullName,
                    Address = d.Address,
                    NumberPhone = d.NumberPhone,
                    Rank = d.Rank,
                    Total = d.Total,
                    IsDelete = d.IsDelete,
                    Note = d.Note,
                    IsActive = d.IsActive
                });

            return filteredQuery;
        }

        public async Task<PagedResultDto<GetCustomerForViewDto>> GetAll(GetAllCustomerInput input)
        {
            try
            {
                var query = await CustomerQuery(input);
                var totalCount = await query.CountAsync();

                var sorting = input.Sorting?.Replace("customer.", "") ?? "Id desc";

                var pagedQuery = query
                    .OrderBy(sorting)
                    .PageBy(input);

                var list = await pagedQuery
                    .Select(d => new GetCustomerForViewDto { Customer = d })
                    .ToListAsync();

                return new PagedResultDto<GetCustomerForViewDto>(totalCount, list);
            }
            catch (SqlException sqlEx) when (sqlEx.Message.Contains("Invalid object name", StringComparison.OrdinalIgnoreCase))
            {
                // Table missing: log and return empty result. User should run DbMigrator or apply EF migrations.
                Logger.LogWarning(sqlEx, "Database table for Customers not found. Run DbMigrator or apply EF migrations to create 'Spin_Customers' table.");
                return new PagedResultDto<GetCustomerForViewDto>(0, Array.Empty<GetCustomerForViewDto>());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetAll customers failed");
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<CreateUpdateCustomerDto> GetForCustomerId(GetAllCustomerInput input)
        {
            var entity = await _customerRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null)
            {
                throw new UserFriendlyException(L["NotFindCustomer"]);
            }

            return ObjectMapper.Map<Customers, CreateUpdateCustomerDto>(entity);
        }

        public async Task CreateOrEdit(CreateUpdateCustomerDto input)
        {
            if (input == null)
            {
                throw new UserFriendlyException(L["ObjectNull"]);
            }

            if (input.Id.HasValue)
            {
                await Update(input);
            }
            else
            {
                await Create(input);
            }
        }

        private async Task Create(CreateUpdateCustomerDto input)
        {
            var existed = await _customerRepository.AnyAsync(x => x.Code == input.Code);
            if (existed)
            {
                throw new UserFriendlyException($"Mã khách hàng '{input.Code}' đã tồn tại!");
            }

            var entity = ObjectMapper.Map<CreateUpdateCustomerDto, Customers>(input);
            await _customerRepository.InsertAsync(entity, autoSave: true);
        }

        private async Task Update(CreateUpdateCustomerDto input)
        {
            var entity = await _customerRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (entity == null)
            {
                throw new UserFriendlyException("Không tìm thấy khách hàng cần cập nhật!");
            }

            var existed = await _customerRepository.AnyAsync(x => x.Code == input.Code && x.Id != input.Id);
            if (existed)
            {
                throw new UserFriendlyException($"Mã khách hàng '{input.Code}' đã tồn tại!");
            }

            ObjectMapper.Map(input, entity);
            await _customerRepository.UpdateAsync(entity, autoSave: true);
        }

        public async Task Delete(int id)
        {
            await _customerRepository.DeleteAsync(x => x.Id == id);
        }
    }
}
