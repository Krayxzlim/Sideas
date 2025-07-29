namespace Sideas.Challenge.Application.Services
{
    /// <summary>
    /// Se crea esta interfaz para facilitar los tests unitarios.
    /// Permite usar mocks que simulan el servicio sin hacer llamadas HTTP reales,
    /// logrando pruebas más rápidas y aisladas.
    /// </summary>
    public interface IHttpService
    {
        Task<T?> GetAsync<T>(string url);
    }
}
