
using static AmsRecords.Coordinates.CoordinateExtension;

namespace AmsRecords.Coordinates;

public static class CoordinateEntityMappings
{
    public static CoordinateDto ToDto(this Coordinate coord)
        => new(coord.PubId, coord.Latitude, coord.Longitude);

    public static Coordinate ToEntity(this CoordinateCreateDto dto, User user)
        => new()
        {
            Latitude = (decimal)dto.Latitude,
            Longitude = (decimal)dto.Longitude,
            IbuId = user.IbuId,
            Ibu = user.Ibu!
        };

    public static void UpdateEntity(this Coordinate entity, CoordinateUpdateDto dto)
    {
        entity.Latitude = (decimal)dto.Latitude;
        entity.Longitude = (decimal)dto.Longitude;
    }

    public static void UpdateFromDto(this Coordinate entity, CoordinateUpdateDto dto)
    {
        entity.Latitude = (decimal)dto.Latitude;
        entity.Longitude = (decimal)dto.Longitude;
    }

}
