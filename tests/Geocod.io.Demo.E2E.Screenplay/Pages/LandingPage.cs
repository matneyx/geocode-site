﻿using Boa.Constrictor.Selenium;
using Geocod.io.Demo.E2E.Framework;
using Ocaramba;

namespace Geocod.io.Demo.E2E.Screenplay.Pages;

public class LandingPage
{
    public static UniversalLocator FileUploadedToast => new("Toast", Locator.Id, "file-uploaded-toast");
    public static UniversalLocator Title => new("Title",Locator.Id, "title");
    public static UniversalLocator UploadSection => new("Upload Section", Locator.Id, "upload-section");

    public static UniversalLocator UploadInput => new("Upload Input", Locator.Id, "upload-input");
    public static UniversalLocator UploadButton => new("Upload Button", Locator.Id, "upload-button");
    public static UniversalLocator FileUploadedToastBody => new("Toast Body", Locator.Id, "file-uploaded-body");
    public static UniversalLocator AddressCardContainer => new("Address Card Container", Locator.Id, "address-cards");
    public static UniversalLocator AddressCard => new("Address Card", Locator.ClassName, "address-card");

    public static UniversalLocator GeocodeTypeSelector => new UniversalLocator("Geocode Type Selector", Locator.Id, "geocode-type-selector");
    public static UniversalLocator LargeBatchOption => new UniversalLocator("Large Batch Option", Locator.Id, "geocode-type-selector");
}
