namespace Sideas.Challenge.Domain.Entities
{
    public class Agrupacion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        // Relación con profesiones a través de la tabla intermedia AgrupacionProfesion
        public ICollection<AgrupacionProfesion> AgrupacionProfesiones { get; set; }
    };
}
