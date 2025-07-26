using Microsoft.EntityFrameworkCore;
using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;
using Sideas.Challenge.Infrastructure.Data;

/// <summary>
/// Repositorio para guardar fueros si no existen, dentro de una transacción.
/// </summary>
public class FueroRepository : IFueroRepository
{
    private readonly AppDbContext _context;

    public FueroRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(List<Fuero> fueros)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            foreach (var fuero in fueros)
            {
                fuero.Descripcion ??= "Sin descripción";

                var tracked = _context.Fueros.Local.FirstOrDefault(f => f.Id == fuero.Id);
                if (tracked == null)
                {
                    var existe = await _context.Fueros.FindAsync(fuero.Id);
                    if (existe == null)
                    {
                        _context.Fueros.Add(fuero);
                    }
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
