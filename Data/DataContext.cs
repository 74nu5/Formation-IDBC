namespace Data;

using Data.Models;

using Microsoft.EntityFrameworkCore;

internal class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Person> Persons => this.Set<Person>();

    public DbSet<Company> Companies => this.Set<Company>();

    public DbSet<Address> Addresses => this.Set<Address>();
}
