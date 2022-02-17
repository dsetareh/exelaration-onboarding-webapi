using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace TodoApi.Models
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