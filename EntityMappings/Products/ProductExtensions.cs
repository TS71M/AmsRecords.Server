using static AmsRecords.Products.ProductsDtos;

namespace AmsRecords.Products;

public static class ProductExtensions
{
    public static ProductMiniDto ToMiniDto(this Product product)
        => new(
            PubId: product.PubId,
            Name: product.ProNam
        );

    public static ProductListDto ToListDto(this Product product)
    {
        var ibuPubId = product.Ibu?.PubId ?? Guid.Empty;
        var accountName = product.Account?.AccDes ?? string.Empty;
        var productTypeName = product.ProductType?.ProductTypeName ?? string.Empty;
        var packUnitLabel = product.UniWeiPack?.UnitShort ?? string.Empty;
        var pack = product.Pack.ToString("0.######");
        var price = product.Price.ToString("0.######");
        var packSize = string.IsNullOrWhiteSpace(packUnitLabel)
            ? pack
            : $"{pack} {packUnitLabel}";
        var pricePerUnit = string.IsNullOrWhiteSpace(packUnitLabel)
            ? price
            : $"{price}/{packUnitLabel}";

        return new(
            PubId: product.PubId,
            IbuPubId: ibuPubId,
            FieldPubId: product.Field?.PubId,
            Name: product.ProNam,
            AccountName: accountName,
            ProductTypeName: productTypeName,
            Active: product.Active,
            PackSize: packSize,
            PricePerUnit: pricePerUnit,
            Image: product.Image,
            OwnerName: product.Ibu?.BusinessUnitName ?? string.Empty
        );
    }

    public static ProductFormDto ToFormDto(this Product product)
    {
        var ibuPubId = product.Ibu?.PubId ?? Guid.Empty;
        var accountPubId = product.Account?.PubId ?? Guid.Empty;
        var accountName = product.Account?.AccDes ?? string.Empty;
        var productTypePubId = product.ProductTypeId is null ? null : product.ProductType?.PubId;
        var productTypeName = product.ProductType?.ProductTypeName ?? string.Empty;
        var unitWeightPubId = product.UniWei?.PubId ?? Guid.Empty;
        var unitSurfacePubId = product.UniSur?.PubId ?? Guid.Empty;
        var unitPackPubId = product.UniWeiPack?.PubId ?? Guid.Empty;
        var unitWeightLabel = product.UniWei?.UnitShort ?? string.Empty;
        var unitSurfaceLabel = product.UniSur?.UnitShort ?? string.Empty;
        var unitPackLabel = product.UniWeiPack?.UnitShort ?? string.Empty;

        return new(
            PubId: product.PubId,
            IbuPubId: ibuPubId,
            FieldPubId: product.Field?.PubId,
            Name: product.ProNam,
            Active: product.Active,
            AccountPubId: accountPubId,
            AccountName: accountName,
            ProductTypePubId: productTypePubId,
            ProductTypeName: productTypeName,
            Rate: product.Rate,
            RecommendedRate: product.RecRat,
            UnitWeightPubId: unitWeightPubId,
            UnitSurfacePubId: unitSurfacePubId,
            UnitPackPubId: unitPackPubId,
            UnitWeightLabel: unitWeightLabel,
            UnitSurfaceLabel: unitSurfaceLabel,
            UnitPackLabel: unitPackLabel,
            Price: product.Price,
            Pack: product.Pack,
            Highlight: product.Highlight,
            Alarm: product.Alarm,
            IsPgr: product.IsPGR,
            Image: product.Image,
            ImagePubId: product.AppImage?.PubId,
            SdsName: product.SdsName,
            SdsPath: product.SdsPath,
            SdsUploadDt: product.SdsUploadDt,
            SdsSize: product.SdsSize,
            Nutrients: product.ProductNutrients.ToLabelOrderedDtos(product.PubId),
            Supplements: product.ProductSupplements
                .OrderBy(x => x.Category)
                .ThenBy(x => x.Name)
                .Select(x => new ProductSupplementDto(
                    PubId: x.PubId,
                    ProductPubId: product.PubId,
                    Name: x.Name,
                    Category: x.Category,
                    Amount: x.Amount,
                    UnitText: x.UnitText,
                    Notes: x.Notes))
                .ToList(),
            Labels: product.ProductLabels
                .OrderByDescending(x => x.UploadDT)
                .Select(x => new ProductLabelDto(
                    PubId: x.PubId,
                    ProductPubId: product.PubId,
                    Name: x.ProLabName,
                    Path: x.AppImage?.RelativePath ?? x.ProLabPath,
                    Size: x.ProLabSize,
                    UploadDt: x.UploadDT,
                    ImagePubId: x.AppImage?.PubId,
                    Image: x.Image))
                .ToList(),
            DensityKgPerL: product.DensityKgPerL,
            AllowedFields: AllowedFields(product)
        );
    }

    public static NutrientLookupDto ToLookupDto(this Nutrient nutrient)
        => new(
            PubId: nutrient.PubId,
            Name: nutrient.NutrientName,
            OrderNumber: nutrient.OrderNumber,
            Code: nutrient.Code
        );

    public static List<ProductNutrientDto> ToLabelOrderedDtos(this IEnumerable<ProductNutrient> nutrients, Guid productPubId)
        => nutrients.OrderBy(x => ProductNutrientLabelOrder.Rank(x.Nutrient.Code))
            .ThenBy(x => x.Nutrient.OrderNumber)
            .ThenBy(x => x.Nutrient.NutrientName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(x => x.Nutrient.Code, StringComparer.Ordinal)
            .Select(x => ToNutrientDto(x, productPubId)).ToList();

    public static ProductNutrientDto ToDto(this ProductNutrient productNutrient)
        => ToNutrientDto(productNutrient, productNutrient.Product.PubId);

    static ProductNutrientDto ToNutrientDto(ProductNutrient productNutrient, Guid productPubId)
        => new(
            PubId: productNutrient.PubId,
            ProductPubId: productPubId,
            NutrientPubId: productNutrient.Nutrient.PubId,
            NutrientName: productNutrient.Nutrient.NutrientName,
            Amount: productNutrient.Amount,
            AnalysisBasis: productNutrient.AnalysisBasis.ToString(),
            AnalysisSource: productNutrient.AnalysisSource,
            IsVerified: productNutrient.IsVerified,
            Forms: ToForms(productNutrient)
        );

    static IReadOnlyList<ProductNutrientFormDto>? ToForms(ProductNutrient nutrient)
        => nutrient.Forms.Count == 0 ? null : nutrient.Forms.OrderBy(f => f.FormCode)
            .Select(f => new ProductNutrientFormDto(f.FormCode, f.Label, f.Amount, f.IsMinimum)).ToArray();

    public static ProductSupplementDto ToDto(this ProductSupplement productSupplement)
        => new(
            PubId: productSupplement.PubId,
            ProductPubId: productSupplement.Product.PubId,
            Name: productSupplement.Name,
            Category: productSupplement.Category,
            Amount: productSupplement.Amount,
            UnitText: productSupplement.UnitText,
            Notes: productSupplement.Notes
        );

    public static ProductLabelDto ToDto(this ProductLabel productLabel)
        => new(
            PubId: productLabel.PubId,
            ProductPubId: productLabel.Product.PubId,
            Name: productLabel.ProLabName,
            Path: productLabel.AppImage?.RelativePath ?? productLabel.ProLabPath,
            Size: productLabel.ProLabSize,
            UploadDt: productLabel.UploadDT,
            ImagePubId: productLabel.AppImage?.PubId,
            Image: productLabel.Image
        );

    public static ProductSupplierDto ToDto(this ProductSupplier productSupplier)
        => new(
            PubId: productSupplier.PubId,
            ProductPubId: productSupplier.Product.PubId,
            ProductName: productSupplier.Product.ProNam,
            SupplierPubId: productSupplier.Supplier.PubId,
            SupplierName: productSupplier.Supplier.SupNam,
            SupplierProductCode: productSupplier.SupplierProductCode,
            PackSize: productSupplier.PackSize,
            UnitPrice: productSupplier.UnitPrice,
            CurrencyCode: productSupplier.CurrencyCode,
            LeadTimeDays: productSupplier.LeadTimeDays,
            MinimumOrderQty: productSupplier.MinimumOrderQty,
            IsPreferred: productSupplier.IsPreferred,
            CanRequestPrice: productSupplier.CanRequestPrice,
            CanOrder: productSupplier.CanOrder,
            IsActive: productSupplier.IsActive,
            Notes: productSupplier.Notes,
            ProductActive: productSupplier.Product.Active,
            ProductFieldPubId: productSupplier.Product.Field?.PubId,
            ProductAllowedFieldPubIds: AllowedFields(productSupplier.Product).Select(f => f.PubId).ToArray(),
            SupplierFieldPubId: productSupplier.Supplier.Field?.PubId
        );

    static ProductAllowedFieldDto[] AllowedFields(Product product)
        => product.AdditionalFields.Where(f => f.Field is not null).Select(f => f.Field)
            .Concat(product.Field is { } primary ? [primary] : [])
            .OrderBy(f => f.FieldName)
            .Select(f => new ProductAllowedFieldDto(f.PubId, f.FieldName)).ToArray();

    public static ProductTypeDto ToDto(this ProductType productType)
        => new(
            productType.PubId,
            productType.IsActive,
            productType.ProductTypeName
        );
}
