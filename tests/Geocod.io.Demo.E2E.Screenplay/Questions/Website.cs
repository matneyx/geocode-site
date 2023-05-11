using Boa.Constrictor.Screenplay;
using Boa.Constrictor.Selenium;
using Geocod.io.Demo.E2E.Framework;
using Geocod.io.Demo.E2E.Screenplay.Pages;
using Ocaramba.Extensions;

namespace Geocod.io.Demo.E2E.Screenplay.Questions;

public class Website : IQuestion<string>
{
    public static Website Title() => new();

    public string RequestAs(IActor actor)
    {
        actor.Logger.Info($"{actor.Name} is attempting to find the title of the page.");
        var driver = actor.Using<BrowseTheWeb>().WebDriver;
        return driver.GetElement(LandingPage.Title.AsElementLocator()).Text;
    }
}
