# Sideas.Challenge

Este proyecto es una solución full-stack para visualizar datos obtenidos desde APIs externas y almacenarlos en una base de datos SQL Server.
La solución incluye un backend desarrollado en ASP.NET Core con Entity Framework y un frontend desarrollado en Angular + Bootstrap.

---

# Archivo .gitignore para excluir archivos y carpetas generados automáticamente,

# temporales, dependencias y datos sensibles que no deben subirse al repositorio.

## Paso a paso para correr el proyecto

### 1. Crear la base de datos

1. Abrí SQL Server Management Studio (SSMS) u otra herramienta compatible.
2. Ejecutá el archivo `SideasChallengeDb.sql` incluido en este repositorio para crear la base de datos y sus tablas necesarias.

> Ubicación sugerida del archivo SQL: `./database/SideasChallengeDb.sql`

> Asegurate de que el nombre de la base de datos creada coincida con el configurado en `appsettings.json` del proyecto backend.

---

### 2. Ejecutar el backend (.NET API)

1. Abrí la solución `Sideas.Challenge.sln` con Visual Studio o desde consola.
2. Verificá la cadena de conexión en `appsettings.json` (proyecto `Sideas.Challenge.API`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=SideasChallengeDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. Restaurá los paquetes NuGet: `dotnet restore`
4. Ejecutá las migraciones si fuera necesario: `dotnet ef migrations add Init` y luego `dotnet ef database update`
5. Corré el proyecto API (`Sideas.Challenge.API`) presionando **F5** o usando `dotnet run` desde consola.

La API expone endpoints como:

- `GET /api/profesiones`
- `GET /api/agrupaciones`
- `GET /api/zonas`
- `GET /api/asignaciones`

---

### 3. Ejecutar el frontend (Angular)

1. Abrí una terminal en la carpeta `sideas-ui`.
2. Instalá las dependencias con:

```bash
npm install
```

3. Iniciá el servidor de desarrollo con:

```bash
ng serve
```

4. Accedé a la app en `http://localhost:4200`.

---

## Funcionamiento general del proyecto

Este proyecto consume datos desde servicios externos (como agrupaciones, profesiones, zonas y asignaciones), los almacena en la base de datos usando Entity Framework, y los expone a través de una API REST. El frontend en Angular se comunica con esa API para mostrar y manipular los datos.

---

## ¿Qué se puede hacer desde la interfaz Angular?

- Ver el listado de profesiones, agrupaciones, zonas y asignaciones.
- Buscar profesiones por texto desde un campo de filtro dinámico.
- Navegar entre los distintos componentes a través del menú.
- Visualizar los datos de forma responsive y amigable gracias a Bootstrap.

---

## Extras posibles

- Agregar tests unitarios o e2e.
- Extender funcionalidades (CRUD completo).
- Añadir paginación, ordenamiento, filtros por relaciones (e.g. zona → asignaciones).

## Decisiones Técnicas y Justificaciones

### Repositorios separados por entidad

> Opté por una solución basada en el patrón de **repositorio por entidad**, utilizando interfaces como `IAgrupacionRepository`, `IProfesionRepository`, etc., cada una con su respectiva implementación.

#### Justificación:

- **Responsabilidad única (SRP)**: cada repositorio se especializa exclusivamente en una entidad.
- **Escalabilidad**: facilita el crecimiento del proyecto sin acoplar lógica entre entidades.
- **Testeo más claro y aislado**: permite escribir pruebas específicas por entidad sin lógica condicional.
- **Legibilidad y mantenimiento**: mejora la comprensión del código al aislar responsabilidades.

### Estructura en capas desacopladas

El proyecto fue organizado en una solución `.NET` con los siguientes proyectos separados:

- `Sideas.Challenge.Domain`: entidades, interfaces y utilidades.
- `Sideas.Challenge.Application`: lógica de negocio, servicios y DTOs/mappers.
- `Sideas.Challenge.Infrastructure`: implementación de repositorios y acceso a datos con EF Core.
- `Sideas.Challenge.API`: capa de presentación y configuración de endpoints REST.

#### Justificación:

- Sigue principios de **arquitectura limpia** y **separación de responsabilidades**.
- Permite evolucionar cada capa de forma independiente.
- Mejora la organización, el testeo y el versionado del código.

### Persistencia con Entity Framework Core

> Se utilizó **Entity Framework Core** con una base de datos SQL Server para la persistencia de datos.

#### Migraciones:

- La carpeta `Migrations/` fue **incluida en el control de versiones**.

#### Justificación:

- Permite que otros desarrolladores puedan ejecutar `dotnet ef database update` y generar la base sin intervención adicional.
- Facilita el rastreo histórico de cambios en el modelo de datos.

### Validaciones y consistencia de datos

> Se agregaron validaciones mínimas para evitar duplicación de registros al guardar datos desde las APIs externas.

#### Justificación:

- Mejora la integridad de los datos almacenados.
- Responde a un escenario realista de sincronización con servicios externos.

---

# Tests Unitarios

## Qué incluyen los tests

- El proyecto cuenta con tests unitarios para validar la lógica de los servicios principales que consumen APIs externas y almacenan datos en la base:

1. FueroServiceTests: Verifica la obtención y almacenamiento de fueros y zonas.

2. AgrupacionServiceTests: Valida la carga y guardado de agrupaciones, profesiones únicas y sus relaciones.

3. AsignacionServiceTests: Comprueba la carga paginada de asignaciones desde la API y su almacenamiento correcto.

- Estos tests usan Moq para simular dependencias externas (como llamadas HTTP y repositorios), evitando conexiones reales y permitiendo pruebas rápidas y aisladas.

### Cómo ejecutar los tests

1. Abrí la solución Sideas.Challenge.sln en Visual Studio o tu IDE favorito.

2. Ubicá el proyecto de tests (generalmente con sufijo .Tests).

3. Ejecutá los tests desde la consola:

```bash
dotnet test
```
