using System.Linq.Expressions;
using static AmsRecords.GrassSpecies.GrassSpeciesChemicalDtos;

namespace AmsRecords.GrassSpecies;

public static class GrassSpeciesChemicalExtensions
{
    public static Expression<Func<GrassSpeciesChemical, GrassSpeciesChemicalDto>> DtoProjection { get; }
        = x => new GrassSpeciesChemicalDto(
            x.PubId,
            x.Active,
            x.GrassSpecieses == null ? Guid.Empty : x.GrassSpecieses.PubId,
            x.GrassSpecieses == null ? "" : x.GrassSpecieses.Name ?? "",
            x.Chem == null ? Guid.Empty : x.Chem.PubId,
            x.Chem == null ? "" : x.Chem.ChemName ?? "",
            x.UseMode,
            x.Notes);

    private static readonly Func<GrassSpeciesChemical, GrassSpeciesChemicalDto> MapDto = DtoProjection.Compile();

    public static GrassSpeciesChemicalDto ToDto(this GrassSpeciesChemical entity) => MapDto(entity);

    public static void UpdateEntity(this GrassSpeciesChemical x, GrassSpeciesChemicalUpdateDto dto)
    {
        x.Active = dto.Active;
        x.UseMode = dto.UseMode;
        x.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
    }
}