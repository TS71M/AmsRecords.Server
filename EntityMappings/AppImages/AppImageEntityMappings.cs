using ImageMagick;
using Lib.Images;
using static AmsRecords.AppImages.AppImageDtos;

using static AmsRecords.AppImages.AppImageExtensions;

namespace AmsRecords.AppImages;

public static class AppImageEntityMappings
{

    public static AppImageDto ToAppImageDto(this AppImage appImage)
    {
        var image = ImageX.ImageToString(appImage.ImageFile);
        var imageInfo = AppImageExtensions.AnalyzeBase64Image(image);

        return new AppImageDto(
            appImage.ImageId,
            image,
            imageInfo.MimeType,
            imageInfo.Format,
            imageInfo.Width,
            imageInfo.Height,
            imageInfo.FileSize,
            appImage.PubId,
            appImage.RelativePath
        )
        {
            CaptureMetadata = appImage.ToCaptureMetadataDto()
        };
    }

    public static GetAppImageWithThumbnailDto ToAppImageWithThumbnailsDto(this AppImage appImage)
    {
        var image = ImageX.ImageToString(appImage.ImageFile);
        var imageInfo = AppImageExtensions.AnalyzeBase64Image(image);
        var thumbnail = appImage.ImageFile is { Length: > 0 }
            ? $"data:image/png;base64,{ImageX.GenerateThumbnailBase64(appImage.ImageFile)}"
            : image;

        return new GetAppImageWithThumbnailDto(
            appImage.ImageId,
            image,
            thumbnail,
            imageInfo.MimeType,
            imageInfo.Format,
            imageInfo.Width,
            imageInfo.Height,
            imageInfo.FileSize
        )
        {
            CaptureMetadata = appImage.ToCaptureMetadataDto()
        };
    }

    public static GetAppImageThumbnailDto ToAppImageThumbnailDto(this AppImage appImage)
    {
        var thumbnail = appImage.ImageFile is { Length: > 0 }
            ? $"data:image/png;base64,{ImageX.GenerateThumbnailBase64(appImage.ImageFile)}"
            : ImageX.ImageToString(appImage.ImageFile);
        var imageInfo = AppImageExtensions.AnalyzeBase64Image(thumbnail);

        return new GetAppImageThumbnailDto(
            appImage.ImageId,
            appImage.ImageName,
            thumbnail,
            imageInfo.MimeType,
            imageInfo.Format,
            imageInfo.Width,
            imageInfo.Height,
            imageInfo.FileSize
        )
        {
            CaptureMetadata = appImage.ToCaptureMetadataDto()
        };
    }

    public static AppImageCaptureMetadataDto? ToCaptureMetadataDto(this AppImage? appImage)
    {
        if (appImage is null
            || (appImage.CapturedAtUtc is null
                && appImage.CaptureLatitude is null
                && appImage.CaptureLongitude is null
                && appImage.CaptureLocationAccuracyMeters is null
                && string.IsNullOrWhiteSpace(appImage.CaptureMetadataSource)))
        {
            return null;
        }

        return new AppImageCaptureMetadataDto(
            appImage.CapturedAtUtc.HasValue
                ? new DateTimeOffset(DateTime.SpecifyKind(appImage.CapturedAtUtc.Value, DateTimeKind.Utc))
                : null,
            appImage.CaptureLatitude.HasValue ? (double)appImage.CaptureLatitude.Value : null,
            appImage.CaptureLongitude.HasValue ? (double)appImage.CaptureLongitude.Value : null,
            appImage.CaptureLocationAccuracyMeters.HasValue ? (double)appImage.CaptureLocationAccuracyMeters.Value : null,
            string.IsNullOrWhiteSpace(appImage.CaptureMetadataSource) ? null : appImage.CaptureMetadataSource.Trim());
    }

}
