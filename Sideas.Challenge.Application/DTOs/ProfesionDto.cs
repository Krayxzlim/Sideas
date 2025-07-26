using System;

namespace Sideas.Challenge.Application.DTOs
{
    /// <summary>
    /// DTO que representa una profesión proveniente de la API externa.
    /// Se utiliza para mapear los datos antes de convertirlos a entidad de dominio.
    /// </summary>
    public class ProfesionDto
    {
        public int Id { get; set; }

        // Código de la profesión (propiedad llamada "profesion" en la API)
        public int Profesion { get; set; }

        public int Especialidad { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }
}
