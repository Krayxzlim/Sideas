using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Sideas.Challenge.Application.Services
{
    /// <summary>
    /// Servicio encargado de obtener fueros y sus zonas desde API externa,
    /// y almacenarlos en base de datos.
    /// </summary>
    public class FueroService
    {
        private readonly HttpService _httpService;
        private readonly IFueroRepository _fueroRepository;
        private readonly IZonaRepository _zonaRepository;
        private readonly ILogger<FueroService> _logger;

        public FueroService(
            HttpService httpService,
            IFueroRepository fueroRepository,
            IZonaRepository zonaRepository,
            ILogger<FueroService> logger)
        {
            _httpService = httpService;
            _fueroRepository = fueroRepository;
            _zonaRepository = zonaRepository;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene los fueros y sus zonas asociadas desde la API,
        /// y persiste los datos en base de datos.
        /// </summary>
        public async Task<List<Zona>> GetAndStoreAllZonasAsync()
        {
            _logger.LogInformation("Solicitando fueros...");
            var fueros = await _httpService.GetAsync<List<Fuero>>("https://consulta-peritos-api.pjn.gov.ar/api/fuero");

            if (fueros == null || fueros.Count == 0)
            {
                _logger.LogWarning("⚠ No se obtuvieron fueros.");
                return new();
            }

            await _fueroRepository.SaveAsync(fueros);
            _logger.LogInformation("Fueros guardados.");

            var allZonas = new List<Zona>();

            foreach (var f in fueros)
            {
                _logger.LogInformation($"Solicitando zonas para fuero {f.Id} - {f.Descripcion}...");
                var zonas = await _httpService.GetAsync<List<Zona>>(
                    $"https://consulta-peritos-api.pjn.gov.ar/api/zona?fuero={f.Id}");

                if (zonas == null || zonas.Count == 0)
                {
                    _logger.LogWarning($"⚠️ No se obtuvieron zonas para fuero {f.Id}");
                    continue;
                }

                // Establecer la relación con el fuero correspondiente
                foreach (var z in zonas)
                {
                    z.FueroId = f.Id;
                }

                allZonas.AddRange(zonas);
                _logger.LogInformation($"Se agregaron {zonas.Count} zonas de fuero {f.Id}");
            }

            await _zonaRepository.SaveAsync(allZonas);
            _logger.LogInformation($"Zonas guardadas en base ({allZonas.Count})");

            return allZonas;
        }
    }
}
