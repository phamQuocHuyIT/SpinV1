using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Dto.Config.Spin.HistorySpin
{
    public class CreateUpdateHistorySpinDto
    {
        public Guid? TenantId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime RewardDate { get; set; }
        public int? SpinId { get; set; }
        public int? ProductId { get; set; }

        public int? EmployeeId { get; set; }
    }
}
