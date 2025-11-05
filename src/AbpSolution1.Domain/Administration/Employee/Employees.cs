using Abp.Domain.Entities;
using AbpSolution1.Administration.Departmant;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Administration.Employee
{
    [Table("Spin_Employees")]
    public class Employees : FullAuditedEntity<int>
    {
        public Guid? TenantId { get; set; }

        [Required, MinLength(1), MaxLength(200)]
        public required string Code { get; set; }

        [Required, MinLength(1), MaxLength(200)]
        public required string FullName { get; set; }

        public DateTime DOB { get; set; }
        public string? Address { get; set; }
        public string? NumberPhone { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual Departments Department { get; set; }
        public int Gender { get; set; }

        public bool IsActive { get; set; }

        public Guid? UserId { get; set; }
    }
}
