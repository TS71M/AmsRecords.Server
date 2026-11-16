using System.Linq.Expressions;
using static AmsRecords.Chemicals.ChemicalAuthorizationDtos;

namespace AmsRecords.Chemicals;

public static class ChemicalAuthorizationExtensions
{
    public static Expression<Func<ChemicalAuthorization, ChemicalAuthorizationDto>> DtoProjection { get; }
        = x => new ChemicalAuthorizationDto(
            x.PubId,
            x.Chemical == null ? Guid.Empty : x.Chemical.PubId,
            x.Chemical == null ? "" : x.Chemical.ChemName ?? "",
            x.Jurisdiction == null ? Guid.Empty : x.Jurisdiction.PubId,
            x.Jurisdiction == null ? "" : x.Jurisdiction.Name ?? "",
            x.Allowed,
            x.ValidFromUtc,
            x.ValidToUtc,
            x.Active);

    private static readonly Func<ChemicalAuthorization, ChemicalAuthorizationDto> MapDto = DtoProjection.Compile();

    public static ChemicalAuthorizationDto ToDto(this ChemicalAuthorization entity) => MapDto(entity);

    public static ChemicalAuthorization ToEntity(this ChemicalAuthorizationCreateDto dto, int chemId, int jurisdictionId)
        => new()
        {
            ChemId = chemId,
            JurisdictionId = jurisdictionId,
            Allowed = dto.Allowed,
            ValidFromUtc = dto.ValidFromUtc,
            ValidToUtc = dto.ValidToUtc,
            Active = true
        };

    public static void UpdateEntity(this ChemicalAuthorization entity, ChemicalAuthorizationUpdateDto dto)
    {
        entity.Allowed = dto.Allowed;
        entity.ValidFromUtc = dto.ValidFromUtc;
        entity.ValidToUtc = dto.ValidToUtc;
        entity.Active = dto.Active;
    }
}