using Microsoft.EntityFrameworkCore;
using Sideas.Challenge.Domain.Entities;

namespace Sideas.Challenge.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Agrupacion> Agrupaciones { get; set; }
        public DbSet<Profesion> Profesiones { get; set; }
        public DbSet<AgrupacionProfesion> AgrupacionProfesiones { get; set; }
        public DbSet<Fuero> Fueros { get; set; }
        public DbSet<Zona> Zonas { get; set; }
        public DbSet<Asignacion> Asignaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /// <summary>
            ///No sabia si era mejor mantener los ids que vienen desde las APis 
            ///o normlizar la tabla asignando ids nuevos.
            ///Estos codigos son para que no sean identity los ids
            /// </summary>
            // Agrupacion
            modelBuilder.Entity<Agrupacion>(builder =>
            {
                builder.HasKey(a => a.Id);
                builder.Property(a => a.Id).ValueGeneratedNever();
            });

            // Asignacion
            modelBuilder.Entity<Asignacion>(builder =>
            {
                builder.HasKey(a => a.Id);
                builder.Property(a => a.Id).ValueGeneratedNever(); 
            });

            // Profesion
            modelBuilder.Entity<Profesion>(builder =>
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Id).ValueGeneratedNever();
            });

            // Fuero 
            modelBuilder.Entity<Fuero>(builder =>
            {
                builder.HasKey(f => f.Id);
                builder.Property(f => f.Id).ValueGeneratedNever();
            });

            //Zona
            modelBuilder.Entity<Zona>(builder =>
            {
                builder.HasKey(z => z.Id);
                builder.Property(z => z.Id).ValueGeneratedNever(); 
            });

            

            // Configuración de clave compuesta en tabla intermedia
            modelBuilder.Entity<AgrupacionProfesion>()
                .HasKey(ap => new { ap.AgrupacionId, ap.ProfesionId });

            // Relaciones
            modelBuilder.Entity<AgrupacionProfesion>()
                .HasOne(ap => ap.Agrupacion)
                .WithMany(a => a.AgrupacionProfesiones)
                .HasForeignKey(ap => ap.AgrupacionId);

            modelBuilder.Entity<AgrupacionProfesion>()
                .HasOne(ap => ap.Profesion)
                .WithMany(p => p.AgrupacionProfesiones)
                .HasForeignKey(ap => ap.ProfesionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
