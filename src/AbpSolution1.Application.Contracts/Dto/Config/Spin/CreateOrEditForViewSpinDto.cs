using AbpSolution1.Dto.Config.Product;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Dto.Config.Spin
{
    public class CreateOrEditForViewSpinDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Note { get; set; }
        public bool IsActive { get; set; }

        public bool IsDefault { get; set; }

        public bool IsEdit { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        
    }
}
