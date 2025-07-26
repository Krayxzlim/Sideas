using Microsoft.AspNetCore.Mvc;
using Sideas.Challenge.Domain.Repositories;
using Sideas.Challenge.Domain.Entities;
/// <summary>
/// Controlador API para gestionar las profesiones.
/// 
/// Este controller expone un endpoint REST para obtener la lista completa de profesiones almacenadas,
/// consumiendo la capa de acceso a datos mediante el repositorio IProfesionRepository.
/// 
/// Interfaz frontend desarrollada en Angular,
/// que necesita mostrar las profesiones mediante una llamada HTTP GET a "api/profesiones".
/// 
/// Método asincrónico para rendimiento y escalabilidad del servicio.
/// </summary>
namespace Sideas.Challenge.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesionesController : ControllerBase
    {
        private readonly IProfesionRepository _profesionRepository;

        public ProfesionesController(IProfesionRepository profesionRepository)
        {
            _profesionRepository = profesionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profesion>>> GetAll()
        {
            var profesiones = await _profesionRepository.GetAllAsync();
            return Ok(profesiones);
        }
    }
}
