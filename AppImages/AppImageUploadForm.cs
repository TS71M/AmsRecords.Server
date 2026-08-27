using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace AmsRecords.AppImages;

public sealed class AppImageUploadForm
{
    [FromForm(Name = "file")]
    public IFormFile File { get; set; } = default!;

    [FromForm(Name = "category")]
    public string Category { get; set; } = ImageCategories.Grasses;

    [FromForm(Name = "capturedAtUtc")]
    public DateTimeOffset? CapturedAtUtc { get; set; }

    double? _latitude;
    double? _longitude;
    double? _locationAccuracyMeters;

    [BindNever]
    public double? Latitude
    {
        get => _latitude ?? ParseNumber(LatitudeFormValue);
        set => _latitude = value;
    }

    [BindNever]
    public double? Longitude
    {
        get => _longitude ?? ParseNumber(LongitudeFormValue);
        set => _longitude = value;
    }

    [BindNever]
    public double? LocationAccuracyMeters
    {
        get => _locationAccuracyMeters ?? ParseNumber(LocationAccuracyMetersFormValue);
        set => _locationAccuracyMeters = value;
    }

    [FromForm(Name = "latitude")]
    public string? LatitudeFormValue { get; set; }

    [FromForm(Name = "longitude")]
    public string? LongitudeFormValue { get; set; }

    [FromForm(Name = "locationAccuracyMeters")]
    public string? LocationAccuracyMetersFormValue { get; set; }

    [FromForm(Name = "metadataSource")]
    public string? MetadataSource { get; set; }

    static double? ParseNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        return double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var invariant)
            ? invariant
            : double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out var current)
                ? current
                : null;
    }
}
