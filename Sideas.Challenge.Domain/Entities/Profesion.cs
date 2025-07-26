using System.Text.Json.Serialization;

namespace Sideas.Challenge.Domain.Entities
{
    public class Profesion
    {
        public int Id { get; set; }
        public int ProfesionCodigo { get; set; }
        public int Especialidad { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        // Relación muchos-a-muchos con Agrupacion
        public ICollection<AgrupacionProfesion> AgrupacionProfesiones { get; set; }

    }
}
