using static AmsRecords.GrassTypes.GrassTypeDtos;

using static AmsRecords.GrassTypes.GrassTypeExtensions;

namespace AmsRecords.GrassTypes;

public static class GrassTypeEntityMappings
{
    public static GrassTypeDto ToDto(this GrassType entity)
        => new(
            entity.PubId,
            entity.GrassTypeName,
            entity.Pathway,
            entity.Description,
            entity.OptimalTemperature,
            entity.Variation
        );

    public static GrassTypeMiniDto ToMiniDto(this GrassType entity)
       => new(
           entity.PubId,
           entity.GrassTypeName
       );

    public static GrassType ToEntity(this GrassTypeCreateDto dto)
        => new()
        {
            GrassTypeName = dto.GrassTypeName,
            Pathway = dto.Pathway,
            Description = dto.Description,
            OptimalTemperature = dto.OptimalTemperatur,
            Variation = dto.Variation
        };

    public static void UpdateEntity(this GrassType entity, GrassTypeUpdateDto dto)
    {
        entity.GrassTypeName = dto.GrassTypeName;
        entity.Pathway = dto.Pathway;
        entity.Description = dto.Description;
        entity.OptimalTemperature = dto.OptimalTemperatur;
        entity.Variation = dto.Variation;
    }

}
