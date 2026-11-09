using static AmsRecords.RoleGroups.RoleGroupDtos;

using static AmsRecords.RoleGroups.RoleGroupExtensions;

namespace AmsRecords.RoleGroups;

public static class RoleGroupEntityMappings
{
    public static RoleGroupDto ToDto(this RoleGroup group)
        => new(
            RoleGroupId: group.RoleGroupId,
            GroupName: group.GroupName,
            RoleIds: [.. group.RoleGroupRoles.Select(rgr => rgr.RoleId)]
        );

    public static RoleGroup ToEntity(this RoleGroupCreateDto dto, List<Role> roles)
        => new()
        {
            GroupName = dto.GroupName,
            RoleGroupRoles = [.. roles.Select(r => new RoleGroupRole { RoleId = r.Id })]
        };

    public static void UpdateEntity(this RoleGroup entity, RoleGroupUpdateDto dto, List<Role> roles)
    {
        entity.GroupName = dto.GroupName;

        var roleIds = dto.RoleIds.ToHashSet();

        // Remove roles not in dto
        var toRemove = entity.RoleGroupRoles
            .Where(rgr => !roleIds.Contains(rgr.RoleId))
            .ToList();

        foreach (var rgr in toRemove)
            entity.RoleGroupRoles.Remove(rgr);

        // Add new roles
        var existingIds = entity.RoleGroupRoles.Select(rgr => rgr.RoleId).ToHashSet();
        var newRoles = roles.Where(r => !existingIds.Contains(r.Id));

        foreach (var role in newRoles)
        {
            entity.RoleGroupRoles.Add(new RoleGroupRole
            {
                RoleId = role.Id,
                RoleGroupId = entity.RoleGroupId
            });
        }
    }

}
