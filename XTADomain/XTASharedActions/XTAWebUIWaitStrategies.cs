using System.Xml.Schema;
using Microsoft.Playwright;
using XTAPlaywright.XConstHouse;

namespace XTADomain.XTASharedActions;

public class XTAWebUIWaitStrategies
{
    internal XTAWebUIWaitStrategies() {}
    
    internal async Task WaitForElementToBeVisibleAsync(IPage in_page, string in_selector, LocatorWaitForOptions? in_waitForOptions = default)
        => await in_page.Locator(in_selector)
            .WaitForAsync(in_waitForOptions ?? new LocatorWaitForOptions
            {
                Timeout = XConsts.ELEMENT_TIMEOUT_MS,
                State = WaitForSelectorState.Visible
            });
}