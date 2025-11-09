using AbpSolution1.Config.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Config.Spins
{
    public class SpinProduct: FullAuditedEntity<int>
    {
        public int SpinId { get; set; }
        public Spin Spin { get; set; }

        public int ProductId { get; set; }
        public Products Product { get; set; }

        public double Proportion { get; set; }

        public bool IsDefault {  get; set; }
    }
}
