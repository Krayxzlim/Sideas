namespace Sideas.Challenge.Domain.Entities
{
    // Entidad de unión muchos-a-muchos entre Agrupacion y Profesion
    public class AgrupacionProfesion
    {
        public int AgrupacionId { get; set; } // Clave foránea a Agrupacion
        public Agrupacion Agrupacion { get; set; } // Navegación a Agrupacion

        public int ProfesionId { get; set; } // Clave foránea a Profesion
        public Profesion Profesion { get; set; } // Navegación a Profesion
    }
}