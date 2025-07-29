using Moq;
using Microsoft.Extensions.Logging;
using Sideas.Challenge.Application.Services;
using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;

public class AsignacionServiceTests
{
    /// <summary>
    ///Este test valida que el método FetchAndStoreAsignaciones del servicio:
    ///Llama varias veces a la API paginada y recoge todos los datos.
    ///Finaliza la carga cuando la API devuelve una página vacía.
    ///Guarda las asignaciones recibidas en la base de datos por cada página con datos.
    ///Devuelve la colección completa de asignaciones cargadas.  
    /// </summary>
    [Fact]
    public async Task FetchAndStoreAsignaciones_ShouldReturnAllAsignaciones_WhenApiReturnsMultiplePages()
    {
        // Arrange: Crear mocks para las dependencias del servicio
        var mockHttpService = new Mock<IHttpService>();
        var mockAsignacionRepo = new Mock<IAsignacionRepository>();
        var mockLogger = new Mock<ILogger<AsignacionService>>();

        // Simular datos de asignaciones para la primera página
        var asignacionesPage1 = new List<Asignacion>
        {
            new Asignacion { Id = 1 },
            new Asignacion { Id = 2 }
        };

        // Simular datos de asignaciones para la segunda página
        var asignacionesPage2 = new List<Asignacion>
        {
            new Asignacion { Id = 3 }
        };

        // Configurar secuencia de respuestas para las llamadas HTTP a la API:
        // Primera llamada (page=0) devuelve una página con asignaciones
        mockHttpService.SetupSequence(s => s.GetAsync<AsignacionApiResponse>(
                It.Is<string>(url => url.Contains("page=0"))))
            .ReturnsAsync(new AsignacionApiResponse { Content = asignacionesPage1 });

        // Segunda llamada (page=1) devuelve otra página con asignaciones
        mockHttpService.SetupSequence(s => s.GetAsync<AsignacionApiResponse>(
                It.Is<string>(url => url.Contains("page=1"))))
            .ReturnsAsync(new AsignacionApiResponse { Content = asignacionesPage2 });

        // Tercera llamada (page=2) devuelve página vacía para indicar fin de datos
        mockHttpService.SetupSequence(s => s.GetAsync<AsignacionApiResponse>(
                It.Is<string>(url => url.Contains("page=2"))))
            .ReturnsAsync(new AsignacionApiResponse { Content = new List<Asignacion>() });

        // Crear instancia del servicio a probar inyectando los mocks
        var service = new AsignacionService(
            mockLogger.Object,
            mockHttpService.Object,
            mockAsignacionRepo.Object);

        // Act: Ejecutar el método que recupera y guarda asignaciones paginadas
        var result = await service.FetchAndStoreAsignaciones();

        // Assert: Verificar que el resultado no sea nulo
        Assert.NotNull(result);

        // Convertir el resultado a lista para facilitar verificaciones
        var resultList = new List<Asignacion>(result);

        // Verificar que se hayan obtenido todas las asignaciones de ambas páginas
        Assert.Equal(3, resultList.Count);

        // Verificar que el repositorio haya guardado dos veces (una por cada página con datos)
        mockAsignacionRepo.Verify(r => r.SaveAsync(It.IsAny<List<Asignacion>>()), Times.Exactly(2));
    }
}
