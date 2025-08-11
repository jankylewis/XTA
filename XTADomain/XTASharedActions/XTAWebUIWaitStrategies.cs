using Microsoft.Playwright;
using XTAInfras.XInfrasConstHouse;

namespace XTADomain.XTASharedActions;

public class XTAWebUIWaitStrategies
{
    public XTAWebUIWaitStrategies() {}
    
    internal async Task WaitForElementToBeVisibleAsync(
        IPage in_xPage, string in_selector, LocatorWaitForOptions? in_waitForOptions = default) 
            => await in_xPage.Locator(in_selector)
                .WaitForAsync(in_waitForOptions ?? new LocatorWaitForOptions
                {
                    Timeout = XTimedoutConsts.MAX_ELEMENT_TIMEOUT_MS,
                    State = WaitForSelectorState.Visible
                });
    
    internal async Task WaitForElementToBeClickableAsync(
        IPage in_xPage, string in_selector, 
        LocatorWaitForOptions? in_locatorWaitForOptions = default, 
        LocatorAssertionsToBeEnabledOptions? in_locatorAssertionsToBeEnabledOptions = default)
    {
        ILocator element = in_xPage.Locator(in_selector);
        
        // Wait for the element to be first visible
        await element.WaitForAsync(in_locatorWaitForOptions ?? new LocatorWaitForOptions
        {
            Timeout = XTimedoutConsts.MAX_ELEMENT_TIMEOUT_MS,
            State = WaitForSelectorState.Visible
        });
        
        // Wait for the element to be enabled (clickable)
        await Expect(element).ToBeEnabledAsync(in_locatorAssertionsToBeEnabledOptions ?? new LocatorAssertionsToBeEnabledOptions
        {
            Timeout = XTimedoutConsts.MAX_ELEMENT_TIMEOUT_MS
        });
    }
    
    internal async Task WaitForElementToBeEnabledAsync(
        IPage in_xPage, string in_selector, LocatorAssertionsToBeEnabledOptions? in_locatorAssertionsToBeEnabledOptions = default)
            => await Expect(in_xPage.Locator(in_selector))
                .ToBeEnabledAsync(in_locatorAssertionsToBeEnabledOptions ?? new LocatorAssertionsToBeEnabledOptions
                {
                    Timeout = XTimedoutConsts.MAX_ELEMENT_TIMEOUT_MS
                });
    
    internal async Task WaitForElementToBeStableAsync(
        IPage in_xPage, string in_selector, LocatorWaitForOptions? in_locatorWaitForOptions = default)
            => await in_xPage.Locator(in_selector)
                .WaitForAsync(in_locatorWaitForOptions ?? new LocatorWaitForOptions
                {
                    Timeout = XTimedoutConsts.MAX_ELEMENT_TIMEOUT_MS,
                    State = WaitForSelectorState.Attached
                });
}