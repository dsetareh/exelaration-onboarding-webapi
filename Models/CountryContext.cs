using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CountryApi.Models
{
    public class CountryContext : DbContext
    {
        public CountryContext(DbContextOptions<CountryContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<State> States { get; set; } = null!;
    }
}