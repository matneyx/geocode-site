using System.ComponentModel;
using System.Linq;
using Boa.Constrictor.Selenium;
using Ocaramba;
using Ocaramba.Extensions;
using Ocaramba.Types;
using OpenQA.Selenium;

namespace Geocod.io.Demo.E2E.Framework;

public static class UniversalLocatorExtensions
{
    public static ElementLocator AsElementLocator(this UniversalLocator locator)
        => new(locator.Type, locator.LocatorString);

    public static By AsBy(this UniversalLocator locator)
        => locator.AsElementLocator().ToBy();

    public static WebLocator AsWebLocator(this UniversalLocator locator)
        => new(locator.Name, locator.AsBy());
}
