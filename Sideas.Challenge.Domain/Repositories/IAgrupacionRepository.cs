using Sideas.Challenge.Domain.Entities;

namespace Sideas.Challenge.Domain.Repositories
{
    //Repositorio para la entidad Agrupacion
    public interface IAgrupacionRepository
    {
        Task SaveAsync(List<Agrupacion> agrupaciones);
    }
}
