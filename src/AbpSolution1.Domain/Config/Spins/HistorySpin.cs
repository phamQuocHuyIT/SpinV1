using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Config.Spins
{
    public class HistorySpin: FullAuditedEntity<int>
    {
        public Guid? TenantId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime RewardDate { get; set; }
        public int SpinId { get; set; }
        public int? ProductId { get; set; }

        public int? EmployeeId { get; set; }
    }
}
