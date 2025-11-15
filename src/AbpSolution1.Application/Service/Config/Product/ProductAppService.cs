using AbpSolution1.Config.Product;
using AbpSolution1.Dto.Config.Product;
using AbpSolution1.Interface.Config.Product;
using AbpSolution1.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;

namespace AbpSolution1.Service.Config.Product
{
    public class ProductAppService: AbpSolution1AppService, IProductAppService
    {
        private readonly Volo.Abp.Domain.Repositories.IRepository<Products, int> _productRepository;

        public ProductAppService(Volo.Abp.Domain.Repositories.IRepository<Products, int> ProductRepository)
        {
            _productRepository = ProductRepository;
        }

        private async Task<IQueryable<ProductDto>> ProductsQuery(GetAllProductInput input)
        {
            var query = await _productRepository.GetQueryableAsync();

            var filteredQuery = query
                .Where(d => !d.IsDeleted && d.TenantId == CurrentTenant.Id)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    d => d.Name.Contains(input.Filter != null ? input.Filter : "")
                      || d.Code.Contains(input.Filter != null ? input.Filter : "")
                      || (d.Note != null && d.Note.Contains(input.Filter != null ? input.Filter : ""))
                )
                .WhereIf(input.Id.HasValue, x => x.Id == input.Id)
                .Select(d => new ProductDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Code = d.Code,
                    Note = d.Note,
                    IsActive = d.IsActive,
                    TenantId = d.TenantId
                });

            return filteredQuery;
        }

        public async Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductInput input)
        {
            var query = await ProductsQuery(input);
            var totalCount = await query.CountAsync();

            //Loại bỏ "Product." trong chuỗi sorting nếu có
            var sorting = input.Sorting?.Replace("product.", "") ?? "Id desc";

            var pagedQuery = query
                .OrderBy(sorting)
                .PageBy(input);

            var list = await pagedQuery
                .Select(d => new GetProductForViewDto { Product = d })
                .ToListAsync();

            return new PagedResultDto<GetProductForViewDto>(totalCount, list);
        }

        [Authorize(AbpSolution1Permissions.Products.Edit)]
        public async Task<CreateUpdateProductDto> GetForProductId(GetAllProductInput input)
        {
            var entity = await _productRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null)
            {
                throw new UserFriendlyException(L["NotFindProduct"]);
            }

            return ObjectMapper.Map<Products, CreateUpdateProductDto>(entity);
        }

        // ===============================
        // 🧩 CREATE OR EDIT
        // ===============================
        [Authorize(AbpSolution1Permissions.Products.Create)]
        [Authorize(AbpSolution1Permissions.Products.Edit)]
        public async Task CreateOrEdit(CreateUpdateProductDto input)
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

        private async Task Create(CreateUpdateProductDto input)
        {
            input.TenantId = CurrentTenant.Id;
            // Kiểm tra trùng mã
            var existed = await _productRepository.AnyAsync(x => x.Code == input.Code && x.TenantId == input.TenantId && !x.IsDeleted);
            if (existed)
            {
                throw new UserFriendlyException($"Mã sản phẩm '{input.Code}' đã tồn tại!");
            }
            
            var entity = ObjectMapper.Map<CreateUpdateProductDto, Products>(input);
            await _productRepository.InsertAsync(entity, autoSave: true);
        }

        private async Task Update(CreateUpdateProductDto input)
        {
            input.TenantId = CurrentTenant.Id;
            var entity = await _productRepository.FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == input.TenantId && !x.IsDeleted);
            if (entity == null)
            {
                throw new UserFriendlyException("Không tìm thấy sản phẩm cần cập nhật!");
            }

            // Kiểm tra trùng mã với sản phẩm khác
            var existed = await _productRepository.AnyAsync(x => x.Code == input.Code && x.Id != input.Id && x.TenantId == input.TenantId && !x.IsDeleted);
            if (existed)
            {
                throw new UserFriendlyException($"Mã sản phẩm '{input.Code}' đã tồn tại!");
            }
            
            // Map lại các giá trị
            ObjectMapper.Map(input, entity);

            // Cập nhật DB
            await _productRepository.UpdateAsync(entity, autoSave: true);
        }
        [Authorize(AbpSolution1Permissions.Products.Delete)]
        public async Task Delete(int id)
        {
            await _productRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<ListResultDto<ProductLookupDto>> GetLookupAsync()
        {
            var Products = await _productRepository.GetListAsync( x => !x.IsDeleted && x.TenantId == CurrentTenant.Id);
            var list = Products
                .Select(x => new ProductLookupDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            return new ListResultDto<ProductLookupDto>(list);
        }
    }
}
