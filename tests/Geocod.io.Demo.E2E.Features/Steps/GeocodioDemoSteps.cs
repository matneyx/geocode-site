﻿using System;
using Boa.Constrictor.Screenplay;
using Boa.Constrictor.Selenium;
using Geocod.io.Demo.E2E.Framework;
using Geocod.io.Demo.E2E.Framework.Logger;
using Geocod.io.Demo.E2E.Screenplay.Pages;
using Ocaramba;
using Shouldly;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace Geocod.io.Demo.E2E.Features.Steps;

[Binding]
public class GeocodioDemoSteps : IClassFixture<UiTestFixture>
{
    private readonly IActor _actor;
    private readonly AppSettings _appSettings;
    private readonly DriverContext _driverContext;

    public GeocodioDemoSteps(UiTestFixture fixture, ITestOutputHelper outputHelper)
    {
        var logger = new NunitBoaConstrictorLogger(outputHelper, LogSeverity.Info);
        _actor = new Actor("Intrepid Geocoder", logger);

        _driverContext = fixture.DriverContext;
        _appSettings = fixture.AppSettings;

        _actor.Can(BrowseTheWeb.With(_driverContext.Driver));
    }
    [Given(@"I have loaded the site")]
    public void GivenIHaveLoadedTheSite() => _actor.AttemptsTo(Navigate.ToUrl(_appSettings.Url));

    [Given(@"There is a file upload field")]
    public void GivenThereIsAFileUploadField() =>
        _actor.WaitsUntil(
            Appearance.Of(LandingPage.UploadSection.AsWebLocator()),
            IsEqualTo.True());

    [When(@"I select a file to upload")]
    public void WhenISelectAFileToUpload() =>
        _actor.AttemptsTo(UploadFile
            .Through(
                LandingPage.UploadInput.AsWebLocator(),
                $"{AppDomain.CurrentDomain.BaseDirectory}Files\\test.csv"));

    [When(@"I click the Upload button")]
    public void WhenIClickTheUploadButton() =>
        _actor.AttemptsTo(Click.On(LandingPage.UploadButton.AsWebLocator()));

    [Then(@"I should get a message that the file was uploaded")]
    public void ThenIShouldGetAMessageThatTheFileWasUploaded()
    {
        _actor.WaitsUntil(Appearance.Of(LandingPage.FileUploadedToast.AsWebLocator()), IsEqualTo.True());
        _actor.AsksFor(Text.Of(LandingPage.FileUploadedToastBody.AsWebLocator())).ShouldBe("File name: test.csv");
    }
}

