using AbpSolution1.Dto.Administration.Customer;
using AbpSolution1.Interface.Administration.Customer;
using AbpSolution1.Interface.Administration.Department;
using AbpSolution1.Localization;
using Volo.Abp.ObjectMapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace AbpSolution1.Web.Pages.Customers
{
    public class CreateOrEditModalModel : AbpSolution1PageModel
    {
        [HiddenInput]
        public int? Id { get; set; }

        [BindProperty]
        public CreateOrEditForViewCustomerDto Customer { get; set; }

        public int? GenderStatus { get; set; }
        public int? RankesStatus { get; set; }

        public List<SelectListItem> Gender { get; set; }
        public List<SelectListItem> Ranked { get; set; }

        private readonly ICustomerAppService _CustomerAppService;
        private readonly IDepartmentAppService _departmentAppService;
        private readonly IStringLocalizer<AbpSolution1Resource> _localizer;

        public CreateOrEditModalModel(ICustomerAppService CustomerAppService, IDepartmentAppService departmentAppService,
            IStringLocalizer<AbpSolution1Resource> localizer)
        {
            _CustomerAppService = CustomerAppService;
            _departmentAppService = departmentAppService;
            _localizer = localizer;
        }

        public async Task OnGetAsync(int? id)
        {
            var query = new CreateUpdateCustomerDto { Code = string.Empty, FullName = string.Empty };
            Gender = DomainHelps.ListGender(null, _localizer);
            Ranked = DomainHelps.ListRanked(null, _localizer);
            if (id.HasValue)
            {
                query = await _CustomerAppService.GetForCustomerId(new GetAllCustomertInput { Id = id.Value });
            }

            if (query != null)
            {
                if (query.Id.HasValue)
                {


                    Customer = ObjectMapper.Map<CreateUpdateCustomerDto, CreateOrEditForViewCustomerDto>(query);
                    Id = query.Id;
                    GenderStatus = query.Gender;
                    RankesStatus = query.Ranked;

                }
            }
            else
            {
                Customer = new CreateOrEditForViewCustomerDto
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
            if (Customer != null)
            {
                CreateUpdateCustomerDto query = ObjectMapper.Map<CreateOrEditForViewCustomerDto, CreateUpdateCustomerDto>(Customer);
                if (Id.HasValue)
                {
                    query.Id = id.Value;
                }
                await _CustomerAppService.CreateOrEdit(query);
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
