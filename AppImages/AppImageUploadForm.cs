using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmsRecords.AppImages;

public sealed class AppImageUploadForm
{
    [FromForm(Name = "file")]
    public IFormFile File { get; set; } = default!;

    [FromForm(Name = "category")]
    public string Category { get; set; } = ImageCategories.Grasses;
}
