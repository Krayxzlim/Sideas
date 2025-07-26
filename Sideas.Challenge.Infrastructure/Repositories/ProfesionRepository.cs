using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;
using Sideas.Challenge.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


/// <summary>
/// Repositorio para insertar nuevas profesiones o actualizar existentes.
/// </summary>
public class ProfesionRepository : IProfesionRepository
{
    private readonly AppDbContext _context;

    public ProfesionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(List<Profesion> profesiones)
    {
        foreach (var profesion in profesiones)
        {
            var existente = await _context.Profesiones.FindAsync(profesion.Id);
            if (existente == null)
            {
                _context.Profesiones.Add(profesion);
            }
            else
            {
                _context.Entry(existente).CurrentValues.SetValues(profesion); // Update valores
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Profesion>> GetAllAsync()
    {
        return await _context.Profesiones.ToListAsync();
    }

}
