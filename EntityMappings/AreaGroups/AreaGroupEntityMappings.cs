using static AmsRecords.AreaGroups.AreaGroupDtos;

using static AmsRecords.AreaGroups.AreaGroupExtensions;

namespace AmsRecords.AreaGroups;

public static class AreaGroupEntityMappings
{
    public static AreaGroupDto ToDto(this AreaGroup x)
        => new(
            x.PubId,
            x.Code,
            x.Name,
            x.Active,
            x.SupportsSurfaces,
            x.SupportsTurfDefaults,
            x.SupportsComposition,
            x.SortOrder
        );

    public static AreaGroupMiniDto ToMiniDto(this AreaGroup x)
        => new(
            x.PubId,
            x.Name
        );
    public static AreaGroup ToEntity(this AreaGroupCreateDto dto)
        => new()
        {
            Code = dto.Code.Trim(),
            Name = dto.Name.Trim(),
            Active = true,
            SupportsSurfaces = dto.SupportsSurfaces,
            SupportsTurfDefaults = dto.SupportsTurfDefaults,
            SupportsComposition = dto.SupportsComposition,
            SortOrder = dto.SortOrder
        };

    public static void UpdateEntity(this AreaGroupUpdateDto dto, AreaGroup entity)
    {
        entity.Code = dto.Code.Trim();
        entity.Name = dto.Name.Trim();
        entity.Active = dto.Active;
        entity.SupportsSurfaces = dto.SupportsSurfaces;
        entity.SupportsTurfDefaults = dto.SupportsTurfDefaults;
        entity.SupportsComposition = dto.SupportsComposition;
        entity.SortOrder = dto.SortOrder;
    }

}
