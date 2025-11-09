
using AbpSolution1.Dto.Config.Spin;
using AbpSolution1.Interface.Administration.Department;

using AbpSolution1.Interface.Config.Spins;
using AbpSolution1.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbpSolution1.Web.Pages.Spins
{
    public class CreateOrEditModalModel : AbpSolution1PageModel
    {

        public int? Id { get; set; }

        [BindProperty]
        public CreateOrEditForViewSpinDto Spin { get; set; }


        private readonly ISpinAppService _SpinAppService;
        private readonly IDepartmentAppService _departmentAppService;
        private readonly IStringLocalizer<AbpSolution1Resource> _localizer;
        public CreateOrEditModalModel(ISpinAppService SpinAppService, IDepartmentAppService departmentAppService,
            IStringLocalizer<AbpSolution1Resource> localizer)
        {
            _SpinAppService = SpinAppService;
            _departmentAppService = departmentAppService;
            _localizer = localizer;
        }

        public async Task OnGetAsync(int? id)
        {
            var query = new CreateUpdateSpinDto { Code = string.Empty, Name = string.Empty };

            if (id.HasValue)
            {
                query = await _SpinAppService.GetForSpinId(new GetAllSpinInput { Id = id.Value });
            }

            if (query != null)
            {
                if (query.Id.HasValue)
                {


                    Spin = ObjectMapper.Map<CreateUpdateSpinDto, CreateOrEditForViewSpinDto>(query);
                    Id = query.Id;
                    Spin.IsEdit = true;

                }
            }
            else
            {
                Spin = new CreateOrEditForViewSpinDto
                {
                    Code = string.Empty,
                    Name = string.Empty,
                    IsActive = true,
                    IsEdit = false

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
            if (Spin != null)
            {
                CreateUpdateSpinDto query = ObjectMapper.Map<CreateOrEditForViewSpinDto, CreateUpdateSpinDto>(Spin);
                if (Id.HasValue)
                {
                    query.Id = id.Value;
                }
                await _SpinAppService.CreateOrEdit(query);
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
