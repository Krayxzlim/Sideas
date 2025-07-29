using Moq;
using Microsoft.Extensions.Logging;
using Sideas.Challenge.Application.Services;
using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;
using Sideas.Challenge.Application.DTOs;

public class AgrupacionServiceTests
{
    /// <summary>
    ///Verifica que el método GetAndStoreAllProfesionesAsync del servicio:
    ///Obtiene correctamente las agrupaciones y profesiones desde la API (simulada con mocks).
    ///Convierte los DTOs a entidades y elimina profesiones duplicadas.
    ///Guarda agrupaciones, profesiones únicas y relaciones en sus respectivos repositorios.
    ///Retorna la lista de profesiones únicas correctamente.    
    /// </summary>
    [Fact]
    public async Task GetAndStoreAllProfesionesAsync_ShouldReturnUniqueProfesiones_WhenApiReturnsData()
    {
        // Arrange: Crear mocks para las dependencias del servicio
        var mockHttpService = new Mock<IHttpService>();
        var mockAgrupacionRepo = new Mock<IAgrupacionRepository>();
        var mockProfesionRepo = new Mock<IProfesionRepository>();
        var mockAgrupacionProfesionRepo = new Mock<IAgrupacionProfesionRepository>();
        var mockLogger = new Mock<ILogger<AgrupacionService>>();

        // Datos simulados que devuelve la API para agrupaciones
        var agrupaciones = new List<Agrupacion>
        {
            new Agrupacion { Id = 1, Descripcion = "Agrupacion 1" },
            new Agrupacion { Id = 2, Descripcion = "Agrupacion 2" }
        };

        // Datos simulados que devuelve la API para profesiones de agrupación 1
        var profesionesDto1 = new List<ProfesionDto>
        {
            new ProfesionDto { Id = 1, Descripcion = "Profesion A" },
            new ProfesionDto { Id = 2, Descripcion = "Profesion B" }
        };

        // Datos simulados que devuelve la API para profesiones de agrupación 2
        // Contiene un duplicado (Id=2) para probar eliminación de duplicados
        var profesionesDto2 = new List<ProfesionDto>
        {
            new ProfesionDto { Id = 2, Descripcion = "Profesion B" }, // duplicado
            new ProfesionDto { Id = 3, Descripcion = "Profesion C" }
        };

        // Configurar el mock para que devuelva las agrupaciones simuladas
        mockHttpService.Setup(s => s.GetAsync<List<Agrupacion>>("https://consulta-peritos-api.pjn.gov.ar/api/agrupacion"))
                       .ReturnsAsync(agrupaciones);

        // Configurar el mock para que devuelva profesiones simuladas para agrupación 1
        mockHttpService.Setup(s => s.GetAsync<List<ProfesionDto>>("https://consulta-peritos-api.pjn.gov.ar/api/profesion?agrupacion=1"))
                       .ReturnsAsync(profesionesDto1);

        // Configurar el mock para que devuelva profesiones simuladas para agrupación 2
        mockHttpService.Setup(s => s.GetAsync<List<ProfesionDto>>("https://consulta-peritos-api.pjn.gov.ar/api/profesion?agrupacion=2"))
                       .ReturnsAsync(profesionesDto2);

        // Crear instancia del servicio a probar, inyectando los mocks
        var service = new AgrupacionService(
            mockLogger.Object,
            mockHttpService.Object,
            mockAgrupacionRepo.Object,
            mockProfesionRepo.Object,
            mockAgrupacionProfesionRepo.Object);

        // Act: Ejecutar el método bajo prueba
        var result = await service.GetAndStoreAllProfesionesAsync();

        // Assert: Verificar que el resultado no sea nulo y tenga las profesiones únicas
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // Debe contener 3 profesiones únicas (Ids 1,2,3)

        // Verificar que se guardaron las agrupaciones una vez
        mockAgrupacionRepo.Verify(r => r.SaveAsync(agrupaciones), Times.Once);

        // Verificar que se guardaron las profesiones únicas una vez
        mockProfesionRepo.Verify(r => r.SaveAsync(It.Is<List<Profesion>>(list => list.Count == 3)), Times.Once);

        // Verificar que se guardaron las relaciones agrupacion-profesion una vez
        mockAgrupacionProfesionRepo.Verify(r => r.SaveAsync(It.IsAny<List<AgrupacionProfesion>>()), Times.Once);
    }
}
