using System;
using Sideas.Challenge.Application.DTOs;
using Sideas.Challenge.Domain.Entities;

namespace Sideas.Challenge.Application.Mappers
{
    /// <summary>
    /// Mapper estático encargado de convertir objetos ProfesionDto a entidades Profesion.
    /// </summary>
    public static class ProfesionMapper
    {
        public static Profesion ToEntity(ProfesionDto dto)
        {
            return new Profesion
            {
                Id = dto.Id,
                ProfesionCodigo = dto.Profesion,
                Especialidad = dto.Especialidad,
                Descripcion = dto.Descripcion,
                Estado = dto.Estado
            };
        }
    }
}
