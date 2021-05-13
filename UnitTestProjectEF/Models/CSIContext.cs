using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace UnitTestProjectEF.Models
{
    public partial class CSIContext : DbContext
    {
        private readonly string connectionString;

        public CSIContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public CSIContext(DbContextOptions<CSIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Personal> Personals { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=CSI;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.Property(e => e.NombreCliente)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.TelefonoCliente)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.ToTable("Personal");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.IdUsuario).ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
