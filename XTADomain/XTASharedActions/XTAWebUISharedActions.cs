using Microsoft.Playwright;

namespace XTADomain.XTASharedActions;

public class XTAWebUISharedActions
{
    internal XTAWebUISharedActions() {}
    
    internal async Task ClickAsync(IPage in_xPage, string in_selector) 
        => await in_xPage.ClickAsync(in_selector);

    internal async Task FillTextAsync(IPage in_xPage, string in_selector, string in_text)
        => await in_xPage.Locator(in_selector).FillAsync(in_text);
}