using Sideas.Challenge.Domain.Entities;

namespace Sideas.Challenge.Domain.Repositories
{
    // Repositorio para la entidad Profesion
    public interface IProfesionRepository
    {
        Task SaveAsync(List<Profesion> profesiones);
        Task<IEnumerable<Profesion>> GetAllAsync();

    }
}
