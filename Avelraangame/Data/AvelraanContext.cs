using Avelraangame.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Avelraangame.Data
{
    public partial class AvelraanContext : DbContext
    {
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Character> Characters { get; set; }

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


        // Items
            modelBuilder.Entity<Item>(entity =>
            {
                // players will not be able to create items

                entity.Property(s => s.Name)
                    .HasMaxLength(200);
            });

        // Players
            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(s => s.Ward)
                    .HasMaxLength(10);
                entity.Property(s => s.Ward)
                    .IsRequired();

                entity.Property(s => s.Name)
                    .HasMaxLength(50);
                entity.Property(s => s.Name)
                    .IsRequired();
            });

        // Characters
            modelBuilder.Entity<Character>(entity =>
            {
                entity.Property(s => s.Name)
                    .HasMaxLength(100);

                entity.HasOne(s => s.Player)
                    .WithMany(s => s.Characters)
                    .HasForeignKey(s => s.PlayerId)
                    .HasPrincipalKey(s => s.Id); // unique identifier in the Player model
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
