using Avelraangame.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Avelraangame.Data
{
    public partial class AvelraanContext : DbContext
    {
        public virtual DbSet<Item> Items { get; set; }

        public AvelraanContext()
        {
        }

        public AvelraanContext(DbContextOptions<AvelraanContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress; Database=Avelraan; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(200);
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
