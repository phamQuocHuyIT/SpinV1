using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Dto.Config.Product
{
    public class ProductLookupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Note { get; set; }
        public bool IsActive { get; set; }

        public Guid? TenantId { get; set; }
    }
}
