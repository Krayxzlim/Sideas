using Microsoft.EntityFrameworkCore;
using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;
using Sideas.Challenge.Infrastructure.Data;

/// <summary>
/// Repositorio para guardar asignaciones nuevas.
/// </summary>
public class AsignacionRepository : IAsignacionRepository
{
    private readonly AppDbContext _context;

    public AsignacionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(List<Asignacion> asignaciones)
    {
        // Evita duplicados comparando con IDs existentes
        var idsExistentes = await _context.Asignaciones
            .Select(a => a.Id)
            .ToListAsync();

        var nuevasAsignaciones = asignaciones
            .Where(a => !idsExistentes.Contains(a.Id))
            .ToList();

        _context.Asignaciones.AddRange(nuevasAsignaciones);
        await _context.SaveChangesAsync();
    }
}
