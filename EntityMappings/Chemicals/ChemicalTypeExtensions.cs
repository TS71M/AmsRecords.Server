using System.Linq.Expressions;
using static AmsRecords.Chemicals.ChemicalTypeDtos;

namespace AmsRecords.Chemicals;

public static class ChemicalTypeExtensions
{
    public static Expression<Func<ChemicalType, ChemicalTypeDto>> DtoProjection { get; }
        = x => new ChemicalTypeDto(
            x.PubId,
            x.ChemTypeName,
            x.DefaultBlockInProtectedZone,
            x.Active);

    private static readonly Func<ChemicalType, ChemicalTypeDto> MapDto = DtoProjection.Compile();

    public static ChemicalTypeDto ToDto(this ChemicalType entity) => MapDto(entity);

    public static ChemicalType ToEntity(this ChemicalTypeCreateDto dto)
        => new()
        {
            ChemTypeName = dto.ChemTypeName?.Trim() ?? "",
            DefaultBlockInProtectedZone = dto.DefaultBlockInProtectedZone,
            Active = true
        };

    public static void UpdateEntity(this ChemicalType x, ChemicalTypeUpdateDto dto)
    {
        x.ChemTypeName = dto.ChemTypeName?.Trim() ?? x.ChemTypeName;
        x.DefaultBlockInProtectedZone = dto.DefaultBlockInProtectedZone;
        x.Active = dto.Active;
    }
}
