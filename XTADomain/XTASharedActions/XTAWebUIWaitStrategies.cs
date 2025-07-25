using Microsoft.Playwright;

namespace XTADomain.XTASharedActions;

public class XTAWebUIWaitStrategies
{
    internal XTAWebUIWaitStrategies() {}
    
    internal async Task WaitForElementToBeVisibleAsync(IPage in_page, string in_selector)
        => await in_page.Locator(in_selector)
            .WaitForAsync(new LocatorWaitForOptions
            {
                Timeout = 17000,
                State = WaitForSelectorState.Visible
            });
}