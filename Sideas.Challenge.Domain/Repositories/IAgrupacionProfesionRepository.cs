using Sideas.Challenge.Domain.Entities;

namespace Sideas.Challenge.Domain.Repositories
{
    // Repositorio para la entidad AgrupacionProfesion (tabla de unión)
    public interface IAgrupacionProfesionRepository
    {
        Task SaveAsync(List<AgrupacionProfesion> relaciones);
    }
}
