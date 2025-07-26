using Newtonsoft.Json;
using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Sideas.Challenge.Application.Services
{
    /// <summary>
    /// Servicio encargado de obtener asignaciones paginadas desde la API externa
    /// y almacenarlas en base de datos.
    /// </summary>
    public class AsignacionService
    {
        private readonly HttpService _httpService;
        private readonly IAsignacionRepository _asignacionRepository;
        private readonly ILogger<AsignacionService> _logger;

        public AsignacionService(
            ILogger<AsignacionService> logger,
            HttpService httpService,
            IAsignacionRepository asignacionRepository)
        {
            _logger = logger;
            _httpService = httpService;
            _asignacionRepository = asignacionRepository;
        }

        public async Task<IEnumerable<Asignacion>> FetchAndStoreAsignaciones()
        {
            var asignaciones = new List<Asignacion>();
            var toDate = DateTime.Today.AddDays(1); // hasta mañana
            string toStr = toDate.ToString("ddMMyyyy");
            int page = 0;
            int pageSize = 1000;
            bool hasMore = true;

            _logger.LogInformation("Iniciando carga de asignaciones hasta el {fecha}.", toStr);

            while (hasMore)
            {
                string url = $"https://consulta-peritos-api.pjn.gov.ar/api/asignacion?page={page}&size={pageSize}&query=date%3C{toStr}";
                _logger.LogInformation("Consultando API de asignaciones: Página {page}", page);

                try
                {
                    var response = await _httpService.GetAsync<AsignacionApiResponse>(url);

                    if (response?.Content != null && response.Content.Count > 0)
                    {
                        _logger.LogInformation("Página {page}: Se obtuvieron {count} asignaciones.", page, response.Content.Count);

                        asignaciones.AddRange(response.Content);
                        await _asignacionRepository.SaveAsync(response.Content);

                        page++;
                    }
                    else
                    {
                        _logger.LogInformation("No se encontraron más asignaciones en la página {page}. Finalizando.");
                        hasMore = false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener o guardar asignaciones en la página {page}", page);
                    hasMore = false;
                }
            }

            _logger.LogInformation("Carga completa. Total de asignaciones: {total}", asignaciones.Count);
            return asignaciones;
        }
    }

    /// <summary>
    /// Representa la respuesta de la API de asignaciones.
    /// </summary>
    public class AsignacionApiResponse
    {
        [JsonProperty("content")]
        public List<Asignacion> Content { get; set; }
    }
}
