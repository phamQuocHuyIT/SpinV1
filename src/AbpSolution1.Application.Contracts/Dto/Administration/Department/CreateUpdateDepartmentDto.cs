using System;
using System.ComponentModel.DataAnnotations;

namespace AbpSolution1.Dto.Administration;

public class CreateUpdateDepartmentDto
{
    public Guid? TenantId { get; set; }
    public int? Id { get; set; }
    [Required]
    [StringLength(128)]
    public required string Name { get; set; }
    [Required]
    [StringLength(20)]
    public required string Code { get; set; }
    public string? Note {  get; set; }
    public bool IsActive {  get; set; }

    
}