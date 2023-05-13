using Boa.Constrictor.Selenium;
using Ocaramba;
using Ocaramba.Extensions;
using Ocaramba.Types;
using OpenQA.Selenium;

namespace Geocod.io.Demo.E2E.Framework;

public class UniversalLocator
{
    public UniversalLocator(string name, Locator type, string locatorString)
    {
        Name = name;
        Type = type;
        LocatorString = locatorString;
    }

    public string Name { get; set; }
    public string LocatorString{ get; set; }
    public Locator Type { get; set; }

    public ElementLocator AsElementLocator()
        => new(Type, LocatorString);

    public By AsBy()
        => AsElementLocator().ToBy();

    public WebLocator AsWebLocator()
        => new(Name, AsBy());
}
