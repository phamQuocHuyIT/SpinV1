using AbpSolution1.Config.Spins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Administration.Customer
{
    [Table("Spin_Customers")]
    public class Customers : FullAuditedEntity<int>
    {
        public Guid? TenantId { get; set; }

        [Required, MinLength(1), MaxLength(200)]
        public required string Code { get; set; }

        [Required, MinLength(1), MaxLength(200)]
        public required string FullName { get; set; }

        public DateTime DOB { get; set; }
        public string? Address { get; set; }
        public string? NumberPhone { get; set; }
        public int Gender { get; set; }

        public bool IsActive { get; set; }

        public int Ranked { get; set; }

        public int TotalPurchase { get; set; }

        public virtual ICollection<SpinCustomer> SpinCustomers { get; set; } = new List<SpinCustomer>();
    }
}
