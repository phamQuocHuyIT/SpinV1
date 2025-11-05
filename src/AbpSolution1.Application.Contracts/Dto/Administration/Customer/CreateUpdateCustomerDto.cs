using System.ComponentModel.DataAnnotations;

namespace AbpSolution1.Dto.Administration.Customer;

public class CreateUpdateCustomerDto
{
    public int? Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Code { get; set; }

    public string? Note { get; set; }
    public bool IsActive { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(20)]
    public string? NumberPhone { get; set; }

    [StringLength(20)]
    public string? Rank { get; set; }

    public decimal Total { get; set; }
    public bool IsDelete { get; set; }
}
