using static AmsRecords.UserRoles.UserRoleDtos;

using static AmsRecords.UserRoles.UserRoleExtensions;

namespace AmsRecords.UserRoles;

public static class UserRoleEntityMappings
{
    public static UserRoleDto ToDto(this UserRole userRole)
        => new(
            UserId: userRole.UserId,
            RoleId: userRole.RoleId,
            RoleName: userRole.Role?.Name ?? string.Empty
        );

    public static UserRole ToEntity(this UserRoleCreateDto dto, User user, AmsModels.Role role)
        => new()
        {
            UserId = dto.UserId,
            RoleId = dto.RoleId,
            User = user,
            Role = role
        };

}
