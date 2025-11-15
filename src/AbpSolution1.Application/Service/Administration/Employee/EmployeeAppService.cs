using AbpSolution1.Administration.Departmant;
using AbpSolution1.Administration.Employee;
using AbpSolution1.Dto.Administration.Employee;
using AbpSolution1.Interface.Administration.Employee;
using AbpSolution1.Localization;
using AbpSolution1.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace AbpSolution1.Service.Administration.Employee
{

    [Authorize(AbpSolution1Permissions.Employees.Default)]
    public class EmployeeAppService : AbpSolution1AppService, IEmployeeAppService
    {
        private readonly Volo.Abp.Domain.Repositories.IRepository<Employees, int> _employeeRepository;
        private readonly Volo.Abp.Domain.Repositories.IRepository<Departments, int> _departmentRepository;
        private readonly IStringLocalizer<AbpSolution1Resource> _localizer;
        private readonly IdentityUserManager _userManager;
        private readonly IRepository<IdentityUser, Guid> _userRepository;

        public EmployeeAppService(Volo.Abp.Domain.Repositories.IRepository<Employees, int> employeeRepository,
            IStringLocalizer<AbpSolution1Resource> localizer, Volo.Abp.Domain.Repositories.IRepository<Departments, int> departmentRepository,
            IdentityUserManager userManager,
        IRepository<IdentityUser, Guid> userRepository)
        {
            _employeeRepository = employeeRepository;
            _localizer = localizer;
            _departmentRepository = departmentRepository;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        private async Task<IQueryable<EmployeeDto>> EmployeesQuery(GetAllEmployeetInput input)
        {
            var employees = await _employeeRepository.GetQueryableAsync();
            var Employees = await _departmentRepository.GetQueryableAsync();

            var query = from e in employees
                        join d in Employees
                            on e.DepartmentId equals d.Id
                        where !e.IsDeleted && e.TenantId == CurrentTenant.Id
                        select new EmployeeDto
                        {
                            Id = e.Id,
                            FullName = e.FullName,
                            Code = e.Code,
                            DOB = e.DOB,
                            Address = e.Address,
                            NumberPhone = e.NumberPhone,
                            DepartmentId = e.DepartmentId,
                            Gender = e.Gender,
                            GenerText = DomainHelps.GenderText(e.Gender, _localizer),
                            DepartmentName = d.Name,

                        };

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(e => e.FullName.Contains(input.Filter) || e.Code.Contains(input.Filter));
            }

            if (input.Id.HasValue)
            {
                query = query.Where(e => e.Id == input.Id.Value);
            }

            return query;
        }



        public async Task<PagedResultDto<GetEmployeeForViewDto>> GetAll(GetAllEmployeetInput input)
        {
            var query = await EmployeesQuery(input);
            var totalCount = await query.CountAsync();

            //Loại bỏ "Employee." trong chuỗi sorting nếu có
            var sorting = input.Sorting?.Replace("employee.", "") ?? "Id desc";

            var pagedQuery = query
                .OrderBy(sorting)
                .PageBy(input);

            var list = await pagedQuery
                .Select(d => new GetEmployeeForViewDto { Employee = d })
                .ToListAsync();

            return new PagedResultDto<GetEmployeeForViewDto>(totalCount, list);
        }

        [Authorize(AbpSolution1Permissions.Employees.Edit)]
        public async Task<CreateUpdateEmployeeDto> GetForEmployeeId(GetAllEmployeetInput input)
        {
            var entity = await _employeeRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null)
            {
                throw new UserFriendlyException(L["NotFindEmployee"]);
            }

            return ObjectMapper.Map<Employees, CreateUpdateEmployeeDto>(entity);
        }

        // ===============================
        // 🧩 CREATE OR EDIT
        // ===============================
        [Authorize(AbpSolution1Permissions.Employees.Create)]
        [Authorize(AbpSolution1Permissions.Employees.Edit)]
        public async Task CreateOrEdit(CreateUpdateEmployeeDto input)
        {
            input.TenantId = CurrentTenant.Id;
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

        private async Task Create(CreateUpdateEmployeeDto input)
        {
            try
            {
                // 🔸 Kiểm tra mã trùng
                var existed = await _employeeRepository.AnyAsync(x => x.Code == input.Code && x.TenantId == CurrentTenant.Id && !x.IsDeleted);
                if (existed)
                    throw new UserFriendlyException($"Mã nhân viên '{input.Code}' đã tồn tại!");

                // 🔸 Map dữ liệu
                var employee = ObjectMapper.Map<CreateUpdateEmployeeDto, Employees>(input);
                employee.TenantId = CurrentTenant.Id;

                // 🔸 Tạo user trong hệ thống Identity
                var userId = GuidGenerator.Create();

                var newUser = new IdentityUser(
                    userId,
                    input.Code, // Username = Code
                    $"{input.Code}@company.local", // Email giả định
                    CurrentTenant.Id
                );

                newUser.Name = input.FullName;
                newUser.Surname = input.FullName;
                newUser.SetIsActive(true);
                newUser.SetEmailConfirmed(true);

                // 🔸 Tạo user
                var createResult = await _userManager.CreateAsync(newUser, "123456Az@@");
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new UserFriendlyException($"Không thể tạo user: {errors}");
                }

                // 🔸 Gán UserId cho nhân viên
                employee.UserId = userId;

                // 🔸 Lưu nhân viên
                await _employeeRepository.InsertAsync(employee, autoSave: true);
            }
            catch (UserFriendlyException)
            {
                // Giữ nguyên lỗi thân thiện (do ta chủ động ném ra)
                throw;
            }
            catch (Exception ex)
            {
                // Ghi log chi tiết để dễ debug trong ABP logs
                Logger.LogError(ex, "Lỗi khi tạo nhân viên.");

                // Hiển thị thông báo thân thiện cho người dùng
                var message = ex.InnerException?.Message ?? ex.Message;
                throw new UserFriendlyException($"Đã xảy ra lỗi khi tạo nhân viên: {message}");
            }
        }



        private async Task Update(CreateUpdateEmployeeDto input)
        {
            var employee = await _employeeRepository.GetAsync(input.Id.Value);

            ObjectMapper.Map(input, employee);

            // 🔹 Đồng bộ thông tin sang user (nếu có)
            if (employee.UserId.HasValue)
            {
                var user = await _userRepository.GetAsync(employee.UserId.Value);
                user.Name = input.FullName;
                user.Surname = input.FullName;
                await _userManager.UpdateAsync(user);
            }

            await _employeeRepository.UpdateAsync(employee, autoSave: true);
        }
        [Authorize(AbpSolution1Permissions.Employees.Delete)]
        public async Task Delete(int id)
        {
            var employee = await _employeeRepository.GetAsync(id);

            // 🔹 Xóa user nếu có
            if (employee.UserId.HasValue)
            {
                var user = await _userRepository.FindAsync(employee.UserId.Value);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            await _employeeRepository.DeleteAsync(employee);
        }



    }

}
