using Microsoft.AspNetCore.Mvc;
using Sideas.Challenge.Application.Services;

namespace Sideas.Challenge.API.Controllers
{
    /// <summary>
    /// Controlador de inicio del proceso de carga de datos desde APIs externas.
    /// Este controlador coordina la obtención y almacenamiento de profesiones, zonas y asignaciones.
    /// </summary>
    [ApiController]
    [Route("start")]
    public class StartController : ControllerBase
    {
        private readonly ILogger<StartController> _logger;
        private readonly AgrupacionService _agrupacionService;
        private readonly FueroService _fueroService;
        private readonly AsignacionService _asignacionService;

        public StartController(
            ILogger<StartController> logger,
            AgrupacionService agrupacionService,
            FueroService fueroService,
            AsignacionService asignacionService)
        {
            _logger = logger;
            _agrupacionService = agrupacionService;
            _fueroService = fueroService;
            _asignacionService = asignacionService;
        }

        /// <summary>
        /// Endpoint GET /start/index
        /// Ejecuta la carga y persistencia de datos de profesiones, zonas y asignaciones.
        /// </summary>
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Inicio del proceso de carga de datos.");

            // Obtener y guardar profesiones desde API externa
            var profesiones = await _agrupacionService.GetAndStoreAllProfesionesAsync();

            // Obtener y guardar zonas desde API externa
            var zonas = await _fueroService.GetAndStoreAllZonasAsync();

            // Obtener y guardar asignaciones desde API externa
            var asignaciones = await _asignacionService.FetchAndStoreAsignaciones();

            int countProfesiones = profesiones?.Count() ?? 0;
            int countZonas = zonas?.Count() ?? 0;
            int countAsignaciones = asignaciones?.Count() ?? 0;

            _logger.LogInformation("Resumen: Profesiones={prof}, Zonas={zon}, Asignaciones={asi}",
                countProfesiones, countZonas, countAsignaciones);

            // Validar si alguna colección está vacía
            if (countProfesiones == 0 || countZonas == 0 || countAsignaciones == 0)
            {
                _logger.LogError("Error al cargar datos: Algunos conjuntos están vacíos.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error: No se pudieron guardar correctamente todos los datos.",
                    resumen = new
                    {
                        profesiones = countProfesiones,
                        zonas = countZonas,
                        asignaciones = countAsignaciones
                    }
                });
            }

            return Ok(new
            {
                mensaje = "Datos cargados correctamente.",
                resumen = new
                {
                    profesiones = countProfesiones,
                    zonas = countZonas,
                    asignaciones = countAsignaciones
                }
            });
        }
    }
}
