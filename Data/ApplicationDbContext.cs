using Microsoft.EntityFrameworkCore;
using System;

namespace EntityFrameworkBasics
{
    /// <summary>
    /// The database representational model for our application 
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        
        #region Public Properties
        
        /// <summary>
        /// The settings for this application
        /// </summary>
        public DbSet<SettingsDataModel> Settings { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor, expecting database options passed in
        /// </summary>
        /// <param name="options">The database context options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        #endregion

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    optionsBuilder.UseSqlServer("Server=TTA\\TUKTUK;Database=TTA_V1;User Id=sa;Password=Password123");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API
            //modelBuilder.Entity<SettingsDataModel>().HasIndex(a => a.Name);
        }
    }
}
