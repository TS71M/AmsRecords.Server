using static AmsRecords.Addresses.AddressDtos;

using static AmsRecords.Addresses.AddressExtensions;

namespace AmsRecords.Addresses;

public static class AddressEntityMappings
{
    public static AddressDto ToDto(this Address address)
        => new(
            PubId: address.PubId,
            Address1: address.Address1,
            Address2: address.Address2,
            State: address.State,
            Town: address.Town,
            Zip: address.Zip,
            CountryPubId: address.Country!.PubId,
            Country: address.Country.CountryName
        );

    public static Address ToEntity(this AddressCreateDto dto, int ibuId, Guid countryPubId, Ibu ibu, Country country)
        => new()
        {
            IbuId = ibuId,
            CountryId = country.CountryId,
            Ibu = ibu,
            Country = country,
            Address1 = dto.Address1,
            Address2 = dto.Address2,
            Town = dto.Town,
            State = dto.State,
            Zip = dto.Zip
        };

    public static void UpdateEntity(this Address entity, AddressUpdateDto dto, Country country)
    {
        entity.Address1 = dto.Address1;
        entity.Address2 = dto.Address2;
        entity.State = dto.State;
        entity.Town = dto.Town;
        entity.Zip = dto.Zip;
        entity.CountryId = country.CountryId;
        entity.Country = country;
    }

}
