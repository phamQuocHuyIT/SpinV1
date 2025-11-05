using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Administration.Customer
{
    [Table("Spin_Customers")]
    public class Customers : FullAuditedEntity<int>
    {
        [Required]
        [StringLength(50)]
        public required string Code { get; set; }

        [Required]
        [StringLength(100)] 
        public required string FullName { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? NumberPhone { get; set; }

        [StringLength(20)]
        public string? Rank { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public bool IsDelete { get; set; }

        public string? Note { get; set; }

        public bool IsActive { get; set; }
    }
}