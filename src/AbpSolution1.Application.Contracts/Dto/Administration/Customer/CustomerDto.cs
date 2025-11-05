using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1.Dto.Administration.Customer
{
    public class CustomerDto
    {
        public int? Id { get; set; }
       
        [Required]
        [StringLength(20)]
        public required string Code { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; }

        public required string FullName { get; set; }  // FULLNAME
        public string? Address { get; set; }        // ADDRESS
        public string? NumberPhone { get; set; }    // NUMBERPHONE
        public string? Rank { get; set; }           // RANK
        public decimal Total { get; set; }          // TOTAL
        public bool IsDelete { get; set; }          // ISDELETE
    }
}
