using System.Linq.Expressions;
using static AmsRecords.Chemicals.ChemicalTypeAuthorizationDtos;

namespace AmsRecords.Chemicals;

public static class ChemicalTypeAuthorizationExtensions
{
    public static Expression<Func<ChemicalTypeAuthorization, ChemicalTypeAuthorizationDto>> DtoProjection { get; }
        = x => new ChemicalTypeAuthorizationDto(
            x.PubId,
            x.ChemType == null ? Guid.Empty : x.ChemType.PubId,
            x.ChemType == null ? "" : x.ChemType.ChemTypeName ?? "",
            x.Jurisdiction == null ? Guid.Empty : x.Jurisdiction.PubId,
            x.Jurisdiction == null ? "" : x.Jurisdiction.Name ?? "",
            x.Allowed,
            x.ValidFromUtc,
            x.ValidToUtc,
            x.Active);

    private static readonly Func<ChemicalTypeAuthorization, ChemicalTypeAuthorizationDto> MapDto = DtoProjection.Compile();

    public static ChemicalTypeAuthorizationDto ToDto(this ChemicalTypeAuthorization entity) => MapDto(entity);

    public static ChemicalTypeAuthorization ToEntity(
        this ChemicalTypeAuthorizationCreateDto dto,
        int chemTypeId,
        int jurisdictionId)
        => new()
        {
            ChemTypeId = chemTypeId,
            JurisdictionId = jurisdictionId,
            Allowed = dto.Allowed,
            ValidFromUtc = dto.ValidFromUtc,
            ValidToUtc = dto.ValidToUtc,
            Active = true
        };

    public static void UpdateEntity(this ChemicalTypeAuthorization entity, ChemicalTypeAuthorizationUpdateDto dto)
    {
        entity.Allowed = dto.Allowed;
        entity.ValidFromUtc = dto.ValidFromUtc;
        entity.ValidToUtc = dto.ValidToUtc;
        entity.Active = dto.Active;
    }
}
