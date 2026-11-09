using static AmsRecords.Years.YearDtos;

using static AmsRecords.Years.YearExtensions;

namespace AmsRecords.Years;

public static class YearEntityMappings
{
    public static YearDto ToDto(this Year year)
         => new(
             PubId: year.PubId,
             YearNumber: year.YearNumber,
             IsCurrent: year.IsCurrent,
             IsVisible: year.IsVisible,
             IsClosed: year.IsClosed
          );

    public static Year ToEntity(this YearCreateDto dto)
        => new()
        {
            YearNumber = dto.YearNumber,
            IsCurrent = dto.IsCurrent,
            IsVisible = dto.IsVisible,
            IsClosed = dto.IsClosed
        };

    public static void UpdateEntity(this Year entity, YearUpdateDto dto)
    {
        entity.YearNumber = dto.YearNumber;
        entity.IsCurrent = dto.IsCurrent;
        entity.IsVisible = dto.IsVisible;
        entity.IsClosed = dto.IsClosed;
    }

}
