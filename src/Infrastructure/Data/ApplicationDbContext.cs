using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Sesion> Sesiones { get; set; }
    
        public DbSet<Rol> Roles { get; set; } = null!;
        public DbSet<RolrolOpciones> RelOpciones { get; set; } = null!;
        public DbSet<RolOpciones> RolOpciones { get; set; } = null!;
        public DbSet<RolUsuarios> RolUsuarios { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.Entity<RolOpciones>()
                .HasKey(ro => new { ro.IdOpcion, ro.IdOpcion });

            modelBuilder.Entity<RolUsuarios>()
                .HasKey(ru => new { ru.UsuarioId, ru.RolId });


            modelBuilder.Entity<RolOpciones>()
                .HasOne(ro => ro.Rol)
                .WithMany(r => r.RolOpciones)
                .HasForeignKey(ro => ro.RolId);

            modelBuilder.Entity<RolOpciones>()
                .HasOne(ro => ro.RolOpc)
                .WithMany()
                .HasForeignKey(ro => ro.OpcionId);

            modelBuilder.Entity<RolUsuarios>()
                .HasOne(ru => ru.Usuario)
                .WithMany(u => u.RolUsuarios)
                .HasForeignKey(ru => ru.UsuarioId);

            modelBuilder.Entity<RolUsuarios>()
                .HasOne(ru => ru.Rol)
                .WithMany(r => r.RolUsuarios)
                .HasForeignKey(ru => ru.RolId);


            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.IdUsuario);
                entity.Property(u => u.UserName).HasMaxLength(60);
                entity.Property(u => u.Password).HasMaxLength(60);
                entity.Property(u => u.Mail).HasMaxLength(120);
                entity.Property(u => u.SessionActive).HasMaxLength(1);
                entity.Property(u => u.Status).HasMaxLength(20);

                entity.HasOne(u => u.Persona)
                      .WithMany()
                      .HasForeignKey(u => u.PersonaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(p => p.IdPersona);
                entity.Property(p => p.Nombres).HasMaxLength(60);
                entity.Property(p => p.Apellidos).HasMaxLength(60);
                entity.Property(p => p.Identificacion).HasMaxLength(10);
            });


        }
    }
}