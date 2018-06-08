using Microsoft.EntityFrameworkCore;

namespace TTAServer
{
    public class TTADbContext : DbContext
    {
        #region Public Properties

        public DbSet<SettingsDataModel> Settings { get; set; }
        public DbSet<WebMenuModel> tblWebMenu { get; set; }

        #endregion

        #region Constructor

        public TTADbContext(DbContextOptions<TTADbContext> options) : base(options)
        {

        }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer("Server=TTA\\TUKTUK;Database=TTA_V1;User Id=sa;Password=Password123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
        }
    }
}
