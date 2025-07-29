using Moq;
using Microsoft.Extensions.Logging;
using Sideas.Challenge.Application.Services;
using Sideas.Challenge.Domain.Entities;
using Sideas.Challenge.Domain.Repositories;

public class FueroServiceTests
{
    /// <summary>
    ///Este test valida que el método GetAndStoreAllZonasAsync de FueroService:
    ///Obtiene una lista de fueros simulada desde la API mockeada.
    ///Para cada fuero, obtiene las zonas correspondientes simuladas.
    ///Guarda correctamente los fueros y las zonas en sus repositorios respectivos.
    ///Devuelve la lista completa de zonas (la suma de las zonas de todos los fueros).
    /// </summary>
    [Fact]
    public async Task GetAndStoreAllZonasAsync_ShouldSaveFuerosAndZonas_WhenApiReturnsData()
    {
        // Arrange: Crear mocks para las dependencias necesarias
        var mockHttpService = new Mock<IHttpService>();  // Mock de la interfaz para simular llamadas HTTP
        var mockFueroRepo = new Mock<IFueroRepository>();
        var mockZonaRepo = new Mock<IZonaRepository>();
        var mockLogger = new Mock<ILogger<FueroService>>();

        // Datos simulados para fueros
        var fueros = new List<Fuero>
        {
            new Fuero { Id = 1, Descripcion = "Fuero Penal" },
            new Fuero { Id = 2, Descripcion = "Fuero Civil" }
        };

        // Datos simulados para zonas relacionadas con el fuero 1
        var zonasFuero1 = new List<Zona>
        {
            new Zona { Id = 1, Descripcion = "Zona 1", FueroId = 1 },
            new Zona { Id = 2, Descripcion = "Zona 2", FueroId = 1 }
        };

        // Datos simulados para zonas relacionadas con el fuero 2
        var zonasFuero2 = new List<Zona>
        {
            new Zona { Id = 3, Descripcion = "Zona 3", FueroId = 2 }
        };

        // Configurar las respuestas mockeadas para las llamadas HTTP simuladas
        mockHttpService.Setup(s => s.GetAsync<List<Fuero>>("https://consulta-peritos-api.pjn.gov.ar/api/fuero"))
                       .ReturnsAsync(fueros);

        mockHttpService.Setup(s => s.GetAsync<List<Zona>>("https://consulta-peritos-api.pjn.gov.ar/api/zona?fuero=1"))
                       .ReturnsAsync(zonasFuero1);

        mockHttpService.Setup(s => s.GetAsync<List<Zona>>("https://consulta-peritos-api.pjn.gov.ar/api/zona?fuero=2"))
                       .ReturnsAsync(zonasFuero2);

        // Crear instancia del servicio inyectando los mocks
        var service = new FueroService(
            mockHttpService.Object,
            mockFueroRepo.Object,
            mockZonaRepo.Object,
            mockLogger.Object);

        // Act: Ejecutar el método a testear
        var result = await service.GetAndStoreAllZonasAsync();

        // Assert: Verificar que el resultado no sea nulo y tenga la cantidad esperada de zonas
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // Se esperan 3 zonas en total

        // Verificar que se haya guardado la lista de fueros exactamente una vez
        mockFueroRepo.Verify(r => r.SaveAsync(fueros), Times.Once);

        // Verificar que se haya guardado la lista combinada de zonas exactamente una vez
        mockZonaRepo.Verify(r => r.SaveAsync(It.Is<List<Zona>>(z => z.Count == 3)), Times.Once);
    }
}
