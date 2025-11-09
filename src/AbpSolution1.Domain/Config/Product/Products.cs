using AbpSolution1.Config.Spins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Config.Product
{
    [Table("Spin_Products")]
    public class Products: SimpleEntity
    {
        public virtual ICollection<SpinProduct> SpinProducts { get; set; } = new List<SpinProduct>();
    }
}
