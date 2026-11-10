using System.Linq.Expressions;
using AmsModels;

namespace AmsRecords.Procural;

/// <summary>Shared account availability for procurement membership, assignment and readiness.</summary>
public static class ProcurementUserAvailability
{
    public static Expression<Func<User, bool>> At(DateTimeOffset now)
        => At(now, null);

    public static Expression<Func<User, bool>> At(DateTimeOffset now, int? hubIbuId)
        => user => user.Active && (!user.LockoutEnd.HasValue || user.LockoutEnd <= now)
            && (!hubIbuId.HasValue || user.IbuId == hubIbuId);
}
