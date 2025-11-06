using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Dto.Administration.Customer
{
    public class CreateOrEditForViewCustomerDto
    {
        public int? Id { get; set; }
        [Required, MinLength(1), MaxLength(200)]
        public required string Code { get; set; }

        [Required, MinLength(1), MaxLength(200)]
        public required string FullName { get; set; }

        public DateTime DOB { get; set; }
        public string? Address { get; set; }
        public string? NumberPhone { get; set; }

        public int Gender { get; set; }

        public bool IsActive { get; set; }

        public int Ranked { get; set; }

        public int TotalPurchase { get; set; }

    }
}
