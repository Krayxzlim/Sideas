using Newtonsoft.Json;
using Sideas.Challenge.Domain.Utils;

namespace Sideas.Challenge.Domain.Entities
{
        public class Asignacion
    {
        public long Id { get; set; }
        public string Tipo { get; set; }
        public string NumeroExp { get; set; }
        public int? AnioExp { get; set; } // (opcional)
        public int? Incidente { get; set; } // (opcional)
        public string Autos { get; set; }
        public string NombreAuxiliar { get; set; }
        public string TipoDocAuxiliar { get; set; }
        public long? DocAuxiliar { get; set; }

        // Conversor personalizado para la fecha de creación (en formato dd-MM-yyyy)
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? CreacionFecha { get; set; }

        public int? IdProfesionAux { get; set; }
        public string EspecialidadAuxiliar { get; set; }
        public string ProfesionAuxiliar { get; set; }
        public int? Fuero { get; set; }
        public int? Zona { get; set; }
        public string Reparticion { get; set; }
    }
}
