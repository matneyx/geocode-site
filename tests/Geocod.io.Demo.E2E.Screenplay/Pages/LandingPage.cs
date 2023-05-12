using Boa.Constrictor.Selenium;
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
}
