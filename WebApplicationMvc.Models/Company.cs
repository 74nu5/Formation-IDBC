namespace WebApplicationMvc.Models;

using System.ComponentModel.DataAnnotations;

internal class Company
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public List<Person> Employees { get; set; } = new();

    public Address? Address { get; set; }
}
