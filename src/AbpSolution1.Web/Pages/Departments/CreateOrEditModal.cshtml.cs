using AbpSolution1.Dto.Administration;
using AbpSolution1.Dto.Administration.Department;
using AbpSolution1.Interface.Administration.Department;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace AbpSolution1.Web.Pages.Departments
{
    public class CreateOrEditModalModel : AbpSolution1PageModel
    {
        // ✅ Thuộc tính Id chỉ dùng để xác định tạo/sửa, không cần SupportsGet
        [HiddenInput]
        public int? Id { get; set; }

        // ✅ Bind Department để nhận dữ liệu từ form
        [BindProperty]
        public CreateOrEditForViewDepartment Department { get; set; }
        
        private readonly IDepartmentAppService _departmentAppService;

        public CreateOrEditModalModel(IDepartmentAppService departmentAppService)
        {
            _departmentAppService = departmentAppService;
        }

        public async Task OnGetAsync(int? id)
        {
            if (id.HasValue)
            {
                // Lấy thông tin khi sửa
                var query = await _departmentAppService.GetForDepartmentId(new GetAllDepartmentInput { Id = id.Value });
                if (query!= null)
                {
                    Department = ObjectMapper.Map<CreateUpdateDepartmentDto, CreateOrEditForViewDepartment>(query);

                }
                Id = query?.Id;
            }
            else
            {
                // Tạo mới
                Department = new CreateOrEditForViewDepartment
                {
                    Code = string.Empty,
                    Name = string.Empty,
                    IsActive = true
                };
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // ⚠️ Nếu validation fail (ví dụ trường Name trống)
                return Page();
            }
            if(Department!= null)
            {
                CreateUpdateDepartmentDto query = ObjectMapper.Map<CreateOrEditForViewDepartment, CreateUpdateDepartmentDto>(Department);
                if (Id.HasValue)
                {
                    query.Id = Id.Value;
                }
                await _departmentAppService.CreateOrEdit(query);
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
