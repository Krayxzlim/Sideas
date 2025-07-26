using Sideas.Challenge.Domain.Entities;

namespace Sideas.Challenge.Domain.Repositories
{
    // Repositorio para la entidad Asignacion
    public interface IAsignacionRepository
    {
        Task SaveAsync(List<Asignacion> asignaciones);
    }
}
