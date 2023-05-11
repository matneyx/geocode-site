using Boa.Constrictor.Screenplay;
using Boa.Constrictor.Selenium;
using Geocod.io.Demo.E2E.Framework;
using Geocod.io.Demo.E2E.Framework.Logger;
using Geocod.io.Demo.E2E.Screenplay.Questions;
using Ocaramba;
using Shouldly;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace Geocod.io.Demo.E2E.Features.StepDefinitions;

[Binding]
public class GeocodioDemoSteps : IClassFixture<TestFixture>
{
    private readonly IActor _actor;
    private readonly AppSettings _appSettings;
    private readonly DriverContext _driverContext;

    public GeocodioDemoSteps(TestFixture fixture, ITestOutputHelper outputHelper)
    {
        var logger = new NunitBoaConstrictorLogger(outputHelper, LogSeverity.Info);
        _actor = new Actor("Intrepid Geocoder", logger);

        _driverContext = fixture.DriverContext;
        _appSettings = fixture.AppSettings;

        _actor.Can(BrowseTheWeb.With(_driverContext.Driver));
    }
    [Given(@"I have a web browser")]
    public void GivenIHaveAWebBrowser()
    {
        _actor.Using<BrowseTheWeb>().WebDriver.ShouldNotBeNull();
    }

    [When(@"I navigate to the site")]
    public void WhenINavigateToTheSite()
    {
        _actor.AttemptsTo(Navigate.ToUrl(_appSettings.Url));
    }

    [Then(@"I should see the site title")]
    public void ThenIShouldSeeTheSiteTitle()
    {
        Verify.That(_driverContext, () =>
        {
            _actor.AsksFor(Website.Title()).ShouldBe("Geocod.io Demo");
        }, true,false);
    }
}

