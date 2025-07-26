using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;
using Sideas.Challenge.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repositorio para guardar relaciones entre agrupaciones y profesiones.
/// </summary>
public class AgrupacionProfesionRepository : IAgrupacionProfesionRepository
{
    private readonly AppDbContext _context;

    public AgrupacionProfesionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(List<AgrupacionProfesion> relaciones)
    {
        // Guardar solo relaciones nuevas (únicas por clave compuesta)
        foreach (var relacion in relaciones.DistinctBy(r => new { r.AgrupacionId, r.ProfesionId }))
        {
            bool yaExiste = await _context.AgrupacionProfesiones
                .AnyAsync(ap => ap.AgrupacionId == relacion.AgrupacionId && ap.ProfesionId == relacion.ProfesionId);

            if (!yaExiste)
            {
                _context.AgrupacionProfesiones.Add(relacion);
            }
        }

        await _context.SaveChangesAsync();
    }
}
