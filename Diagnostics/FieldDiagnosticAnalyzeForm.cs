using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmsRecords.Diagnostics;

public sealed class FieldDiagnosticAnalyzeForm
{
    [FromForm(Name = "file")]
    public IFormFile? File { get; set; }

    [FromForm(Name = "files")]
    public List<IFormFile> Files { get; set; } = [];

    [FromForm(Name = "imageRoles")]
    public List<string> ImageRoles { get; set; } = [];

    [FromForm(Name = "imageCapturedAtUtc")]
    public List<string> ImageCapturedAtUtc { get; set; } = [];

    [FromForm(Name = "imageLatitude")]
    public List<string> ImageLatitude { get; set; } = [];

    [FromForm(Name = "imageLongitude")]
    public List<string> ImageLongitude { get; set; } = [];

    [FromForm(Name = "imageLocationAccuracyMeters")]
    public List<string> ImageLocationAccuracyMeters { get; set; } = [];

    [FromForm(Name = "imageMetadataSource")]
    public List<string> ImageMetadataSource { get; set; } = [];

    [FromForm(Name = "diagnosticRequestPubId")]
    public Guid? DiagnosticRequestPubId { get; set; }

    [FromForm(Name = "diagnosticSubmittedAtUtc")]
    public DateTimeOffset? DiagnosticSubmittedAtUtc { get; set; }

    [FromForm(Name = "surfacePubId")]
    public Guid? SurfacePubId { get; set; }

    [FromForm(Name = "note")]
    public string? Note { get; set; }
}
