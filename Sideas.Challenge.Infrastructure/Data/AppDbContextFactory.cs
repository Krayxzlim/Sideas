using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sideas.Challenge.Infrastructure.Data;

namespace Sideas.Challenge.Infrastructure
{
    /// <summary>
    /// DB en tiempo de diseño.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Cadena de conexión local para desarrollo
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SideasChallengeDb;Trusted_Connection=True;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
