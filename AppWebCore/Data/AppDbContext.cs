using AppWebCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AppWebCore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AppWebCore.Models.Product> Products { get; set; }
        public DbSet<AppWebCore.Models.VarianteProduct> Variantes { get; set; }
        public DbSet<AppWebCore.Models.Contacto> Contactos { get; set; }
        public DbSet<AppWebCore.Models.Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Variantes)
                .WithOne(v => v.Product)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .Property(p => p.FechaCreacion)
                .HasColumnType("timestamp with time zone")
                .HasConversion(
                v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
                v => v);
            modelBuilder.Entity<Event>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Event>()
                .Property(e => e.Start)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Event>()
                .Property(e => e.End)
                .HasColumnType("timestamp with time zone");
        }

    }
}
