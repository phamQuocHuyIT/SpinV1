using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace AbpSolution1.Dto.Administration.Department
{
    public class GetAllDepartmentInput: PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }

        public int? Id { get; set; }

        public Guid? TenantId { get; set; }
    }
}
