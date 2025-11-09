using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AbpSolution1.Permissions.AbpSolution1Permissions;

namespace AbpSolution1.Dto.Config.Spin
{
    public class SpinProductDto
    {
        public int? SpinId { get; set; }

        public int? ProductId { get; set; }

        public string? ProductName { get; set; }

        public double? Proportion { get; set; }

        public bool IsDefault { get; set; }
    }
}
