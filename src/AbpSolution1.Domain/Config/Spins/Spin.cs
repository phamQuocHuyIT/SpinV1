using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Config.Spins
{
    [Table("Spin_Spins")]
    public class Spin: SimpleEntity
    {
        public bool IsDefault { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // 🔹 Quan hệ nhiều–nhiều
        public virtual ICollection<SpinCustomer> SpinCustomers { get; set; } = new List<SpinCustomer>();
        public virtual ICollection<SpinProduct> SpinProducts { get; set; } = new List<SpinProduct>();
    }
}
