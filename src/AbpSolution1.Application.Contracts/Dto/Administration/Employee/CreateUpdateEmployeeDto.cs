using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Dto.Administration.Employee
{
    public class CreateUpdateEmployeeDto
    {
        public int? Id { get; set; }
        public Guid? TenantId { get; set; }

        [Required, MinLength(1), MaxLength(200)]
        public required string Code { get; set; }

        [Required, MinLength(1), MaxLength(200)]
        public required string FullName { get; set; }

        public DateTime DOB { get; set; }
        public string? Address { get; set; }
        public string? NumberPhone { get; set; }
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = null!;

        public int Gender { get; set; }

        public bool IsActive { get; set; }
    }
}
