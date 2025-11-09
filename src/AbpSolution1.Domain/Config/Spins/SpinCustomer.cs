using AbpSolution1.Administration.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Config.Spins
{
    public class SpinCustomer : FullAuditedEntity<int>
    {
        public int SpinId { get; set; }
        public Spin Spin { get; set; }

        public int CustomerId { get; set; }
        public Customers Customer { get; set; }

        public int SpinCount {  get; set; }
    }
}
