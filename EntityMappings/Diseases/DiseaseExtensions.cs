using static AmsRecords.Diseases.DiseaseDtos;

namespace AmsRecords.Diseases;

public static class DiseaseExtensions
{
    public static DiseaseSummaryDto ToSummaryDto(this Disease disease)
        => ToSummaryDto(disease.PubId, disease.Name, disease.SymptomsAndSigns, disease.DiseaseManagement,
            disease.IndicatorPicture?.PubId, disease.DiseaseAliases.Select(alias => alias.AliasName),
            disease.DiseaseGrassTypes.Select(link => link.GrassType.GrassTypeName), disease.DiseasePictures.Count);

    public static DiseaseSummaryDto ToSummaryDto(Guid pubId, string name, string? symptoms, string? management,
        Guid? indicatorPicturePubId, IEnumerable<string> aliases, IEnumerable<string> grassTypes, int pictureCount)
        => new(
            PubId: pubId,
            Name: name,
            Summary: BuildSummary(symptoms, management),
            IndicatorPicturePubId: indicatorPicturePubId,
            IndicatorThumbnailUrl: null,
            Aliases: aliases.Where(alias => !string.IsNullOrWhiteSpace(alias)).OrderBy(alias => alias).ToList(),
            GrassTypes: grassTypes.Where(type => !string.IsNullOrWhiteSpace(type)).OrderBy(type => type).ToList(),
            PictureCount: pictureCount);

    public static DiseaseAdminListDto ToAdminListDto(this Disease disease)
        => new(
            PubId: disease.PubId,
            Name: disease.Name,
            IndicatorPicturePubId: disease.IndicatorPicture?.PubId,
            AliasCount: disease.DiseaseAliases.Count,
            GrassTypeCount: disease.DiseaseGrassTypes.Count,
            PictureCount: disease.DiseasePictures.Count,
            TrainingExampleCount: disease.DiseaseTrainingExamples.Count);

    public static DiseaseDetailDto ToDetailDto(this Disease disease)
        => new(
            PubId: disease.PubId,
            Name: disease.Name,
            History: disease.History,
            SymptomsAndSigns: disease.SymptomsAndSigns,
            CausalAgent: disease.CausalAgent,
            DiseasePictureDescription: disease.DiseasePictureDescription,
            Epidemiology: disease.Epidemiology,
            DiseaseManagement: disease.DiseaseManagement,
            TempMin: disease.TempMin,
            TempMax: disease.TempMax,
            TempNight: disease.TempNight,
            Wet: disease.Wet,
            Humid: disease.Humid,
            Dry: disease.Dry,
            VeryDry: disease.VeryDry,
            IndicatorPicturePubId: disease.IndicatorPicture?.PubId,
            DiseaseCyclePicturePubId: disease.DiseaseCyclePicture?.PubId,
            Aliases: disease.DiseaseAliases
                .Select(x => x.AliasName)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x)
                .ToList(),
            GrassTypes: disease.DiseaseGrassTypes
                .OrderBy(x => x.GrassType.GrassTypeName)
                .Select(x => new DiseaseGrassTypeDto(
                    x.PubId,
                    x.GrassType.PubId,
                    x.GrassType.GrassTypeName))
                .ToList(),
            Pictures: disease.DiseasePictures
                .OrderBy(x => x.DiseasePictureId)
                .Select(x => new DiseasePictureDto(x.PubId, x.AppImage?.PubId))
                .ToList(),
            GrassSpeciesSusceptibilities: disease.GrassSpeciesSusceptibilities
                .Where(x => x.Active)
                .OrderByDescending(x => x.SusceptibilityLevel)
                .ThenBy(x => x.GrassSpecies.Name)
                .Select(x => new DiseaseGrassSpeciesSusceptibilityDto(
                    x.PubId,
                    x.GrassSpecies.PubId,
                    x.GrassSpecies.Name,
                    x.GrassSpecies.LatinName,
                    x.SusceptibilityLevel,
                    x.ContentOwner,
                    x.ReviewedAtUtc,
                    x.EvidenceNote,
                    x.SourceUrl))
                .ToList());

    public static DiseaseTrainingExampleDto ToTrainingDto(this DiseaseTrainingExample example)
        => new(
            PubId: example.PubId,
            DiseasePubId: example.Disease.PubId,
            DiseaseName: example.Disease.Name,
            ImagePubId: example.AppImage.PubId,
            AiPredictedCondition: example.AiPredictedCondition,
            FinalCondition: example.FinalCondition,
            SeasonTag: example.SeasonTag,
            WeatherTag: example.WeatherTag,
            Confidence: example.Confidence,
            ReviewNote: example.ReviewNote,
            IsApproved: example.IsApproved,
            UseForLearning: example.UseForLearning,
            UseForRetrieval: example.UseForRetrieval,
            Active: example.Active,
            CreatedAtUtc: example.CreatedAtUtc,
            ReviewedAtUtc: example.ReviewedAtUtc,
            SubmittedByName: example.SubmittedBy?.FullNameSnapshot,
            ReviewedByName: example.ReviewedBy?.FullNameSnapshot,
            Findings: example.Findings
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.DiseaseTrainingExampleFindingId)
                .Select(x => new DiseaseFindingDto(
                    x.PubId,
                    x.Disease?.PubId,
                    x.Disease?.Name,
                    x.ConditionName,
                    x.Role,
                    x.Confidence,
                    x.Note,
                    x.SortOrder))
                .ToList(),
            ConfirmationMethod: example.ConfirmationMethod,
            OutbreakGroup: example.OutbreakGroup);

    static string BuildSummary(string? symptoms, string? management)
    {
        var source = !string.IsNullOrWhiteSpace(symptoms) ? symptoms : management;

        if (string.IsNullOrWhiteSpace(source))
            return "";

        var trimmed = source.Trim();
        return trimmed.Length <= 220
            ? trimmed
            : trimmed[..217].TrimEnd() + "...";
    }
}
