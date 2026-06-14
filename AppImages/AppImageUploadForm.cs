using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmsRecords.AppImages;

public sealed class AppImageUploadForm
{
    [FromForm(Name = "file")]
    public IFormFile File { get; set; } = default!;

    [FromForm(Name = "category")]
    public string Category { get; set; } = ImageCategories.Grasses;

    [FromForm(Name = "capturedAtUtc")]
    public DateTimeOffset? CapturedAtUtc { get; set; }

    [FromForm(Name = "latitude")]
    public double? Latitude { get; set; }

    [FromForm(Name = "longitude")]
    public double? Longitude { get; set; }

    [FromForm(Name = "locationAccuracyMeters")]
    public double? LocationAccuracyMeters { get; set; }

    [FromForm(Name = "metadataSource")]
    public string? MetadataSource { get; set; }
}
