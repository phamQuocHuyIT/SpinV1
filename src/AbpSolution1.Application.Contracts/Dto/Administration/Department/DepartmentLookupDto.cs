using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace AbpSolution1.Dto.Administration.Department
{
    public class DepartmentLookupDto : EntityDto<int>
    {
        public string Name { get; set; }
    }
}
