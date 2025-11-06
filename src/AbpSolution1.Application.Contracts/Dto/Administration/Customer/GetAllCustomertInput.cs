using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace AbpSolution1.Dto.Administration.Customer
{
    public class GetAllCustomertInput : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }

        public int? Id { get; set; }

        public Guid? TenantId { get; set; }

    }
}
