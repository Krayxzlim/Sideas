using Sideas.Challenge.Domain.Entities;

namespace Sideas.Challenge.Domain.Repositories
{
    // Repositorio para la entidad Zona
    public interface IZonaRepository
    {
        Task SaveAsync(List<Zona> zonas);
    }
}
