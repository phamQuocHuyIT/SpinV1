using AbpSolution1.Dto.Administration;
using AbpSolution1.Dto.Config.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpSolution1.Interface.Config.Product
{
    public interface IProductAppService: IApplicationService
    {
        Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductInput input);
        Task<CreateUpdateProductDto> GetForProductId(GetAllProductInput input);
        Task CreateOrEdit(CreateUpdateProductDto input);

        Task Delete(int id);
        Task<ListResultDto<ProductLookupDto>> GetLookupAsync();
    }
}
