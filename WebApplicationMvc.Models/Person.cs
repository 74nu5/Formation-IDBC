namespace Data.Models;

using System.ComponentModel.DataAnnotations;

internal class Person
{
    public int Id { get; set; }

    [Required]
    public string Firstname { get; set; } = string.Empty;

    [Required]
    public string Lastname { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public Address? Address { get; set; }
}
