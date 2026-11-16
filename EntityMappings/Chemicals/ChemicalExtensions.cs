using System.Linq.Expressions;
using static AmsRecords.Chemicals.ChemicalDtos;

namespace AmsRecords.Chemicals;

public static class ChemicalExtensions
{
    public static Expression<Func<Chemical, ChemicalDto>> DtoProjection { get; }
        = x => new ChemicalDto(
            x.PubId,
            x.ChemName,
            x.ChemicalFormula ?? "",
            x.IsElemental,
            x.ChemType == null ? Guid.Empty : x.ChemType.PubId,
            x.ChemType == null ? "" : x.ChemType.ChemTypeName ?? "",
            x.UnitType == null ? null : x.UnitType.PubId,
            x.UnitType == null ? "" : x.UnitType.UnitTypeName ?? "",
            x.Active);

    public static Expression<Func<Chemical, ChemicalMiniDto>> MiniDtoProjection { get; }
        = x => new ChemicalMiniDto(x.PubId, x.ChemName, x.UnitType == null ? null : x.UnitType.PubId);

    private static readonly Func<Chemical, ChemicalDto> MapDto = DtoProjection.Compile();
    private static readonly Func<Chemical, ChemicalMiniDto> MapMiniDto = MiniDtoProjection.Compile();

    public static ChemicalDto ToDto(this Chemical entity) => MapDto(entity);
    public static ChemicalMiniDto ToMiniDto(this Chemical entity) => MapMiniDto(entity);

    public static void UpdateEntity(this Chemical entity, ChemicalUpdateDto dto, int chemTypeId)
    {
        entity.ChemName = dto.ChemName.Trim();
        entity.ChemicalFormula = dto.ChemForm?.Trim();
        entity.IsElemental = dto.IsElemental;
        entity.ChemTypeId = chemTypeId;
        entity.Active = dto.Active;
    }

    public static Chemical ToEntity(this ChemicalCreateDto dto, int chemTypeId)
        => new()
        {
            ChemName = dto.ChemName.Trim(),
            ChemicalFormula = dto.ChemForm?.Trim(),
            IsElemental = dto.IsElemental,
            ChemTypeId = chemTypeId,
        };
}
