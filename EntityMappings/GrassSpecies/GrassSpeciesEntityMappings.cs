using static AmsRecords.GrassSpecies.GrassSpeciesDtos;

using static AmsRecords.GrassSpecies.GrassSpeciesExtensions;

namespace AmsRecords.GrassSpecies;

public static class GrassSpeciesEntityMappings
{

    public static GrassSpeciesDto ToDto(AmsModels.GrassSpecies dto) =>
       new(
           dto.PubId,
           dto.Active,
           dto.Name,
           dto.LatinName,
           dto.GrassType.PubId,
           dto.GrassType.GrassTypeName,
           dto.ShowInGtsInterpretation,
           dto.NMaxYear,
           dto.NMaxMonth,
           dto.KNRatioMin,
           dto.KNRatioMax,
           dto.PHMin,
           dto.PHMax,
           dto.SaltToleranceMin,
           dto.SaltToleranceMax,
           dto.GrowthHabit,
           dto.LiguleLeafMargin,
           dto.AuriclesLeafHairs,
           dto.VernationLeafStructure,
           dto.CollarLeafArrangement,
           dto.SeedheadColorFlowerColor,
           dto.RootType,
           dto.CreatedAtUtc,
           dto.UpdatedAtUtc,
           dto.MainAppImage?.PubId,
           0
       );

    public static AmsModels.GrassSpecies ToEntity(this GrassSpeciesCreateDto dto, int grassTypeId)
        => new()
        {
            Name = dto.Name,
            LatinName = dto.LatinName,
            GrassTypeId = grassTypeId,
            NMaxYear = dto.NMaxYear,
            NMaxMonth = dto.NMaxMonth,
            KNRatioMin = dto.KNRatioMin,
            KNRatioMax = dto.KNRatioMax,
            PHMin = dto.PHMin,
            PHMax = dto.PHMax,
            SaltToleranceMin = dto.SaltToleranceMin,
            SaltToleranceMax = dto.SaltToleranceMax,
            GrowthHabit = dto.GrowthHabit,
            RootType = dto.RootType,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            ShowInGtsInterpretation = dto.ShowInGtsInterpretation
        };

    public static void UpdateEntity(this AmsModels.GrassSpecies entity, GrassSpeciesUpdateDto dto, int grassTypeId)
    {
        entity.PubId = dto.PubId;
        entity.Active = dto.Active;
        entity.Name = dto.Name;
        entity.LatinName = dto.LatinName;
        entity.GrassTypeId = grassTypeId;
        entity.NMaxYear = dto.NMaxYear;
        entity.NMaxMonth = dto.NMaxMonth;
        entity.KNRatioMin = dto.KNRatioMin;
        entity.KNRatioMax = dto.KNRatioMax;
        entity.PHMin = dto.PHMin;
        entity.PHMax = dto.PHMax;
        entity.SaltToleranceMin = dto.SaltToleranceMin;
        entity.SaltToleranceMax = dto.SaltToleranceMax;
        entity.GrowthHabit = dto.GrowthHabit;
        entity.LiguleLeafMargin = dto.LiguleLeafMargin;
        entity.AuriclesLeafHairs = dto.AuriclesLeafHairs;
        entity.VernationLeafStructure = dto.VernationLeafStructure;
        entity.CollarLeafArrangement = dto.CollarLeafArrangement;
        entity.SeedheadColorFlowerColor = dto.SeedheadColorFlowerColor;
        entity.RootType = dto.RootType;
        entity.UpdatedAtUtc = DateTime.UtcNow;
        entity.CreatedAtUtc = entity.CreatedAtUtc; 
        entity.MainImageId = entity.MainImageId; 
        entity.ShowInGtsInterpretation = dto.ShowInGtsInterpretation;
    }

}
