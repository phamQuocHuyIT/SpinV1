using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AbpSolution1.Permissions.AbpSolution1Permissions;

namespace AbpSolution1.Dto.Config.Spin
{
    public class SpinCustomerDto
    {
        public int? SpinId { get; set; }
      
        public int? CustomerId { get; set; }

        public int? SpinCount { get; set; }
    }
}
