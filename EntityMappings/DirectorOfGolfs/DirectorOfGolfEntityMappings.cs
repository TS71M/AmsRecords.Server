using AmsRecords.ContactDetails;
using AmsRecords.Name;
using static AmsRecords.DirectorOfGolfs.DirectorOfGolfDtos;

using static AmsRecords.DirectorOfGolfs.DirectorOfGolfExtensions;

namespace AmsRecords.DirectorOfGolfs;

public static class DirectorOfGolfEntityMappings
{
    public static DirectorOfGolfDto ToDto(this DirectorOfGolf entity)
        => new(
            PubId: entity.PubId,
            FieldPubId: entity.Fields.FirstOrDefault()?.PubId,
            IbuPubId: entity.Ibu.PubId,
            Name: entity.Name?.ToDto(),
            Contact: entity.Contact?.ToDto(),
            ImagePubId: entity.AppImage?.PubId
        );

}
