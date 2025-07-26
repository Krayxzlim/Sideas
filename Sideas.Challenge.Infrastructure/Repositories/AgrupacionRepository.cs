using Microsoft.EntityFrameworkCore;
using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;
using Sideas.Challenge.Infrastructure.Data;

namespace Sideas.Challenge.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio para guardar agrupaciones si no existen.
    /// </summary>
    public class AgrupacionRepository : IAgrupacionRepository
    {
        private readonly AppDbContext _context;

        public AgrupacionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(List<Agrupacion> agrupaciones)
        {
            foreach (var agrupacion in agrupaciones)
            {
                agrupacion.Descripcion ??= "Sin descripción";

                var existe = await _context.Agrupaciones.FindAsync(agrupacion.Id);
                if (existe == null)
                {
                    _context.Agrupaciones.Add(agrupacion);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
