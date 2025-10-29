using AbpSolution1.Administration.Departmant;
using AbpSolution1.Dto.Administration.Department;
using AbpSolution1.Interface.Administration.Department;
using AbpSolution1.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
namespace AbpSolution1.Service.Administration.Department
{
    [Authorize(AbpSolution1Permissions.Departments.Default)]
    public class DepartmentAppService: ApplicationService, IDepartmentAppService
    {
        private readonly IRepository<Departments, int> _departmentRepository;
        public DepartmentAppService(IRepository<Departments, int> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        private async Task<IQueryable<DepartmentDto>> QueyryDepartments(GetAllDepartmentInput input)
        {
            var query = await _departmentRepository.GetQueryableAsync();

            var filteredQuery = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    d => d.Name.Contains(input.Filter)
                      || d.Code.Contains(input.Filter)
                      || (d.Note != null && d.Note.Contains(input.Filter))
                )
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Code = d.Code,
                    Note = d.Note,
                    IsActive = d.IsActive
                });

            return filteredQuery;
        }

        public async Task<PagedResultDto<GetDepartmentForViewDto>> GetAll(GetAllDepartmentInput input)
        {
            // Lấy IQueryable từ repository
            var query = await QueyryDepartments(input); // ⬅ await

            // Đếm tổng trước khi phân trang
            var totalCount = await query.CountAsync();

            // Phân trang & sorting
            var pagedQuery = query
                .OrderBy(input.Sorting ?? "Id desc")  // default sắp xếp theo Id desc
                .PageBy(input);                       // extension của ABP dùng Skip + Take

            // Map sang GetDepartmentForViewDto
            var list = await pagedQuery
                .Select(d => new GetDepartmentForViewDto
                {
                    Department = d // ⬅ d đã là DepartmentDto
                })
                .ToListAsync();

            // Trả về PagedResultDto
            return new PagedResultDto<GetDepartmentForViewDto>(
                totalCount,
                list
            );
        }


    }
}
