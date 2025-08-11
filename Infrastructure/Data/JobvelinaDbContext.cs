using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class JobvelinaDbContext : DbContext
{
    public JobvelinaDbContext(DbContextOptions<JobvelinaDbContext> options) : base(options)
    {
    }

    public DbSet<Job> Jobs { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
}