using AmsRecords.Addresses;
using AmsRecords.ContactDetails;
using AmsRecords.Name;
using static AmsRecords.Architects.ArchitectDtos;

using static AmsRecords.Architects.ArchitectExtensions;

namespace AmsRecords.Architects;

public static class ArchitectEntityMappings
{
    public static ArchitectDto ToDto(this Architect architect)
        => new(
            PubId: architect.PubId,
            FieldPubId: architect.Fields.FirstOrDefault()?.PubId,
            Name: architect.Name?.ToDto(),
            Address: architect.Address?.ToDto(),
            Contact: architect.Contact?.ToDto(),
            ImagePubId: architect.AppImage?.PubId
        );

}
