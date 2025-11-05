using AbpSolution1.Administration.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Administration.Departmant
{
    [Table("Spin_Departments")]
    public class Departments: SimpleEntity
    {
        // Quan hệ 1-n: Một Department có nhiều Employee
        public virtual ICollection<Employees> Employees { get; set; } = new List<Employees>();
    }
}
