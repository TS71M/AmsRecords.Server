using static AmsRecords.Name.NameDtos;

using static AmsRecords.Name.NameExtensions;

namespace AmsRecords.Name;

public static class NameEntityMappings
{
    public static NameDto ToDto(this AmsModels.Name name)
        => new(
            PubId: name.PubId,
            FirstName: name.FirstName,
            MiddleName: name.MiddleName,
            LastName: name.LastName,
            IbuPubId: name.Ibu.PubId
        );

    public static AmsModels.Name ToEntity(this NameCreateDto dto, Ibu ibu)
        => new()
        {
            IbuId = ibu.IbuId,
            Ibu = ibu,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName
        };

    public static void UpdateEntity(this AmsModels.Name entity, NameUpdateDto dto, Ibu ibu)
    {
        entity.FirstName = dto.FirstName;
        entity.MiddleName = dto.MiddleName;
        entity.LastName = dto.LastName;
        entity.Ibu = ibu;
        entity.IbuId = ibu.IbuId;
    }

}
