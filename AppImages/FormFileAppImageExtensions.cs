using Microsoft.AspNetCore.Http;

namespace AmsRecords.AppImages;

public static class FormFileAppImageExtensions
{
    public static async Task<string?> ToBase64Async(this IFormFile file, CancellationToken ct)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);
        var bytes = ms.ToArray();
        if (bytes.Length == 0)
            return null;

        return Convert.ToBase64String(bytes);
    }
}
