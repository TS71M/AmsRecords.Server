using static AmsRecords.Suppliers.SupplierDtos;
using AmsRecords.ContactDetails;

using static AmsRecords.Suppliers.SupplierExtensions;

namespace AmsRecords.Suppliers;

public static class SupplierEntityMappings
{
    static string? ToSingleLine(Address? address)
    {
        if (address is null)
            return null;

        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(address.Address1))
            parts.Add(address.Address1);

        if (!string.IsNullOrWhiteSpace(address.Address2))
            parts.Add(address.Address2);

        var cityLineParts = new List<string>();

        if (!string.IsNullOrWhiteSpace(address.Zip))
            cityLineParts.Add(address.Zip);

        if (!string.IsNullOrWhiteSpace(address.Town))
            cityLineParts.Add(address.Town);

        if (cityLineParts.Count > 0)
            parts.Add(string.Join(" ", cityLineParts));

        if (!string.IsNullOrWhiteSpace(address.State))
            parts.Add(address.State);

        if (!string.IsNullOrWhiteSpace(address.Country?.CountryName))
            parts.Add(address.Country.CountryName);

        return parts.Count == 0
            ? null
            : string.Join(", ", parts);
    }

    public static string? ToSingleLine(this AddressDtoLike? address)
        => address is null
            ? null
            : ToSingleLine(new Address
            {
                Address1 = address.Address1 ?? "",
                Address2 = address.Address2,
                State = address.State,
                Town = address.Town ?? "",
                Zip = address.Zip ?? "",
                Country = string.IsNullOrWhiteSpace(address.CountryName) ? null : new Country { CountryName = address.CountryName }
            });

    public static SupplierMiniDto ToMiniDto(this Supplier supplier)
        => new(
            PubId: supplier.PubId,
            Name: supplier.SupNam
        );

    public static SupplierFormDto ToFormDto(this Supplier supplier)
    {
        var primarySupplierContact = supplier.SupplierContacts
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.IsPrimary)
            .FirstOrDefault();

        var primaryAddress = supplier.SupplierAddresses
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.IsPrimary)
            .Select(x => x.Address)
            .FirstOrDefault();

        var effectiveContact = primarySupplierContact?.Contact ?? supplier.Contact;
        var effectiveAddress = primaryAddress ?? supplier.Address;

        return new(
            PubId: supplier.PubId,
            IbuPubId: supplier.Ibu.PubId,
            FieldPubId: supplier.Field?.PubId,
            Name: supplier.SupNam,
            FieldName: supplier.Field?.FieldName,
            Active: supplier.Active,
            IsVisible: supplier.IsVisible,
            Image: supplier.Image,
            AppImagePubId: supplier.AppImage?.PubId,
            AddressPubId: effectiveAddress?.PubId,
            ContactPubId: effectiveContact?.PubId,
            PrimaryPhone: ContactDetailsExtensions.ReadableOrNull(effectiveContact?.Phone),
            PrimaryEmail: ContactDetailsExtensions.ReadableOrNull(effectiveContact?.Email),
            PrimaryWebsite: effectiveContact?.Website,
            Address1: effectiveAddress?.Address1,
            Address2: effectiveAddress?.Address2,
            State: effectiveAddress?.State,
            Town: effectiveAddress?.Town,
            Zip: effectiveAddress?.Zip,
            CountryPubId: effectiveAddress?.Country?.PubId,
            CountryName: effectiveAddress?.Country?.CountryName
        );
    }

    public static SupplierListDto ToListDto(this Supplier supplier)
    {
        var primarySupplierContact = supplier.SupplierContacts
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.IsPrimary)
            .ThenBy(x => x.FullName)
            .FirstOrDefault();

        var primaryAddress = supplier.SupplierAddresses
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.IsPrimary)
            .Select(x => x.Address)
            .FirstOrDefault();

        var effectiveAddress = primaryAddress ?? supplier.Address;
        var effectiveContact = primarySupplierContact?.Contact ?? supplier.Contact;
        var primaryContactName = primarySupplierContact?.FullName
            ?? (supplier.ContactId.HasValue ? supplier.SupNam : null);
        var primaryContactPosition = primarySupplierContact?.Position
            ?? (supplier.ContactId.HasValue ? "Primary company contact" : null);

        var distinctAddressIds = supplier.SupplierAddresses
            .Where(x => x.IsActive)
            .Select(x => x.AddressId)
            .ToHashSet();

        var addressCount = distinctAddressIds.Count;
        if (supplier.AddressId.HasValue && distinctAddressIds.Add(supplier.AddressId.Value))
            addressCount++;

        var contactCount = supplier.SupplierContacts.Count(x => x.IsActive);
        if (contactCount == 0 && supplier.ContactId.HasValue)
            contactCount = 1;

        return new(
            PubId: supplier.PubId,
            IbuPubId: supplier.Ibu.PubId,
            FieldPubId: supplier.Field?.PubId,
            Name: supplier.SupNam,
            FieldName: supplier.Field?.FieldName,
            Active: supplier.Active,
            IsVisible: supplier.IsVisible,
            IsFieldScoped: supplier.IsFieldScoped,
            Image: supplier.Image,
            PrimaryContactName: primaryContactName,
            PrimaryContactPosition: primaryContactPosition,
            PrimaryPhone: ContactDetailsExtensions.ReadableOrNull(effectiveContact?.Phone),
            PrimaryEmail: ContactDetailsExtensions.ReadableOrNull(effectiveContact?.Email),
            PrimaryWebsite: effectiveContact?.Website,
            PrimaryAddress: ToSingleLine(effectiveAddress),
            ContactCount: contactCount,
            AddressCount: addressCount
        );
    }

    public static SupplierContactListDto ToListDto(this SupplierContact supplierContact, Guid supplierPubId, string? imageUrl = null)
        => new(
            SupplierContactId: supplierContact.SupplierContactId,
            SupplierPubId: supplierPubId,
            FullName: supplierContact.FullName,
            Position: supplierContact.Position,
            Phone: ContactDetailsExtensions.ReadableOrNull(supplierContact.Contact?.Phone),
            Email: ContactDetailsExtensions.ReadableOrNull(supplierContact.Contact?.Email),
            Website: supplierContact.Contact?.Website,
            Image: imageUrl,
            IsPrimary: supplierContact.IsPrimary,
            IsActive: supplierContact.IsActive
        );

    public static SupplierContactFormDto ToFormDto(this SupplierContact supplierContact, Guid supplierPubId, string? imageUrl = null)
        => new(
            SupplierContactId: supplierContact.SupplierContactId,
            SupplierPubId: supplierPubId,
            NamePubId: supplierContact.Name?.PubId,
            ContactPubId: supplierContact.Contact?.PubId,
            AppImagePubId: supplierContact.AppImage?.PubId,
            FirstName: supplierContact.Name?.FirstName ?? "",
            MiddleName: supplierContact.Name?.MiddleName,
            LastName: supplierContact.Name?.LastName ?? "",
            Position: supplierContact.Position,
            Phone: ContactDetailsExtensions.ReadableOrNull(supplierContact.Contact?.Phone),
            Email: ContactDetailsExtensions.ReadableOrNull(supplierContact.Contact?.Email),
            Website: supplierContact.Contact?.Website,
            Image: imageUrl,
            IsPrimary: supplierContact.IsPrimary,
            IsActive: supplierContact.IsActive
        );

    public static SupplierAddressListDto ToListDto(this SupplierAddress supplierAddress, Guid supplierPubId)
        => new(
            SupplierAddressId: supplierAddress.SupplierAddressId,
            SupplierPubId: supplierPubId,
            AddressType: supplierAddress.AddressType,
            PrimaryAddress: ToSingleLine(supplierAddress.Address),
            IsPrimary: supplierAddress.IsPrimary,
            IsActive: supplierAddress.IsActive
        );

    public static SupplierAddressFormDto ToFormDto(this SupplierAddress supplierAddress, Guid supplierPubId)
        => new(
            SupplierAddressId: supplierAddress.SupplierAddressId,
            SupplierPubId: supplierPubId,
            AddressPubId: supplierAddress.Address?.PubId,
            AddressType: supplierAddress.AddressType,
            Address1: supplierAddress.Address?.Address1,
            Address2: supplierAddress.Address?.Address2,
            State: supplierAddress.Address?.State,
            Town: supplierAddress.Address?.Town,
            Zip: supplierAddress.Address?.Zip,
            CountryPubId: supplierAddress.Address?.Country?.PubId,
            CountryName: supplierAddress.Address?.Country?.CountryName,
            IsPrimary: supplierAddress.IsPrimary,
            IsActive: supplierAddress.IsActive
        );

}
