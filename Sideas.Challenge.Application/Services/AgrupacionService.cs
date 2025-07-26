using Sideas.Challenge.Application.Mappers;
using Sideas.Challenge.Application.DTOs;
using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Sideas.Challenge.Application.Services
{
    /// <summary>
    /// Servicio encargado de coordinar la carga y persistencia de agrupaciones,
    /// profesiones y la relación entre ambas desde APIs externas.
    /// </summary>
    public class AgrupacionService
    {
        private readonly HttpService _httpService;
        private readonly IAgrupacionRepository _agrupacionRepository;
        private readonly IProfesionRepository _profesionRepository;
        private readonly IAgrupacionProfesionRepository _agrupacionProfesionRepository;
        private readonly ILogger<AgrupacionService> _logger;

        public AgrupacionService(
            ILogger<AgrupacionService> logger,
            HttpService httpService,
            IAgrupacionRepository agrupacionRepository,
            IProfesionRepository profesionRepository,
            IAgrupacionProfesionRepository agrupacionProfesionRepository)
        {
            _logger = logger;
            _httpService = httpService;
            _agrupacionRepository = agrupacionRepository;
            _profesionRepository = profesionRepository;
            _agrupacionProfesionRepository = agrupacionProfesionRepository;
        }

        /// <summary>
        /// Obtiene agrupaciones y sus profesiones desde la API,
        /// las guarda en base de datos y devuelve la lista de profesiones únicas.
        /// </summary>
        public async Task<List<Profesion>> GetAndStoreAllProfesionesAsync()
        {
            _logger.LogInformation("Solicitando agrupaciones...");
            var agrupaciones = await _httpService.GetAsync<List<Agrupacion>>("https://consulta-peritos-api.pjn.gov.ar/api/agrupacion");

            if (agrupaciones == null || agrupaciones.Count == 0)
            {
                _logger.LogWarning("⚠ No se obtuvieron agrupaciones.");
                return new();
            }

            _logger.LogInformation("Se obtuvieron {Count} agrupaciones.", agrupaciones.Count);
            await _agrupacionRepository.SaveAsync(agrupaciones);

            var allProfesiones = new List<Profesion>();
            var agrupacionProfesionLinks = new List<(int AgrupacionId, int ProfesionId)>();

            foreach (var ag in agrupaciones)
            {
                _logger.LogInformation("Solicitando profesiones para agrupación {Id} - {Descripcion}...", ag.Id, ag.Descripcion);

                var profesionesDto = await _httpService.GetAsync<List<ProfesionDto>>(
                    $"https://consulta-peritos-api.pjn.gov.ar/api/profesion?agrupacion={ag.Id}");

                if (profesionesDto == null || profesionesDto.Count == 0)
                {
                    _logger.LogWarning("⚠️ No se obtuvieron profesiones para agrupación {Id}", ag.Id);
                    continue;
                }

                // Convertir DTOs en entidades
                var profesiones = profesionesDto.Select(ProfesionMapper.ToEntity).ToList();

                // Acumular profesiones y relaciones
                foreach (var prof in profesiones)
                {
                    allProfesiones.Add(prof);
                    agrupacionProfesionLinks.Add((ag.Id, prof.Id));
                }

                _logger.LogInformation("Se agregaron {Count} profesiones de agrupación {Id}", profesiones.Count, ag.Id);
            }

            // Eliminar duplicados por Id
            var profesionesUnicas = allProfesiones
                .GroupBy(p => p.Id)
                .Select(g => g.First())
                .ToList();

            await _profesionRepository.SaveAsync(profesionesUnicas);
            _logger.LogInformation("Profesiones únicas guardadas en base ({Count})", profesionesUnicas.Count);

            // Guardar relaciones agrupación-profesión
            var relaciones = agrupacionProfesionLinks
                .Select(link => new AgrupacionProfesion
                {
                    AgrupacionId = link.AgrupacionId,
                    ProfesionId = link.ProfesionId
                })
                .ToList();

            await _agrupacionProfesionRepository.SaveAsync(relaciones);
            _logger.LogInformation("Relaciones Agrupación <-> Profesión guardadas ({Count})", relaciones.Count);

            return profesionesUnicas;
        }
    }
}
