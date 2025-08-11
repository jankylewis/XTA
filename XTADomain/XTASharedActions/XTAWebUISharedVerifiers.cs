using FluentAssertions;
using Microsoft.Playwright;

namespace XTADomain.XTASharedActions;

public class XTAWebUISharedVerifiers
{
    public XTAWebUISharedVerifiers() {}
    
    internal async Task VerifyIfTextContentMatchedAsync(IPage in_xPage, string in_selector, string in_expectedText) 
        => (await in_xPage.TextContentAsync(in_selector)).Should().Be(in_expectedText);

    internal async Task VerifyIfElementIsVisibleWithoutWaitsAsync(IPage in_xPage, string in_selector) 
        => (await in_xPage.IsVisibleAsync(in_selector)).Should().Be(true);

    internal async Task VerifyIfElementIsVisibleWithWaitsAsync(
        IPage in_xPage, string in_selector, LocatorAssertionsToBeVisibleOptions? in_locatorAssertionsToBeVisibleOpts = default) 
            => await Expect(in_xPage.Locator(in_selector))
                .ToBeVisibleAsync(in_locatorAssertionsToBeVisibleOpts);

    internal async Task VerifyIfElementIsClickableAsync(
        IPage in_xPage,
        string in_selector,
        LocatorClickOptions? in_clickOptions = default)
    {
        LocatorClickOptions locatorClickOpts = in_clickOptions ?? new LocatorClickOptions
        {
            Trial = true,
            Timeout = XTAInfras.XInfrasConstHouse.XTimedoutConsts.MAX_ELEMENT_TIMEOUT_MS
        };

        await in_xPage.Locator(in_selector).ClickAsync(locatorClickOpts);
    }
}