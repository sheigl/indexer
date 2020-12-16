using System.Reflection;
using indexer.common.Models;
using Microsoft.EntityFrameworkCore;

namespace indexer.api
{
    public class IndexerContext : DbContext
    {
        public virtual DbSet<FileEntry> FileEntry { get; set; }

        public IndexerContext(DbContextOptions opts)
            : base(opts)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<FileEntry>().ToTable("FileEntry");
            modelBuilder.Entity<FileEntry>(entity =>
            {
                entity.HasKey(e => e.FullName);

                entity.Property(e => e.Path)
                    .HasMaxLength(4096)
                    .IsRequired();

                entity.Property(e => e.FullName)
                    .HasMaxLength(4096 + 255)
                    .IsRequired();

                entity.Property(e => e.Extension)
                    .HasMaxLength(255)
                    .IsRequired();
                
                entity.Property(e => e.IndexedDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}