using Sideas.Challenge.Domain.Entities;

namespace Sideas.Challenge.Domain.Repositories
{
    // Repositorio para la entidad Fuero
    public interface IFueroRepository
    {
        Task SaveAsync(List<Fuero> fueros);
    }
}
