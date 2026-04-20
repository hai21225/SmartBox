using Core.Entity;
using Microsoft.EntityFrameworkCore;

public class AppDbContext: DbContext
{

    public DbSet<Users> Users { get; set; }
    public DbSet<Lockers> Lockers { get; set; }
    public DbSet<Usage> Usages { get; set; }
    public DbSet<Command> Commands { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

}