using Microsoft.EntityFrameworkCore;

namespace AppUtils.Data
{
    public class DbAppUtils : DbContext
    {
        // DbSet
        #region DbSet

        public DbSet<User> Users { get; set; }

        public DbSet<Repairer> Repairers { get; set; }
        #endregion

        //constructor
        public DbAppUtils(DbContextOptions options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("USER");
                user.HasKey(u => u.UID);
                user.Property(u => u.HoTen).IsUnicode().HasMaxLength(200);
                user.Property(u => u.DiaChi).IsUnicode().HasMaxLength(200).IsRequired(true);
                user.HasIndex(u => u.NumberPhone).IsUnique();
                user.Property(u => u.NumberPhone).HasMaxLength(13);
                user.Property(u => u.Email).HasMaxLength(200);
                user.Property(u => u.MatKhau).HasMaxLength(100);
               
            });

            modelBuilder.Entity<Repairer>(repairer =>
            {
                repairer.ToTable("REPAIRER");
                repairer.HasKey(r => r.ID);
                repairer.Property(r => r.HoTen).IsUnicode().HasMaxLength(200);
                repairer.Property(u => u.DiaChi).IsUnicode().HasMaxLength(200);
                repairer.Property(u => u.NumberPhone).HasMaxLength(13);
                repairer.Property(u => u.Email).HasMaxLength(200);
                repairer.Property(u => u.MatKhau).HasMaxLength(100);
                
            });

         
        }
    }
}
