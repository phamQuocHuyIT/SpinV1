using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Dto.Config.Spin.HistorySpin
{
    public class HistorySpinDto
    {
        public int? Id { get; set; }
        public Guid? TenantId { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime RewardDate { get; set; }
        public string RewardDateString => RewardDate.ToString("dd/MM/yyyy HH:mm:ss");
        public int? SpinId { get; set; }
        public string? SpinName { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }

        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
    }
}
