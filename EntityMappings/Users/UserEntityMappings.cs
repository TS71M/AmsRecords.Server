using AmsRecords.ContactDetails;
using AmsRecords.Name;
using AmsRecords.Roles;
using static AmsRecords.Users.UserDtos;

using static AmsRecords.Users.UserExtensions;

namespace AmsRecords.Users;

public static class UserEntityMappings
{
    public static UserDto ToDto(this User user, DateTime? fallbackLastSeenUtc = null)
        => new(
            PubId: user.PubId,
            Email: user.Email!,
            UserName: user.UserName!,
            Active: user.Active,
            LockedOut: user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow,
            LockoutEnd: user.LockoutEnd,
            LastSeenUtc: user.LastSeenUtc ?? fallbackLastSeenUtc,
            IbuPubId: user.Ibu!.PubId,
            Name: user.Name?.ToDto(),
            ImagePubId: user.UserImg?.PubId,
            PreferredCountryPubId: user.PreferredCountry?.PubId,
            PreferredCountryCode: user.PreferredCountry?.CountryCode,
            PreferredCountryName: user.PreferredCountry?.CountryName,
            PreferredLanguagePubId: user.PreferredLanguage?.PubId,
            PreferredLanguageCode: user.PreferredLanguage?.LanguageCode,
            PreferredLanguageName: user.PreferredLanguage?.LanguageName,
            Contact: user.ContactDetail?.ToDto(),
            ManagerUserPubId: user.ManagerUser?.PubId,
            ManagerUserName: ResolveDisplayName(user.ManagerUser),
            UserPositionPubId: user.UserPosition?.PubId,
            UserPositionName: user.UserPosition?.UserPositionName,
            AccessProfilePubId: user.AccessProfile?.PubId,
            AccessProfileName: user.AccessProfile?.ProfileName,
            Roles: [.. user.UserRole.Select(ur => ur.Role.ToDto())],
            VisualizationTempUnitPubId: user.VisualizationTempUnit?.PubId,
            VisualizationRainUnitPubId: user.VisualizationRainUnit?.PubId,
            VisualizationWindUnitPubId: user.VisualizationWindUnit?.PubId,
            VisualizationGreenSpeedUnitPubId: user.VisualizationGreenSpeedUnit?.PubId,
            VisualizationCuttingHeightUnitPubId: user.VisualizationCuttingHeightUnit?.PubId,
            VisualizationClippingsUnitPubId: user.VisualizationClippingsUnit?.PubId,
            VisualizationSurfaceUnitPubId: user.VisualizationSurfaceUnit?.PubId
            )
        {
            ManagerUserPubIds = [.. GetReportingManagers(user).Select(x => x.PubId)],
            ManagerUserNames = [.. GetReportingManagers(user).Select(ResolveDisplayName).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x!)],
            UserPositionHierarchyLevel = user.UserPosition?.HierarchyLevel,
            UserPositionDepartmentName = user.UserPosition?.Department?.DepartmentName
        };

    public static User ToEntity(this UserCreateDto dto, int ibuId, int? countryId)
        => new()
        {
            Email = dto.Email,
            UserName = dto.UserName,
            LockoutEnabled = true,
            LockoutEnd = dto.LockedOut
                ? DateTimeOffset.MaxValue
                : null,
            Active = true,
            IbuId = ibuId,
            PreferredCountryId = countryId,
            ManagerUserId = null,
            EmailConfirmed = true // Admin-created users are treated as already verified by the creating administrator
        };

    public static UserTemporaryReplacementDto ToDto(this UserTemporaryReplacement entity)
        => new(
            PubId: entity.PubId,
            IbuPubId: entity.Ibu.PubId,
            AbsentUserPubId: entity.AbsentUser.PubId,
            AbsentUserName: ResolveDisplayName(entity.AbsentUser),
            ReplacementUserPubId: entity.ReplacementUser.PubId,
            ReplacementUserName: ResolveDisplayName(entity.ReplacementUser),
            StartDate: entity.StartDate,
            EndDate: entity.EndDate,
            Reason: entity.Reason,
            Active: entity.Active);

    public static void UpdateEntity(this User user, UserUpdateDto dto, Ibu ibu)
    {
        user.Email = dto.Email;
        user.UserName = dto.UserName;
        user.LockoutEnabled = true;
        user.LockoutEnd = dto.LockedOut
            ? DateTimeOffset.MaxValue
            : null;
        if (!dto.LockedOut)
            user.AccessFailedCount = 0;

        // FK only (avoid nav assignment)
        user.IbuId = ibu.IbuId;

        user.ManagerUserId = null;
    }

    // Snapshot projections often omit Name; fall back to the existing account display policy.
    public static string? ResolveSnapshotDisplayName(this User? user)
        => !string.IsNullOrWhiteSpace(user?.FullNameSnapshot)
            ? user.FullNameSnapshot.Trim()
            : ResolveDisplayName(user);

    static string? ResolveDisplayName(User? user)
    {
        if (user is null)
            return null;

        var fullName = user.Name?.FullName?.Trim();
        var userName = user.UserName?.Trim();

        if (!string.IsNullOrWhiteSpace(fullName) && !string.IsNullOrWhiteSpace(userName) && !string.Equals(fullName, userName, StringComparison.OrdinalIgnoreCase))
            return $"{fullName} ({userName})";

        if (!string.IsNullOrWhiteSpace(fullName))
            return fullName;

        return !string.IsNullOrWhiteSpace(userName)
            ? userName
            : user.Email;
    }

    static List<User> GetReportingManagers(User user)
    {
        var managers = user.ReportingLines
            .Select(x => x.ManagerUser)
            .Where(x => x is not null)
            .Cast<User>()
            .Where(x => x.Active)
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .OrderBy(ResolveDisplayName)
            .ToList();

        if (managers.Count == 0 && user.ManagerUser is not null && user.ManagerUser.Active)
            managers.Add(user.ManagerUser);

        return managers;
    }

}
