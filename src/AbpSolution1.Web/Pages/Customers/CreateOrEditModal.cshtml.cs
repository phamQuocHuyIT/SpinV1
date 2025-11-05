using AbpSolution1.Dto.Administration;
using AbpSolution1.Dto.Administration.Customer;
using AbpSolution1.Interface.Administration.Customer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AbpSolution1.Web.Pages.Customers
{
    public class CreateOrEditModalModel : AbpSolution1PageModel
    {
        [HiddenInput]
        public int? Id { get; set; }

        [BindProperty]
        public CreateOrEditForViewCustomer Customer { get; set; }

        private readonly ICustomerAppService _customerAppService;

        public CreateOrEditModalModel(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }

        public async Task OnGetAsync(int? id)
        {
            if (id.HasValue)
            {
                var query = await _customerAppService.GetForCustomerId(new GetAllCustomerInput { Id = id.Value });
                if (query != null)
                {
                    Customer = ObjectMapper.Map<CreateUpdateCustomerDto, CreateOrEditForViewCustomer>(query);
                    Id = query.Id;
                }
            }
            else
            {
                Customer = new CreateOrEditForViewCustomer
                {
                    Code = string.Empty,
                    FullName = string.Empty,
                    Address = string.Empty,
                    NumberPhone = string.Empty,
                    Rank = string.Empty,
                    Total = 0,
                    IsDelete = false
                };
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var dto = ObjectMapper.Map<CreateOrEditForViewCustomer, CreateUpdateCustomerDto>(Customer);

            if (Id.HasValue)
            {
                dto.Id = Id.Value;
            }

            await _customerAppService.CreateOrEdit(dto);

            return NoContent();
        }
    }
}
