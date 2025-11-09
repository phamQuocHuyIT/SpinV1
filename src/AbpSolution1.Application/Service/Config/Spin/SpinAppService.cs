using AbpSolution1.Config.Spins;
using AbpSolution1.Dto.Config.Product;
using AbpSolution1.Interface.Config.Spins;
using AbpSolution1.Permissions;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using System.Linq.Dynamic.Core;
using Volo.Abp;
using AbpSolution1.Dto.Config.Spin;
using AbpSolution1.Config.Product;

namespace AbpSolution1.Service.Config.Spin
{
    public class SpinAppService: AbpSolution1AppService, ISpinAppService
    {
        private readonly Volo.Abp.Domain.Repositories.IRepository<AbpSolution1.Config.Spins.Spin, int> _spinRepository;
        private readonly Volo.Abp.Domain.Repositories.IRepository<SpinProduct, int> _spinProductRepositpry;
        private readonly IRepository<SpinCustomer, int> _spinCustomerRepository;
        private readonly IRepository<Products, int> _productRepository; 

        public SpinAppService(Volo.Abp.Domain.Repositories.IRepository<AbpSolution1.Config.Spins.Spin, int> ProductRepository, IRepository<SpinProduct, int> spinProductRepositpry, IRepository<SpinCustomer, int> spinCustomerRepository,
            IRepository<Products, int> productRepository)
        {
            _spinRepository = ProductRepository;
            _spinProductRepositpry = spinProductRepositpry;
            _spinCustomerRepository = spinCustomerRepository;
            _productRepository = productRepository;
        }

        private async Task<IQueryable<SpinDto>> SpinQuery(GetAllSpinInput input)
        {
            var query = await _spinRepository.GetQueryableAsync();

            var filteredQuery = query
                .Where(d => !d.IsDeleted && d.TenantId == CurrentTenant.Id)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    d => d.Name.Contains(input.Filter ?? "")
                      || d.Code.Contains(input.Filter ?? "")
                      || (d.Note != null && d.Note.Contains(input.Filter ?? ""))
                )
                .WhereIf(input.Id.HasValue, x => x.Id == input.Id)
                .Select(d => new SpinDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Code = d.Code,
                    Note = d.Note,
                    IsActive = d.IsActive,
                    TenantId = d.TenantId,

                    // Lấy danh sách sản phẩm và khách hàng
                    Products = d.SpinProducts.Select(p => new SpinProductDto
                    {
                        SpinId = p.Id,
                        ProductId = p.ProductId,
                        Proportion = p.Proportion,
                        IsDefault = p.IsDefault
                    }).ToList(),

                    Customers = d.SpinCustomers.Select(c => new SpinCustomerDto
                    {
                        SpinId = c.Id,
                        CustomerId = c.CustomerId,
                        SpinCount = c.SpinCount
                    }).ToList()
                });

            return filteredQuery;
        }


        public async Task<PagedResultDto<GetSpinForViewDto>> GetAll(GetAllSpinInput input)
        {
            var query = await SpinQuery(input);
            var totalCount = await query.CountAsync();

            //Loại bỏ "Product." trong chuỗi sorting nếu có
            var sorting = input.Sorting?.Replace("spin.", "") ?? "Id desc";

            var pagedQuery = query
                .OrderBy(sorting)
                .PageBy(input);

            var list = await pagedQuery
                .Select(d => new GetSpinForViewDto { Spin = d })
                .ToListAsync();

            return new PagedResultDto<GetSpinForViewDto>(totalCount, list);
        }

        [Authorize(AbpSolution1Permissions.Spins.Edit)]
        public async Task<CreateUpdateSpinDto> GetForSpinId(GetAllSpinInput input)
        {
            var entity = await _spinRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null)
            {
                throw new UserFriendlyException(L["NotFindProduct"]);
            }

            return ObjectMapper.Map<AbpSolution1.Config.Spins.Spin, CreateUpdateSpinDto>(entity);
        }

        private async Task<IQueryable<SpinDto>> SpinQueryBy(GetAllSpinInput input)
        {
            var query = await _spinRepository.GetQueryableAsync();

            var filteredQuery = query
                .Where(d => !d.IsDeleted && d.TenantId == CurrentTenant.Id)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    d => d.Name.Contains(input.Filter ?? "")
                      || d.Code.Contains(input.Filter ?? "")
                      || (d.Note != null && d.Note.Contains(input.Filter ?? ""))
                )
                .WhereIf(input.Id.HasValue, x => x.Id == input.Id)
                .WhereIf(!input.Id.HasValue, x => x.IsDefault)
                .Select(d => new SpinDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Code = d.Code,
                    Note = d.Note,
                    IsActive = d.IsActive,
                    TenantId = d.TenantId,

                    // Lấy danh sách sản phẩm và khách hàng
                    Products = d.SpinProducts.Select(p => new SpinProductDto
                    {
                        SpinId = p.Id,
                        ProductId = p.ProductId,
                        Proportion = p.Proportion,
                        IsDefault = p.IsDefault
                    }).ToList(),

                    Customers = d.SpinCustomers.Select(c => new SpinCustomerDto
                    {
                        SpinId = c.Id,
                        CustomerId = c.CustomerId,
                        SpinCount = c.SpinCount
                    }).ToList()
                });

            return filteredQuery;
        }

        public async Task<PagedResultDto<GetSpinForViewDto>> GetSpinByEmployee(GetAllSpinInput input)
        {
            var getSpinid = await _spinCustomerRepository.FirstOrDefaultAsync(x => x.CustomerId == input.CustomerId);
            input.Id = getSpinid?.SpinId;
            var query = await SpinQueryBy(input);
            var totalCount = await query.CountAsync();

            //Loại bỏ "Product." trong chuỗi sorting nếu có
            var sorting = input.Sorting?.Replace("spin.", "") ?? "Id desc";

            var pagedQuery = query
                .OrderBy(sorting)
                .PageBy(input);

            var list = await pagedQuery
                .Select(d => new GetSpinForViewDto { Spin = d })
                .ToListAsync();
            foreach ( var item in list)
            {
                foreach (var item2 in item.Spin.Products)
                {
                    var getNameProduct = await _productRepository.FirstOrDefaultAsync(x => x.Id == item2.ProductId);
                    item2.ProductName = getNameProduct?.Name;
                }
            }

            return new PagedResultDto<GetSpinForViewDto>(totalCount, list);
        }

        // ===============================
        // 🧩 CREATE OR EDIT
        // ===============================
        [Authorize(AbpSolution1Permissions.Spins.Create)]
        [Authorize(AbpSolution1Permissions.Spins.Edit)]
        public async Task CreateOrEdit(CreateUpdateSpinDto input)
        {
            input.TenantId = CurrentTenant.Id;
            if (input == null)
            {
                throw new UserFriendlyException(L["ObjectNull"]);
            }

            if (input.IsEdit)
            {
                await Update(input);
            }
            else
            {

                await Create(input);
            }
        }

        private async Task Create(CreateUpdateSpinDto input)
        {
            // Kiểm tra trùng mã
            var existed = await _spinRepository.AnyAsync(x => x.Code == input.Code);
            if (existed)
            {
                throw new UserFriendlyException($"Mã vòng quay '{input.Code}' đã tồn tại!");
            }

            input.TenantId = CurrentTenant.Id;
            var entity = ObjectMapper.Map<CreateUpdateSpinDto, AbpSolution1.Config.Spins.Spin>(input);

            // Tạo Spin
            var spin = await _spinRepository.InsertAsync(entity, autoSave: true);

            // Tạo SpinProduct nếu có
            if (input.Products?.Any() == true)
            {
                foreach (var product in input.Products)
                {
                    product.SpinId = spin.Id;
                    var spinProduct = ObjectMapper.Map<SpinProductDto, SpinProduct>(product);
                    await _spinProductRepositpry.InsertAsync(spinProduct, autoSave: true);
                }
            }

            // Tương tự, tạo SpinCustomer nếu có
            if (input.Customers?.Any() == true)
            {
                foreach (var customer in input.Customers)
                {
                    customer.SpinId = spin.Id;
                    var spinCustomer = ObjectMapper.Map<SpinCustomerDto, SpinCustomer>(customer);
                    await _spinCustomerRepository.InsertAsync(spinCustomer, autoSave: true);
                }
            }
        }


        private async Task Update(CreateUpdateSpinDto input)
        {
            // Lấy Spin theo Id
            var entity = await _spinRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (entity == null)
                throw new UserFriendlyException("Không tìm thấy Spin cần cập nhật!");

            // Kiểm tra trùng mã với Spin khác
            var existed = await _spinRepository.AnyAsync(x => x.Code == input.Code && x.Id != input.Id);
            if (existed)
                throw new UserFriendlyException($"Mã Spin '{input.Code}' đã tồn tại!");

            // Map dữ liệu từ DTO
            input.TenantId = CurrentTenant.Id;
            ObjectMapper.Map(input, entity);

            // Cập nhật Spin
            await _spinRepository.UpdateAsync(entity, autoSave: true);

            // ===== Xoá tất cả SpinProduct cũ =====
            var oldProducts = await _spinProductRepositpry.GetListAsync(x => x.SpinId == entity.Id);
            if (oldProducts.Any())
            {
                await _spinProductRepositpry.DeleteManyAsync(oldProducts, autoSave: false);
            }

            // ===== Thêm danh sách SpinProduct mới =====
            if (input.Products?.Any() == true)
            {
                var newProducts = input.Products.Select(p =>
                {
                    p.SpinId = entity.Id;
                    return ObjectMapper.Map<SpinProductDto, SpinProduct>(p);
                }).ToList();

                await _spinProductRepositpry.InsertManyAsync(newProducts, autoSave: false);
            }

            // ===== Xoá tất cả SpinCustomer cũ =====
            var oldCustomers = await _spinCustomerRepository.GetListAsync(x => x.SpinId == entity.Id);
            if (oldCustomers.Any())
            {
                await _spinCustomerRepository.DeleteManyAsync(oldCustomers, autoSave: false);
            }

            // ===== Thêm danh sách SpinCustomer mới =====
            if (input.Customers?.Any() == true)
            {
                var newCustomers = input.Customers.Select(c =>
                {
                    c.SpinId = entity.Id;
                    return ObjectMapper.Map<SpinCustomerDto, SpinCustomer>(c);
                }).ToList();

                await _spinCustomerRepository.InsertManyAsync(newCustomers, autoSave: false);
            }

            // Lưu tất cả một lần
            await CurrentUnitOfWork.SaveChangesAsync();
        }





        [Authorize(AbpSolution1Permissions.Spins.Delete)]
        public async Task Delete(int id)
        {
            await _spinCustomerRepository.DeleteAsync(x => x.SpinId == id);
            await _spinProductRepositpry.DeleteAsync(x => x.SpinId == id);
            await _spinRepository.DeleteAsync(x => x.Id == id);
        }

        
    }
}
