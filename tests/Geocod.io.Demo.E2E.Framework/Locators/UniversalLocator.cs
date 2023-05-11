using Ocaramba;

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
}
