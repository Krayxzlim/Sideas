using Microsoft.EntityFrameworkCore;
using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;
using Sideas.Challenge.Infrastructure.Data;

/// <summary>
/// Repositorio para guardar zonas si no existen.
/// </summary>
public class ZonaRepository : IZonaRepository
{
    private readonly AppDbContext _context;

    public ZonaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(List<Zona> zonas)
    {
        foreach (var zona in zonas)
        {
            if (string.IsNullOrWhiteSpace(zona.Reparticion))
            {
                zona.Reparticion = "Sin asignar";
            }

            var exists = await _context.Zonas.AnyAsync(z => z.Id == zona.Id);
            if (!exists)
            {
                _context.Zonas.Add(zona);
            }
        }

        await _context.SaveChangesAsync();
    }
}
