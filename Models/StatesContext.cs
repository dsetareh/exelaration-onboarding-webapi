using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CountryApi.Models
{
    public class StatesContext : DbContext
    {
        public StatesContext(DbContextOptions<StatesContext> options)
            : base(options)
        {
        }

        public DbSet<StatesItem> StatesItems { get; set; } = null!;
    }
}