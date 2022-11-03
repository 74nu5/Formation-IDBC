namespace Data.Models;

using System.ComponentModel.DataAnnotations;

public class Address : ADataObject
{
    [Required]
    public string Road { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string ZipCode { get; set; } = string.Empty;
}
