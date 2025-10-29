using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1
{
    public class SimpleEntity: FullAuditedEntity<int>
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; }
    }
}
