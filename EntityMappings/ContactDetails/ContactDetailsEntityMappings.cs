using static AmsRecords.ContactDetails.ContactDetailDtos;

using static AmsRecords.ContactDetails.ContactDetailsExtensions;

namespace AmsRecords.ContactDetails;

public static class ContactDetailsEntityMappings
{

    public static ContactDetailDto ToDto(this ContactDetail contact)
        => new(
            contact.PubId,
            ReadableOrNull(contact.Phone),
            ReadableOrNull(contact.Email),
            contact.Website,
            contact.Ibu.PubId
        );

    public static ContactDetail ToEntity(this ContactDetailCreateDto dto, Ibu ibu)
        => new()
        {
            Phone = dto.Phone,
            Email = dto.Email,
            Website = dto.Website,
            Ibu = ibu
        };

    public static void UpdateFromDto(this ContactDetail entity, ContactDetailUpdateDto dto)
    {
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.Website = dto.Website;
    }

}
