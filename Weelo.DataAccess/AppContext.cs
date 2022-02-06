using Microsoft.EntityFrameworkCore;
using Weelo.Domain.Entities;

namespace Weelo.DataAccess;

/// <summary>
/// App context
/// </summary>
public class AppContext : DbContext
{
    #region Constructors
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options"></param>
    public AppContext(DbContextOptions options) : base(options) { }
    #endregion

    #region DbSets
    /// <summary>
    /// Db set of owners
    /// </summary>
    public DbSet<Owner> Owners { get; set; }
    /// <summary>
    /// Db set of properties
    /// </summary>
    public DbSet<Property> Properties { get; set; }
    /// <summary>
    /// Db set of property traces
    /// </summary>
    public DbSet<PropertyTrace> PropertyTraces { get; set; }
    /// <summary>
    /// Db set of property images
    /// </summary>
    public DbSet<PropertyImage> PropertyImages { get; set; }
    #endregion

    #region Overrides
    /// <summary>
    /// On model creating for configure entities
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);
    }
    #endregion
}
