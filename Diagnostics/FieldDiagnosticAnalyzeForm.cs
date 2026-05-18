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

    [FromForm(Name = "diagnosticRequestPubId")]
    public Guid? DiagnosticRequestPubId { get; set; }

    [FromForm(Name = "diagnosticSubmittedAtUtc")]
    public DateTimeOffset? DiagnosticSubmittedAtUtc { get; set; }

    [FromForm(Name = "surfacePubId")]
    public Guid? SurfacePubId { get; set; }

    [FromForm(Name = "note")]
    public string? Note { get; set; }
}
