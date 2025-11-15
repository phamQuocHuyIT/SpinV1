using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Dto.Config.Spin.HistorySpin
{
    public class CreateOrEditForViewHistorySpin
    {
        public int? Id { get; set; }
        public Guid? TenantId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime RewardDate { get; set; }
        public int SpinId { get; set; }
        public string SpinName { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
    }
}
