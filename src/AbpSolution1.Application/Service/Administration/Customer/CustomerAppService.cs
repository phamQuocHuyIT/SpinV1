using AbpSolution1.Administration.Customer;
using AbpSolution1.Dto.Administration;
using AbpSolution1.Dto.Administration.Customer;
using AbpSolution1.Interface.Administration.Customer;
using AbpSolution1.Localization;
using AbpSolution1.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;
using Volo.Abp;

namespace AbpSolution1.Service.Administration.Customer
{
    [Authorize(AbpSolution1Permissions.Customers.Default)]
    public class CustomerAppService: AbpSolution1AppService, ICustomerAppService
    {
        private readonly Volo.Abp.Domain.Repositories.IRepository<Customers, int> _customerRepository;
        private readonly IStringLocalizer<AbpSolution1Resource> _localizer;
        public CustomerAppService(Volo.Abp.Domain.Repositories.IRepository<Customers, int> CustomerRepository, IStringLocalizer<AbpSolution1Resource> localizer)
        {
            _customerRepository = CustomerRepository;
            _localizer = localizer;
        }

        private async Task<IQueryable<CustomerDto>> CustomersQuery(GetAllCustomertInput input)
        {
            var query = await _customerRepository.GetQueryableAsync();

            var filteredQuery = query
                .Where(d => !d.IsDeleted && d.TenantId == CurrentTenant.Id)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    d => d.FullName.Contains(input.Filter != null ? input.Filter : "")
                      || d.Code.Contains(input.Filter != null ? input.Filter : "")
                )
                .WhereIf(input.Id.HasValue, x => x.Id == input.Id)
                .Select(d => new CustomerDto
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    Code = d.Code,
                    IsActive = d.IsActive,
                    TenantId = d.TenantId,
                    Address = d.Address,
                    DOB = d.DOB,
                    Gender = d.Gender,
                    GenerText = DomainHelps.GenderText(d.Gender, _localizer),
                    NumberPhone = d.NumberPhone,
                    Ranked = d.Ranked,
                    RankedName = DomainHelps.RankedText(d.Ranked, _localizer),
                    TotalPurchase = d.TotalPurchase
                });

            return filteredQuery;
        }

        public async Task<PagedResultDto<GetCustomerForViewDto>> GetAll(GetAllCustomertInput input)
        {
            var query = await CustomersQuery(input);
            var totalCount = await query.CountAsync();

            //Loại bỏ "Customer." trong chuỗi sorting nếu có
            var sorting = input.Sorting?.Replace("customer.", "") ?? "Id desc";

            var pagedQuery = query
                .OrderBy(sorting)
                .PageBy(input);

            var list = await pagedQuery
                .Select(d => new GetCustomerForViewDto { Customer = d })
                .ToListAsync();

            return new PagedResultDto<GetCustomerForViewDto>(totalCount, list);
        }

        [Authorize(AbpSolution1Permissions.Customers.Edit)]
        public async Task<CreateUpdateCustomerDto> GetForCustomerId(GetAllCustomertInput input)
        {
            var entity = await _customerRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null)
            {
                throw new UserFriendlyException(L["NotFindCustomer"]);
            }

            return ObjectMapper.Map<Customers, CreateUpdateCustomerDto>(entity);
        }

        // ===============================
        // 🧩 CREATE OR EDIT
        // ===============================
        [Authorize(AbpSolution1Permissions.Customers.Create)]
        [Authorize(AbpSolution1Permissions.Customers.Edit)]
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
            // Kiểm tra trùng mã
            var existed = await _customerRepository.AnyAsync(x => x.Code == input.Code);
            if (existed)
            {
                throw new UserFriendlyException($"Mã phòng ban '{input.Code}' đã tồn tại!");
            }
            input.TenantId = CurrentTenant.Id;
            var entity = ObjectMapper.Map<CreateUpdateCustomerDto, Customers>(input);
            await _customerRepository.InsertAsync(entity, autoSave: true);
        }

        private async Task Update(CreateUpdateCustomerDto input)
        {
            var entity = await _customerRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (entity == null)
            {
                throw new UserFriendlyException("Không tìm thấy phòng ban cần cập nhật!");
            }

            // Kiểm tra trùng mã với phòng ban khác
            var existed = await _customerRepository.AnyAsync(x => x.Code == input.Code && x.Id != input.Id);
            if (existed)
            {
                throw new UserFriendlyException($"Mã phòng ban '{input.Code}' đã tồn tại!");
            }
            input.TenantId = CurrentTenant.Id;
            // Map lại các giá trị
            ObjectMapper.Map(input, entity);

            // Cập nhật DB
            await _customerRepository.UpdateAsync(entity, autoSave: true);
        }
        [Authorize(AbpSolution1Permissions.Customers.Delete)]
        public async Task Delete(int id)
        {
            await _customerRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<ListResultDto<CustomerLookupDto>> GetLookupAsync()
        {
            var Customers = await _customerRepository.GetListAsync();
            var list = Customers
                .Select(x => new CustomerLookupDto
                {
                    Id = x.Id,
                    Name = x.FullName
                }).ToList();

            return new ListResultDto<CustomerLookupDto>(list);
        }
    }
}
