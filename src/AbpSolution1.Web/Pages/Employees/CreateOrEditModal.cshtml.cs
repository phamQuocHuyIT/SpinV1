using AbpSolution1.Dto.Administration;
using AbpSolution1.Dto.Administration.Department;
using AbpSolution1.Dto.Administration.Employee;
using AbpSolution1.Interface.Administration.Department;
using AbpSolution1.Interface.Administration.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AbpSolution1.Web.Pages.Employees
{
    public class CreateOrEditModalModel : AbpSolution1PageModel
    {
        [HiddenInput]
        public int? Id { get; set; }

        [BindProperty]
        public CreateOrEditForViewEmployeeDto Employee { get; set; }

        public int? DepartmentId {  get; set; }

        public List<SelectListItem> Departments { get; set; }

        private readonly IEmployeeAppService _employeeAppService;
        private readonly IDepartmentAppService _departmentAppService;

        public CreateOrEditModalModel(IEmployeeAppService employeeAppService, IDepartmentAppService departmentAppService)
        {
            _employeeAppService = employeeAppService;
            _departmentAppService = departmentAppService;
        }

        public async Task OnGetAsync(int? id)
        {
            var query = new CreateUpdateEmployeeDto { Code = string.Empty, FullName= string.Empty} ;
            var departments = await _departmentAppService.GetLookupAsync();
            Departments = departments.Items
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToList();
            if(id.HasValue)
            {
                query = await _employeeAppService.GetForEmployeeId(new GetAllEmployeetInput { Id = id.Value });
            }
            
            if (query != null)
            {
                if (query.Id.HasValue)
                {


                    Employee = ObjectMapper.Map<CreateUpdateEmployeeDto, CreateOrEditForViewEmployeeDto>(query);
                    Id = query.Id;
                    DepartmentId = query.DepartmentId;

                }
            }
            else
            {
                Employee = new CreateOrEditForViewEmployeeDto
                {
                    Code = string.Empty,
                    FullName = string.Empty,
                    IsActive = true
                };
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                // ⚠️ Nếu validation fail (ví dụ trường Name trống)
                return Page();
            }
            if (Employee != null)
            {
                CreateUpdateEmployeeDto query = ObjectMapper.Map<CreateOrEditForViewEmployeeDto, CreateUpdateEmployeeDto>(Employee);
                if (Id.HasValue)
                {
                    query.Id = id.Value;
                }
                await _employeeAppService.CreateOrEdit(query);
                // ✅ Cho phép modal đóng lại
            }
            else
            {
                return Page();
            }
            return NoContent();

        }
    }

}
