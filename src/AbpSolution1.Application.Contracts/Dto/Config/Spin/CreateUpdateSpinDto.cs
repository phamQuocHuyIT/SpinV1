using AbpSolution1.Dto.Administration.Customer;
using AbpSolution1.Dto.Config.Product;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Dto.Config.Spin
{
    public class CreateUpdateSpinDto
    {
        [CanBeNull]
        public int? Id { get; set; }
        public string? Name { get; set; } = null!;
        public string? Code { get; set; } = null!;
        public string? Note { get; set; }
        public bool? IsActive { get; set; }

        public Guid? TenantId { get; set; }

        public bool? IsDefault { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<SpinProductDto>? Products { get; set; } = new();
        public List<SpinCustomerDto>? Customers { get; set; } = new();

        public bool IsEdit {  get; set; }
    }
}
