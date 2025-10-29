using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Dto.Administration.Department
{
    public class CreateOrEditForViewDepartment
    {
        [Required]
        [StringLength(128)]
        public required string Name { get; set; }
        [Required]
        [StringLength(20)]
        public required string Code { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; }
    }
}
