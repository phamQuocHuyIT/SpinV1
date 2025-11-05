using AbpSolution1.Dto.Config.Product;
using AbpSolution1.Interface.Config.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AbpSolution1.Web.Pages.Products
{
    public class CreateOrEditModalModel : AbpSolution1PageModel
    {
        // Thuộc tính Id chỉ dùng để xác định tạo/sửa, không cần SupportsGet
        [HiddenInput]
        public int? Id { get; set; }

        // Bind Product để nhận dữ liệu từ form
        [BindProperty]
        public CreateOrEditForViewProductDto Product { get; set; }

        private readonly IProductAppService _ProductAppService;

        public CreateOrEditModalModel(IProductAppService ProductAppService)
        {
            _ProductAppService = ProductAppService;
        }

        public async Task OnGetAsync(int? id)
        {
            if (id.HasValue)
            {
                // Lấy thông tin khi sửa
                var query = await _ProductAppService.GetForProductId(new GetAllProductInput { Id = id.Value });
                if (query != null)
                {
                    Product = ObjectMapper.Map<CreateUpdateProductDto, CreateOrEditForViewProductDto>(query);

                }
                Id = query?.Id;
            }
            else
            {
                // Tạo mới
                Product = new CreateOrEditForViewProductDto
                {
                    Code = string.Empty,
                    Name = string.Empty,
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
            if (Product != null)
            {
                CreateUpdateProductDto query = ObjectMapper.Map<CreateOrEditForViewProductDto, CreateUpdateProductDto>(Product);
                if (id.HasValue)
                {
                    query.Id = id.Value;
                }
                await _ProductAppService.CreateOrEdit(query);
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
