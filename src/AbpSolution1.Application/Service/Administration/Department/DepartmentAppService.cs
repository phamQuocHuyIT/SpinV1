using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using AbpSolution1.Administration.Departmant;
using AbpSolution1.Dto.Administration;
using AbpSolution1.Dto.Administration.Department;
using AbpSolution1.Interface.Administration.Department;
using AbpSolution1.Localization;
using AbpSolution1.MultiTenancy;
using AbpSolution1.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.TenantManagement;

namespace AbpSolution1.Service.Administration.Department
{
    [Authorize(AbpSolution1Permissions.Departments.Default)]
    public class DepartmentAppService : AbpSolution1AppService, IDepartmentAppService
    {
        private readonly Volo.Abp.Domain.Repositories.IRepository<Departments, int> _departmentRepository;

        public DepartmentAppService(Volo.Abp.Domain.Repositories.IRepository<Departments, int> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        private async Task<IQueryable<DepartmentDto>> DepartmentsQuery(GetAllDepartmentInput input)
        {
            var query = await _departmentRepository.GetQueryableAsync();

            var filteredQuery = query
                .Where(d => !d.IsDeleted && d.TenantId == CurrentTenant.Id)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    d => d.Name.Contains(input.Filter != null ? input.Filter : "")
                      || d.Code.Contains(input.Filter != null ? input.Filter : "")
                      || (d.Note != null && d.Note.Contains(input.Filter != null ? input.Filter : ""))
                )
                .WhereIf(input.Id.HasValue, x => x.Id == input.Id)
                .Select(d => new DepartmentDto
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

        public async Task<PagedResultDto<GetDepartmentForViewDto>> GetAll(GetAllDepartmentInput input)
        {
            var query = await DepartmentsQuery(input);
            var totalCount = await query.CountAsync();

            //Loại bỏ "department." trong chuỗi sorting nếu có
            var sorting = input.Sorting?.Replace("department.", "") ?? "Id desc";

            var pagedQuery = query
                .OrderBy(sorting)
                .PageBy(input);

            var list = await pagedQuery
                .Select(d => new GetDepartmentForViewDto { Department = d })
                .ToListAsync();

            return new PagedResultDto<GetDepartmentForViewDto>(totalCount, list);
        }

        [Authorize(AbpSolution1Permissions.Departments.Edit)]
        public async Task<CreateUpdateDepartmentDto> GetForDepartmentId(GetAllDepartmentInput input)
        {
            var entity = await _departmentRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null)
            {
                throw new UserFriendlyException(L["NotFindDepartment"]);
            }

            return ObjectMapper.Map<Departments, CreateUpdateDepartmentDto>(entity);
        }

        // ===============================
        // 🧩 CREATE OR EDIT
        // ===============================
        [Authorize(AbpSolution1Permissions.Departments.Create)]
        [Authorize(AbpSolution1Permissions.Departments.Edit)]
        public async Task CreateOrEdit(CreateUpdateDepartmentDto input)
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

        private async Task Create(CreateUpdateDepartmentDto input)
        {
            // Kiểm tra trùng mã
            var existed = await _departmentRepository.AnyAsync(x => x.Code == input.Code);
            if (existed)
            {
                throw new UserFriendlyException($"Mã phòng ban '{input.Code}' đã tồn tại!");
            }
            input.TenantId = CurrentTenant.Id;
            var entity = ObjectMapper.Map<CreateUpdateDepartmentDto, Departments>(input);
            await _departmentRepository.InsertAsync(entity, autoSave: true);
        }

        private async Task Update(CreateUpdateDepartmentDto input)
        {
            var entity = await _departmentRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (entity == null)
            {
                throw new UserFriendlyException("Không tìm thấy phòng ban cần cập nhật!");
            }

            // Kiểm tra trùng mã với phòng ban khác
            var existed = await _departmentRepository.AnyAsync(x => x.Code == input.Code && x.Id != input.Id);
            if (existed)
            {
                throw new UserFriendlyException($"Mã phòng ban '{input.Code}' đã tồn tại!");
            }
            input.TenantId = CurrentTenant.Id;
            // Map lại các giá trị
            ObjectMapper.Map(input, entity);

            // Cập nhật DB
            await _departmentRepository.UpdateAsync(entity, autoSave: true);
        }
        [Authorize(AbpSolution1Permissions.Departments.Delete)]
        public async Task Delete(int id)
        {
            await _departmentRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<ListResultDto<DepartmentLookupDto>> GetLookupAsync()
        {
            var departments = await _departmentRepository.GetListAsync();
            var list = departments
                .Select(x => new DepartmentLookupDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            return new ListResultDto<DepartmentLookupDto>(list);
        }

    }
}
