
using static AmsRecords.Countries.CountryExtensions;

namespace AmsRecords.Countries;

public static class CountryEntityMappings
{
    public static CountryDto ToDto(this Country country)
        => new(
            PubId: country.PubId,
            CountryCode: country.CountryCode,
            CountryName: country.CountryName
        );

    public static Country ToEntity(this CountryCreateDto dto)
        => new()
        {
            CountryCode = dto.CountryCode.ToUpperInvariant().Trim(),
            CountryName = dto.CountryName
        };

    public static void UpdateFromDto(this Country entity, CountryUpdateDto dto)
    {
        entity.CountryCode = dto.CountryCode.ToUpperInvariant().Trim();
        entity.CountryName = dto.CountryName;
    }

}
