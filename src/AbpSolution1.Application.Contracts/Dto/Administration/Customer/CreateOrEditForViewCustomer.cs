using System;
using System.ComponentModel.DataAnnotations;

namespace AbpSolution1.Dto.Administration.Customer
{
    public class CreateOrEditForViewCustomer
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
