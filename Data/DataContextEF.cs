using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Data;

// We inherit from the DBContext class and overwrite some of its methods
public class DataContextEF : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DataContextEF(IConfiguration config)
    {
        _configuration = config;
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    // OnConfiguring is called when DBContext is created
    // We should give EF access to the connection string here
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                _connectionString,
                options => options.EnableRetryOnFailure());
        }
    }
    // Now we should tell EF to go to the DB and look for tables that match our models
    // So that we can manipulate and access that table directly using EF

    public DbSet<Users>? Users { get; set; }

    // Now we need modelbuilder to map our model to an actual table in db
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("FitnessTrackerSchema");
        // Remember to pass in which field is the primary key
        modelBuilder.Entity<Users>().HasKey(user => user.UserId);
        /*.ToTable("Users","FitnessTrackerSchema");*/
        /*.ToTable("table name","Schema name");*/

        // Be careful that the table name should match your model name 
        // Otherwise you have to specifically tell EF by using the ToTable method!
    }
}