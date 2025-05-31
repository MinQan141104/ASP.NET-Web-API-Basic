using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace MyWebAPI.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public virtual DbSet<HangHoa> HangHoas { get; set; }

        public virtual DbSet<Loai> Loais { get; set; }

        public virtual DbSet<DonHang> DonHangs { get; set; }

        public virtual DbSet<DonHangChiTiet> DonHangChiTiets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonHang>(e => {
                e.ToTable("DonHang");
                e.HasKey(dh => dh.MaDh);
                e.Property(dh => dh.NgayDat).HasDefaultValueSql("getutcdate()");
                e.Property(dh => dh.NguoiNhan).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<DonHangChiTiet>(e =>
            {
                e.ToTable("ChiTietDonHang");
                e.HasKey(e => new { e.MaDh, e.MaHh });

                e.HasOne(e => e.DonHang)
                 .WithMany(e => e.DonHangChiTiets)
                 .HasForeignKey(e => e.MaDh);

                e.HasOne(e => e.HangHoa)
                 .WithMany(e => e.DonHangChiTiets)
                 .HasForeignKey(e => e.MaHh);
            });
        }
        public static string GetConnectionString(string connectionStringName)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = config.GetConnectionString(connectionStringName);
            return connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"));
    }
}
