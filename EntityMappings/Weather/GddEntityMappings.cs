using AmsRecords.Messages;
using AmsRecords.Units;

using static AmsRecords.Weather.GddDtos;

namespace AmsRecords.Weather;

public static class GddEntityMappings
{

    public static GddSeriesDto EmptySeries(
        Field field,
        DateTime fromUtc,
        DateTime toUtc,
        decimal baseTempC,
        decimal? capTempC,
        string? biofixMode,
        DateTime? biofixDateUtc)
        => new(
            FieldPubId: field.PubId,
            FieldName: field.FieldName,
            FromUtc: fromUtc,
            ToUtc: toUtc,
            BaseTempC: baseTempC,
            CapTempC: capTempC,
            BaseTemp: null,
            CapTemp: null,
            TemperatureUnitShort: "°C",
            Method: new(
                Code: GddAppMessageCodes.Methods.TminTmaxAvg,
                Severity: null,
                Args: null),
            GeneratedAtUtc: DateTimeOffset.UtcNow,
            BiofixMode: biofixMode is "none" or null ? null : biofixMode,
            BiofixDateUtc: biofixDateUtc,
            CumGddSinceBiofix: null,
            SeasonStart: null,
            Days: []
        );

}
