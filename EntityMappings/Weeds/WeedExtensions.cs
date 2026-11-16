using System.Linq.Expressions;
using static AmsRecords.Weeds.WeedDtos;

namespace AmsRecords.Weeds;

public static class WeedExtensions
{
    public static Expression<Func<Weed, WeedDto>> DtoProjection { get; }
        = weed => new WeedDto(weed.PubId, weed.Code, weed.DefaultName, weed.Active, weed.SortOrder,
            weed.PreferredFromGts, weed.PreferredToGts, weed.AcceptableFromGts, weed.AcceptableToGts,
            weed.ManagementNote);

    private static readonly Func<Weed, WeedDto> MapDto = DtoProjection.Compile();

    public static WeedDto ToDto(this Weed weed) => MapDto(weed);

    public static Weed ToEntity(this WeedCreateDto dto)
        => new()
        {
            Code = dto.Code.Trim(),
            DefaultName = dto.DefaultName.Trim(),
            Active = dto.Active,
            SortOrder = dto.SortOrder,
            PreferredFromGts = dto.PreferredFromGts,
            PreferredToGts = dto.PreferredToGts,
            AcceptableFromGts = dto.AcceptableFromGts,
            AcceptableToGts = dto.AcceptableToGts,
            ManagementNote = string.IsNullOrWhiteSpace(dto.ManagementNote) ? null : dto.ManagementNote.Trim()
        };

    public static Weed UpdateEntity(this Weed entity, WeedUpdateDto dto)
    {
        entity.Code = dto.Code.Trim();
        entity.DefaultName = dto.DefaultName.Trim();
        entity.Active = dto.Active;
        entity.SortOrder = dto.SortOrder;
        entity.PreferredFromGts = dto.PreferredFromGts;
        entity.PreferredToGts = dto.PreferredToGts;
        entity.AcceptableFromGts = dto.AcceptableFromGts;
        entity.AcceptableToGts = dto.AcceptableToGts;
        entity.ManagementNote = string.IsNullOrWhiteSpace(dto.ManagementNote) ? null : dto.ManagementNote.Trim();

        return entity;
    }
}
